using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Metadata.Models;
using Intent.Modelers.UI.Api;
using Intent.Modules.Common;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.Types.Api;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.ProjectItemTemplate.Partial", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.Component.LayoutComponentStyle
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class LayoutComponentStyleTemplate : IntentTemplateBase<LayoutModel>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.Component.LayoutComponentStyleTemplate";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public LayoutComponentStyleTemplate(IOutputTarget outputTarget, LayoutModel model) : base(TemplateId, outputTarget, model)
        {
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
            return new TemplateFileConfig(
               fileName: $"{LayoutName.ToKebabCase()}.component",
               relativeLocation: $"{string.Join("/", Model.GetParentFolderNames().Select(f => f.ToKebabCase()))}/{LayoutName.ToKebabCase()}",
               fileExtension: "scss"
           );
        }

    }
}