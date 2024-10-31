using System;
using System.Collections.Generic;
using Intent.Engine;
using Intent.Modules.Angular.Shared;
using Intent.Modules.Common;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.TypeScript.Templates.TypescriptTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Angular.ServiceProxies.Templates.Environment
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class EnvironmentTemplate : TypeScriptTemplateBase<object>
    {
        private readonly List<(string Key, string DefaultValue)> _configVariables = [];

        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.ServiceProxies.Environment";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public EnvironmentTemplate(IOutputTarget outputTarget, object model = null) : base(TemplateId, outputTarget, model)
        {
            FulfillsRole("Intent.Angular.Environment.Environment");
            ExecutionContext.EventDispatcher.SubscribeToAngularConfigVariableRequiredEvent(Handle);
        }

        private void Handle(string variableKey, string defaultValue)
        {
            _configVariables.Add((variableKey, defaultValue));
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new TypeScriptFileConfig(
                className: $"environment",
                fileName: $"environment"
            );
        }
    }
}