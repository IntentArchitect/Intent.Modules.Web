using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Metadata.Models;
using Intent.Modelers.WebClient.Angular.Api;
using Intent.Modules.Angular.Templates.Component.AngularComponentTs;
using Intent.Modules.Angular.Templates.Module.AngularModule;
using Intent.Modules.Angular.Templates.Shared.IntentDecorators;
using Intent.Modules.Common;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.Types.Api;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.TypeScript.Templates.TypescriptTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.Module.AngularRoutingModule
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class AngularRoutingModuleTemplate : TypeScriptTemplateBase<Intent.Modelers.WebClient.Angular.Api.RoutingModel>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.Module.AngularRoutingModule";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public AngularRoutingModuleTemplate(IOutputTarget outputTarget, Intent.Modelers.WebClient.Angular.Api.RoutingModel model) : base(TemplateId, outputTarget, model)
        {
            AddTypeSource(AngularComponentTsTemplate.TemplateId);
            AddTemplateDependency(IntentDecoratorsTemplate.TemplateId);
        }

        public string ModuleName => Model.Module.GetModuleName() + "Routing";

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new TypeScriptFileConfig(
                overwriteBehaviour: OverwriteBehaviour.Always,
                fileName: $"{ModuleName.ToKebabCase()}.module",
                relativeLocation: $"{string.Join("/", Model.Module.GetParentFolderNames().Concat(new[] { Model.Module.GetModuleName().ToKebabCase() }))}",
                className: $"{ModuleName}Module"
            );
        }

        public override void BeforeTemplateExecution()
        {
            base.BeforeTemplateExecution();
            if (!Model.Module.IsRootModule())
            {
                return;
            }

            foreach (var route in Model.Routes.Where(x => x.RoutesToModule))
            {
                ExecutionContext.EventDispatcher.Publish(new AngularAppRouteCreatedEvent(
                    moduleName: new ModuleModel((IElement)route.TypeReference.Element).GetModuleName(),
                    route: GetRoute(route)));
            }
        }

        private string GetRoute(RouteModel route)
        {
            return route.Name;
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

        private string GetNgModuleImports()
        {
            return Model.Module.IsRootModule()
                ? "RouterModule.forRoot(routes)"
                : "RouterModule.forChild(routes)";
        }
    }
}