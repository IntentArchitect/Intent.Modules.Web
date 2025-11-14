using System.Collections.Generic;
using Intent.Modelers.Services.Api;
using Intent.Modelers.Types.ServiceProxies.Api;
using Intent.Modules.Angular.HttpClients.Templates.DtoContract;
using Intent.Modules.Angular.HttpClients.Templates.EnumContract;
using Intent.Modules.Angular.HttpClients.Templates.HttpServiceProxy;
using Intent.Modules.Angular.HttpClients.Templates.JsonResponse;
using Intent.Modules.Angular.HttpClients.Templates.PagedResult;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.Types.Api;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: DefaultIntentManaged(Mode.Fully, Targets = Targets.Usings)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateExtensions", Version = "1.0")]

namespace Intent.Modules.Angular.HttpClients.Templates
{
    public static class TemplateExtensions
    {
        public static string GetDtoContractTemplateName<T>(this IIntentTemplate<T> template) where T : DTOModel
        {
            return template.GetTypeName(DtoContractTemplate.TemplateId, template.Model);
        }

        public static string GetDtoContractTemplateName(this IIntentTemplate template, DTOModel model)
        {
            return template.GetTypeName(DtoContractTemplate.TemplateId, model);
        }

        public static string GetEnumContractTemplateName<T>(this IIntentTemplate<T> template) where T : EnumModel
        {
            return template.GetTypeName(EnumContractTemplate.TemplateId, template.Model);
        }

        public static string GetEnumContractTemplateName(this IIntentTemplate template, EnumModel model)
        {
            return template.GetTypeName(EnumContractTemplate.TemplateId, model);
        }

        public static string GetHttpServiceProxyTemplateName<T>(this IIntentTemplate<T> template) where T : ServiceProxyModel
        {
            return template.GetTypeName(HttpServiceProxyTemplate.TemplateId, template.Model);
        }

        public static string GetHttpServiceProxyTemplateName(this IIntentTemplate template, ServiceProxyModel model)
        {
            return template.GetTypeName(HttpServiceProxyTemplate.TemplateId, model);
        }

        public static string GetJsonResponseTemplateName(this IIntentTemplate template)
        {
            return template.GetTypeName(JsonResponseTemplate.TemplateId);
        }

        public static string GetPagedResultTemplateName(this IIntentTemplate template)
        {
            return template.GetTypeName(PagedResultTemplate.TemplateId);
        }

    }
}