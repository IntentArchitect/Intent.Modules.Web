using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Metadata.Models;
using Intent.Modules.Common;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.TypeScript.Events;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.ProjectItemTemplate.Partial", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.Core.IndexHtml
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class IndexHtmlTemplate : IntentTemplateBase<object>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.Core.IndexHtmlTemplate";

        private List<ClientResourceConfigurationRequestEvent> _resourceRequests = [];

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public IndexHtmlTemplate(IOutputTarget outputTarget, object model = null) : base(TemplateId, outputTarget, model)
        {
            ExecutionContext.EventDispatcher.Subscribe<ClientResourceConfigurationRequestEvent>(HandleClientResourceRequest);
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new TemplateFileConfig(
                fileName: $"index",
                fileExtension: "html"
            );
        }

        public string Title => OutputTarget.ApplicationName().ToPascalCase();

        private void HandleClientResourceRequest(ClientResourceConfigurationRequestEvent @event)
        {
            _resourceRequests.Add(@event);
        }

    }
}