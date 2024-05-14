using Intent.Engine;
using Intent.Modules.Npm;
using Intent.Modules.Npm.Templates.PackageJsonFile;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateDecorator", Version = "1.0")]

namespace Intent.Modules.Angular.Decorators
{
    [IntentManaged(Mode.Merge)]
    public class PackageDotJsonDecorator : PackageJsonFileDecorator
    {
        [IntentManaged(Mode.Fully)]
        public const string DecoratorId = "Intent.Angular.PackageDotJsonDecorator";

        [IntentManaged(Mode.Fully)]
        private readonly PackageJsonFileTemplate _template;
        [IntentManaged(Mode.Fully)]
        private readonly IApplication _application;

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public PackageDotJsonDecorator(PackageJsonFileTemplate template, IApplication application)
        {
            _template = template;
            _application = application;
        }

        public override void UpdateSettings(JsonEditor fileEditor)
        {
            var scripts = new JsonEditor(fileEditor.GetProperty("scripts"));
            var dependencies = new JsonEditor(fileEditor.GetProperty("dependencies"));
            var devDependencies = new JsonEditor(fileEditor.GetProperty("devDependencies"));

            if (dependencies.PropertyExists("@angular/common"))
            {
                return;
            }

            scripts.AddPropertyIfNotExists("ng", "ng");
            scripts.AddPropertyIfNotExists("start", "ng serve");
            scripts.AddPropertyIfNotExists("build", "ng build");
            scripts.AddPropertyIfNotExists("watch", "ng build --watch --configuration development");
            scripts.AddPropertyIfNotExists("test", "ng test");

            dependencies.AddPropertyIfNotExists("@angular/animations", "16.2.0");
            dependencies.AddPropertyIfNotExists("@angular/cdk", "16.2.2");
            dependencies.AddPropertyIfNotExists("@angular/common", "16.2.0");
            dependencies.AddPropertyIfNotExists("@angular/compiler", "16.2.0");
            dependencies.AddPropertyIfNotExists("@angular/core", "16.2.0");
            dependencies.AddPropertyIfNotExists("@angular/forms", "16.2.0");
            dependencies.AddPropertyIfNotExists("@angular/material", "16.2.2");
            dependencies.AddPropertyIfNotExists("@angular/platform-browser", "16.2.0");
            dependencies.AddPropertyIfNotExists("@angular/platform-browser-dynamic", "16.2.0");
            dependencies.AddPropertyIfNotExists("@angular/router", "16.2.0");
            dependencies.AddPropertyIfNotExists("rxjs", "7.8.0");
            dependencies.AddPropertyIfNotExists("tslib", "2.3.0");
            dependencies.AddPropertyIfNotExists("zone.js", "0.13.0");

            devDependencies.AddPropertyIfNotExists("@angular-devkit/build-angular", "16.2.1");
            devDependencies.AddPropertyIfNotExists("@angular/cli", "16.2.1");
            devDependencies.AddPropertyIfNotExists("@angular/compiler-cli", "16.2.0");
            devDependencies.AddPropertyIfNotExists("@types/jasmine", "4.3.0");
            devDependencies.AddPropertyIfNotExists("jasmine-core", "4.6.0");
            devDependencies.AddPropertyIfNotExists("karma", "6.4.0");
            devDependencies.AddPropertyIfNotExists("karma-chrome-launcher", "3.2.0");
            devDependencies.AddPropertyIfNotExists("karma-coverage", "2.2.0");
            devDependencies.AddPropertyIfNotExists("karma-jasmine", "5.1.0");
            devDependencies.AddPropertyIfNotExists("karma-jasmine-html-reporter", "2.1.0");
            devDependencies.AddPropertyIfNotExists("typescript", "5.1.3");
        }
    }
}