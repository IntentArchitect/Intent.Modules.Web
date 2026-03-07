using Intent.Modules.Angular.Settings;
using Intent.Modules.Common.TypeScript.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intent.Modules.Angular.NpmPackages.Version21;

internal class NpmPackageVersion(AngularSettings.AngularVersionOptionsEnum angularVersion) : INpmPackageResolver
{
    public bool Applicable() => angularVersion == AngularSettings.AngularVersionOptionsEnum._21;

    public IEnumerable<NpmPackageDependency> GetPackages()
    {
        yield return new NpmPackageDependency("zone.js", "~0.15.0");

        yield return new NpmPackageDependency("jsdom", "^28.0.0", true);
        yield return new NpmPackageDependency("prettier", "^3.8.1", true);
        yield return new NpmPackageDependency("typescript", "~5.9.2", true);
        yield return new NpmPackageDependency("vitest", "^4.0.8", true);
    }
}
