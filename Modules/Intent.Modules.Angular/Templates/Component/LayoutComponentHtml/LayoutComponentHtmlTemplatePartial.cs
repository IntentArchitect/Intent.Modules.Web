using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Intent.Engine;
using Intent.Modelers.UI.Api;
using Intent.Modules.Common;
using Intent.Modules.Common.Html.Templates;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.Types.Api;
using Intent.Modules.Common.TypeScript.Builder;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Html.Templates.HtmlFileTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.Component.LayoutComponentHtml
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class LayoutComponentHtmlTemplate : HtmlTemplateBase<Intent.Modelers.UI.Api.LayoutModel>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.Component.LayoutComponentHtml";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public LayoutComponentHtmlTemplate(IOutputTarget outputTarget, Intent.Modelers.UI.Api.LayoutModel model) : base(TemplateId, outputTarget, model)
        {

        }

        public string LayoutName
        {
            get
            {
                if (Model.Name.EndsWith("Layout", StringComparison.InvariantCultureIgnoreCase))
                {
                    return Model.Name[..^"Layout".Length];
                }
                return Model.Name;
            }
        }

        public string BuildLayoutHtml()
        {
            var sb = new StringBuilder();

            sb.AppendLine(GetHeaderText());
            sb.AppendLine("<div class=\"app-layout\">");
            sb.AppendLine($"  {GetSiderText()}");
            sb.AppendLine("  <main class=\"app-content\">");
            sb.AppendLine("    <router-outlet></router-outlet>");
            sb.AppendLine("  </main>");
            sb.AppendLine(GetFooterText());
            sb.AppendLine("</div>");

            return sb.ToString();
        }

        private string GetSiderText()
        {
            if (Model.Sider is not null)
            {
                return $"<{Model.Sider.Name.ToKebabCase()}></{Model.Sider.Name.ToKebabCase()}>";
            }

            return string.Empty;
        }

        private string GetHeaderText()
        {
            if (Model.Header is not null)
            {
                return $"<{Model.Header.Name.ToKebabCase()}></{Model.Header.Name.ToKebabCase()}>";
            }

            return string.Empty;
        }

        private string GetFooterText()
        {
            if (Model.Header is not null)
            {
                return $"<{Model.Footer.Name.ToKebabCase()}></{Model.Footer.Name.ToKebabCase()}>";
            }

            return string.Empty;
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