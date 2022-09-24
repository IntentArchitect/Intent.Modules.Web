using System.IO;
using Intent.Engine;
using Intent.Modules.Common.Plugins;
using Intent.Plugins.FactoryExtensions;
using Intent.RoslynWeaver.Attributes;
using Intent.Utils;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.FactoryExtension", Version = "1.0")]

namespace Intent.Modules.Angular.ApiAuthorization.FactoryExtensions
{
    [IntentManaged(Mode.Merge)]
    public class NpmCliInstaller : FactoryExtensionBase, IExecutionLifeCycle
    {
        public override string Id => "Intent.Angular.ApiAuthorization.NpmCliInstaller";
        public override int Order => 0;

        [IntentManaged(Mode.Ignore)]
        public void OnStep(IApplication application, string step)
        {
            if (step == ExecutionLifeCycleSteps.AfterCommitChanges)
            {
                var outputTarget = CliCommand.GetFrontEndOutputTarget(application);
                if (outputTarget == null)
                {
                    Logging.Log.Warning("Could not find a location where the Angular application is installed. Ensure that a Web Client package has been created.");
                    return;
                }
                if (!Directory.Exists(Path.Combine(outputTarget.Location, "node_modules", "oidc-client")))
                {
                    CliCommand.Run(outputTarget.Location, $@"npm i oidc-client");
                }
            }
        }
    }
}