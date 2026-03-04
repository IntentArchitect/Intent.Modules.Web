using Intent.Engine;
using Intent.Modules.Angular.Settings;
using Intent.Modules.Common.FileBuilders.DataFileBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intent.Modules.Angular.Templates.Core.AngularDotJsonFile.Patches;

internal interface IAngularJsonPatch
{
    AngularSettings.AngularVersionOptionsEnum AngularVersion { get; }

    bool Applicable();

    void Apply(IDataFile file);

    int Order => 0; 
}
