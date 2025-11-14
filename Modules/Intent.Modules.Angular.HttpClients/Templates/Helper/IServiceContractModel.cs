using Intent.Metadata.Models;
using Intent.Modules.Common;
using Intent.Modules.Common.Types.Api;
using System.Collections.Generic;

namespace Intent.Modules.Angular.HttpClients.Templates.Helper;

public interface IServiceContractModel : IMetadataModel, IHasName, IElementWrapper, IHasFolder
{
    IReadOnlyList<IServiceContractOperationModel> Operations { get; }
}

public interface IServiceContractOperationModel : IMetadataModel, IHasName, IHasTypeReference, IElementWrapper
{
    IReadOnlyList<IServiceContractOperationParameterModel> Parameters { get; }
}

public interface IServiceContractOperationParameterModel : IMetadataModel, IHasName, IHasTypeReference;
