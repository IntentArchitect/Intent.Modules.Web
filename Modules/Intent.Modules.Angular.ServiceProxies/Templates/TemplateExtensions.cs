using System.Collections.Generic;
using Intent.Modelers.Services.Api;
using Intent.Modelers.Types.ServiceProxies.Api;
using Intent.Modules.Angular.ServiceProxies.Templates.AngularDTO;
using Intent.Modules.Angular.ServiceProxies.Templates.AngularServiceProxy;
using Intent.Modules.Angular.ServiceProxies.Templates.ApiService;
using Intent.Modules.Angular.ServiceProxies.Templates.Environment;
using Intent.Modules.Angular.ServiceProxies.Templates.JsonResponse;
using Intent.Modules.Angular.ServiceProxies.Templates.PagedResult;
using Intent.Modules.Common.Templates;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: DefaultIntentManaged(Mode.Fully, Targets = Targets.Usings)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateExtensions", Version = "1.0")]

namespace Intent.Modules.Angular.ServiceProxies.Templates
{
    public static class TemplateExtensions
    {
        public static string GetAngularDTOName<T>(this IIntentTemplate<T> template) where T : DTOModel
        {
            return template.GetTypeName(AngularDTOTemplate.TemplateId, template.Model);
        }

        public static string GetAngularDTOName(this IIntentTemplate template, DTOModel model)
        {
            return template.GetTypeName(AngularDTOTemplate.TemplateId, model);
        }

        public static string GetAngularServiceProxyName<T>(this IIntentTemplate<T> template) where T : ServiceProxyModel
        {
            return template.GetTypeName(AngularServiceProxyTemplate.TemplateId, template.Model);
        }

        public static string GetAngularServiceProxyName(this IIntentTemplate template, ServiceProxyModel model)
        {
            return template.GetTypeName(AngularServiceProxyTemplate.TemplateId, model);
        }

        public static string GetApiServiceName(this IIntentTemplate template)
        {
            return template.GetTypeName(ApiServiceTemplate.TemplateId);
        }

        public static string GetEnvironmentName(this IIntentTemplate template)
        {
            return template.GetTypeName(EnvironmentTemplate.TemplateId);
        }

        public static string GetJsonResponseName(this IIntentTemplate template)
        {
            return template.GetTypeName(JsonResponseTemplate.TemplateId);
        }

        public static string GetPagedResultName(this IIntentTemplate template)
        {
            return template.GetTypeName(PagedResultTemplate.TemplateId);
        }

    }
}