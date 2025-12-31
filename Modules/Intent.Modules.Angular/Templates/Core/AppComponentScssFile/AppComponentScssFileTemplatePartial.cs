using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Metadata.Models;
using Intent.Modelers.UI.Api;
using Intent.Modules.Common;
using Intent.Modules.Common.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.ProjectItemTemplate.Partial", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.Core.AppComponentScssFile
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class AppComponentScssFileTemplate : IntentTemplateBase<object>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.Core.AppComponentScssFileTemplate";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public AppComponentScssFileTemplate(IOutputTarget outputTarget, object model = null) : base(TemplateId, outputTarget, model)
        {
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new TemplateFileConfig(
                fileName: $"app.component",
                fileExtension: "scss"
            );
        }

        public override bool CanRunTemplate()
        {
            // This template is only needed if there are no layouts defined in the UI modeller
            return base.CanRunTemplate() &&
                !ExecutionContext.MetadataManager.UserInterface(ExecutionContext.GetApplicationConfig().Id).GetLayoutModels().Any();
        }
    }
}