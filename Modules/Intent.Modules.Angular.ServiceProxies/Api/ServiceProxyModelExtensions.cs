using System.Linq;
using Intent.Engine;
using Intent.Metadata.Models;
using Intent.Modelers.Types.ServiceProxies.Api;
using Intent.Modules.Angular.Shared;
using Intent.Modules.Common;
using Intent.Modules.Common.Templates;

namespace Intent.Modules.Angular.ServiceProxies.Api;

internal static class ServiceProxyModelExtensions
{
    public static (IElement? Element, string? Name) GetModule(
        this ServiceProxyModel model,
        ISoftwareFactoryExecutionContext executionContext)
    {
        var moduleElement = model.InternalElement.GetParentPath().Reverse()
            .FirstOrDefault(x => x.SpecializationTypeId == SpecializationTypeIds.Angular.Module);

        if (moduleElement == null)
        {
            var moduleElements = executionContext.MetadataManager.WebClient(executionContext.GetApplicationConfig().Id)
                .GetElementsOfType(SpecializationTypeIds.Angular.Module)
                .ToArray();

            moduleElement = moduleElements
                .FirstOrDefault(x => x.Name == "AppComponent");

            moduleElement ??= moduleElements
                .FirstOrDefault(x => x.ChildElements
                    .Any(componentElement =>
                    {
                        if (componentElement.SpecializationTypeId is not SpecializationTypeIds.WebClient.Angular.Component ||
                            !componentElement.HasStereotype(Stereotypes.AngularComponent.Id))
                        {
                            return false;
                        }

                        var selector = componentElement.GetStereotypeProperty<string>(
                            Stereotypes.AngularComponent.Id, Stereotypes.AngularComponent.Properties.Selector);

                        return selector == "app-root";
                    }));
        }

        return (moduleElement, moduleElement?.Name.RemoveSuffix("Module"));
    }
}