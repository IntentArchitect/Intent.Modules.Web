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
using System.Linq;
using Intent.Modules.Angular.Templates.Core.StylesDotScssFile;
using Intent.Modules.Common.TypeScript.Templates;

namespace Intent.Modules.Angular
{
    [Description("Angular NgxBootrap Installer")]
    public class AngularCliNgxBootstrapInstaller : FactoryExtensionBase, IExecutionLifeCycle
    {
        public override int Order => 200;

        protected override void OnBeforeTemplateExecution(IApplication application)
        {
            base.OnBeforeTemplateExecution(application);

            var outputTarget = application.OutputTargets.FirstOrDefault(x => x.HasRole("Front End"));
            if (outputTarget == null)
            {
                Logging.Log.Warning("Could not find a location to install ngx-bootstrap. Ensure that a Web Client package has been created.");
                return;
            }
            if (!IsNgxBootstrapInstalled(outputTarget.Location))
            {
                Logging.Log.Info($"Installing Ngx-Bootstrap into Angular app at location [{outputTarget.Location}]");

                application.EventDispatcher.Publish(new NpmPackageDependency("bootstrap", "^5.2.3"));
                application.EventDispatcher.Publish(new NpmPackageDependency("ngx-bootstrap", "^11.0.2"));
                application.EventDispatcher.Publish(new StyleRequest("@import \"./node_modules/bootstrap/scss/bootstrap\";"));
                application.EventDispatcher.Publish(new StyleRequest("@import \"node_modules/ngx-bootstrap/datepicker/bs-datepicker\";"));

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
                return packageFile.dependencies["ngx-bootstrap"] is not null ||
                       packageFile.devDependencies["ngx-bootstrap"] is not null;
            }
        }
    }
}
