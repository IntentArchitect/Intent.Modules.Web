using Intent.Engine;
using Intent.Eventing;
using Intent.Modules.Angular.Components.Material.Settings;
using Intent.Modules.Angular.Settings;
using Intent.Modules.Common;
using Intent.Modules.Common.FileBuilders.DataFileBuilder;
using Intent.Modules.Common.Plugins;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.TypeScript.Events;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.Plugins.FactoryExtensions;
using Intent.RoslynWeaver.Attributes;
using System.Linq;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.FactoryExtension", Version = "1.0")]

namespace Intent.Modules.Angular.Components.Material.FactoryExtensions
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class MaterialConfiguration : FactoryExtensionBase
    {
        public override string Id => "Intent.Angular.Components.Material.MaterialConfiguration";

        [IntentManaged(Mode.Ignore)]
        public override int Order => 0;

        /// <summary>
        /// This is an example override which would extend the
        /// <see cref="ExecutionLifeCycleSteps.AfterTemplateRegistrations"/> phase of the Software Factory execution.
        /// See <see cref="FactoryExtensionBase"/> for all available overrides.
        /// </summary>
        /// <remarks>
        /// It is safe to update or delete this method.
        /// </remarks>
        protected override void OnAfterTemplateRegistrations(IApplication application)
        {
            application.EventDispatcher.Publish(new NpmPackageDependency("@angular/animations", "^19.2.15"));
            application.EventDispatcher.Publish(new NpmPackageDependency("@angular/material", "^19.2.19"));

            application.EventDispatcher.Publish(new ClientResourceConfigurationRequestEvent("stylesheet", "https://fonts.googleapis.com/css?family=Roboto:300,400,500,700&display=swap"));
            application.EventDispatcher.Publish(new ClientResourceConfigurationRequestEvent("stylesheet", "https://fonts.googleapis.com/icon?family=Material+Icons"));

            application.EventDispatcher.Publish(new ServiceConfigurationRequestEvent("provideAnimations", "@angular/platform-browser/animations"));

            var angJsonTemplate = application.FindTemplateInstance<IDataFileBuilderTemplate>("Intent.Angular.Core.AngularDotJsonFileTemplate");

            angJsonTemplate.DataFile.AfterBuild(file =>
            {
                if (!(file.RootObject.ContainsKey("projects")))
                {
                    return;
                }
                var projectsNode = angJsonTemplate.DataFile?.RootObject["projects"] as IDataFileObjectValue;

                if (!projectsNode.ContainsKey(application.GetApplicationConfig().Name.ToCamelCase()))
                {
                    return;
                }
                var applicationNode = projectsNode[application.GetApplicationConfig().Name.ToCamelCase()] as IDataFileObjectValue;

                if (!applicationNode.ContainsKey("architect"))
                {
                    return;
                }
                var architectNode = applicationNode["architect"] as IDataFileObjectValue;

                UpdateStylesNode("build", application, architectNode);
                UpdateStylesNode("test", application, architectNode);
            });
        }

        private static void UpdateStylesNode(string key, IApplication application, IDataFileObjectValue architectNode)
        {
            if (!architectNode.ContainsKey(key))
            {
                return;
            }
            var keyNode = architectNode[key] as IDataFileObjectValue;

            if (!keyNode.ContainsKey("options"))
            {
                return;
            }
            var optionsNode = keyNode["options"] as IDataFileObjectValue;

            if (!optionsNode.ContainsKey("styles"))
            {
                return;
            }
            var stylesNode = optionsNode["styles"] as IDataFileArrayValue;

            var materialTheme = application.Settings.GetAngularSettings().MaterialTheme();
            if (materialTheme is not null && materialTheme.Value is not null && materialTheme.AsEnum() != AngularSettingsExtensions.MaterialThemeOptionsEnum.Custom)
            {
                var materialCssLocation = $"node_modules/@angular/material/prebuilt-themes/{materialTheme.Value}.css";

                if (!stylesNode.Contains(new DataFileScalarValue(materialCssLocation)))
                {
                    stylesNode.WithValue(materialCssLocation);
                }
            }
        }
    }
}