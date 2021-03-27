using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Modules.Angular.Templates.Module.AngularModuleTemplate;
using Intent.Modules.Common;
using Intent.Modules.Common.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;
using Intent.Metadata.Models;
using System;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.Angular.Api;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.TypeScript.Templates.TypescriptTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.App.AppRoutingModuleTemplate
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class AppRoutingModuleTemplate : TypeScriptTemplateBase<Intent.Angular.Api.AngularWebAppModel>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.App.AppRoutingModuleTemplate";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public AppRoutingModuleTemplate(IOutputTarget outputTarget, Intent.Angular.Api.AngularWebAppModel model) : base(TemplateId, outputTarget, model)
        {
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new TypeScriptFileConfig(
                className: "AppRoutingModule",
                fileName: $"app-routing.module"
            );
        }

        public IList<RedirectModel> Redirects => Model.Routing?.Redirects ?? new List<RedirectModel>();
        public IList<RouteModel> Routes => Model.Routing?.Routes ?? new List<RouteModel>();

        public override void BeforeTemplateExecution()
        {
            foreach (var route in Routes.Where(x => x.RoutesToModule))
            {
                ExecutionContext.EventDispatcher.Publish(new AngularAppRouteCreatedEvent(
                    moduleName: new ModuleModel((IElement)route.TypeReference.Element).GetModuleName(),
                    route: GetRoute(route)));
            }
        }

        private string GetModulePath(RouteModel route)
        {
            var template = GetTemplate<ITemplate>(AngularModuleTemplate.TemplateId, route.TypeReference.Element, new TemplateDiscoveryOptions() { TrackDependency = false });
            return GetMetadata().GetFullLocationPath().GetRelativePath(template.GetMetadata().GetFilePathWithoutExtension()).Normalize();
        }

        private string GetModuleClassName(RouteModel route)
        {
            return GetTypeName(AngularModuleTemplate.TemplateId, route.TypeReference.Element, new TemplateDiscoveryOptions() { TrackDependency = false });
        }

        private string GetRoute(RouteModel route)
        {
            return route.Name;
        }
    }
}