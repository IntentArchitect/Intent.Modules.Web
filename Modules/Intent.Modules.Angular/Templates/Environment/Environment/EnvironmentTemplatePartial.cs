using Intent.Engine;
using Intent.Modules.Common;
using Intent.Modules.Common.Html.Templates;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.TypeScript.Builder;
using Intent.Modules.Common.TypeScript.Events;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.TypeScript.Templates.TypescriptTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.Environment.Environment
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public partial class EnvironmentTemplate : EnvironmentTemplateBase, ITypescriptFileBuilderTemplate
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.Environment.Environment";

        private readonly IList<ConfigurationVariableRequiredEvent> _environmentVariables = new List<ConfigurationVariableRequiredEvent>();

        [IntentManaged(Mode.Merge, Signature = Mode.Fully, Body = Mode.Ignore)]
        public EnvironmentTemplate(IOutputTarget outputTarget, object model = null) : base(TemplateId, outputTarget, model)
        {
            TypescriptFile = new TypescriptFile(this.GetFolderPath(), this);

            ExecutionContext.EventDispatcher.Subscribe<ConfigurationVariableRequiredEvent>(HandleConfigVariableRequiredEvent);
        }

        [IntentManaged(Mode.Fully)]
        public TypescriptFile TypescriptFile { get; }

        [IntentManaged(Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return TypescriptFile.GetConfig($"Environment");
        }

        [IntentManaged(Mode.Fully)]
        public override string TransformText()
        {
            return TypescriptFile.ToString();
        }

        public void HandleConfigVariableRequiredEvent(ConfigurationVariableRequiredEvent @event)
        {
            _environmentVariables.Add(@event);
        }

        public override string RunTemplate()
        {
            return GenerateFile();
        }
    }
}