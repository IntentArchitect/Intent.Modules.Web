using Intent.Modules.Angular.Settings;
using Intent.Modules.Common.TypeScript.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intent.Modules.Angular.NpmPackages.Version20;

internal class NpmPackageVersion(AngularSettings.AngularVersionOptionsEnum angularVersion) : INpmPackageResolver
{
    public bool Applicable() => angularVersion == AngularSettings.AngularVersionOptionsEnum._20;

    public IEnumerable<NpmPackageDependency> GetPackages()
    {
        yield return new NpmPackageDependency("zone.js", "~0.15.0");

        yield return new NpmPackageDependency("@types/jasmine", "~5.1.0", true);
        yield return new NpmPackageDependency("jasmine-core", "~5.9.0", true);
        yield return new NpmPackageDependency("karma", "~6.4.0", true);
        yield return new NpmPackageDependency("karma-chrome-launcher", "~3.2.0", true);
        yield return new NpmPackageDependency("karma-coverage", "~2.2.0", true);
        yield return new NpmPackageDependency("karma-jasmine", "~5.1.0", true);
        yield return new NpmPackageDependency("karma-jasmine-html-reporter", "~2.1.0", true);
        yield return new NpmPackageDependency("typescript", "~5.9.2", true);
    }
}
