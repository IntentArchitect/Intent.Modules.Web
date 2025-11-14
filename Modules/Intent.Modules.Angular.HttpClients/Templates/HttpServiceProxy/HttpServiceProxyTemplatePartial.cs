using Intent.Engine;
using Intent.Metadata.Models;
using Intent.Modelers.Services.Api;
using Intent.Modelers.Services.CQRS.Api;
using Intent.Modelers.Types.ServiceProxies.Api;
using Intent.Modules.Angular.HttpClients.Templates.DtoContract;
using Intent.Modules.Angular.HttpClients.Templates.Helper;
using Intent.Modules.Angular.HttpClients.Templates.PagedResult;
using Intent.Modules.Angular.ServiceProxies.Templates.Proxies.PagedResult;
using Intent.Modules.Angular.Templates.Helper;
using Intent.Modules.Common;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.TypeScript.Builder;
using Intent.Modules.Common.TypeScript.Events;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.Modules.Metadata.WebApi.Models;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;
using Intent.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.TypeScript.Templates.TypescriptTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Angular.HttpClients.Templates.HttpServiceProxy
{
    [IntentIgnore]
    public partial class HttpServiceProxyTemplate : TypeScriptTemplateBase<IServiceProxyModel>, ITypescriptFileBuilderTemplate
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.HttpClients.HttpServiceProxy";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public HttpServiceProxyTemplate(IOutputTarget outputTarget,
            IServiceProxyModel model) : base(TemplateId, outputTarget, model)
        {
            AddTypeSource(DtoContractTemplate.TemplateId);
            PagedResultTypeSource.ApplyTo(this, PagedResultTemplate.TemplateId);

            TypescriptFile = new TypescriptFile(this.GetFolderPath())
                .AddClass($"{Model.Name}", @class =>
                {
                    @class.Export();
                    @class.AddDecorator(UseType("Injectable", "@angular/core"), decorator =>
                    {
                        var obj = new TypescriptVariableObject();
                        obj.AddField("providedIn", "'root'");

                        //TODO sort out indentation on final line of parameter
                        decorator.AddArgument(obj.GetText(TypescriptFile.Indentation));
                    });

                    @class.AddField("baseUrl", @base =>
                    {
                        @base.PrivateReadOnly();
                        var environmentTemplate = GetTemplate<TypeScriptTemplateBase<object>>("Intent.Angular.Environment.Environment", new TemplateDiscoveryOptions { TrackDependency = false });
                        AddImport("environment", this.GetRelativePath(environmentTemplate));
                        @base.WithValue($"environment.{Model.Name.ToCamelCase()}BaseUrl");
                    });

                    @class.AddConstructor(ctor =>
                    {
                        ctor.AddParameter("httpClient", UseType("HttpClient", "@angular/common/http"), param => param.WithPrivateFieldAssignment());
                    });

                    foreach (var endpoint in Model.Endpoints)
                    {
                        var inputsBySource = endpoint.Inputs
                             .GroupBy(x => x.Source)
                             .Where(x => x.Key != null)
                             .ToDictionary(x => x.Key!.Value, x => x.ToArray());

                        @class.AddMethod(endpoint.Name.ToCamelCase(true), $"{UseType("Observable", "rxjs")}<{GetReturnType(endpoint)}>", method =>
                        {
                            if (Model.UnderlyingModel is ServiceProxyModel serviceProxyModel && serviceProxyModel.Operations.Any())
                            {
                                var operationModel = serviceProxyModel.Operations.Single(x => x.Mapping?.ElementId == endpoint.Id);
                            }

                            method.Public();

                            string? parameterName;
                            string? bodyPayload = null;
                            bool objectParam = false;
                            if (model.CreateParameterPerInput)
                            {
                                foreach (var input in endpoint.Inputs)
                                {
                                    parameterName = input.Name.ToCamelCase(true);
                                    method.AddParameter(parameterName, GetTypeName(input.TypeReference));
                                }

                                parameterName = null;
                            }
                            else
                            {
                                var fields = endpoint.InternalElement.ChildElements.Where(x => x.IsDTOFieldModel()).ToArray();

                                switch (fields.Length)
                                {
                                    case 0:
                                        parameterName = null;
                                        break;
                                    case 1:
                                        parameterName = fields[0].Name.ToCamelCase(true);
                                        method.AddParameter(parameterName, GetTypeName(fields[0].TypeReference));
                                        bodyPayload = $"new {{ {fields[0].Name.ToPascalCase()} = {parameterName} }}";
                                        break;
                                    default:
                                        parameterName = endpoint.InternalElement.SpecializationTypeId switch
                                        {
                                            CommandModel.SpecializationTypeId => "command",
                                            QueryModel.SpecializationTypeId => "query",
                                            _ => endpoint.InternalElement.Name.ToCamelCase(true)

                                        };
                                        method.AddParameter(parameterName, GetTypeName(endpoint.InternalElement));
                                        objectParam = true;
                                        break;
                                }
                            }

                            var url = endpoint.Route.Replace("{", "${");
                            var endpointRoute = endpoint.Route;
                            foreach (var input in endpoint.Inputs)
                            {
                                url = url.Replace($"${{{input.Name.ToLowerInvariant()}}}", $"${{{(!string.IsNullOrWhiteSpace(parameterName) && objectParam ? $"{parameterName}." : string.Empty)}{input.Name.ToCamelCase(true)}}}");
                            }

                            method.AddStatement($"const url = `${{this.baseUrl}}{url}`;");
                            method.AddStatements(GetPreDataServiceCallStatements(endpoint, objectParam ? parameterName : ""));
                            method.AddStatement(BuidReturnStatement(endpoint));
                        });
                    }
                });
        }

        public override void BeforeTemplateExecution()
        {
            var proxyUrl = GetProxyUrl(Model);

            ExecutionContext.EventDispatcher.Publish(new ConfigurationVariableRequiredEvent($"{Model.Name.ToCamelCase()}BaseUrl", $"'{proxyUrl}'"));
        }

        [IntentManaged(Mode.Fully)]
        public TypescriptFile TypescriptFile { get; }

        [IntentManaged(Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return TypescriptFile.GetConfig($"{Model.Name}");
        }

        [IntentManaged(Mode.Fully)]
        public override string TransformText()
        {
            return TypescriptFile.ToString();
        }

        private string UseType(string type, string location)
        {
            this.AddImport(type, location);
            return type;
        }

        private string GetReturnType(IHttpEndpointModel operation)
        {
            if (operation.ReturnType == null)
            {
                return "boolean";
            }

            return GetTypeName(operation.ReturnType);
        }

        private string BuidReturnStatement(IHttpEndpointModel endpoint)
        {
            if(endpoint.ReturnType?.Element is null)
            {
                return $"return this.httpClient.{GetDataServiceCall(endpoint)};";
            }

            return $@"return this.httpClient.{GetDataServiceCall(endpoint)}
      .pipe({UseType("map", "rxjs/operators")}((response: {GetApiResponseType(endpoint)}) => {{
        {GetApiResponseExpression(endpoint)}
      }}));";
        }

        private string GetSourceExpression(string? methodParameterName, IHttpEndpointModel endpoint, IHttpEndpointInputModel input)
        {
            var hasSingleFieldChild = endpoint.InternalElement.ChildElements.Count(x => x.IsDTOFieldModel()) == 1;
            if (!Model.CreateParameterPerInput && hasSingleFieldChild)
            {
                return methodParameterName!;
            }

            return Model.CreateParameterPerInput || endpoint.InternalElement.Id == input.TypeReference.ElementId
                ? input.Name.ToCamelCase(true)
                : $"{methodParameterName!}.{input.Name.ToCamelCase()}";
        }

        public string GetApplicationName(IServiceProxyModel model)
        {
            return string.Concat(model.Endpoints[0].InternalElement.Package.Name
                .RemoveSuffix(".Services")
                .Split('.')
                .Select(x => x.ToCamelCase()));
        }

        public string GetProxyUrl(IServiceProxyModel proxy)
        {
            var url = string.Empty;

            var package = proxy.InternalElement?.Package;
            // this if for if the service is not defined in a folder, then the proxy.InternalElement is null. In which 
            // case we revert to trying to get the package from the first element on the proxy
            package = package is null && proxy.Endpoints.Any() ? proxy.Endpoints[0].InternalElement?.Package : package;

            if (package == null)
            {
                return url;
            }

            return ProxyUrlHelper.GetProxyApplicationtUrl(package, ExecutionContext);
        }

        private List<string> GetPreDataServiceCallStatements(IHttpEndpointModel operation, string parameterName)
        {
            var statements = new List<string>();

            var queryParams = operation.Inputs.Where(x => x.Source == HttpInputSource.FromQuery).ToList();
            if (queryParams.Count != 0)
            {
                if (!string.IsNullOrWhiteSpace(parameterName))
                {
                    parameterName = $"{parameterName}.";
                }

                statements.Add($"let httpParams = new {UseType("HttpParams", "@angular/common/http")}()");
                var localIndentation = "  ";
                foreach (var queryParam in queryParams.Where(p => !p.TypeReference.IsNullable))
                {
                    if (queryParam.TypeReference.Element.Name == "date" || queryParam.TypeReference.Element.Name == "datetime")
                    {
                        statements.Add($@"{localIndentation}.set(""{queryParam.Name.ToCamelCase(true)}"", {parameterName}{queryParam.Name.ToCamelCase(true)}.toISOString())");
                        continue;
                    }
                    statements.Add($@"{localIndentation}.set(""{queryParam.Name.ToCamelCase(true)}"", {parameterName}{queryParam.Name.ToCamelCase(true)})");
                }

                // add the trailing ;
                int lastIndex = statements.Count - 1;
                statements[lastIndex] = statements[lastIndex].TrimEnd(';') + ";";

                foreach (var queryParam in queryParams.Where(p => p.TypeReference.IsNullable))
                {
                    statements.Add("");
                    statements.Add($"if({parameterName}{queryParam.Name.ToCamelCase(true)} != null) {{");

                    if (queryParam.TypeReference.Element.Name == "date" || queryParam.TypeReference.Element.Name == "datetime")
                    {
                        statements.Add($@"{localIndentation}httpParams = httpParams.set(""{queryParam.Name.ToCamelCase(true)}"", {parameterName}{queryParam.Name.ToCamelCase(true)}.toISOString());");
                        continue;
                    }
                    statements.Add($@"{localIndentation}httpParams = httpParams.set(""{queryParam.Name.ToCamelCase(true)}"", {parameterName}{queryParam.Name.ToCamelCase(true)});");

                    statements.Add("}");
                }
            }

            var formDataFields = operation.Inputs.Where(x => x.Source == HttpInputSource.FromForm).ToList();
            if (formDataFields.Count != 0)
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
                    statements.Add($@"{TypescriptFile.Indentation}.append(""{header.HeaderName}"", {header.Name.ToCamelCase(true)})");
                }
                statements.Add(";");
            }

            if (!statements.Any())
            {
                return new List<string>();
            }

            return statements;
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
                    exprBuilder.Append(operation.Inputs.Any(x => x.Source == HttpInputSource.FromForm) ? "postWithFormData" : "post");
                    break;
                case HttpVerb.Put:
                    exprBuilder.Append(operation.Inputs.Any(x => x.Source == HttpInputSource.FromForm) ? "putWithFormData" : "put");
                    break;
                case HttpVerb.Delete:
                    exprBuilder.Append("delete");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var responseType = GetApiResponseType(operation);
            exprBuilder.Append($"<{responseType}>");
            exprBuilder.Append("(url");

            var arguments = new List<string>();

            if (operation.Inputs.Any(x => x.Source == HttpInputSource.FromForm))
            {
                arguments.Add("formData");
            }
            else if (operation.Inputs.FirstOrDefault(x => x.Source == HttpInputSource.FromBody) is { } bodyParam)
            {
                arguments.Add(bodyParam.Name.ToCamelCase(true));
            }
            else if (operation.Verb is HttpVerb.Post or HttpVerb.Put)
            {
                arguments.Add("{}"); // empty body if required
            }

            // Construct options object
            var options = new List<string>();

            if (operation.Inputs.Any(x => x.Source == HttpInputSource.FromQuery))
            {
                options.Add("params: httpParams");
            }

            if (operation.Inputs.Any(x => x.Source == HttpInputSource.FromHeader))
            {
                options.Add("headers");
            }

            if (operation.ReturnType is not null && operation.ReturnType?.Element is not null && ShouldReadAsRawText(operation))
            {
                options.Add("responseType: 'text'");
            }

            if (options.Count != 0)
            {
                arguments.Add($"{{ {string.Join(", ", options)} }}");
            }

            exprBuilder.Append(arguments.Count > 0 ? ", " + string.Join(", ", arguments) : string.Empty);
            exprBuilder.Append(')');

            return exprBuilder.ToString();
        }

        private bool ShouldReadAsRawText(IHttpEndpointModel operation)
        {
            return (!HasWrappedReturnType(operation) && IsReturnTypePrimitive(operation))
                || (!HasWrappedReturnType(operation) && operation.ReturnType.HasStringType() && !operation.ReturnType.IsCollection);
        }

        private bool HasWrappedReturnType(IHttpEndpointModel operationModel)
        {
            if (operationModel.MediaType != null)
            {
                return operationModel.MediaType == HttpMediaType.ApplicationJson &&
                    GetTypeInfo(operationModel.ReturnType)?.Template?.Id != PagedResultTemplate.TemplateId;
            }

            return false;
        }

        private bool IsReturnTypePrimitive(IHttpEndpointModel operation)
        {
            return GetTypeInfo(operation.ReturnType).IsPrimitive && !operation.ReturnType.IsCollection;
        }

        private string GetApiResponseType(IHttpEndpointModel endpoint)
        {
            if (HasWrappedReturnType(endpoint))
            {
                return $"{this.GetJsonResponseTemplateName()}<{GetTypeName(endpoint.ReturnType)}>";
            }

            return $"{GetTypeName(endpoint.ReturnType)}";
        }

        private string GetApiResponseExpression(IHttpEndpointModel endpoint)
        {
            var statements = new List<string>();

            if (endpoint.ReturnType != null && ShouldReadAsRawText(endpoint))
            {
                statements.Add(@"return response.replace(/^[""']|[""']$/g, """");");
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
    }
}