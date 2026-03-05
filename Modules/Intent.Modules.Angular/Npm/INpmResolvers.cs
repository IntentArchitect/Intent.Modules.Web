using Intent.Modules.Common.TypeScript.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intent.Modules.Angular.NpmPackages;

internal interface INpmPackageResolver
{
    bool Applicable();

    IEnumerable<NpmPackageDependency> GetPackages();
}

internal interface INpmEntryResolver
{
    bool Applicable();

    IEnumerable<NpmPackageEntry> GetEntries();
}

internal interface INpmScriptResolver
{
    bool Applicable();

    IEnumerable<NpmPackageScript> GetScripts();
}
