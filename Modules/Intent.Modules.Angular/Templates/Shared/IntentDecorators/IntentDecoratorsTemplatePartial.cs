using System;
using System.Collections.Generic;
using Intent.Engine;
using Intent.Modules.Common;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.TypeScript;
using Intent.Modules.Common.TypeScript.Builder;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.TypeScript.Templates.TypescriptTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.Shared.IntentDecorators
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public partial class IntentDecoratorsTemplate : TypeScriptTemplateBase<object>, ITypescriptFileBuilderTemplate
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.Shared.IntentDecorators";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully, Body = Mode.Ignore)]
        public IntentDecoratorsTemplate(IOutputTarget outputTarget, object model = null) : base(TemplateId, outputTarget, model)
        {
            TypescriptFile = new TypescriptFile(this.GetFolderPath());

            AddDecoratorAndInterface(TypescriptFile, IntentIgnore);
            AddDecoratorAndInterface(TypescriptFile, IntentIgnoreBody);
            AddDecoratorAndInterface(TypescriptFile, IntentMerge);
            AddDecoratorAndInterface(TypescriptFile, IntentManage);
            AddDecoratorAndInterface(TypescriptFile, IntentManageClass);
        }

        private TypescriptFile AddDecoratorAndInterface(TypescriptFile file, string decoratorName)
        {
            file.AddVariable(decoratorName, intentIgnore =>
             {
                 intentIgnore.WithExpressionFunctionValue(exp =>
                 {
                     exp.AddParameter("identifier?", "string");

                     var innerExp = new TypeScriptVariableExpressionFunction()
                         .AddParameter("target", "any")
                         .AddParameter("propertyKey?", "string")
                         .AddParameter("descriptor?", "any");

                     exp.AddStatement($"return {innerExp.GetText(file.Indentation)}");
                 });

                 intentIgnore.Export().Const();
             })
            .AddInterface(decoratorName, @interface => @interface.Export());

            return file;
        }

        public string IntentIgnore => "IntentIgnore";
        public string IntentIgnoreBody => "IntentIgnoreBody";
        public string IntentManage => "IntentManage";
        public string IntentMerge => "IntentMerge";
        public string IntentManageClass => "IntentManageClass";

        [IntentManaged(Mode.Fully)]
        public TypescriptFile TypescriptFile { get; }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new TypeScriptFileConfig(
                overwriteBehaviour: OverwriteBehaviour.Always,
                fileName: "intent.decorators",
                relativeLocation: "",
                className: IntentIgnore
            );
        }

        [IntentManaged(Mode.Fully)]
        public override string TransformText()
        {
            return TypescriptFile.ToString();
        }
    }
}