using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Intent.Modules.Common.Plugins;
using Intent.SoftwareFactory;
using Intent.Engine;
using Intent.Modelers.WebClient.Angular.Api;
using Intent.Modelers.WebClient.Api;
using Intent.Plugins.FactoryExtensions;
using Intent.Utils;
using Newtonsoft.Json;

namespace Intent.Modules.Angular
{
    [Description("Angular CLI Installer")]
    public class AngularCliInstaller : FactoryExtensionBase, IExecutionLifeCycle
    {
        private readonly IMetadataManager _metadataManager;
        private readonly IList<CliInstallationRequest> _installationRequests = new List<CliInstallationRequest>();
        public override int Order => 100;

        public AngularCliInstaller(IMetadataManager metadataManager)
        {
            _metadataManager = metadataManager;
        }

        protected override void OnAfterMetadataLoad(IApplication application)
        {
            base.OnAfterMetadataLoad(application);

            if (!_metadataManager.WebClient(application).GetModuleModels().Any(x => x.IsRootModule() && x.Name == "AppModule"))
            {
                throw new Exception("No Web Client Package found. Create a new Package and Root Module 'AppModule' in the Web Client designer.");
            }
            application.EventDispatcher.Subscribe<CliInstallationRequest>(request => _installationRequests.Add(request));
        }

        protected override void OnBeforeTemplateExecution(IApplication application)
        {
            base.OnBeforeTemplateExecution(application);

            var outputTarget = CliCommand.GetFrontEndOutputTarget(application);
            if (outputTarget == null)
            {
                Logging.Log.Warning("Could not find a location to install Angular application. Ensure that a Web Client package has been created.");
                return;
            }

            if (!AngularInstalled(application))
            {
                Logging.Log.Info($"Installing Angular into project: [{outputTarget.Name}]");
                CliCommand.Run(outputTarget.Location, $"ng new {application.Name} --directory=. --skip-git --style=scss --interactive=false --force=true");
                CliCommand.Run(outputTarget.Location, "ng add @cypress/schematic --interactive=false --skip-confirmation=true --force=true");
                CliCommand.Run(outputTarget.Location, "npm i @cypress/schematic --save-dev");
                CliCommand.Run(outputTarget.Location, "ng generate environments");
            }
            else
            {
                Logging.Log.Info("Angular app already installed. Skipping Angular CLI installation");
            }

            foreach (var installationRequest in _installationRequests)
            {
                if (!IsPackageInstalled(application, installationRequest.NpmPackageName))
                {
                    CliCommand.Run(outputTarget.Location, $"{installationRequest.Command}");
                }
            }
        }

        public override string Id => "Intent.Angular.CLIInstaller";

        public bool AngularInstalled(IApplication application)
        {
            var project = CliCommand.GetFrontEndOutputTarget(application);
            return project != null && File.Exists(Path.Combine(project.Location, "angular.json"));
        }

        public bool IsPackageInstalled(IApplication application, string package)
        {
            string appLocation = CliCommand.GetFrontEndOutputTarget(application).Location;
            if (!File.Exists($@"{appLocation}/package.json"))
            {
                return true;
            }
            using (var file = File.OpenText($@"{appLocation}/package.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                var packageFile = (dynamic)serializer.Deserialize(new JsonTextReader(file));
                return packageFile.dependencies[package] != null;
            }
        }
    }

    public class CliInstallationRequest
    {
        public CliInstallationRequest(string npmPackageName, string command)
        {
            NpmPackageName = npmPackageName;
            Command = command;
        }

        public string NpmPackageName { get; set; }
        public string Command { get; }
    }
}
