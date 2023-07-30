using System.Collections.Generic;
using Intent.Modules.Angular.Templates.Component.AngularComponentTs;
using Intent.Modules.Angular.Templates.Core.ApiService;
using Intent.Modules.Angular.Templates.Core.CoreModule;
using Intent.Modules.Angular.Templates.Environment.Environment;
using Intent.Modules.Angular.Templates.Environment.EnvironmentDotDevelopment;
using Intent.Modules.Angular.Templates.Model.FormGroup;
using Intent.Modules.Angular.Templates.Model.Model;
using Intent.Modules.Angular.Templates.Module.AngularModule;
using Intent.Modules.Angular.Templates.Module.AngularResolver;
using Intent.Modules.Angular.Templates.Module.AngularRoutingModule;
using Intent.Modules.Angular.Templates.Shared.IntentDecorators;
using Intent.Modules.Common.Templates;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: DefaultIntentManaged(Mode.Fully, Targets = Targets.Usings)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateExtensions", Version = "1.0")]

namespace Intent.Modules.Angular.Templates
{
    public static class TemplateExtensions
    {
        public static string GetAngularComponentTsName<T>(this IIntentTemplate<T> template) where T : Intent.Modelers.WebClient.Angular.Api.ComponentModel
        {
            return template.GetTypeName(AngularComponentTsTemplate.TemplateId, template.Model);
        }

        public static string GetAngularComponentTsName(this IIntentTemplate template, Intent.Modelers.WebClient.Angular.Api.ComponentModel model)
        {
            return template.GetTypeName(AngularComponentTsTemplate.TemplateId, model);
        }

        public static string GetApiServiceName(this IIntentTemplate template)
        {
            return template.GetTypeName(ApiServiceTemplate.TemplateId);
        }

        public static string GetCoreModuleName(this IIntentTemplate template)
        {
            return template.GetTypeName(CoreModuleTemplate.TemplateId);
        }

        public static string GetEnvironmentName(this IIntentTemplate template)
        {
            return template.GetTypeName(EnvironmentTemplate.TemplateId);
        }

        public static string GetEnvironmentDotDevelopmentName(this IIntentTemplate template)
        {
            return template.GetTypeName(EnvironmentDotDevelopmentTemplate.TemplateId);
        }

        public static string GetFormGroupName<T>(this IIntentTemplate<T> template) where T : Intent.Modelers.WebClient.Angular.Api.FormGroupDefinitionModel
        {
            return template.GetTypeName(FormGroupTemplate.TemplateId, template.Model);
        }

        public static string GetFormGroupName(this IIntentTemplate template, Intent.Modelers.WebClient.Angular.Api.FormGroupDefinitionModel model)
        {
            return template.GetTypeName(FormGroupTemplate.TemplateId, model);
        }

        public static string GetModelName<T>(this IIntentTemplate<T> template) where T : Intent.Modelers.WebClient.Angular.Api.ModelDefinitionModel
        {
            return template.GetTypeName(ModelTemplate.TemplateId, template.Model);
        }

        public static string GetModelName(this IIntentTemplate template, Intent.Modelers.WebClient.Angular.Api.ModelDefinitionModel model)
        {
            return template.GetTypeName(ModelTemplate.TemplateId, model);
        }

        public static string GetAngularModuleName<T>(this IIntentTemplate<T> template) where T : Intent.Modelers.WebClient.Angular.Api.ModuleModel
        {
            return template.GetTypeName(AngularModuleTemplate.TemplateId, template.Model);
        }

        public static string GetAngularModuleName(this IIntentTemplate template, Intent.Modelers.WebClient.Angular.Api.ModuleModel model)
        {
            return template.GetTypeName(AngularModuleTemplate.TemplateId, model);
        }

        public static string GetAngularResolverName<T>(this IIntentTemplate<T> template) where T : Intent.Modelers.WebClient.Angular.Api.ResolverModel
        {
            return template.GetTypeName(AngularResolverTemplate.TemplateId, template.Model);
        }

        public static string GetAngularResolverName(this IIntentTemplate template, Intent.Modelers.WebClient.Angular.Api.ResolverModel model)
        {
            return template.GetTypeName(AngularResolverTemplate.TemplateId, model);
        }

        public static string GetAngularRoutingModuleName<T>(this IIntentTemplate<T> template) where T : Intent.Modelers.WebClient.Angular.Api.RoutingModel
        {
            return template.GetTypeName(AngularRoutingModuleTemplate.TemplateId, template.Model);
        }

        public static string GetAngularRoutingModuleName(this IIntentTemplate template, Intent.Modelers.WebClient.Angular.Api.RoutingModel model)
        {
            return template.GetTypeName(AngularRoutingModuleTemplate.TemplateId, model);
        }

        public static string GetIntentDecoratorsName(this IIntentTemplate template)
        {
            return template.GetTypeName(IntentDecoratorsTemplate.TemplateId);
        }

    }
}