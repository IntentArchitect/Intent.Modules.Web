using Intent.Modules.Angular.Settings;
using Intent.Modules.Common.FileBuilders.DataFileBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intent.Modules.Angular.Templates.Core.AngularDotJsonFile.Patches;

internal class ServeBuilderApplication : IAngularJsonPatch
{
    private readonly string _applicationName;

    public ServeBuilderApplication(AngularSettings.AngularVersionOptionsEnum angularVersion, string applicationName)
    {
        AngularVersion = angularVersion;
        _applicationName = applicationName;
    }

    public AngularSettings.AngularVersionOptionsEnum AngularVersion { get; internal set; }

    public bool Applicable() => AngularVersion != AngularSettings.AngularVersionOptionsEnum._192;

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
        if (!architect.ContainsKey("serve"))
        {
            return;
        }

        var serveObject = architect["serve"] as IDataFileObjectValue;
        if (!serveObject.ContainsKey("builder"))
        {
            serveObject.WithValue("builder", "@angular/build:dev-server", 0);
            return;
        }

        var builderProperty = serveObject["builder"] as IDataFileScalarValue;
        builderProperty.Value = "@angular/build:dev-server";
    }

}
