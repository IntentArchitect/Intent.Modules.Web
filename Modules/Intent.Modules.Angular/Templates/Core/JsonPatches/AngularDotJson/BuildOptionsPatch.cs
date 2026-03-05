using Intent.Modules.Angular.Settings;
using Intent.Modules.Angular.Templates.Core.JsonPatches;
using Intent.Modules.Common.FileBuilders.DataFileBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intent.Modules.Angular.Templates.Core.JsonPatches.AngularDotJson;

internal class BuildOptionsPatch : IAngularJsonPatch
{
    private readonly string _applicationNameCamel;
    private readonly string _applicationNameKebab;

    public BuildOptionsPatch(AngularSettings.AngularVersionOptionsEnum angularVersion, string applicationNameCamel, string applicationNameKebab)
    {
        AngularVersion = angularVersion;
        _applicationNameCamel = applicationNameCamel;
        _applicationNameKebab = applicationNameKebab;
    }
    public AngularSettings.AngularVersionOptionsEnum AngularVersion { get; internal set; }

    public bool Applicable() => AngularVersion == AngularSettings.AngularVersionOptionsEnum._19;
    
    public void Apply(IDataFile file)
    {
        if (!file.RootObject.ContainsKey("projects"))
        {
            return;
        }

        var projects = file.RootObject["projects"] as IDataFileObjectValue;
        if (!projects.ContainsKey(_applicationNameCamel))
        {
            return;
        }

        var appProject = projects[_applicationNameCamel] as IDataFileObjectValue;
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
        if (!optionsObject.ContainsKey("index"))
        {
            optionsObject.WithValue("index", "src/index.html", 0);
        }

        if (!optionsObject.ContainsKey("outputPath"))
        {
            optionsObject.WithValue("outputPath", $"dist/{_applicationNameKebab}", 0);
        }

        if (!optionsObject.ContainsKey("scripts"))
        {
            optionsObject.WithArray("scripts", scripts => { });
        }
    }
}
