using Intent.Modules.Angular.Settings;
using Intent.Modules.Common.FileBuilders.DataFileBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intent.Modules.Angular.Templates.Core.AngularDotJsonFile.Patches;

internal class CliObjectPatch : IAngularJsonPatch
{
    public CliObjectPatch(AngularSettings.AngularVersionOptionsEnum angularVersion) => AngularVersion = angularVersion;

    public AngularSettings.AngularVersionOptionsEnum AngularVersion { get; internal set; }

    public bool Applicable() => AngularVersion == AngularSettings.AngularVersionOptionsEnum._210;

    public void Apply(IDataFile file)
    {
        if (file.RootObject.ContainsKey("cli"))
        {
            return;
        }

        file.RootObject.WithObject("cli", 2, cli =>
        {
            cli.WithValue("packageManager", "npm");
        });
    }

    public int Order => 0;
}
