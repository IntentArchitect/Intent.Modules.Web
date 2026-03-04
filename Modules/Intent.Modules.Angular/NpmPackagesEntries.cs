using Intent.Modules.Angular.Settings;
using Intent.Modules.Common.TypeScript.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intent.Modules.Angular;

public static class NpmPackagesEntries
{
    public static void AddCorePackagesEntries(this TypeScriptTemplateBase<object> template)
    {
        foreach (var dependency in GetNpmVersionEntries(template.ExecutionContext.Settings.GetAngularSettings().AngularVersion().AsEnum()))
        {
            template.ExecutionContext.EventDispatcher.Publish(dependency);
        }
    }

    private static IEnumerable<NpmPackageEntry> GetNpmVersionEntries(AngularSettings.AngularVersionOptionsEnum version) =>
        version switch
    {
        AngularSettings.AngularVersionOptionsEnum._210 => Get21NpmPackagesEntries(),
        AngularSettings.AngularVersionOptionsEnum._202 => Get20NpmPackagesEntries(),
        AngularSettings.AngularVersionOptionsEnum._192 => Get19NpmPackagesEntries(),
        _ => throw new NotSupportedException($"Unsupported Angular version: {version}")
    };

    public static IEnumerable<NpmPackageEntry> Get21NpmPackagesEntries()
    {
        yield return new NpmPackageEntry("packageManager", "npm@11.9.0");
    }

    public static IEnumerable<NpmPackageEntry> Get20NpmPackagesEntries()
    {
        yield break;
    }

    public static IEnumerable<NpmPackageEntry> Get19NpmPackagesEntries()
    {
        yield break;
    }
}
