using Intent.Modules.Angular.Settings;
using Intent.Modules.Common.TypeScript.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intent.Modules.Angular.NpmPackages;

public class NpmPackageScriptsCore : INpmScriptResolver
{
    public bool Applicable() => true;

    public IEnumerable<NpmPackageScript> GetScripts()
    {
        yield return new NpmPackageScript("ng", "ng");
        yield return new NpmPackageScript("start", "ng serve");
        yield return new NpmPackageScript("build", "ng build");
        yield return new NpmPackageScript("watch", "ng build --watch --configuration development");
        yield return new NpmPackageScript("test", "ng test");
    }
}
