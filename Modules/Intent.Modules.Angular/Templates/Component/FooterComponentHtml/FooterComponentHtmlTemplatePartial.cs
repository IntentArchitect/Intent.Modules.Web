using Intent.Engine;
using Intent.Modelers.UI.Api;
using Intent.Modules.Common.Html.Templates;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.Types.Api;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;
using System;
using System.Collections.Generic;
using System.Linq;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Html.Templates.HtmlFileTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.Component.FooterComponentHtml
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class FooterComponentHtmlTemplate : HtmlTemplateBase<Intent.Modelers.UI.Api.LayoutFooterModel>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.Component.FooterComponentHtml";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public FooterComponentHtmlTemplate(IOutputTarget outputTarget, Intent.Modelers.UI.Api.LayoutFooterModel model) : base(TemplateId, outputTarget, model)
        {
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return this.GetLayoutItemHtmlFileConfig();
        }

    }
}