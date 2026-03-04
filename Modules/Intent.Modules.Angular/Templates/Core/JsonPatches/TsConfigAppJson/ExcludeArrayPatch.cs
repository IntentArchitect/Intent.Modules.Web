using Intent.Modules.Angular.Settings;
using Intent.Modules.Common.FileBuilders.DataFileBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intent.Modules.Angular.Templates.Core.JsonPatches.TsConfigAppJson;

internal class ExcludeArrayPatch : IAngularJsonPatch
{
    public ExcludeArrayPatch(AngularSettings.AngularVersionOptionsEnum angularVersion) => AngularVersion = angularVersion;

    public AngularSettings.AngularVersionOptionsEnum AngularVersion { get; internal set; }

    public bool Applicable() => AngularVersion != AngularSettings.AngularVersionOptionsEnum._192;

    public void Apply(IDataFile file)
    {
        if (!file.RootObject.ContainsKey("exclude"))
        {
            file.RootObject.WithArray("exclude", array =>
            {
                array.WithValue("src/**/*.spec.ts");
            });
        }
    }
}
