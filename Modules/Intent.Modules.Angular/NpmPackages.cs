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
        foreach (var dependency in GetNpmPackages())
        {
            template.AddDependency(dependency);
        }
    }

    public static IEnumerable<NpmPackageDependency> GetNpmPackages()
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

        yield return new NpmPackageDependency("@angular-devkit/build-angular", "^19.2.19", true);
        yield return new NpmPackageDependency("@angular/cli", "^19.2.19", true);
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
