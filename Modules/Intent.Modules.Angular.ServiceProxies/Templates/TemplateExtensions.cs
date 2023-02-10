using System.Collections.Generic;
using Intent.Modules.Angular.ServiceProxies.Templates.Proxies.AngularDTO;
using Intent.Modules.Angular.ServiceProxies.Templates.Proxies.AngularServiceProxy;
using Intent.Modules.Angular.ServiceProxies.Templates.Proxies.JsonResponse;
using Intent.Modules.Common.Templates;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: DefaultIntentManaged(Mode.Fully, Targets = Targets.Usings)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateExtensions", Version = "1.0")]

namespace Intent.Modules.Angular.ServiceProxies.Templates
{
    public static class TemplateExtensions
    {
        public static string GetAngularDTOName<T>(this IntentTemplateBase<T> template) where T : Intent.Modelers.Types.ServiceProxies.Api.ServiceProxyDTOModel
        {
            return template.GetTypeName(AngularDTOTemplate.TemplateId, template.Model);
        }

        public static string GetAngularDTOName(this IntentTemplateBase template, Intent.Modelers.Types.ServiceProxies.Api.ServiceProxyDTOModel model)
        {
            return template.GetTypeName(AngularDTOTemplate.TemplateId, model);
        }

        public static string GetAngularServiceProxyName<T>(this IntentTemplateBase<T> template) where T : Intent.Modelers.Types.ServiceProxies.Api.ServiceProxyModel
        {
            return template.GetTypeName(AngularServiceProxyTemplate.TemplateId, template.Model);
        }

        public static string GetAngularServiceProxyName(this IntentTemplateBase template, Intent.Modelers.Types.ServiceProxies.Api.ServiceProxyModel model)
        {
            return template.GetTypeName(AngularServiceProxyTemplate.TemplateId, model);
        }

        public static string GetJsonResponseName<T>(this IntentTemplateBase<T> template)
        {
            return template.GetTypeName(JsonResponseTemplate.TemplateId);
        }

    }
}