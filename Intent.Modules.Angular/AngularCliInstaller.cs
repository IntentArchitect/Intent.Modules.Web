using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Intent.Modules.Common.Plugins;
using Intent.Modules.Constants;
using Intent.SoftwareFactory;
using Intent.Engine;
using Intent.Modules.Common.VisualStudio;
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
                    var outputTarget = CliCommand.GetWebCoreProject(application);
                    if (outputTarget == null)
                    {
                        Logging.Log.Failure("Could not find project to install Angular application.");
                        return;
                    }
                    Logging.Log.Info($"Installing Angular into project: [{ outputTarget.Name }]");
                    CliCommand.Run(outputTarget.Location, $@"ng new {application.Name} --directory ClientApp --minimal --defaults --skipGit=true --force=true");
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
            var project = CliCommand.GetWebCoreProject(application);
            return project != null && File.Exists(Path.Combine(project.Location, "ClientApp", "angular.json"));
        }
    }
}
