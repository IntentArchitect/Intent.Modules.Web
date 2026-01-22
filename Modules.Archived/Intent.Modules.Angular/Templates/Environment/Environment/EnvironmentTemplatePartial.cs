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

namespace Intent.Modules.Angular.Templates.Environment.Environment
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class EnvironmentTemplate : TypeScriptTemplateBase<object>
    {
        private readonly IList<ConfigVariable> _configVariables = new List<ConfigVariable>();

        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.Environment.Environment";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public EnvironmentTemplate(IOutputTarget outputTarget, object model = null) : base(TemplateId, outputTarget, model)
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

        //      protected override TypeScriptFile CreateOutputFile()
        //      {
        //          var file = GetExistingFile() ?? base.CreateOutputFile();
        //          var variable = file.Children.First(x => x.Identifier == "environment");

        //          foreach (var configVariable in _configVariables)
        //          {
        //              var assigned = variable.Children.FirstOrDefault();//.GetAssignedValue<TypescriptObjectLiteralExpression>();
        //              if (assigned != null && assigned.Children.All(x => x.Identifier != configVariable.Name))
        //              {
        //                  assigned.InsertAfter(assigned.Children.Last(), $@"
        //{configVariable.Name}: {configVariable.DefaultValue}");
        //              }
        //          }

        //          return file;
        //      }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new TemplateFileConfig(
                overwriteBehaviour: OverwriteBehaviour.Always,
                codeGenType: CodeGenType.Basic,
                fileName: "environment",
                fileExtension: "ts", // Change to desired file extension.
                relativeLocation: ""
            );
        }
    }
}