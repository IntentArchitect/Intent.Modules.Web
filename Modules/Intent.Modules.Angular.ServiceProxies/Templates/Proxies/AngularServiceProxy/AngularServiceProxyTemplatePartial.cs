using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Intent.Angular.ServiceProxies.Api;
using Intent.Engine;
using Intent.Metadata.Models;
using Intent.Metadata.WebApi.Api;
using Intent.Modelers.Types.ServiceProxies.Api;
using Intent.Modelers.WebClient.Angular.Api;
using Intent.Modules.Angular.ServiceProxies.Templates.Proxies.AngularDTO;
using Intent.Modules.Angular.ServiceProxies.Templates.Proxies.PagedResult;
using Intent.Modules.Angular.Templates;
using Intent.Modules.Angular.Templates.Core.ApiService;
using Intent.Modules.Common;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.TypeScript.Builder;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.Modules.Metadata.WebApi.Models;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;
using Intent.Utils;
using ServiceOperationModel = Intent.Modelers.Services.Api.OperationModel;
using ProxyOperationModel = Intent.Modelers.Types.ServiceProxies.Api.OperationModel;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.TypeScript.Templates.TypescriptTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Angular.ServiceProxies.Templates.Proxies.AngularServiceProxy
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public partial class AngularServiceProxyTemplate : TypeScriptTemplateBase<Intent.Modelers.Types.ServiceProxies.Api.ServiceProxyModel>, ITypescriptFileBuilderTemplate
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.ServiceProxies.Proxies.AngularServiceProxy";

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public AngularServiceProxyTemplate(IOutputTarget outputTarget, Intent.Modelers.Types.ServiceProxies.Api.ServiceProxyModel model) : base(TemplateId, outputTarget, model)
        {
            //ServiceMetadataQueries.Validate(this, model);
            AddTypeSource(AngularDTOTemplate.TemplateId);
            PagedResultTypeSource.ApplyTo(this, PagedResultTemplate.TemplateId);

            TypescriptFile = new TypescriptFile($"{string.Join("/", Model.GetModule(ExecutionContext).InternalElement.GetFolderPath(additionalFolders: Model.GetModule(ExecutionContext).GetModuleName().ToKebabCase()))}")
                .AddClass(Model.Name, @class =>
                {
                    @class.Export();
                    @class.AddDecorator(UseType("Injectable", "@angular/core"));
                    @class.AddConstructor(ctor =>
                    {
                        ctor.AddParameter("apiService", this.GetApiServiceName(), param => param.WithPrivateFieldAssignment());
                    });

                    foreach (var operation in Model.Operations)
                    {
                        var endpoint = HttpEndpointModelFactory.GetEndpoint((IElement)operation.Mapping.Element);
                        if (endpoint is null)
                        {
                            Logging.Log.Warning($"Operation [{operation.Name}] on {ServiceProxyModel.SpecializationType} [{Model.Name}] is not mapped to an Http-exposed service");
                            continue;
                        }
                        AddOperationMethod(@class, operation.Name.ToCamelCase(true), endpoint);
                    }

                    if (!model.Operations.Any())
                    {
                        var endpoints = Model.GetMappedEndpoints();
                        foreach (var endpoint in endpoints)
                        {
                            AddOperationMethod(@class, endpoint.Name.ToCamelCase(true), endpoint);
                        }
                    }
                });

            //if (Model.MappedService == null)
            //{
            //    Logging.Log.Warning($"{ServiceProxyModel.SpecializationType} [{Model.Name}] is not mapped to an underlying Service");
            //}

            //foreach (var operation in Model.Operations)
            //{
            //    if (!operation.IsMapped || operation.Mapping == null)
            //    {
            //        Logging.Log.Warning($"Operation [{operation.Name}] on {ServiceProxyModel.SpecializationType} [{Model.Name}] is not mapped to an underlying Service Operation");
            //    }
            //}
        }

        private void AddOperationMethod(TypescriptClass @class, string name, IHttpEndpointModel endpoint)
        {
            @class.AddMethod(name, $"{UseType("Observable", "rxjs")}<{GetReturnType(endpoint)}>", method =>
            {
                var url = endpoint.Route.Replace("{", "${");
                method.Public();
                foreach (var input in endpoint.Inputs)
                {
                    method.AddParameter(input.Name.ToCamelCase(true), GetTypeName(input.TypeReference));
                    url = url.Replace($"${{{input.Name.ToLowerInvariant()}}}", $"${{{input.Name.ToCamelCase(true)}}}");
                }

                method.AddStatement($"let url = `/{url}`;");
                method.AddStatements(GetPreDataServiceCallStatements(endpoint));
                method.AddStatement($@"return this.apiService.{GetDataServiceCall(endpoint)}
      .pipe({UseType("map", "rxjs/operators")}((response: {GetApiResponseType(endpoint)}) => {{
        {GetApiResponseExpression(endpoint)}
      }}));");
            });
        }

        [IntentManaged(Mode.Fully)]
        public TypescriptFile TypescriptFile { get; }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new TypeScriptFileConfig(
                overwriteBehaviour: OverwriteBehaviour.Always,
                fileName: $"{Model.Name.ToKebabCase()}.service",
                relativeLocation: $"{string.Join("/", Model.GetModule(ExecutionContext).InternalElement.GetFolderPath(additionalFolders: Model.GetModule(ExecutionContext).GetModuleName().ToKebabCase()))}",
                className: "${Model.Name}"
            );
        }

        [IntentManaged(Mode.Fully)]
        public override string TransformText()
        {
            return TypescriptFile.ToString();
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
                moduleId: Model.GetModule(ExecutionContext).Id));
        }

        private string GetReturnType(IHttpEndpointModel operation)
        {
            if (operation.ReturnType == null)
            {
                return "boolean";
            }

            return GetTypeName(operation.ReturnType);
        }

        private string GetParameterDefinitions(ServiceOperationModel operation)
        {
            return string.Join(", ", operation.Parameters.Select(x => x.Name.ToCamelCase(true) + (x.TypeReference.IsNullable ? "?" : "") + ": " + Types.Get(x.TypeReference, "{0}[]")));
        }

        private string GetDataServiceCall(IHttpEndpointModel operation)
        {
            var exprBuilder = new StringBuilder();

            switch (operation.Verb)
            {
                case HttpVerb.Get:
                    exprBuilder.Append("get");
                    break;
                case HttpVerb.Post:
                    if (operation.Inputs.Any(x => x.Source == HttpInputSource.FromForm))
                    {
                        exprBuilder.Append("postWithFormData");
                    }
                    else
                    {
                        exprBuilder.Append("post");
                    }
                    break;
                case HttpVerb.Put:
                    if (operation.Inputs.Any(x => x.Source == HttpInputSource.FromForm))
                    {
                        exprBuilder.Append("putWithFormData");
                    }
                    else
                    {
                        exprBuilder.Append("put");
                    }
                    break;
                case HttpVerb.Delete:
                    exprBuilder.Append("delete");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            exprBuilder.Append("(url");

            var arguments = new List<string>();

            if (operation.Inputs.Any(x => x.Source == HttpInputSource.FromForm))
            {
                arguments.Add("formData");
            }
            else if (operation.Inputs.FirstOrDefault(x => x.Source == HttpInputSource.FromBody) != null)
            {
                var bodyParam = operation.Inputs.First(x => x.Source == HttpInputSource.FromBody);
                arguments.Add($"{bodyParam.Name.ToCamelCase(true)}");
            }
            else if (operation.Verb is HttpVerb.Put or HttpVerb.Post)
            {
                arguments.Add("{}");
            }

            if (operation.Inputs.Any(x => x.Source == HttpInputSource.FromQuery))
            {
                arguments.Add("httpParams");
            }
            else
            {
                arguments.Add("undefined");
            }

            if (operation.Inputs.Any(x => x.Source == HttpInputSource.FromHeader))
            {
                arguments.Add("headers");
            }
            else
            {
                arguments.Add("undefined");
            }

            if (operation.ReturnType != null && ShouldReadAsRawText(operation))
            {
                arguments.Add("'text'");
            }

            for (var index = arguments.Count - 1; index >= 0; index--)
            {
                var current = arguments[index];
                if (current == "undefined")
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

        private bool ShouldReadAsRawText(IHttpEndpointModel operation)
        {

            return (!HasWrappedReturnType(operation) && IsReturnTypePrimitive(operation))
                || (!HasWrappedReturnType(operation) && operation.ReturnType.HasStringType() && !operation.ReturnType.IsCollection);
        }

        private bool HasWrappedReturnType(IHttpEndpointModel operationModel)
        {
            return ServiceMetadataQueries.HasJsonWrappedReturnType(operationModel);
        }

        private bool IsReturnTypePrimitive(IHttpEndpointModel operation)
        {
            return GetTypeInfo(operation.ReturnType).IsPrimitive && !operation.ReturnType.IsCollection;
        }

        private HttpVerb GetHttpVerb(ServiceOperationModel operation)
        {
            return Enum.TryParse(operation.GetHttpSettings().Verb().Value, out HttpVerb verbEnum) ? verbEnum : HttpVerb.Post;
        }



        //private string GetRelativeUri(ServiceOperationModel operation)
        //{
        //    var relativeUri = ServiceMetadataQueries.GetRelativeUri(operation);
        //    return "/" + relativeUri;
        //}

        private string GetApiResponseType(IHttpEndpointModel endpoint)
        {
            if (HasWrappedReturnType(endpoint))
            {
                return $"{this.GetJsonResponseName()}<{GetTypeName(endpoint.ReturnType)}>";
            }
            return "any";
        }

        private string GetApiResponseExpression(IHttpEndpointModel endpoint)
        {
            var statements = new List<string>();

            if (endpoint.ReturnType != null && ShouldReadAsRawText(endpoint))
            {
                statements.Add(@"if (response && (response.startsWith(""\"""") || response.startsWith(""'""))) { response = response.substring(1, response.length - 1); }");
                statements.Add($"return response;");
            }
            else if (endpoint.ReturnType != null && HasWrappedReturnType(endpoint))
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

        private List<string> GetPreDataServiceCallStatements(IHttpEndpointModel operation)
        {
            var statements = new List<string>();

            var queryParams = operation.Inputs.Where(x => x.Source == HttpInputSource.FromQuery).ToList();
            if (queryParams.Any())
            {
                statements.Add($"let httpParams = new {UseType("HttpParams", "@angular/common/http")}()");
                foreach (var queryParam in queryParams)
                {
                    if (queryParam.TypeReference.Element.Name == "date" || queryParam.TypeReference.Element.Name == "datetime")
                    {
                        statements.Add($@"  .set(""{queryParam.Name.ToCamelCase(true)}"", {queryParam.Name.ToCamelCase(true)}.toISOString())");
                        continue;
                    }
                    statements.Add($@"  .set(""{queryParam.Name.ToCamelCase(true)}"", {queryParam.Name.ToCamelCase(true)})");
                }
                statements.Add(";");
            }

            var formDataFields = operation.Inputs.Where(x => x.Source == HttpInputSource.FromForm).ToList();
            if (formDataFields.Any())
            {
                statements.Add($"let formData: FormData = new FormData();");
                foreach (var field in formDataFields)
                {
                    statements.Add($@"formData.append(""{field.Name.ToCamelCase(true)}"", {field.Name.ToCamelCase(true)}{(!field.TypeReference.HasStringType() ? ".toString()" : "")});");
                }
            }

            var headerFields = operation.Inputs.Where(x => x.Source == HttpInputSource.FromHeader).ToList();
            if (headerFields.Any())
            {
                statements.Add($"let headers = new {UseType("HttpHeaders", "@angular/common/http")}()");
                foreach (var header in headerFields)
                {
                    statements.Add($@"  .append(""{header.HeaderName}"", {header.Name.ToCamelCase(true)})");
                }
                statements.Add(";");
            }

            if (!statements.Any())
            {
                return new List<string>();
            }

            return statements;
        }

        private string UseType(string type, string location)
        {
            this.AddImport(type, location);
            return type;
        }

    }
}