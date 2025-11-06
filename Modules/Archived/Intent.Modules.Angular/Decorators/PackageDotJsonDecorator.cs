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

            dependencies.AddPropertyIfNotExists("@angular/animations", "17.3.8");
            dependencies.AddPropertyIfNotExists("@angular/cdk", "17.3.8");
            dependencies.AddPropertyIfNotExists("@angular/common", "17.3.8");
            dependencies.AddPropertyIfNotExists("@angular/compiler", "17.3.8");
            dependencies.AddPropertyIfNotExists("@angular/core", "17.3.8");
            dependencies.AddPropertyIfNotExists("@angular/forms", "17.3.8");
            dependencies.AddPropertyIfNotExists("@angular/material", "17.3.8");
            dependencies.AddPropertyIfNotExists("@angular/platform-browser", "17.3.8");
            dependencies.AddPropertyIfNotExists("@angular/platform-browser-dynamic", "17.3.8");
            dependencies.AddPropertyIfNotExists("@angular/router", "17.3.8");
            dependencies.AddPropertyIfNotExists("rxjs", "7.8.0");
            dependencies.AddPropertyIfNotExists("tslib", "2.3.0");
            dependencies.AddPropertyIfNotExists("zone.js", "0.14.5");

            devDependencies.AddPropertyIfNotExists("@angular-devkit/build-angular", "17.3.7");
            devDependencies.AddPropertyIfNotExists("@angular/cli", "17.3.7");
            devDependencies.AddPropertyIfNotExists("@angular/compiler-cli", "17.3.8");
            devDependencies.AddPropertyIfNotExists("@types/jasmine", "4.3.0");
            devDependencies.AddPropertyIfNotExists("jasmine-core", "4.6.0");
            devDependencies.AddPropertyIfNotExists("karma", "6.4.0");
            devDependencies.AddPropertyIfNotExists("karma-chrome-launcher", "3.2.0");
            devDependencies.AddPropertyIfNotExists("karma-coverage", "2.2.0");
            devDependencies.AddPropertyIfNotExists("karma-jasmine", "5.1.0");
            devDependencies.AddPropertyIfNotExists("karma-jasmine-html-reporter", "2.1.0");
            devDependencies.AddPropertyIfNotExists("typescript", "5.4.5");
        }
    }
}