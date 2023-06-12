using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Intent.Modules.Common.Plugins;
using Intent.SoftwareFactory;
using Intent.Engine;
using Intent.Modules.Angular.Templates.Component.AngularComponentHtml;
using Intent.Modules.Common.Templates;
using Intent.Plugins.FactoryExtensions;
using Intent.Utils;
using Newtonsoft.Json;

namespace Intent.Modules.Angular
{
    [Description("Angular NgxBootrap Installer")]
    public class AngularCliNgxBootstrapInstaller : FactoryExtensionBase, IExecutionLifeCycle
    {
        public override int Order => 200;

        protected override void OnBeforeTemplateExecution(IApplication application)
        {
            base.OnBeforeTemplateExecution(application);

            var outputTarget = CliCommand.GetFrontEndOutputTarget(application);
            if (outputTarget == null)
            {
                Logging.Log.Warning("Could not find a location to install ngx-bootstrap. Ensure that a Web Client package has been created.");
                return;
            }
            if (!IsNgxBootstrapInstalled(outputTarget.Location))
            {
                Logging.Log.Info($"Installing Ngx-Bootstrap into Angular app at location [{outputTarget.Location}]");

                // Needed to force it to use legacy peer deps which is required for 10.0.2 with Angular 16.
                CliCommand.Run(outputTarget.Location, "echo legacy-peer-deps=true > .npmrc");
                CliCommand.Run(outputTarget.Location, $@"ng add ngx-bootstrap@10.2.0");
                CliCommand.Run(outputTarget.Location, $@"npm i ngx-bootstrap@10.2.0 --save-dev");

                var appComponent = application.FindTemplateInstance(AngularComponentHtmlTemplate.TemplateId, t => t.GetMetadata().FileName == "app.component");
                if (appComponent != null)
                {
                    if (File.Exists(appComponent.GetMetadata().GetFilePath()))
                    {
                        Logging.Log.Info($"Overriding app.component.html file.");
                        File.WriteAllText(appComponent.GetMetadata().GetFilePath(), "");
                    }
                }
            }
            else
            {
                Logging.Log.Info("Ngx-Bootstrap app already installed. Skipping installation");
            }
        }

        public override string Id => "Intent.Angular.CLIInstaller";

        public bool IsNgxBootstrapInstalled(string appLocation)
        {
            if (!File.Exists($@"{appLocation}/package.json"))
            {
                return true;
            }
            using (var file = File.OpenText($@"{appLocation}/package.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                var packageFile = (dynamic)serializer.Deserialize(new JsonTextReader(file));
                return packageFile.dependencies["ngx-bootstrap"] != null;
            }
        }
    }
}
