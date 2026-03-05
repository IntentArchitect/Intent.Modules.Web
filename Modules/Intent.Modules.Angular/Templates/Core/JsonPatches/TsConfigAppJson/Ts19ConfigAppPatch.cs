using Intent.Modules.Angular.Settings;
using Intent.Modules.Common.FileBuilders.DataFileBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intent.Modules.Angular.Templates.Core.JsonPatches.TsConfigAppJson;

internal class Ts19ConfigAppPatch : IAngularJsonPatch
{
    public Ts19ConfigAppPatch(AngularSettings.AngularVersionOptionsEnum angularVersion) => AngularVersion = angularVersion;

    public AngularSettings.AngularVersionOptionsEnum AngularVersion { get; internal set; }

    public bool Applicable() => AngularVersion == AngularSettings.AngularVersionOptionsEnum._19;

    public void Apply(IDataFile file)
    {
        if (!file.RootObject.ContainsKey("files"))
        {
            file.RootObject.WithArray("files", array => 
            {
                array.WithValue("src/main.ts");
            });
        }

        var include = file.RootObject["include"] as IDataFileArrayValue;
        var item = include.FirstOrDefault(v => v is IDataFileScalarValue df && df?.Value is string s && s == "src/**/*.ts");

        if(item is not null)
        {
            include.Remove(item);
        }

        include.WithValue("src/**/*.d.ts");

    }
}
