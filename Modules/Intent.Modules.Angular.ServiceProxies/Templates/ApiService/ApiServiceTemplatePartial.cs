using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using Intent.Engine;
using Intent.Modules.Angular.Shared;
using Intent.Modules.Common;
using Intent.Modules.Common.Configuration;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.TypeScript.Templates.TypescriptTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Angular.ServiceProxies.Templates.ApiService
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class ApiServiceTemplate : TypeScriptTemplateBase<object>
    {
        private string _sslPort = "{api_port}";

        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.ServiceProxies.ApiService";

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
            ExecutionContext.EventDispatcher.PublishAngularConfigVariableRequiredEvent("api_base_url", $"\"https://localhost:{_sslPort}\"");
        }

        public string EnvironmentTypeName => GetTypeName("Intent.Angular.Environment.Environment");

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
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