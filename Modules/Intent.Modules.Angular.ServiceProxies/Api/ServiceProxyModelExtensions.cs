using System.Linq;
using Intent.Modelers.Types.ServiceProxies.Api;
using Intent.Modelers.WebClient.Angular.Api;
using Intent.Modules.Common;

namespace Intent.Angular.ServiceProxies.Api
{
    public static class ServiceProxyModelExtensions
    {
        public static ModuleModel GetModule(this ServiceProxyModel model)
        {
            var module = new ModuleModel(model.InternalElement.GetParentPath().Reverse().First(x => x.SpecializationTypeId == ModuleModel.SpecializationTypeId));
            return module;
        }
    }
}