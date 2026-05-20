using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Metadata.Models;
using Intent.Modelers.UI.Api;
using Intent.Modules.Common;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.Types.Api;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.ProjectItemTemplate.Partial", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.Component.ComponentHtml
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class ComponentHtmlTemplate : IntentTemplateBase<ComponentModel>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.Component.ComponentHtmlTemplate";

        private string _content = string.Empty;

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public ComponentHtmlTemplate(IOutputTarget outputTarget, ComponentModel model) : base(TemplateId, outputTarget, model)
        {
        }

        public string ComponentName
        {
            get
            {
                if (Model.Name.EndsWith("Component", StringComparison.InvariantCultureIgnoreCase))
                {
                    return Model.Name.Substring(0, Model.Name.Length - "Component".Length);
                }
                return Model.Name;
            }
        }

        public void SetContent(string content)
        {
            _content = content;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new TemplateFileConfig(
                fileName: $"{ComponentName.ToKebabCase()}.component",
                relativeLocation: $"{string.Join("/", Model.GetParentFolderNames().Select(f => f.ToKebabCase()))}/{ComponentName.ToKebabCase()}",
                fileExtension: "html",
                overwriteBehaviour: OverwriteBehaviour.OverwriteDisabled
            )
            .WithAISummary("Implementation of the html for the " + Model.Name + " component/page.")
            .WithAIContext(
                """
                - Search for and use any appropriate skills when implementing or modiying this file. If you find a qualifying skill, you MUST follow its instructions, unless specifically instructed otherwise.
                - Treat 'use an applicable skill' as a hard requirement; do not proceed with implementation until you’ve either used a skill or explicitly confirmed no skill applies.
                """);
        }
    }
}