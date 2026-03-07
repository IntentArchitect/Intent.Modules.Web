using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Intent.Engine;
using Intent.Modules.Angular.NpmPackages;
using Intent.Modules.Angular.Settings;
using Intent.Modules.Angular.Templates.Core.AppRoutes;
using Intent.Modules.Common;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.TypeScript.Builder;
using Intent.Modules.Common.TypeScript.Events;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.TypeScript.Templates.TypescriptTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.Core.AppConfig
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public partial class AppConfigTemplate : TypeScriptTemplateBase<object>, ITypescriptFileBuilderTemplate
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.Core.AppConfig";

        private readonly List<ServiceConfigurationRequestEvent> _serviceConfigurations = [];

        [IntentManaged(Mode.Merge, Signature = Mode.Fully, Body = Mode.Ignore)]
        public AppConfigTemplate(IOutputTarget outputTarget, object model = null) : base(TemplateId, outputTarget, model)
        {
            this.AddCoreDependencies();

            ExecutionContext.EventDispatcher.Subscribe<ServiceConfigurationRequestEvent>(HandleServiceConfigurationRequest);

            TypescriptFile = new TypescriptFile(this.GetFolderPath(), this)
                .AddImport("ApplicationConfig", "@angular/core")
                .AddImport("provideZoneChangeDetection", "@angular/core")
                
                .AddVariable("appConfig", "ApplicationConfig", config =>
                {
                    config.Export().Const();
                    config.WithObjectValue(obj =>
                    {
                        obj.AddField("providers", BuildProvidersArray());
                    });
                }).AfterBuild(file =>
                {
                    var routeTemplate = GetTemplate<TypeScriptTemplateBase<object>>(AppRoutesTemplate.TemplateId, new TemplateDiscoveryOptions { TrackDependency = false });
                    file.AddImport("routes", this.GetRelativePath(routeTemplate));

                    foreach (var serviceConfig in _serviceConfigurations)
                    {
                        var configVar = file.Variables.First(v => v.Name == "appConfig");
                        var configVarValue = configVar.Value as TypescriptVariableObject;

                        var providersField = configVarValue.Fields.First(f => f.Name == "providers") as TypescriptVariableField;
                        var providersFieldValue = providersField.Value as TypescriptVariableArray;

                        if (!providersFieldValue.Items.Any(i => i.Value.GetText("") == $"{serviceConfig.ImportBinding}()"))
                        {
                            providersFieldValue.AddValue($"{serviceConfig.ImportBinding}()");
                            file.AddImport(serviceConfig.ImportBinding, serviceConfig.ModuleSpecifier);
                        }
                    }
                });
        }

        private TypescriptVariableArray BuildProvidersArray()
        {
            var providersArray = new TypescriptVariableArray
            {
                Indentation = TypescriptFile.Indentation
            };

            var angularVersion = ExecutionContext.Settings.GetAngularSettings().AngularVersion().AsEnum();

            providersArray.AddValue("provideRouter(routes)");
            AddImport("provideRouter", "@angular/router");

            switch (angularVersion)
            {
                case AngularSettings.AngularVersionOptionsEnum._19:
                    providersArray.AddValue("provideZoneChangeDetection({ eventCoalescing: true })");
                    AddImport("provideZoneChangeDetection", "@angular/core");
                    break;

                case AngularSettings.AngularVersionOptionsEnum._20:
                    providersArray.AddValue("provideBrowserGlobalErrorListeners()");
                    providersArray.AddValue("provideZoneChangeDetection({ eventCoalescing: true })");
                    AddImport("provideZoneChangeDetection", "@angular/core");
                    AddImport("provideBrowserGlobalErrorListeners", "@angular/core");
                    break;

                case AngularSettings.AngularVersionOptionsEnum._21:
                    providersArray.AddValue("provideBrowserGlobalErrorListeners()");
                    providersArray.AddValue("provideZoneChangeDetection({ eventCoalescing: true })");
                    AddImport("provideBrowserGlobalErrorListeners", "@angular/core");
                    AddImport("provideZoneChangeDetection", "@angular/core");
                    break;

                default:
                    providersArray.AddValue("provideZoneChangeDetection({ eventCoalescing: true })");
                    break;
            }

            return providersArray;
        }

        public override void BeforeTemplateExecution()
        {
            this.AddCorePackagesScripts();
            this.AddCorePackagesEntries();

            base.BeforeTemplateExecution();
        }

        public void HandleServiceConfigurationRequest(ServiceConfigurationRequestEvent @event)
        {
            _serviceConfigurations.Add(@event);
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