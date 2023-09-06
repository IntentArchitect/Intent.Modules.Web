using System.Linq;
using Intent.Angular.Api;
using Intent.Engine;
using Intent.Modelers.Types.ServiceProxies.Api;
using Intent.Modelers.WebClient.Angular.Api;
using Intent.Modelers.WebClient.Api;
using Intent.Modules.Common;

namespace Intent.Angular.ServiceProxies.Api
{
    public static class ServiceProxyModelExtensions
    {
        public static ModuleModel GetModule(
            this ServiceProxyModel model,
            ISoftwareFactoryExecutionContext executionContext)
        {
            var moduleElement = model.InternalElement.GetParentPath().Reverse()
                .FirstOrDefault(x => x.SpecializationTypeId == ModuleModel.SpecializationTypeId);

            if (moduleElement == null)
            {
                var modules = executionContext.MetadataManager.WebClient(executionContext.GetApplicationConfig().Id)
                    .GetModuleModels();

                moduleElement = modules
                    .FirstOrDefault(x => x.Name == "AppComponent")?
                    .InternalElement;

                moduleElement ??= modules
                    .First(x => x.Components.Any(c => c.GetAngularComponentSettings()?.Selector() == "app-root"))
                    .InternalElement;

            }

            var module = new ModuleModel(moduleElement);

            return module;
        }
    }
}