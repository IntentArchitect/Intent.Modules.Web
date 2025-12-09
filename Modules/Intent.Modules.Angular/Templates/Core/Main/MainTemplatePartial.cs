using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Modelers.UI.Api;
using Intent.Modules.Angular.Templates.Component.LayoutComponentTypescript;
using Intent.Modules.Angular.Templates.Core.AppComponent;
using Intent.Modules.Common;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.TypeScript.Builder;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.Modules.Common.TypeScript.Utils;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.TypeScript.Templates.TypescriptTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.Core.Main
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public partial class MainTemplate : TypeScriptTemplateBase<object>, ITypescriptFileBuilderTemplate
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.Core.Main";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully, Body = Mode.Ignore)]
        public MainTemplate(IOutputTarget outputTarget, object model = null) : base(TemplateId, outputTarget, model)
        {
            TypescriptFile = new TypescriptFile(this.GetFolderPath(), this);
        }

        [IntentManaged(Mode.Fully)]
        public TypescriptFile TypescriptFile { get; }

        [IntentManaged(Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return TypescriptFile.GetConfig($"Main");
        }

        [IntentManaged(Mode.Ignore)]
        public override string TransformText()
        {
            // the default component
            var bootStrapComponentName = "AppComponent";
            var additionalImport = string.Empty;

            // if there is a layout, use the first one
            if (ExecutionContext.MetadataManager.UserInterface(ExecutionContext.GetApplicationConfig().Id).GetLayoutModels().Any())
            {
                var layoutModel = ExecutionContext.MetadataManager.UserInterface(ExecutionContext.GetApplicationConfig().Id).GetLayoutModels().First();
                var layoutTemplate = ExecutionContext.FindTemplateInstance(LayoutComponentTypescriptTemplate.TemplateId, layoutModel);
                AddTemplateDependency(LayoutComponentTypescriptTemplate.TemplateId, layoutModel);

                if (layoutTemplate is ITypescriptFileBuilderTemplate builder)
                {
                    bootStrapComponentName = builder.TypescriptFile.Classes.First()?.Name ?? bootStrapComponentName;
                }
            }
            else
            {
                // otherwise use this template, which is only run if there is no layouts
                var appComponentTemplate = ExecutionContext.FindTemplateInstance(AppComponentTemplate.TemplateId);
                additionalImport = $"import {{{" AppComponent "}}} from '{this.GetRelativePath(appComponentTemplate)}';";
            }

            return $@"
import {{ bootstrapApplication }} from '@angular/platform-browser';
import {{ appConfig }} from './app/app.config';
{additionalImport}

bootstrapApplication({bootStrapComponentName}, appConfig)
  .catch((err) => console.error(err));
";
        }
    }
}