using Intent.Engine;
using Intent.Modules.Angular.HttpClients.Templates.PagedResult;
using Intent.Modules.Angular.ServiceProxies.Templates.Proxies.PagedResult;
using Intent.Modules.Common;
using Intent.Modules.Common.Plugins;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.Plugins.FactoryExtensions;
using Intent.RoslynWeaver.Attributes;
using System.Linq;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.FactoryExtension", Version = "1.0")]

namespace Intent.Modules.Angular.HttpClients.FactoryExtensions
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class PagedResultTypeSourceInstaller : FactoryExtensionBase
    {
        public override string Id => "Intent.Angular.HttpClients.PagedResultTypeSourceInstaller";

        [IntentManaged(Mode.Ignore)]
        public override int Order => 0;

        protected override void OnAfterTemplateRegistrations(IApplication application)
        {
            var pagedResultTemplate = application.FindTemplateInstance<PagedResultTemplate>(PagedResultTemplate.TemplateId);
            if (pagedResultTemplate == null)
            {
                return;
            }

            // This will ensure that all template instances will know about our new type
            // and will resolve the namespace correctly. However, this is not the ideal
            // method and will be relooked in the future using some "Global Type Source"
            // idea.

            // GCB REPLY TO ABOVE COMMENT:
            // This is not ideal. See what I did for the Angular Http Clients that also need
            // to resolve a paged result. It uses our standard type resolution mechanisms (which are far more
            // intuitive)
            var templates = pagedResultTemplate?.ExecutionContext
                .OutputTargets
                .SelectMany(s => s.TemplateInstances)
                .OfType<TypeScriptTemplateBase<Intent.Modelers.UI.Api.ComponentModel>>()
                .Where(p => p.HasTypeResolver())
                .ToArray();
            if (templates != null)
            {
                foreach (var template in templates)
                {
                    if (template != null)
                    {
                        PagedResultTypeSource.ApplyTo(template, PagedResultTemplate.TemplateId);
                        //template.AddTypeSource(new PagedResultTypeSource(template, PagedResultTemplate.temp));
                    }
                }
            }
        }
    }
}