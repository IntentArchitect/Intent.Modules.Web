using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Modelers.Types.ServiceProxies.Api;
using Intent.Modelers.WebClient.Api;
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
            return ExecutionContext
                .MetadataManager
                .WebClient(ExecutionContext.GetApplicationConfig().Id)
                .GetServiceProxyModels()
                .SelectMany(s => s.Operations)
                .Select(x => HttpEndpointModelFactory.GetEndpoint(x.InternalElement))
                .Where(x => x != null)
                .Any(ServiceMetadataQueries.HasJsonWrappedReturnType);
        }
    }
}