using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Intent.Angular.ServiceProxies.Api;
using Intent.Engine;
using Intent.Metadata.WebApi.Api;
using Intent.Modelers.Types.ServiceProxies.Api;
using Intent.Modelers.WebClient.Angular.Api;
using Intent.Modules.Angular.ServiceProxies.Templates.Proxies.AngularDTO;
using Intent.Modules.Angular.Templates;
using Intent.Modules.Angular.Templates.Core.ApiService;
using Intent.Modules.Common;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;
using Intent.Utils;
using ServiceOperationModel = Intent.Modelers.Services.Api.OperationModel;
using ProxyOperationModel = Intent.Modelers.Types.ServiceProxies.Api.OperationModel;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.TypeScript.Templates.TypescriptTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Angular.ServiceProxies.Templates.Proxies.AngularServiceProxy
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class AngularServiceProxyTemplate : TypeScriptTemplateBase<Intent.Modelers.Types.ServiceProxies.Api.ServiceProxyModel>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.ServiceProxies.Proxies.AngularServiceProxy";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public AngularServiceProxyTemplate(IOutputTarget outputTarget, Intent.Modelers.Types.ServiceProxies.Api.ServiceProxyModel model) : base(TemplateId, outputTarget, model)
        {
            ServiceMetadataQueries.Validate(this, model);
            AddTypeSource(AngularDTOTemplate.TemplateId);

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
        
        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new TypeScriptFileConfig(
                overwriteBehaviour: OverwriteBehaviour.Always,
                fileName: $"{Model.Name.ToKebabCase()}.service",
                relativeLocation: $"{string.Join("/", Model.GetModule().InternalElement.GetFolderPath(additionalFolders: Model.GetModule().GetModuleName().ToKebabCase()))}",
                className: "${Model.Name}"
            );
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

        private string GetReturnType(ServiceOperationModel operation)
        {
            if (operation.ReturnType == null)
            {
                return "boolean";
            }

            return GetTypeName(operation.ReturnType);
        }

        private string GetParameterDefinitions(ServiceOperationModel operation)
        {
            return string.Join(", ", operation.Parameters.Select(x => x.Name.ToCamelCase() + (x.TypeReference.IsNullable ? "?" : "") + ": " + Types.Get(x.TypeReference, "{0}[]")));
        }

        private string GetDataServiceCall(ServiceOperationModel operation)
        {
            var exprBuilder = new StringBuilder();

            switch (GetHttpVerb(operation))
            {
                case HttpVerb.GET:
                    exprBuilder.Append("get");
                    break;
                case HttpVerb.POST:
                    if (ServiceMetadataQueries.GetFormUrlEncodedParameters(operation).Any())
                    {
                        exprBuilder.Append("postWithFormData");
                    }
                    else
                    {
                        exprBuilder.Append("post");
                    }
                    break;
                case HttpVerb.PUT:
                    if (ServiceMetadataQueries.GetFormUrlEncodedParameters(operation).Any())
                    {
                        exprBuilder.Append("putWithFormData");
                    }
                    else
                    {
                        exprBuilder.Append("put");
                    }
                    break;
                case HttpVerb.DELETE:
                    exprBuilder.Append("delete");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            exprBuilder.Append("(url");

            var arguments = new List<string>();
            
            if (ServiceMetadataQueries.GetFormUrlEncodedParameters(operation).Any())
            {
                arguments.Add("formData");
            }
            else if (ServiceMetadataQueries.GetBodyParameter(this, operation) != null)
            {
                var bodyParam = ServiceMetadataQueries.GetBodyParameter(this, operation);
                arguments.Add($"{bodyParam.Name.ToCamelCase()}");
            }
            else if (GetHttpVerb(operation) is HttpVerb.PUT or HttpVerb.POST)
            {
                arguments.Add("{}");
            }

            if (ServiceMetadataQueries.GetQueryParameters(this, operation).Any())
            {
                arguments.Add("httpParams");
            }
            else
            {
                arguments.Add("null");
            }

            if (ServiceMetadataQueries.GetHeaderParameters(operation).Any())
            {
                arguments.Add("headers");
            }
            else
            {
                arguments.Add("null");
            }

            if (operation.ReturnType != null && ShouldReadAsRawText(operation))
            {
                arguments.Add("'text'");
            }

            for (var index = arguments.Count - 1; index >= 0; index--)
            {
                var current = arguments[index];
                if (current == "null")
                {
                    arguments.RemoveAt(index);
                    continue;
                }

                break;
            }

            exprBuilder.Append((arguments.Any() ? ", " : string.Empty) + string.Join(", ", arguments));

            exprBuilder.Append(")");
            
            return exprBuilder.ToString();
        }

        private bool ShouldReadAsRawText(ServiceOperationModel operation)
        {
            
            return (!HasWrappedReturnType(operation) && IsReturnTypePrimitive(operation)) 
                || (!HasWrappedReturnType(operation) && operation.ReturnType.HasStringType() && !operation.ReturnType.IsCollection);
        }

        private bool HasWrappedReturnType(ServiceOperationModel operationModel)
        {
            return ServiceMetadataQueries.HasJsonWrappedReturnType(operationModel);
        }

        private bool IsReturnTypePrimitive(ServiceOperationModel operation)
        {
            return GetTypeInfo(operation.ReturnType).IsPrimitive && !operation.ReturnType.IsCollection;
        }

        private HttpVerb GetHttpVerb(ServiceOperationModel operation)
        {
            return Enum.TryParse(operation.GetHttpSettings().Verb().Value, out HttpVerb verbEnum) ? verbEnum : HttpVerb.POST;
        }

        private string GetRelativeUri(ServiceOperationModel operation)
        {
            var relativeUri = ServiceMetadataQueries.GetRelativeUri(operation);
            return "/" + relativeUri;
        }
        
        private string GetApiResponseType(ServiceOperationModel operation)
        {
            if (HasWrappedReturnType(operation))
            {
                return $"{this.GetJsonResponseName()}<{GetTypeName(operation.ReturnType)}>";
            }
            return "any";
        }

        private string GetApiResponseExpression(ServiceOperationModel operation)
        {
            var statements = new List<string>();

            if (operation.ReturnType != null && ShouldReadAsRawText(operation))
            {
                statements.Add(@"if (response && (response.startsWith(""\"""") || response.startsWith(""'""))) { response = response.substring(1, response.length - 1); }");
                statements.Add($"return response;");
            }
            else if (operation.ReturnType != null && HasWrappedReturnType(operation))
            {
                statements.Add($"return response.value;");
            }
            else
            {
                statements.Add($"return response;");
            }

            const string newLine = @"
        ";
            return string.Join(newLine, statements);
        }

        private string GetPreDataServiceCallStatements(ServiceOperationModel operation)
        {
            var statements = new List<string>();

            var queryParams = ServiceMetadataQueries.GetQueryParameters(this, operation);
            if (queryParams.Any())
            {
                statements.Add($"let httpParams = new {UseType("HttpParams", "@angular/common/http")}()");
                foreach (var queryParam in queryParams)
                {
                    if (queryParam.Type.Element.Name == "date" || queryParam.Type.Element.Name == "datetime")
                    {
                        statements.Add($@"  .set(""{queryParam.Name.ToCamelCase()}"", {queryParam.Name.ToCamelCase()}.toISOString())");
                        continue;
                    }
                    statements.Add($@"  .set(""{queryParam.Name.ToCamelCase()}"", {queryParam.Name.ToCamelCase()})");
                }
                statements.Add(";");
            }

            var formDataFields = ServiceMetadataQueries.GetFormUrlEncodedParameters(operation);
            if (formDataFields.Any())
            {
                statements.Add($"let formData: FormData = new {UseType("FormData", "@angular/common/http")}();");
                foreach (var field in formDataFields)
                {
                    statements.Add($@"formData.append(""{field.Name.ToCamelCase()}"", {field.Name.ToCamelCase()});");
                }
            }

            var headerFields = ServiceMetadataQueries.GetHeaderParameters(operation);
            if (headerFields.Any())
            {
                statements.Add($"let headers = new {UseType("HttpHeaders", "@angular/common/http")}()");
                foreach (var header in headerFields)
                {
                    statements.Add($@"  .append(""{header.HeaderName}"", {header.Parameter.Name.ToCamelCase()})");
                }
                statements.Add(";");
            }
            
            if (!statements.Any())
            {
                return string.Empty;
            }
            
            const string newLine = @"
    ";
            return newLine + string.Join(newLine, statements);
        }
        
        private string UseType(string type, string location)
        {
            this.AddImport(type, location);
            return type;
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