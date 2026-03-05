using Intent.Modules.Angular.Settings;
using Intent.Modules.Common.FileBuilders.DataFileBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intent.Modules.Angular.Templates.Core.JsonPatches.TsConfigJson;

internal class OptionTypesViTestPatch : IAngularJsonPatch
{
    public OptionTypesViTestPatch(AngularSettings.AngularVersionOptionsEnum angularVersion) => AngularVersion = angularVersion;

    public AngularSettings.AngularVersionOptionsEnum AngularVersion { get; internal set; }

    public bool Applicable() => AngularVersion == AngularSettings.AngularVersionOptionsEnum._21;

    public void Apply(IDataFile file)
    {
        var compilerOptions = file.RootObject["compilerOptions"] as IDataFileObjectValue;
        if (!compilerOptions.ContainsKey("types"))
        {
            compilerOptions.WithArray("types", types => 
            {
                types.WithValue("vitest/globals");
            });
        }
        else
        {
            var types = compilerOptions["types"] as IDataFileArrayValue;

            if(!types.Any(t => t is IDataFileScalarValue type && type?.Value as string == "vitest/globals"))
            {
                types.WithValue("vitest/globals");
            }
        }
    }
}

