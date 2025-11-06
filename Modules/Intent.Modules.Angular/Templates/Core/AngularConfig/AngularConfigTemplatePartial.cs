using System;
using System.Collections.Generic;
using Intent.Engine;
using Intent.Modules.Angular.Templates.Core.AngularRoutes;
using Intent.Modules.Common;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.TypeScript.Builder;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.TypeScript.Templates.TypescriptTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.Core.AngularConfig
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public partial class AngularConfigTemplate : TypeScriptTemplateBase<object>, ITypescriptFileBuilderTemplate
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.Core.AngularConfig";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully, Body = Mode.Ignore)]
        public AngularConfigTemplate(IOutputTarget outputTarget, object model = null) : base(TemplateId, outputTarget, model)
        {

            TypescriptFile = new TypescriptFile(this.GetFolderPath())
                .AddImport("ApplicationConfig", "@angular/core")
                .AddImport("provideZoneChangeDetection", "@angular/core")
                .AddImport("provideRouter", "@angular/router")
                .AddVariable("appConfig", "ApplicationConfig", config =>
                {
                    config.Export().Const();
                    config.WithObjectValue(obj =>
                    {
                        obj.AddField("providers", "[provideZoneChangeDetection({ eventCoalescing: true }), provideRouter(routes)]");
                    });
                }).AfterBuild(file =>
                {
                    var routeTemplate = GetTemplate<TypeScriptTemplateBase<object>>(AngularRoutesTemplate.TemplateId, new TemplateDiscoveryOptions { TrackDependency = false });
                    file.AddImport("routes", this.GetRelativePath(routeTemplate));
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
                fileName: "app.config",
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