using Intent.Modules.Angular.Settings;
using Intent.Modules.Common.FileBuilders.DataFileBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intent.Modules.Angular.Templates.Core.JsonPatches.TsConfigJson;

internal class Ts19ConfigPatch : IAngularJsonPatch
{
    public Ts19ConfigPatch(AngularSettings.AngularVersionOptionsEnum angularVersion) => AngularVersion = angularVersion;

    public AngularSettings.AngularVersionOptionsEnum AngularVersion { get; internal set; }

    public bool Applicable() => AngularVersion == AngularSettings.AngularVersionOptionsEnum._19;

    public void Apply(IDataFile file)
    {
        if (!file.RootObject.ContainsKey("compilerOptions"))
        {
            return;
        }

        var compilerOptions = file.RootObject["compilerOptions"] as IDataFileObjectValue;
        if (!compilerOptions.ContainsKey("outDir"))
        {
            compilerOptions.WithValue("outDir", "./dist/out-tsc", 0);
        }

        if (!compilerOptions.ContainsKey("esModuleInterop"))
        {
            compilerOptions.WithValue("esModuleInterop", true);
        }

        if (!compilerOptions.ContainsKey("moduleResolution"))
        {
            compilerOptions.WithValue("moduleResolution", "bundler");
        }

        if (!compilerOptions.ContainsKey("module"))
        {
            compilerOptions.WithValue("module", "ES2022");
        }
        else
        {
            var module = compilerOptions[key: "module"] as IDataFileScalarValue;
            module.Value = "ES2022";
        }
    }
}
