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

namespace Intent.Modules.Angular.Templates.Core.AngularRoutes
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public partial class AngularRoutesTemplate : TypeScriptTemplateBase<object>, ITypescriptFileBuilderTemplate
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.Core.AngularRoutes";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully, Body = Mode.Ignore)]
        public AngularRoutesTemplate(IOutputTarget outputTarget, object model = null) : base(TemplateId, outputTarget, model)
        {
            TypescriptFile = new TypescriptFile(this.GetFolderPath())
                .AddImport("Routes", "@angular/router")
                .AddVariable("routes", "Routes", @var =>
                {
                    var.Export().Const();
                    var.WithArrayValue(arr => { });

                    //var.WithArrayValue(arr =>
                    //{
                    //    arr.AddObject(obj =>
                    //    {
                    //        obj.AddProperty("one", "1");
                    //        obj.AddProperty("two", "2");
                    //    });
                    //    arr.AddObject(obj =>
                    //    {
                    //        obj.AddProperty("three", "3");
                    //        obj.AddProperty("four", "4");
                    //    });
                    //});
                });
        }

        [IntentManaged(Mode.Fully)]
        public TypescriptFile TypescriptFile { get; }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new TemplateFileConfig(
                fileName: $"app.routes",
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