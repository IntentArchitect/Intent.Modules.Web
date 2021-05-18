using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Eventing;
using Intent.Metadata.Models;
using Intent.Modules.Common;
using Intent.Modules.Common.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.Modules.Angular.Api;
using Intent.Modules.Common.Configuration;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.TypeScript.Templates.TypescriptTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.Core.ApiServiceTemplate
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class ApiServiceTemplate
        : TypeScriptTemplateBase<object>
    {
        private string _sslPort = "{api_port}";

        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.Core.ApiServiceTemplate";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public ApiServiceTemplate(IOutputTarget outputTarget, object model = null) : base(TemplateId, outputTarget, model)
        {
            ExecutionContext.EventDispatcher.Subscribe<HostingSettingsCreatedEvent>(Handle);
        }

        private void Handle(HostingSettingsCreatedEvent @event)
        {
            _sslPort = @event.SslPort.ToString();
        }

        public override void BeforeTemplateExecution()
        {
            ExecutionContext.EventDispatcher.Publish(new AngularConfigVariableRequiredEvent("api_base_url", $"\"https://localhost:{_sslPort}\""));
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new TypeScriptFileConfig(
                overwriteBehaviour: OverwriteBehaviour.Always,
                fileName: "api.service",
                relativeLocation: "",
                className: "ApiService"
            );
        }


    }
}