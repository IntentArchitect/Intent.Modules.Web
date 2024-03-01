using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Metadata.Models;
using Intent.Modelers.Types.ServiceProxies.Api;
using Intent.Modelers.WebClient.Api;
using Intent.Modules.Common;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.Modules.Metadata.WebApi.Models;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.TypeScript.Templates.TypescriptTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Angular.ServiceProxies.Templates.Proxies.JsonResponse
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class JsonResponseTemplate : TypeScriptTemplateBase<object>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.ServiceProxies.Proxies.JsonResponse";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public JsonResponseTemplate(IOutputTarget outputTarget, object model = null) : base(TemplateId, outputTarget, model)
        {
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new TypeScriptFileConfig(
                className: $"JsonResponse",
                fileName: $"json-response"
            );
        }

        public override bool CanRunTemplate()
        {
            var proxyModels = ExecutionContext
                .MetadataManager
                .WebClient(ExecutionContext.GetApplicationConfig().Id)
                .GetServiceProxyModels();

            var proxyOperations = proxyModels
                .SelectMany(s => s.Operations)
                .ToArray();

            if (proxyOperations.Length > 0)
            {
                return proxyOperations
                    .Where(x => x.InternalElement?.MappedElement?.Element is not null)
                    .Select(x => HttpEndpointModelFactory.GetEndpoint((IElement)x.InternalElement.MappedElement.Element))
                    .Where(x => x is not null)
                    .Any(ServiceMetadataQueries.HasJsonWrappedReturnType);
            }

            return proxyModels
                .SelectMany(s => s.MappedService.Operations)
                .Select(x => HttpEndpointModelFactory.GetEndpoint(x.InternalElement))
                .Where(x => x is not null)
                .Any(ServiceMetadataQueries.HasJsonWrappedReturnType);
        }
    }
}