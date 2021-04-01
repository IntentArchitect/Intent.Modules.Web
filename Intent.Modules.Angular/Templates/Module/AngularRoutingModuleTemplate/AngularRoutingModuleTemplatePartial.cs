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
using Intent.Modelers.WebClient.Angular.Api;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.TypeScript.Templates.TypescriptTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.Module.AngularRoutingModuleTemplate
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class AngularRoutingModuleTemplate : TypeScriptTemplateBase<Intent.Modelers.WebClient.Angular.Api.RoutingModel>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.Module.AngularRoutingModuleTemplate";

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
                relativeLocation: $"{ Model.Module.GetModuleName().ToKebabCase() }",
                className: "${ModuleName}"
            );
        }
    }
}