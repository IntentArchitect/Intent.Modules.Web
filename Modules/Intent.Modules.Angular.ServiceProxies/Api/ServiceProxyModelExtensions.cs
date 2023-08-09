using System.Linq;
using Intent.Metadata.Models;
using Intent.Modelers.Services.Api;
using Intent.Modelers.Types.ServiceProxies.Api;
using Intent.Modelers.WebClient.Angular.Api;
using Intent.Modules.Common;

namespace Intent.Angular.ServiceProxies.Api
{
    public static class ServiceProxyModelExtensions
    {
        public static ModuleModel GetModule(this ServiceProxyModel model) => model.InternalElement.GetModule();

        public static ModuleModel GetModule(this DTOModel model) => model.InternalElement.GetModule();

        private static ModuleModel GetModule(this IElement element)
        {
            var module = new ModuleModel(element.GetParentPath().Reverse().First(x => x.SpecializationTypeId == ModuleModel.SpecializationTypeId));
            return module;
        }
    }
}