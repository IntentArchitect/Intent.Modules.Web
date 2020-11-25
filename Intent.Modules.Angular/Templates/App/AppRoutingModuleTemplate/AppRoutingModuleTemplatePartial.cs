using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Modules.Angular.Api;
using Intent.Modules.Angular.Templates.Module.AngularModuleTemplate;
using Intent.Modules.Common;
using Intent.Modules.Common.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;
using Intent.Metadata.Models;
using System;
using Intent.Modules.Angular.Templates.Shared.IntentDecoratorsTemplate;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.Modelers.WebClient.Api;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("ModuleBuilder.TypeScript.Templates.TypescriptTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.App.AppRoutingModuleTemplate
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class AppRoutingModuleTemplate : TypeScriptTemplateBase<AngularWebAppModel>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Angular.App.AppRoutingModuleTemplate";

        public AppRoutingModuleTemplate(IOutputTarget project, AngularWebAppModel model) : base(TemplateId, project, model)
        {
            AddTemplateDependency(IntentDecoratorsTemplate.TemplateId);
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