using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Intent.Engine;
using Intent.Metadata.Models;
using Intent.Modelers.UI.Api;
using Intent.Modules.Common;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.Types.Api;
using Intent.Modules.Common.TypeScript.Builder;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.ProjectItemTemplate.Partial", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.Component.LayoutComponentHtml
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class LayoutComponentHtmlTemplate : IntentTemplateBase<LayoutModel>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.Component.LayoutComponentHtmlTemplate";

        private string _content = string.Empty;

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public LayoutComponentHtmlTemplate(IOutputTarget outputTarget, LayoutModel model) : base(TemplateId, outputTarget, model)
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

        public void SetContent(string content)
        {
            _content = content;
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new TemplateFileConfig(
                fileName: $"{LayoutName.ToKebabCase()}.component",
                fileExtension: "html",
                relativeLocation: $"{string.Join("/", Model.GetParentFolderNames().Select(f => f.ToKebabCase()))}/{LayoutName.ToKebabCase()}",
                overwriteBehaviour: OverwriteBehaviour.OverwriteDisabled
            );
        }

    }
}