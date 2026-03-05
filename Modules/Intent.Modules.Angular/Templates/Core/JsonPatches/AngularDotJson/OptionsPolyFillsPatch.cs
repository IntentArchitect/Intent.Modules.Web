using Intent.Modules.Angular.Settings;
using Intent.Modules.Angular.Templates.Core.JsonPatches;
using Intent.Modules.Common.FileBuilders.DataFileBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intent.Modules.Angular.Templates.Core.JsonPatches.AngularDotJson;

internal class OptionsPolyFillsPatch : IAngularJsonPatch
{
    private readonly string _applicationName;

    public OptionsPolyFillsPatch(AngularSettings.AngularVersionOptionsEnum angularVersion, string applicationName)
    {
        AngularVersion = angularVersion;
        _applicationName = applicationName;
    }
    public AngularSettings.AngularVersionOptionsEnum AngularVersion { get; internal set; }

    public bool Applicable() => AngularVersion == AngularSettings.AngularVersionOptionsEnum._19 || AngularVersion == AngularSettings.AngularVersionOptionsEnum._20;

    public void Apply(IDataFile file)
    {
        if (!file.RootObject.ContainsKey("projects"))
        {
            return;
        }

        var projects = file.RootObject["projects"] as IDataFileObjectValue;
        if (!projects.ContainsKey(_applicationName))
        {
            return;
        }

        var appProject = projects[_applicationName] as IDataFileObjectValue;
        if (!appProject.ContainsKey("architect"))
        {
            return;
        }

        var architect = appProject["architect"] as IDataFileObjectValue;
        if (!architect.ContainsKey("build"))
        {
            return;
        }

        var build = architect["build"] as IDataFileObjectValue;
        if (!build.ContainsKey("options"))
        {
            return;
        }

        var optionsObject = build["options"] as IDataFileObjectValue;
        if (!optionsObject.ContainsKey("polyfills"))
        {
            var position = AngularVersion == AngularSettings.AngularVersionOptionsEnum._19 ? 3 : 1;

            optionsObject.WithArray("polyfills", position, poly =>
            {
                poly.WithValue("zone.js");
            });
        }
    }
}
