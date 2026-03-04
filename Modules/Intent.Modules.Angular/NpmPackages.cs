using Intent.Modules.Angular.Settings;
using Intent.Modules.Common.TypeScript.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intent.Modules.Angular;

public static class NpmPackages
{
    public static void AddCoreDependencies(this TypeScriptTemplateBase<object> template)
    {
        foreach (var dependency in GetNpmVersionPackages(template.ExecutionContext.Settings.GetAngularSettings().AngularVersion().AsEnum()))
        {
            template.AddDependency(dependency);
        }
    }

    private static IEnumerable<NpmPackageDependency> GetNpmVersionPackages(AngularSettings.AngularVersionOptionsEnum version) =>
        version switch
    {
        AngularSettings.AngularVersionOptionsEnum._210 => Get21NpmPackages(),
        AngularSettings.AngularVersionOptionsEnum._202 => Get20NpmPackages(),
        AngularSettings.AngularVersionOptionsEnum._192 => Get19NpmPackages(),
        _ => throw new NotSupportedException($"Unsupported Angular version: {version}")
    };

    public static IEnumerable<NpmPackageDependency> Get21NpmPackages()
    {
        yield return new NpmPackageDependency("@angular/common", "^21.2.0");
        yield return new NpmPackageDependency("@angular/compiler", "^21.2.0");
        yield return new NpmPackageDependency("@angular/core", "^21.2.0");
        yield return new NpmPackageDependency("@angular/forms", "^21.2.0");
        yield return new NpmPackageDependency("@angular/platform-browser", "^21.2.0");
        yield return new NpmPackageDependency("@angular/router", "^21.2.0");
        yield return new NpmPackageDependency("rxjs", "~7.8.0");
        yield return new NpmPackageDependency("tslib", "^2.3.0");

        yield return new NpmPackageDependency("@angular/build", "^21.2.0", true);
        yield return new NpmPackageDependency("@angular/cli", "^21.2.0", true);
        yield return new NpmPackageDependency("@angular/compiler-cli", "^21.2.0", true);
        yield return new NpmPackageDependency("jsdom", "^28.0.0", true);
        yield return new NpmPackageDependency("prettier", "^3.8.1", true);
        yield return new NpmPackageDependency("typescript", "~5.9.2", true);
        yield return new NpmPackageDependency("vitest", "^4.0.8", true);
    }

    public static IEnumerable<NpmPackageDependency> Get20NpmPackages()
    {
        yield return new NpmPackageDependency("@angular/common", "^20.3.0");
        yield return new NpmPackageDependency("@angular/compiler", "^20.3.0");
        yield return new NpmPackageDependency("@angular/core", "^20.3.0");
        yield return new NpmPackageDependency("@angular/forms", "^20.3.0");
        yield return new NpmPackageDependency("@angular/platform-browser", "^20.3.0");
        yield return new NpmPackageDependency("@angular/router", "^20.3.0");
        yield return new NpmPackageDependency("rxjs", "~7.8.0");
        yield return new NpmPackageDependency("tslib", "^2.3.0");
        yield return new NpmPackageDependency("zone.js", "~0.15.0");

        yield return new NpmPackageDependency("@angular/build", "^20.3.18", true);
        yield return new NpmPackageDependency("@angular/cli", "^20.3.18", true);
        yield return new NpmPackageDependency("@angular/compiler-cli", "^20.3.0", true);
        yield return new NpmPackageDependency("@types/jasmine", "~5.1.0", true);
        yield return new NpmPackageDependency("jasmine-core", "~5.9.0", true);
        yield return new NpmPackageDependency("karma", "~6.4.0", true);
        yield return new NpmPackageDependency("karma-chrome-launcher", "~3.2.0", true);
        yield return new NpmPackageDependency("karma-coverage", "~2.2.0", true);
        yield return new NpmPackageDependency("karma-jasmine", "~5.1.0", true);
        yield return new NpmPackageDependency("karma-jasmine-html-reporter", "~2.1.0", true);
        yield return new NpmPackageDependency("typescript", "~5.9.2", true);
    }

    public static IEnumerable<NpmPackageDependency> Get19NpmPackages()
    {
        yield return new NpmPackageDependency("@angular/common", "^19.2.0");
        yield return new NpmPackageDependency("@angular/compiler", "^19.2.0");
        yield return new NpmPackageDependency("@angular/core", "^19.2.0");
        yield return new NpmPackageDependency("@angular/forms", "^19.2.0");
        yield return new NpmPackageDependency("@angular/platform-browser", "^19.2.0");
        yield return new NpmPackageDependency("@angular/platform-browser-dynamic", "^19.2.0");
        yield return new NpmPackageDependency("@angular/router", "^19.2.0");
        yield return new NpmPackageDependency("rxjs", "~7.8.0");
        yield return new NpmPackageDependency("tslib", "^2.3.0");
        yield return new NpmPackageDependency("zone.js", "~0.15.0");

        yield return new NpmPackageDependency("@angular-devkit/build-angular", "^19.2.22", true);
        yield return new NpmPackageDependency("@angular/cli", "^19.2.22", true);
        yield return new NpmPackageDependency("@angular/compiler-cli", "^19.2.0", true);
        yield return new NpmPackageDependency("@types/jasmine", "~5.1.0", true);
        yield return new NpmPackageDependency("jasmine-core", "~5.6.0", true);
        yield return new NpmPackageDependency("karma", "~6.4.0", true);
        yield return new NpmPackageDependency("karma-chrome-launcher", "~3.2.0", true);
        yield return new NpmPackageDependency("karma-coverage", "~2.2.0", true);
        yield return new NpmPackageDependency("karma-jasmine", "~5.1.0", true);
        yield return new NpmPackageDependency("karma-jasmine-html-reporter", "~2.1.0", true);
        yield return new NpmPackageDependency("typescript", "~5.7.2", true);
    }
}
