using System.Linq;
using System.Text.RegularExpressions;
using Intent.Engine;
using Intent.Eventing;
using Intent.Modules.Angular.Components.Material.Settings;
using Intent.Modules.Angular.Settings;
using Intent.Modules.Angular.Templates.Core.ThemeDotScssFile;
using Intent.Modules.Common;
using Intent.Modules.Common.FileBuilders.DataFileBuilder;
using Intent.Modules.Common.Plugins;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.TypeScript.Events;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.Plugins.FactoryExtensions;
using Intent.RoslynWeaver.Attributes;
using static System.Net.Mime.MediaTypeNames;

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

            // only set all the content themes if both colors are not custom
            if (application.Settings.GetAngularSettings().PrimaryColor().AsEnum() != AngularSettingsExtensions.PrimaryColorOptionsEnum.Custom &&
                application.Settings.GetAngularSettings().AccentColor().AsEnum() != AngularSettingsExtensions.AccentColorOptionsEnum.Custom)
            {
                var themeTemplate = application.FindTemplateInstance<ThemeDotScssFileTemplate>(TemplateDependency.OnTemplate(ThemeDotScssFileTemplate.TemplateId));
                SetThemeContent(themeTemplate, application);
            }
        }

        private static void SetThemeContent(ThemeDotScssFileTemplate template, IApplication application)
        {
            // Parse and replace the existing content
            var primaryColor = application.Settings.GetAngularSettings().PrimaryColor().Value;
            var accentColor = application.Settings.GetAngularSettings().AccentColor().Value;

            // if there is no existing content, then output the file as per the ThemeDefaultStyle
            // with the themese selected
            if (!template.TryGetExistingFileContent(out var existingFileContent))
            {
                template.SetContent(string.Format(ThemeDefaultStyle, primaryColor, accentColor));
            }
            else
            {
                var updatedContent = UpdatePaletteReferences(existingFileContent, primaryColor, accentColor);
                template.SetContent(updatedContent);
            }
        }

        private static string UpdatePaletteReferences(string content, string primaryColor, string accentColor)
        {
            // Replace $my-primary palette reference
            content = FactoryRegex.PrimaryPaletteRegex().Replace(content, $"$my-primary: mat.m2-define-palette(mat.$m2-{primaryColor}-palette);");

            // Replace $my-accent palette reference
            content = FactoryRegex.AccentPaletteRegex().Replace(content, $"$my-accent: mat.m2-define-palette(mat.$m2-{accentColor}-palette);");

            return content;
        }

        private const string ThemeDefaultStyle = @"@use '@angular/material' as mat;

// Define your Material theme
$my-primary: mat.m2-define-palette(mat.$m2-blue-palette);
$my-accent: mat.m2-define-palette(mat.$m2-deep-orange-palette);
$my-warn: mat.m2-define-palette(mat.$m2-red-palette);

$my-theme: mat.m2-define-light-theme((
  color: (
    primary: $my-primary,
    accent: $my-accent,
    warn: $my-warn,
  ),
  typography: mat.m2-define-typography-config(),
  density: 0,
));

// Extract colors from Material theme
$primary-color: mat.m2-get-color-from-palette($my-primary, 500);
$primary-dark: mat.m2-get-color-from-palette($my-primary, 700);
$primary-light: mat.m2-get-color-from-palette($my-primary, 300);
$accent-color: mat.m2-get-color-from-palette($my-accent, 500);
$accent-dark: mat.m2-get-color-from-palette($my-accent, 700);
$accent-light: mat.m2-get-color-from-palette($my-accent, 300);
$warn-color: mat.m2-get-color-from-palette($my-warn, 500);
$warn-light: mat.m2-get-color-from-palette($my-warn, 50);
";


    }
}