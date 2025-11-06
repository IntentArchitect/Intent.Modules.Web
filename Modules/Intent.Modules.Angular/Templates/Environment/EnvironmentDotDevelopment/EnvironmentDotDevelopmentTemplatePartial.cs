using System;
using System.Collections.Generic;
using Intent.Engine;
using Intent.Modules.Common;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.TypeScript.Builder;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.TypeScript.Templates.TypescriptTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.Environment.EnvironmentDotDevelopment
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public partial class EnvironmentDotDevelopmentTemplate : TypeScriptTemplateBase<object>, ITypescriptFileBuilderTemplate
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.Environment.EnvironmentDotDevelopment";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully, Body = Mode.Ignore)]
        public EnvironmentDotDevelopmentTemplate(IOutputTarget outputTarget, object model = null) : base(TemplateId, outputTarget, model)
        {
            TypescriptFile = new TypescriptFile(this.GetFolderPath())
               .AddVariable("environment", @var =>
               {
                   @var.Const();
                   @var.Export();
                   @var.WithComments("//@IntentCanAdd()");
               });
        }

        [IntentManaged(Mode.Fully)]
        public TypescriptFile TypescriptFile { get; }

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

        [IntentManaged(Mode.Fully)]
        public override string TransformText()
        {
            return TypescriptFile.ToString();
        }
    }
}