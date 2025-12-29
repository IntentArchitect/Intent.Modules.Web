using Intent.Engine;
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
using Intent.Modules.Common.Types.Api;
using Intent.Modules.Common.TypeScript.Builder;
using Intent.Modules.Common.TypeScript.Events;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.Modules.Metadata.WebApi.Models;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

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

            TypescriptFile = new TypescriptFile(this.GetFolderPath(), this)
                .AddClass($"{Model.Name}", @class =>
                {
                    @class.Export();
                    @class.AddDecorator(UseType("Injectable", "@angular/core"), decorator =>
                    {
                        var obj = new TypescriptVariableObject
                        {
                            Indentation = TypescriptFile.Indentation
                        };
                        obj.AddField("providedIn", "'root'");

                        decorator.AddArgument(obj.GetText(""));
                    });

                    AddServiceEnvironmentFields(@class);

                    @class.AddConstructor(ctor =>
                    {
                        ctor.AddParameter("httpClient", UseType("HttpClient", "@angular/common/http"), param => param.WithPrivateFieldAssignment());
                    });

                    this.ExecutionContext.EventDispatcher.Publish(new ServiceConfigurationRequestEvent("provideHttpClient", "@angular/common/http"));

                    foreach (var endpoint in Model.Endpoints)
                    {
                        var inputsBySource = endpoint.Inputs
                             .GroupBy(x => x.Source)
                             .Where(x => x.Key != null)
                             .ToDictionary(x => x.Key!.Value, x => x.ToArray());

                        @class.AddMethod(endpoint.Name.ToCamelCase(true), $"{UseType("Observable", "rxjs")}<{GetReturnType(endpoint)}>", method =>
                        {
                            if (Model is IServiceProxyModel serviceProxyModel && serviceProxyModel.Endpoints.Any())
                            {
                                var operationModel = serviceProxyModel.Endpoints.Single(x => x.InternalElement.Id == endpoint.Id);
                                method.RepresentsModel(operationModel);
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
                                        parameterName = $"{fields[0].Name.ToCamelCase(true)}";
                                        var parameterType = $"{GetTypeName(fields[0].TypeReference)}{(fields[0].TypeReference.IsNullable ? " | null" : "")}";
                                        method.AddParameter(parameterName, parameterType);
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

                            var url = ToTypeScriptTemplateRoute(endpoint.Route);
                            foreach (var input in endpoint.Inputs)
                            {
                                url = url.Replace($"${{{input.Name.ToLowerInvariant()}}}", $"${{{(!string.IsNullOrWhiteSpace(parameterName) && objectParam ? $"{parameterName}." : string.Empty)}{input.Name.ToCamelCase(true)}}}");
                            }

                            url = AddEncodeURIComponent(url);

                            method.AddStatement($"const url = `${{this.baseUrl}}{url}`;");
                            method.AddStatements(GetPreDataServiceCallStatements(endpoint, objectParam ? parameterName : ""));
                            method.AddStatement(BuildReturnStatement(endpoint));
                        });
                    }
                });
        }

        private void AddServiceEnvironmentFields(TypescriptClass @class)
        {
            var packageName = Model.InternalElement.Package.Name.Replace(".", "").ToCamelCase().Singularize();
            var serviceConfigName = $"{packageName}Config";
            var environmentTemplate = GetTemplate<TypeScriptTemplateBase<object>>("Intent.Angular.Environment.Environment", new TemplateDiscoveryOptions { TrackDependency = false });
            AddImport("environment", this.GetRelativePath(environmentTemplate));

            @class.AddField("baseUrl", @base =>
            {
                @base.PrivateReadOnly();
                @base.WithValue($"environment.{serviceConfigName}.services?.{Model.Name.ToCamelCase(true)}?.baseUrl ?? environment.{serviceConfigName}.baseUrl");
            });

            @class.AddField("timeoutMs", @timeout =>
            {
                AddImport("timeout", "rxjs/operators");

                @timeout.PrivateReadOnly();
                @timeout.WithValue($"environment.{serviceConfigName}.services?.{Model.Name.ToCamelCase(true)}?.timeoutMs ?? environment.{serviceConfigName}.timeoutMs ?? 10_000");
            });

            @class.AddField("retries", @timeout =>
            {
                AddImport("retry", "rxjs/operators");

                @timeout.PrivateReadOnly();
                @timeout.WithValue($"environment.{serviceConfigName}.services?.{Model.Name.ToCamelCase(true)}?.retries ?? environment.{serviceConfigName}.retries ?? 0");
            });
        }

        private static string ToTypeScriptTemplateRoute(string route)
        {
            ArgumentNullException.ThrowIfNull(route);

            // Matches {param} or {param:constraint}, captures "param" in group 1
            return TypeScriptRouteRegex().Replace(route, @"$${$1}");
        }

        private static string AddEncodeURIComponent(string route)
        {
            ArgumentNullException.ThrowIfNull(route);

            // Match ${...} and capture the inner identifier/expression
            var pattern = RouteMatchRegex();

            return pattern.Replace(route, match =>
            {
                var inner = match.Groups[1].Value; // e.g. "id"
                return "${encodeURIComponent(" + inner + ")}";
            });
        }

        public override void BeforeTemplateExecution()
        {
            var proxyUrl = GetProxyUrl(Model);

            PublishStaticInterfaces();

            var packageName = Model.InternalElement.Package.Name.Replace(".", "").ToPascalCase().Singularize();

            ExecutionContext.EventDispatcher.Publish(new EnvironmentRegistrationRequestEvent
            {
                TypeName = $"{packageName}ServicesConfig",
                Comment = $"optional overrides for {packageName}",
                Kind = EnvironmentTypeKind.Interface,
                Fields = [new EnvironmentFieldDescriptor
                {
                    Name = $"{Model.Name.ToCamelCase(true)}",
                    Type = "ServiceOverride",
                    IsOptional = true
                }]
            });

            ExecutionContext.EventDispatcher.Publish(new EnvironmentRegistrationRequestEvent
            {
                EnvironmentName = $"{packageName.ToCamelCase(true)}Config",
                Comment = $"specific configuration for {packageName}",
                TypeName = $"{packageName}Config",
                Kind = EnvironmentTypeKind.Interface,
                Extends = ["HttpConfig"],
                Fields =
                [
                    new EnvironmentFieldDescriptor
                    {
                        Name = "services",
                        Type = $"{packageName}ServicesConfig",
                        IsOptional = true
                    }
                ],
                DefaultValue = @$"{{ 
    baseUrl: '{proxyUrl}'
  }}"
            });
        }

        private void PublishStaticInterfaces()
        {
            ExecutionContext.EventDispatcher.Publish(new EnvironmentRegistrationRequestEvent
            {
                TypeName = "HttpConfig",
                Comment = "base config for http service proxies",
                Kind = EnvironmentTypeKind.Interface,
                Fields =
                [
                   new() { Name = "baseUrl", Type = "string", IsOptional = false },
                   new() { Name = "timeoutMs", Type = "number", IsOptional = true },
                   new() { Name = "retries", Type = "number", IsOptional = true }
                ]
            });

            ExecutionContext.EventDispatcher.Publish(new EnvironmentRegistrationRequestEvent
            {
                TypeName = "ServiceOverride",
                Comment = "interface allows for overrides per service",
                Kind = EnvironmentTypeKind.Interface,
                Fields =
               [
                   new() { Name = "baseUrl", Type = "string", IsOptional = true },
                   new() { Name = "timeoutMs", Type = "number", IsOptional = true },
                   new() { Name = "retries", Type = "number", IsOptional = true }
               ]
            });
        }

        [IntentManaged(Mode.Fully)]
        public TypescriptFile TypescriptFile { get; }

        [IntentManaged(Mode.Ignore)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new TypeScriptFileConfig(
               overwriteBehaviour: OverwriteBehaviour.Always,
               fileName: $"{Model.Name.ToKebabCase()}",
               relativeLocation: $"{string.Join("/", Model.GetParentFolderNames().Select(f => f.ToKebabCase()))}",
               //relativeLocation: RelativeLocationHelper.GetPackageBasedRelativeLocation<IServiceProxyModel>(this, []),
               className: $"{Model.Name}"
           );

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

        private string BuildReturnStatement(IHttpEndpointModel endpoint)
        {
            if (endpoint.ReturnType?.Element is null)
            {
                return @$"return this.httpClient.{GetDataServiceCall(endpoint)}
      .pipe(
        timeout(this.timeoutMs), 
        retry(this.retries)
      );";
            }

            return $@"return this.httpClient.{GetDataServiceCall(endpoint)}
      .pipe(
        timeout(this.timeoutMs),
        retry(this.retries),
        {UseType("map", "rxjs/operators")}((response: {GetApiResponseType(endpoint)}) => {{
          {GetApiResponseExpression(endpoint)}
      }}));";
        }

        public static string GetApplicationName(IServiceProxyModel model) => string.Concat(model.Endpoints[0].InternalElement.Package.Name
                .RemoveSuffix(".Services")
                .Split('.')
                .Select(x => x.ToCamelCase(true)));

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

            if (statements.Count == 0)
            {
                return [];
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
                var fields = operation.InternalElement.ChildElements.Where(x => x.IsDTOFieldModel()).ToArray();

                arguments.Add(fields.Length == 1 ? $"{{ {fields[0].Name.ToCamelCase(true)} }}" : bodyParam.Name.ToCamelCase(true));
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

        [GeneratedRegex(@"\{([^}:]+)(:[^}]*)?\}")]
        private static partial Regex TypeScriptRouteRegex();
        [GeneratedRegex(@"\$\{([^}]+)\}")]
        private static partial Regex RouteMatchRegex();
    }
}