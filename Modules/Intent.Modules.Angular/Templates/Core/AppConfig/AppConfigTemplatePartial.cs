using Intent.Engine;
using Intent.Modules.Angular.Templates.Core.AppRoutes;
using Intent.Modules.Common;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.TypeScript.Builder;
using Intent.Modules.Common.TypeScript.Events;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.TypeScript.Templates.TypescriptTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.Core.AppConfig
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public partial class AppConfigTemplate : TypeScriptTemplateBase<object>, ITypescriptFileBuilderTemplate
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.Core.AppConfig";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully, Body = Mode.Ignore)]
        public AppConfigTemplate(IOutputTarget outputTarget, object model = null) : base(TemplateId, outputTarget, model)
        {
            this.AddCoreDependencies();

            ExecutionContext.EventDispatcher.Subscribe<ServiceConfigurationRequestEvent>(HandleServiceConfigurationRequest);

            TypescriptFile = new TypescriptFile(this.GetFolderPath(), this)
                .AddImport("ApplicationConfig", "@angular/core")
                .AddImport("provideZoneChangeDetection", "@angular/core")
                .AddImport("provideRouter", "@angular/router")
                .AddVariable("appConfig", "ApplicationConfig", config =>
                {
                    config.Export().Const();
                    config.WithObjectValue(obj =>
                    {
                        var providersArray = new TypescriptVariableArray();
                        providersArray.Indentation = TypescriptFile.Indentation;
                        providersArray.AddValue("provideZoneChangeDetection({ eventCoalescing: true })");
                        providersArray.AddValue("provideRouter(routes)");

                        obj.AddField("providers", providersArray);
                    });
                }).AfterBuild(file =>
                {
                    var routeTemplate = GetTemplate<TypeScriptTemplateBase<object>>(AppRoutesTemplate.TemplateId, new TemplateDiscoveryOptions { TrackDependency = false });
                    file.AddImport("routes", this.GetRelativePath(routeTemplate));
                });
        }

        public void HandleServiceConfigurationRequest(ServiceConfigurationRequestEvent @event)
        {
            var configVar = TypescriptFile.Variables.First(v => v.Name == "appConfig");
            var configVarValue = configVar.Value as TypescriptVariableObject;

            var providersField = configVarValue.Fields.First(f => f.Name == "providers") as TypescriptVariableField;
            var providersFieldValue = providersField.Value as TypescriptVariableArray;

            if (!providersFieldValue.Items.Any(i => i.Value.GetText("") == $"{@event.ImportBinding}()"))
            {
                providersFieldValue.AddValue($"{@event.ImportBinding}()");
                TypescriptFile.AddImport(@event.ImportBinding, @event.ModuleSpecifier);
            }
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