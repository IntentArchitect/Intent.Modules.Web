using Intent.Modules.Angular.Settings;
using Intent.Modules.Common.FileBuilders.DataFileBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intent.Modules.Angular.Templates.Core.JsonPatches.TsConfigJson;

internal class FilesReferencePatch : IAngularJsonPatch
{
    public FilesReferencePatch(AngularSettings.AngularVersionOptionsEnum angularVersion) => AngularVersion = angularVersion;

    public AngularSettings.AngularVersionOptionsEnum AngularVersion { get; internal set; }

    public bool Applicable() => AngularVersion != AngularSettings.AngularVersionOptionsEnum._19;

    public void Apply(IDataFile file)
    {
        if (!file.RootObject.ContainsKey("files"))
        {
            file.RootObject.WithArray("files", array => { });
        }

        if (!file.RootObject.ContainsKey("references"))
        {
            file.RootObject.WithArray("references", array =>
            {
                array
                .WithObject(obj =>
                    {
                        obj.WithValue("path", "./tsconfig.app.json");
                    })
                .WithObject(obj =>
                    {
                        obj.WithValue("path", "./tsconfig.spec.json");
                    });
            });
        }
    }
}
