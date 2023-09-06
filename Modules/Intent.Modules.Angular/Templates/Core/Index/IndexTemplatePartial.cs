using System.Collections.Generic;
using Intent.Engine;
using Intent.Modules.Common;
using Intent.Modules.Common.Html.Templates;
using Intent.Modules.Common.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Html.Templates.HtmlFileTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.Core.Index
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class IndexTemplate : HtmlTemplateBase<object>
    {
        private readonly List<IndexHeaderLinkRequired> _headerLinks = new();

        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.Core.Index";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public IndexTemplate(IOutputTarget outputTarget, object model = null) : base(TemplateId, outputTarget, model)
        {
            ExecutionContext.EventDispatcher.Subscribe<IndexHeaderLinkRequired>(_headerLinks.Add);
        }

        public string Title => OutputTarget.ApplicationName().ToPascalCase();

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new HtmlFileConfig(
                fileName: "index",
                fileExtension: "html",
                relativeLocation: ""
            );
        }

    }
}