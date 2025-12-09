using Intent.Engine;
using Intent.Modules.Common;
using Intent.Modules.Common.Html.Templates;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.TypeScript.Events;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;
using System.Collections.Generic;
using System.Text;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Html.Templates.HtmlFileTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.Core.IndexHtmlFile
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class IndexHtmlFileTemplate : HtmlTemplateBase<object>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.Core.IndexHtmlFile";

        private readonly List<ClientResourceConfigurationRequestEvent> _clientResources = new();

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public IndexHtmlFileTemplate(IOutputTarget outputTarget, object model = null) : base(TemplateId, outputTarget, model)
        {
            ExecutionContext.EventDispatcher.Subscribe<ClientResourceConfigurationRequestEvent>(HandleServiceConfigurationRequest);
        }

        public string Title => OutputTarget.ApplicationName().ToPascalCase();

        [IntentManaged(Mode.Fully, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new HtmlFileConfig(
                fileName: $"Index",
                relativeLocation: "",
                overwriteBehaviour: OverwriteBehaviour.Always
            );
        }

        private void HandleServiceConfigurationRequest(ClientResourceConfigurationRequestEvent @event)
        {
            _clientResources.Add(@event);
        }

        private string RenderResources()
        {
            var sb = new StringBuilder();
            foreach (var resource in _clientResources)
            {
                sb.AppendLine($"  <link rel=\"{resource.RelationshipType}\" href=\"{resource.ResourceValue}\">");
            }
            return sb.ToString();

        }
    }
}