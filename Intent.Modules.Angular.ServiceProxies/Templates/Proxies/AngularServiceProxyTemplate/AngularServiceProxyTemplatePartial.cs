using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Intent.Angular.Api;
using Intent.Angular.ServiceProxies.Api;
using Intent.Engine;
using Intent.Metadata.Models;
using Intent.Metadata.WebApi.Api;
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
using EnumModel = Intent.Modules.Common.Types.Api.EnumModel;
using ProxyOperationModel = Intent.Modelers.Types.ServiceProxies.Api.OperationModel;
using OperationModel = Intent.Modelers.Services.Api.OperationModel;
using TypeDefinitionModel = Intent.Modules.Common.Types.Api.TypeDefinitionModel;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.TypeScript.Templates.TypescriptTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Angular.ServiceProxies.Templates.Proxies.AngularServiceProxyTemplate
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class AngularServiceProxyTemplate : TypeScriptTemplateBase<Intent.Modelers.Types.ServiceProxies.Api.ServiceProxyModel>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.ServiceProxies.Proxies.AngularServiceProxyTemplate";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public AngularServiceProxyTemplate(IOutputTarget outputTarget, Intent.Modelers.Types.ServiceProxies.Api.ServiceProxyModel model) : base(TemplateId, outputTarget, model)
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

            var path = GetPath(operation);
            var urlParameters = mappedOperation.Parameters.Where(x => !IsComplexObject(x.Type.Element) &&
                                                                      !path.Contains($"${{{x.Name.ToCamelCase()}}}")).ToList();
            if (!urlParameters.Any())
            {
                return "";
            }

            return $@"
        url = `${{url}}?{string.Join("&", urlParameters.Select((x, index) => $"{x.Name.ToCamelCase()}=${{{operation.Parameters[index].Name.ToCamelCase()}}}"))}`;";
        }

        private bool IsComplexObject(ICanBeReferencedType element)
        {
            return element.SpecializationTypeId != TypeDefinitionModel.SpecializationTypeId &&
                   element.SpecializationTypeId != EnumModel.SpecializationTypeId;
        }

        private string GetDataServiceCall(ProxyOperationModel operation)
        {
            switch (GetHttpVerb(operation))
            {
                case HttpVerb.GET:
                    return $"get(url)";
                case HttpVerb.POST:
                    return $"post(url, {operation.Parameters.FirstOrDefault(x => IsComplexObject(x.TypeReference.Element))?.Name.ToCamelCase() ?? "null"})";
                case HttpVerb.PUT:
                    return $"put(url, {operation.Parameters.FirstOrDefault(x => IsComplexObject(x.TypeReference.Element))?.Name.ToCamelCase() ?? "null"})";
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
            return Enum.TryParse(operation.MappedOperation.GetHttpSettings().Verb().Value, out HttpVerb verbEnum) ? verbEnum : HttpVerb.POST;
        }

        private string GetPath(ProxyOperationModel operation)
        {
            var servicePath = Model.MappedService.GetHttpServiceSettings()?.Route()?.ToLower()
                .Replace("[controller]", Model.MappedService.Name.ToLower().RemoveSuffix("controller", "service")) ?? $"api/{Model.MappedService.Name.ToLower().RemoveSuffix("controller", "service")}";
            var operationPath = operation.MappedOperation.GetHttpSettings()?.Route()?.ToLower()
                .Replace("[action]", operation.MappedOperation.Name.ToLower());
            if (!string.IsNullOrWhiteSpace(operationPath))
            {
                foreach (var parameter in operation.Parameters)
                {
                    var startIndex = operationPath.IndexOf($"{{{parameter.Name}", StringComparison.InvariantCultureIgnoreCase);
                    if (startIndex != -1)
                    {
                        var endIndex = operationPath.IndexOf("}", startIndex, StringComparison.InvariantCultureIgnoreCase);
                        if (endIndex != -1)
                        {
                            operationPath = operationPath.Remove(startIndex, endIndex - startIndex + 1);
                            operationPath = operationPath.Insert(startIndex, $"${{{parameter.Name.ToCamelCase()}}}");
                        }
                    }
                }
            }

            return string.IsNullOrWhiteSpace(operationPath) ? $"/{servicePath}" : $"/{servicePath}/{operationPath}";
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