using Intent.Modules.Angular.Settings;
using Intent.Modules.Angular.Templates.Core.JsonPatches;
using Intent.Modules.Common.FileBuilders.DataFileBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intent.Modules.Angular.Templates.Core.JsonPatches.AngularDotJson;

internal class TestBuilderUnitTestPatch : IAngularJsonPatch
{
    private readonly string _applicationName;

    public TestBuilderUnitTestPatch(AngularSettings.AngularVersionOptionsEnum angularVersion, string applicationName)
    {
        AngularVersion = angularVersion;
        _applicationName = applicationName;
    }

    public AngularSettings.AngularVersionOptionsEnum AngularVersion { get; internal set; }

    public bool Applicable() => AngularVersion == AngularSettings.AngularVersionOptionsEnum._21;

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
        if (!architect.ContainsKey("test"))
        {
            architect.WithObject("test", test =>
            {
                test
                    .WithValue("builder", "@angular/build:unit-test");
            });
        }
    }
}

