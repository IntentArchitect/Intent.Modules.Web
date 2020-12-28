using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Intent.Modules.Common.Plugins;
using Intent.SoftwareFactory;
using Intent.Engine;
using Intent.Plugins.FactoryExtensions;
using Intent.Utils;

namespace Intent.Modules.Angular
{
    [Description("Angular CLI Installer")]
    public class AngularCliInstaller : FactoryExtensionBase, IExecutionLifeCycle
    {
        public override int Order => 100;

        public void OnStep(IApplication application, string step)
        {
            if (step == ExecutionLifeCycleSteps.BeforeTemplateExecution)
            {
                if (!AngularInstalled(application))
                {
                    var outputTarget = CliCommand.GetFrontEndOutputTarget(application);
                    if (outputTarget == null)
                    {
                        Logging.Log.Warning("Could not find a location to install Angular application. Ensure that a Web Client package has been created.");
                        return;
                    }
                    Logging.Log.Info($"Installing Angular into project: [{ outputTarget.Name }]");
                    CliCommand.Run(outputTarget.Location, $@"npm i @angular/cli@8 --save-dev"); // Ensure this version - typescript fix
                    // add --skipInstall to skip running the npm i
                    CliCommand.Run(outputTarget.Location, $@"ng new {application.Name} --directory=. --skipGit --style=scss --interactive=false --force=true");
                    CliCommand.Run(outputTarget.Location, $@"npm i @types/node@8.10.52"); // Ensure this version - typescript fix
                }
                else
                {
                    Logging.Log.Info("Angular app already installed. Skipping Angular CLI installation");
                }
            }
        }

        public override string Id => "Intent.Angular.CLIInstaller";

        public bool AngularInstalled(IApplication application)
        {
            var project = CliCommand.GetFrontEndOutputTarget(application);
            return project != null && File.Exists(Path.Combine(project.Location, "angular.json"));
        }
    }
}
