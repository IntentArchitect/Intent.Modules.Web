using Intent.Modules.Angular.Settings;
using Intent.Modules.Common.TypeScript.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intent.Modules.Angular.NpmPackages.Version21;

public class NpmPackageEntryVersion(AngularSettings.AngularVersionOptionsEnum angularVersion) : INpmEntryResolver
{
    public bool Applicable() => angularVersion == AngularSettings.AngularVersionOptionsEnum._21;

    public IEnumerable<NpmPackageEntry> GetEntries()
    {
        yield return new NpmPackageEntry("packageManager", "npm@11.9.0");
    }
}



