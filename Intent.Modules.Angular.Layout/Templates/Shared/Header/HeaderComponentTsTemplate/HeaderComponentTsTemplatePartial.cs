using System.Collections.Generic;
using System.IO;
using Intent.Engine;
using Intent.Modules.Angular.Templates;
using Intent.Modules.Angular.Templates.Shared.IntentDecoratorsTemplate;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;


[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.TypeScript.Templates.TypescriptTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Angular.Layout.Templates.Shared.Header.HeaderComponentTsTemplate
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class HeaderComponentTsTemplate : TypeScriptTemplateBase<object>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.Layout.Shared.Header.HeaderComponentTsTemplate";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public HeaderComponentTsTemplate(IOutputTarget outputTarget, object model = null) : base(TemplateId, outputTarget, model)
        {
            AddTemplateDependency(IntentDecoratorsTemplate.TemplateId);
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new TypeScriptFileConfig(
                overwriteBehaviour: OverwriteBehaviour.Always,
                fileName: "header.component",
                relativeLocation: "",
                className: "HeaderComponent"
            );
        }

        public override void BeforeTemplateExecution()
        {
            //if (File.Exists(GetMetadata().GetFilePath()))
            //{
            //    return;
            //}

            // New Component:
            ExecutionContext.EventDispatcher.Publish(new AngularComponentCreatedEvent(modelId: TemplateId, moduleId: "AppModule"));

            ExecutionContext.EventDispatcher.Publish(new AngularImportDependencyRequiredEvent(
                moduleId: "AppModule", 
                dependency: "CollapseModule.forRoot()", 
                import: "import { CollapseModule } from 'ngx-bootstrap/collapse';"));
        }
    }
}