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

namespace Intent.Modules.Angular.Templates.Core.AppComponent
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public partial class AppComponentTemplate : TypeScriptTemplateBase<object>, ITypescriptFileBuilderTemplate
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.Core.AppComponent";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully, Body = Mode.Ignore)]
        public AppComponentTemplate(IOutputTarget outputTarget, object model = null) : base(TemplateId, outputTarget, model)
        {
            TypescriptFile = new TypescriptFile(this.GetFolderPath(), this)
                .AddImport("Component", "@angular/core")
                .AddImport("RouterOutlet", "@angular/router")
                .AddClass($"AppComponent", @class =>
                {
                    @class.Export();
                    @class.AddField("title", @field =>
                    {
                        field.WithValue($"\"{outputTarget.ApplicationName()}\"");
                    });

                    @class.AddDecorator("@Component", component =>
                    {
                        var obj = new TypescriptVariableObject();
                        obj.AddField("selector", "'app-root'");
                        obj.AddField("imports", "[RouterOutlet]");
                        obj.AddField("standalone", "true");
                        obj.AddField("templateUrl", "'./app.component.html'");
                        obj.AddField("styleUrls", "['./app.component.scss']");

                        component.AddArgument(obj.GetText("  "));
                    });
                });
        }

        [IntentManaged(Mode.Fully)]
        public TypescriptFile TypescriptFile { get; }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new TemplateFileConfig(
                fileName: $"app.component",
                fileExtension: "ts"
            );
        }

        [IntentManaged(Mode.Fully)]
        public override string TransformText()
        {
            return TypescriptFile.ToString();
        }
    }
}