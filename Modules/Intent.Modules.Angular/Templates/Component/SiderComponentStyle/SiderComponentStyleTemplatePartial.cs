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

namespace Intent.Modules.Angular.Templates.Component.SiderComponentStyle
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class SiderComponentStyleTemplate : IntentTemplateBase<LayoutSiderModel>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.Component.SiderComponentStyleTemplate";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public SiderComponentStyleTemplate(IOutputTarget outputTarget, LayoutSiderModel model) : base(TemplateId, outputTarget, model)
        {
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return this.GetLayoutItemStyleFileConfig();
        }

    }
}