using Intent.Modules.Angular.NpmPackages;
using Intent.Modules.Common.TypeScript.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intent.Modules.Angular.Npm;

internal class NpmPackageCore(string angularVersion) : INpmPackageResolver
{
    private readonly string _angularVersion = angularVersion.TrimStart('_');

    public bool Applicable() => true;

    public IEnumerable<NpmPackageDependency> GetPackages()
    {
        yield return new NpmPackageDependency("@angular/common", $"^{_angularVersion}.0.0");
        yield return new NpmPackageDependency("@angular/compiler", $"^{_angularVersion}.0.0");
        yield return new NpmPackageDependency("@angular/core", $"^{_angularVersion}.0.0");
        yield return new NpmPackageDependency("@angular/forms", $"^{_angularVersion}.0.0");
        yield return new NpmPackageDependency("@angular/platform-browser", $"^{_angularVersion}.0.0");
        yield return new NpmPackageDependency("@angular/router", $"^{_angularVersion}.0.0");
        yield return new NpmPackageDependency("rxjs", "~7.8.0");
        yield return new NpmPackageDependency("tslib", "^2.3.0");

        yield return new NpmPackageDependency("@angular/build", $"^{_angularVersion}.0.0", true);
        yield return new NpmPackageDependency("@angular/cli", $"^{_angularVersion}.0.0", true);
        yield return new NpmPackageDependency("@angular/compiler-cli", $"^{_angularVersion}.0.0", true);
    }
}
