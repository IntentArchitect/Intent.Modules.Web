using Intent.Engine;
using Intent.Metadata.Models;
using Intent.Metadata.WebApi.Api;
using Intent.Modelers.Services.Api;
using Intent.Modelers.Types.ServiceProxies.Api;
using Intent.Modules.Common;
using Intent.Modules.Metadata.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intent.Modules.Angular.HttpClients.Templates.Helper;

public static class ServiceProxyHelper
{

    public static IList<IServiceProxyModel> GetServiceProxyModels(
        this IMetadataManager metadataManager,
        string applicationId,
        params Func<string, IDesigner>[] getDesigners)
    {
        var @explicit = getDesigners
            .SelectMany(getDesigner => getDesigner(applicationId).GetServiceProxyModels())
            .Select(IServiceProxyModel (p) => new ServiceProxyModelAdapter(p))
            .Where(x => x.Endpoints.Count > 0);

        var @implicit = metadataManager.GetImplicitHttpProxyEndpoints(applicationId, getDesigners)
            .Select(IServiceProxyModel (x) => new ImplicitServiceProxyModel(x));

        return @explicit
            .Concat(@implicit)
            .OrderBy(x => x.Name)
            .ThenBy(x => x.Id)
            .ToArray();
    }

    public static IList<IServiceContractModel> GetServiceContractModels(
        this IMetadataManager metadataManager,
        string applicationId,
        params Func<string, IDesigner>[] getDesigners)
    {
        var @explicit = getDesigners
            .SelectMany(getDesigner => getDesigner(applicationId).GetServiceProxyModels())
            .Select(IServiceContractModel (p) => new HttpServiceContractModel(p))
            .Where(x => x.Operations.Count > 0);

        var @implicit = GetImplicitHttpProxyEndpoints(metadataManager, applicationId, getDesigners)
            .Select(IServiceContractModel (x) => new ImplicitServiceProxyContractModel(x));

        return @explicit
            .Concat(@implicit)
            .OrderBy(x => x.Name)
            .ThenBy(x => x.Id)
            .ToArray();
    }

    public static IEnumerable<IHttpEndpointModel> GetMappedEndpoints(this ServiceProxyModel model)
    {
        // Backwards compatibility - when we didn't have operations on service proxies
        if (model.Mapping?.Element?.IsServiceModel() == true && !model.Operations.Any())
        {
            return model.MappedService.Operations
                .Where(x => x.HasHttpSettings())
                .Select(x => HttpEndpointModelFactory.GetEndpoint(x.InternalElement));
        }

        return model.Operations
            .Select(x => x.Mapping?.Element)
            .Cast<IElement>()
            .Where(x => x?.TryGetHttpSettings(out _) == true)
            .Select(HttpEndpointModelFactory.GetEndpoint);
    }

    /// <summary>
    /// Returns implicit http proxy endpoints (those with Invocation associations direct to the
    /// service without going via a service proxy element) grouped by their parent.
    /// </summary>
    public static IReadOnlyList<IReadOnlyList<IElement>> GetImplicitHttpProxyEndpoints(
        this IMetadataManager metadataManager,
        string applicationId,
        params Func<string, IDesigner>[] getDesigners)
    {
        var designers = getDesigners
            .Select(getDesigner => getDesigner(applicationId))
            .ToArray();

        const string callServiceOperationTypeId = "3e69085c-fa2f-44bd-93eb-41075fd472f8";
        const string uiCallServiceOperationTypeId = "fe5a5cd8-aabd-472f-8d42-f5c233e658dc";

        var localPackageIds = designers
            .SelectMany(x => x.Packages)
            .Select(x => x.Id)
            .ToHashSet();

        return Enumerable.Empty<IAssociation>()
            .Concat(designers.SelectMany(x => x.GetAssociationsOfType(callServiceOperationTypeId)))
            .Concat(designers.SelectMany(x => x.GetAssociationsOfType(uiCallServiceOperationTypeId)))
            .Where(x =>
            {
                var targetElement = x.TargetEnd.TypeReference?.Element as IElement;

                return targetElement?.Package.Id != null &&
                       !localPackageIds.Contains(targetElement.Package.Id) &&
                       targetElement.HasHttpSettings();
            })
            .Select(x => (IElement)x.TargetEnd.TypeReference.Element)
            .DistinctBy(x => x.Id)
            .OrderBy(x => x.ParentElement?.Name)
            .ThenBy(x => x.ParentElement?.Id)
            .ThenBy(x => x.Name)
            .ThenBy(x => x.Id)
            .GroupBy(x => x.ParentId)
            .Select(x => x.ToArray())
            .ToArray();
    }

    public const string HttpSettingsDefinitionId = "b4581ed2-42ec-4ae2-83dd-dcdd5f0837b6";
    public static bool HasHttpSettings(this IHasStereotypes? hasStereotypes)
    {
        return hasStereotypes?.HasStereotype(HttpSettingsDefinitionId) == true;
    }
}
