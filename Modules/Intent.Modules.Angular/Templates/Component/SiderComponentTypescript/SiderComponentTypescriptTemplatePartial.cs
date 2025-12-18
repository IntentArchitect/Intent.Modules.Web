using Intent.Engine;
using Intent.Modelers.UI.Api;
using Intent.Modules.Angular.Templates.Shared.IntentDecorators;
using Intent.Modules.Common;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.TypeScript.Builder;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;
using System;
using System.Collections.Generic;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.TypeScript.Templates.TypescriptTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.Component.SiderComponentTypescript
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public partial class SiderComponentTypescriptTemplate : TypeScriptTemplateBase<Intent.Modelers.UI.Api.LayoutSiderModel>, ITypescriptFileBuilderTemplate
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.Component.SiderComponentTypescript";

        [IntentManaged(Mode.Ignore, Signature = Mode.Fully)]
        public SiderComponentTypescriptTemplate(IOutputTarget outputTarget, Intent.Modelers.UI.Api.LayoutSiderModel model) : base(TemplateId, outputTarget, model)
        {
            AddImport("Component", "@angular/core");

            TypescriptFile = new TypescriptFile(this.GetFolderPath(), this)
                .AddClass($"{this.GetLayoutItemClassName()}", @class =>
                {
                    @class.Export();
                    @class.AddDecorator("IntentMerge");
                    @class.AddDecorator("Component", component =>
                    {
                        // TODO Clean this up
                        var obj = new TypescriptVariableObject
                        {
                            Indentation = TypescriptFile.Indentation
                        };
                        obj.AddField("selector", $"'{this.GetLayoutItemSelector()}'");
                        obj.AddField("standalone", "true");
                        obj.AddField("templateUrl", $"'{this.GetRootFilename()}.html'");
                        obj.AddField("styleUrls", $"['{this.GetRootFilename()}.scss']");

                        component.AddArgument(obj.GetText(""));
                    });
                }).AfterBuild(file =>
                {
                    var intentDecoratorTemplate = GetTemplate<TypeScriptTemplateBase<object>>(IntentDecoratorsTemplate.TemplateId);
                    file.AddImport("IntentIgnoreBody", this.GetRelativePath(intentDecoratorTemplate));
                    file.AddImport("IntentMerge", this.GetRelativePath(intentDecoratorTemplate));
                });
        }

        [IntentManaged(Mode.Fully)]
        public TypescriptFile TypescriptFile { get; }

        [IntentManaged(Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return this.GetLayoutItemTypescriptFileConfig();
        }

        [IntentManaged(Mode.Fully)]
        public override string TransformText()
        {
            return TypescriptFile.ToString();
        }
    }
}