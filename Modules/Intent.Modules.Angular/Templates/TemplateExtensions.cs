using System.Collections.Generic;
using Intent.Modules.Angular.Templates.Core.AngularConfig;
using Intent.Modules.Angular.Templates.Core.AngularRoutes;
using Intent.Modules.Angular.Templates.Core.AppComponent;
using Intent.Modules.Angular.Templates.Environment.Environment;
using Intent.Modules.Angular.Templates.Environment.EnvironmentDotDevelopment;
using Intent.Modules.Common.Templates;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: DefaultIntentManaged(Mode.Fully, Targets = Targets.Usings)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateExtensions", Version = "1.0")]

namespace Intent.Modules.Angular.Templates
{
    public static class TemplateExtensions
    {
        public static string GetAngularConfigTemplateName(this IIntentTemplate template)
        {
            return template.GetTypeName(AngularConfigTemplate.TemplateId);
        }

        public static string GetAngularRoutesTemplateName(this IIntentTemplate template)
        {
            return template.GetTypeName(AngularRoutesTemplate.TemplateId);
        }

        public static string GetAppComponentTemplateName(this IIntentTemplate template)
        {
            return template.GetTypeName(AppComponentTemplate.TemplateId);
        }
        public static string GetEnvironmentTemplateName(this IIntentTemplate template)
        {
            return template.GetTypeName(EnvironmentTemplate.TemplateId);
        }

        public static string GetEnvironmentDotDevelopmentTemplateName(this IIntentTemplate template)
        {
            return template.GetTypeName(EnvironmentDotDevelopmentTemplate.TemplateId);
        }

    }
}