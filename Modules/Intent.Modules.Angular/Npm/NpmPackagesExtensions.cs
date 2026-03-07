using Intent.Modules.Angular.Npm;
using Intent.Modules.Angular.Settings;
using Intent.Modules.Common.TypeScript.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intent.Modules.Angular.NpmPackages;

public static class NpmPackagesExtensions
{
    public static void AddCoreDependencies(this TypeScriptTemplateBase<object> template)
    {
        var versionEnum = template.ExecutionContext.Settings.GetAngularSettings().AngularVersion().AsEnum();

        var resolvers = new List<INpmPackageResolver>
        {
            new NpmPackageCore(versionEnum.ToString()),
            new Version19.NpmPackageVersion(versionEnum),
            new Version20.NpmPackageVersion(versionEnum),
            new Version21.NpmPackageVersion(versionEnum),
        };

        foreach (var resolver in resolvers.Where(r => r.Applicable()))
        {
            resolver.GetPackages().ToList().ForEach(template.AddDependency);
        }
    }

    public static void AddCorePackagesEntries(this TypeScriptTemplateBase<object> template)
    {
        var versionEnum = template.ExecutionContext.Settings.GetAngularSettings().AngularVersion().AsEnum();

        var resolvers = new List<INpmEntryResolver>
        {
            new Version21.NpmPackageEntryVersion(versionEnum),
        };

        foreach (var entry in resolvers.Where(r => r.Applicable()))
        {
            entry.GetEntries().ToList().ForEach(template.ExecutionContext.EventDispatcher.Publish);
        }
    }

    public static void AddCorePackagesScripts(this TypeScriptTemplateBase<object> template)
    {
        var versionEnum = template.ExecutionContext.Settings.GetAngularSettings().AngularVersion().AsEnum();

        var resolvers = new List<INpmScriptResolver>
        {
            new NpmPackageScriptsCore()
        };

        foreach (var script in resolvers.Where(r => r.Applicable()))
        {
            script.GetScripts().ToList().ForEach(template.ExecutionContext.EventDispatcher.Publish);
        }
    }

}
