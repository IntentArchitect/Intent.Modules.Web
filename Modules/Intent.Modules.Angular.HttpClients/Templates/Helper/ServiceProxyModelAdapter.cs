using Intent.Metadata.Models;
using Intent.Modelers.Types.ServiceProxies.Api;
using Intent.Modules.Common.Types.Api;
using Intent.Modules.Metadata.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intent.Modules.Angular.HttpClients.Templates.Helper;

internal class ServiceProxyModelAdapter : IServiceProxyModel
{
    private readonly ServiceProxyModel _model;

    public ServiceProxyModelAdapter(ServiceProxyModel model)
    {
        _model = model;
        Endpoints = model.GetMappedEndpoints().ToArray();
    }

    public string Name => _model.Name;

    public string Id => _model.Id;

    public IMetadataModel UnderlyingModel => _model;

    public bool CreateParameterPerInput => true;

    public FolderModel Folder => _model.Folder;

    public IReadOnlyList<IHttpEndpointModel> Endpoints { get; }

    public IElement InternalElement => _model.InternalElement;
}
