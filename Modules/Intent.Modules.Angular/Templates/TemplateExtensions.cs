using System.Collections.Generic;
using Intent.Modelers.UI.Api;
using Intent.Modules.Angular.Templates.Component.ComponentTypeScript;
using Intent.Modules.Angular.Templates.Component.LayoutComponentTypescript;
using Intent.Modules.Angular.Templates.Core.AppComponent;
using Intent.Modules.Angular.Templates.Core.AppConfig;
using Intent.Modules.Angular.Templates.Core.AppRoutes;
using Intent.Modules.Angular.Templates.Environment.Environment;
using Intent.Modules.Angular.Templates.Environment.EnvironmentDotDevelopment;
using Intent.Modules.Angular.Templates.Shared.IntentDecorators;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: DefaultIntentManaged(Mode.Merge, Targets = Targets.Usings)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateExtensions", Version = "1.0")]

namespace Intent.Modules.Angular.Templates
{
    public static class TemplateExtensions
    {
        public static string GetComponentTypeScriptTemplateName<T>(this IIntentTemplate<T> template) where T : ComponentModel
        {
            return template.GetTypeName(ComponentTypeScriptTemplate.TemplateId, template.Model);
        }

        public static string GetComponentTypeScriptTemplateName(this IIntentTemplate template, ComponentModel model)
        {
            return template.GetTypeName(ComponentTypeScriptTemplate.TemplateId, model);
        }

        public static string GetLayoutComponentTypescriptTemplateName<T>(this IIntentTemplate<T> template) where T : LayoutModel
        {
            return template.GetTypeName(LayoutComponentTypescriptTemplate.TemplateId, template.Model);
        }

        public static string GetLayoutComponentTypescriptTemplateName(this IIntentTemplate template, LayoutModel model)
        {
            return template.GetTypeName(LayoutComponentTypescriptTemplate.TemplateId, model);
        }

        public static string GetAppComponentTemplateName(this IIntentTemplate template)
        {
            return template.GetTypeName(AppComponentTemplate.TemplateId);
        }

        public static string GetAppConfigTemplateName(this IIntentTemplate template)
        {
            return template.GetTypeName(AppConfigTemplate.TemplateId);
        }

        public static string GetAppRoutesTemplateName(this IIntentTemplate template)
        {
            return template.GetTypeName(AppRoutesTemplate.TemplateId);
        }
        public static string GetEnvironmentTemplateName(this IIntentTemplate template)
        {
            return template.GetTypeName(EnvironmentTemplate.TemplateId);
        }

        public static string GetEnvironmentDotDevelopmentTemplateName(this IIntentTemplate template)
        {
            return template.GetTypeName(EnvironmentDotDevelopmentTemplate.TemplateId);
        }

        public static string GetIntentDecoratorsTemplateName(this IIntentTemplate template)
        {
            return template.GetTypeName(IntentDecoratorsTemplate.TemplateId);
        }

        [IntentIgnore]
        public static string UseType<T>(this TypeScriptTemplateBase<T> template, string type, string location)
        {
            template.AddImport(type, location);
            return type;
        }

    }
}