using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Intent.Angular.ServiceProxies.Api;
using Intent.Engine;
using Intent.Metadata.Models;
using Intent.Modelers.Services.Api;
using Intent.Modelers.Types.ServiceProxies.Api;
using Intent.Modules.Angular.Templates.Core.ApiServiceTemplate;
using Intent.Modules.Angular.Templates.Module.AngularModuleTemplate;
using Intent.Modules.Common;
using Intent.Modules.Common.Plugins;
using Intent.Modules.Common.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;
using Intent.Utils;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.Modules.Angular.Templates;
using ProxyOperationModel = Intent.Modelers.Types.ServiceProxies.Api.OperationModel;
using OperationModel = Intent.Modelers.Services.Api.OperationModel;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("ModuleBuilder.TypeScript.Templates.TypescriptTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Angular.ServiceProxies.Templates.Proxies.AngularServiceProxyTemplate
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class AngularServiceProxyTemplate : TypeScriptTemplateBase<ServiceProxyModel>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Angular.ServiceProxies.Proxies.AngularServiceProxyTemplate";

        public AngularServiceProxyTemplate(IOutputTarget project, ServiceProxyModel model) : base(TemplateId, project, model)
        {
            AddTypeSource(AngularDTOTemplate.AngularDTOTemplate.TemplateId);

            if (Model.MappedService == null)
            {
                Logging.Log.Warning($"{ServiceProxyModel.SpecializationType} [{Model.Name}] is not mapped to an underlying Service");
            }

            foreach (var operation in Model.Operations)
            {
                if (!operation.IsMapped || operation.Mapping == null)
                {
                    Logging.Log.Warning($"Operation [{operation.Name}] on {ServiceProxyModel.SpecializationType} [{Model.Name}] is not mapped to an underlying Service Operation");
                }
            }
        }

        public string ApiServiceClassName => GetTypeName(ApiServiceTemplate.TemplateId);

        public override void BeforeTemplateExecution()
        {
            if (File.Exists(GetMetadata().GetFilePath()))
            {
                return;
            }

            // New Proxy:
            ExecutionContext.EventDispatcher.Publish(new AngularServiceProxyCreatedEvent(
                templateId: TemplateId,
                modelId: Model.Id,
                moduleId: Model.GetModule().Id));
        }

        private string GetReturnType(ProxyOperationModel operation)
        {
            if (operation.ReturnType == null)
            {
                return "boolean";
            }

            return GetTypeName(operation.ReturnType);
        }

        private string GetParameterDefinitions(ProxyOperationModel operation)
        {
            return string.Join(", ", operation.Parameters.Select(x => x.Name.ToCamelCase() + (x.TypeReference.IsNullable ? "?" : "") + ": " + Types.Get(x.TypeReference, "{0}[]")));
        }

        private string GetUpdateUrl(ProxyOperationModel operation)
        {
            var mappedOperation = new OperationModel((IElement)operation.Mapping.Element);
            if (mappedOperation?.Parameters.Count != operation.Parameters.Count)
            {
                throw new Exception($"Different number of properties for mapped operation [{operation.Name}] on {ServiceProxyModel.SpecializationType} [{Model.Name}]");
            }
            if (!mappedOperation.Parameters.Any() || mappedOperation.Parameters.All(x => x.Type.Element.SpecializationType == DTOModel.SpecializationType))
            {
                return "";
            }

            return $@"
        url = `${{url}}?{string.Join("&", mappedOperation.Parameters.Where(x => x.Type.Element.SpecializationType != DTOModel.SpecializationType)
                .Select((x, index) => $"{x.Name.ToCamelCase()}=${{{operation.Parameters[index].Name.ToCamelCase()}}}"))}`;";
        }

        private string GetDataServiceCall(ProxyOperationModel operation)
        {
            switch (GetHttpVerb(operation))
            {
                case HttpVerb.GET:
                    return $"get(url)";
                case HttpVerb.POST:
                    return $"post(url, {operation.Parameters.FirstOrDefault(x => x.TypeReference.Element.SpecializationType == DTOModel.SpecializationType)?.Name.ToCamelCase() ?? "null"})";
                case HttpVerb.PUT:
                    return $"put(url, {operation.Parameters.FirstOrDefault(x => x.TypeReference.Element.SpecializationType == DTOModel.SpecializationType)?.Name.ToCamelCase() ?? "null"})";
                case HttpVerb.DELETE:
                    return $"delete(url)";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new TypeScriptFileConfig(
                overwriteBehaviour: OverwriteBehaviour.Always,
                fileName: $"{Model.Name.ToKebabCase()}.service",
                relativeLocation: $"{Model.GetModule().GetModuleName().ToKebabCase()}",
                className: "${Model.Name}"
            );
        }

        private HttpVerb GetHttpVerb(ProxyOperationModel operation)
        {
            var verb = operation.GetStereotypeProperty("Http", "Verb", "AUTO").ToUpper();
            if (verb != "AUTO")
            {
                return Enum.TryParse(verb, out HttpVerb verbEnum) ? verbEnum : HttpVerb.POST;
            }
            if (operation.ReturnType == null || operation.Parameters.Any(x => x.TypeReference.Element.SpecializationType == DTOModel.SpecializationType))
            {
                var hasIdParam = operation.Parameters.Any(x => x.Name.ToLower().EndsWith("id") && x.TypeReference.Element.SpecializationType != DTOModel.SpecializationType);
                if (hasIdParam && new[] { "delete", "remove" }.Any(x => operation.Name.ToLower().Contains(x)))
                {
                    return HttpVerb.DELETE;
                }
                return hasIdParam ? HttpVerb.PUT : HttpVerb.POST;
            }
            return HttpVerb.GET;
        }

        private string GetPath(ProxyOperationModel operation)
        {
            var path = operation.GetStereotypeProperty<string>("Http", "Route")?.ToLower();
            return path ?? $"/{Model.MappedService.Name.ToLower()}/{operation.Mapping.Element.Name.ToLower()}";
        }
    }

    internal enum HttpVerb
    {
        GET,
        POST,
        PUT,
        DELETE
    }
}