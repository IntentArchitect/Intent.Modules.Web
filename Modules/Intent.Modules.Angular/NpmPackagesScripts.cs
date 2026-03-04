using Intent.Modules.Angular.Settings;
using Intent.Modules.Common.TypeScript.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intent.Modules.Angular;

public static class NpmPackagesScripts
{
    public static void AddCorePackagesScripts(this TypeScriptTemplateBase<object> template)
    {
        foreach (var dependency in GetNpmVersionScripts(template.ExecutionContext.Settings.GetAngularSettings().AngularVersion().AsEnum()))
        {
            template.ExecutionContext.EventDispatcher.Publish(dependency);
        }
    }

    private static IEnumerable<NpmPackageScript> GetNpmVersionScripts(AngularSettings.AngularVersionOptionsEnum version) =>
        version switch
    {
        _ => GetNpmPackagesScripts()
    };

    public static IEnumerable<NpmPackageScript> GetNpmPackagesScripts()
    {
        yield return new NpmPackageScript("ng", "ng");
        yield return new NpmPackageScript("start", "ng serve");
        yield return new NpmPackageScript("build", "ng build");
        yield return new NpmPackageScript("watch", "ng build --watch --configuration development");
        yield return new NpmPackageScript("test", "ng test");

    }
}
