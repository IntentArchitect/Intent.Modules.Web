using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Modules.Angular.Shared;
using Intent.Modules.Common;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.TypeScript.Templates.TypescriptTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.Environment.EnvironmentDotDevelopment
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class EnvironmentDotDevelopmentTemplate : TypeScriptTemplateBase<object>
    {
        private readonly IList<ConfigVariable> _configVariables = new List<ConfigVariable>();

        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.Environment.EnvironmentDotDevelopment";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public EnvironmentDotDevelopmentTemplate(IOutputTarget outputTarget, object model = null) : base(TemplateId, outputTarget, model)
        {
            ExecutionContext.EventDispatcher.SubscribeToAngularConfigVariableRequiredEvent(HandleConfigVariableRequiredEvent);
        }

        private void HandleConfigVariableRequiredEvent(string variableKey, string defaultValue)
        {
            _configVariables.Add(new ConfigVariable(
                name: variableKey,
                defaultValue: defaultValue));
        }

        public string GetEnvironmentVariables()
        {
            return string.Join(@",
  ", _configVariables.Select(x => $"{x.Name}: {x.DefaultValue}"));
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new TemplateFileConfig(
                overwriteBehaviour: OverwriteBehaviour.Always,
                codeGenType: CodeGenType.Basic,
                fileName: "environment.development",
                fileExtension: "ts",
                relativeLocation: ""
            );
        }
    }
}