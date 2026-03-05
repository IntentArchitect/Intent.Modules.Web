using Intent.Modules.Angular.Settings;
using Intent.Modules.Common.FileBuilders.DataFileBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intent.Modules.Angular.Templates.Core.JsonPatches.TsConfigJson;

internal class CompilerHostBindingsPatch : IAngularJsonPatch
{
    public CompilerHostBindingsPatch(AngularSettings.AngularVersionOptionsEnum angularVersion) => AngularVersion = angularVersion;

    public AngularSettings.AngularVersionOptionsEnum AngularVersion { get; internal set; }

    public bool Applicable() => AngularVersion == AngularSettings.AngularVersionOptionsEnum._20;

    public void Apply(IDataFile file)
    {
        var compilerOptions = file.RootObject["angularCompilerOptions"] as IDataFileObjectValue;
        if (!compilerOptions.ContainsKey("typeCheckHostBindings"))
        {
            compilerOptions.WithValue("typeCheckHostBindings", true);
        }
    }
}

