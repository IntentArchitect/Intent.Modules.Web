using System.Collections.Generic;
using Intent.Engine;
using Intent.Modules.Common.Html.Templates;
using Intent.Modules.Common.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Html.Templates.HtmlFileTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.Core.AppComponentHtml
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class AppComponentHtmlTemplate : HtmlTemplateBase<object>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.Core.AppComponentHtml";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public AppComponentHtmlTemplate(IOutputTarget outputTarget, object model = null) : base(TemplateId, outputTarget, model)
        {
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new HtmlFileConfig(
                fileName: $"app.component",
                relativeLocation: ""
            );
        }

    }
}