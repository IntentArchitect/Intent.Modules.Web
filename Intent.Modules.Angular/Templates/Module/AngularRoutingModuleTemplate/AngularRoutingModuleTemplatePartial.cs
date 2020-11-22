using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Metadata.Models;
using Intent.Modules.Angular.Api;
using Intent.Modules.Angular.Templates.Component.AngularComponentTsTemplate;
using Intent.Modules.Angular.Templates.Module.AngularModuleTemplate;
using Intent.Modules.Angular.Templates.Shared.IntentDecoratorsTemplate;
using Intent.Modules.Common;
using Intent.Modules.Common.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;
using Intent.Modules.Common.TypeScript.Templates;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("ModuleBuilder.TypeScript.Templates.TypescriptTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.Module.AngularRoutingModuleTemplate
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class AngularRoutingModuleTemplate : TypeScriptTemplateBase<RoutingModel>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Angular.Module.AngularRoutingModuleTemplate";

        public AngularRoutingModuleTemplate(IOutputTarget project, RoutingModel model) : base(TemplateId, project, model)
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
                relativeLocation: $"{ Model.Module.GetModuleName().ToKebabCase() }",
                className: "${ModuleName}"
            );
        }
    }
}