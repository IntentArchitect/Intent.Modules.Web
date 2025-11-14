using Intent.Engine;
using Intent.Modelers.UI.Api;
using Intent.Modules.Common.Html.Templates;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.Types.Api;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;
using System;
using System.Collections.Generic;
using System.Linq;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Html.Templates.HtmlFileTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.Component.LayoutCompeonentHtml
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class LayoutCompeonentHtmlTemplate : HtmlTemplateBase<Intent.Modelers.UI.Api.LayoutModel>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.Component.LayoutCompeonentHtml";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public LayoutCompeonentHtmlTemplate(IOutputTarget outputTarget, Intent.Modelers.UI.Api.LayoutModel model) : base(TemplateId, outputTarget, model)
        {
            foreach (var item in model.Sider.InternalElement.ChildElements)
            {
            }
        }

        public string LayoutName
        {
            get
            {
                if (Model.Name.EndsWith("Layout", StringComparison.InvariantCultureIgnoreCase))
                {
                    return Model.Name.Substring(0, Model.Name.Length - "Layout".Length);
                }
                return Model.Name;
            }
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new HtmlFileConfig(
                fileName: $"{LayoutName.ToKebabCase()}.component",
                relativeLocation: $"{string.Join("/", Model.GetParentFolderNames().Select(f => f.ToKebabCase()))}/{LayoutName.ToKebabCase()}"
            );
        }

    }
}