using Intent.Metadata.Models;
using Intent.Modelers.Services.Api;
using Intent.Modules.Angular.HttpClients.ImplementationStrategies.Infrastructure;
using Intent.Modules.Common.TypeScript.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intent.Modules.Angular.HttpClients.ImplementationStrategies;

internal class NoParameterStrategy(InteractionMetadata interactionMetadata) : BaseImplementationStrategy, IImplementationStrategy, IIsSourceStrategy
{
    public bool IsMatch()
    {
        // the operation on the proxy service
        var clientFields = interactionMetadata.ServiceProxyOperation.InternalElement.ChildElements.Where(x => x.IsDTOFieldModel()).ToArray();

        return clientFields.Length == 0;
    }

    public GenerateRequestResult GenerateImplementation()
    {
        // No implementation required
        return new GenerateRequestResult(string.Empty, string.Empty);
    }
}
