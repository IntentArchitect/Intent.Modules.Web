using Intent.Metadata.Models;
using Intent.Modelers.Services.Api;
using Intent.Modules.Angular.HttpClients.ImplementationStrategies.Infrastructure;
using Intent.Modules.Common.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intent.Modules.Angular.HttpClients.ImplementationStrategies;

internal class SingleValueParameterStrategy(InteractionMetadata interactionMetadata, IAssociation association) : BaseImplementationStrategy, IImplementationStrategy, IIsSourceStrategy
{
    public bool IsMatch()
    {
        // the operation on the proxy service
        var clientFields = interactionMetadata.ServiceProxyOperation.InternalElement.ChildElements.Where(x => x.IsDTOFieldModel()).ToArray();

        return clientFields.Length == 1 && !interactionMetadata.ServiceProxyModel.CreateParameterPerInput;
    }

    public GenerateRequestResult GenerateImplementation()
    {
        var requestMapping = association.TargetEnd.Mappings.FirstOrDefault(m => m.TypeId == "e4a4111b-cf13-4efe-8a5d-fea9457ac8ad"); // Call Service Mapping

        foreach (var operation in interactionMetadata.ComponentTemplateBase.Model.Operations)
        {
            SetSourceReplacement([interactionMetadata.ComponentTemplateBase.Model.InternalElement, operation.InternalElement], "");
            SetSourceReplacement(operation.InternalElement, "");
        }
        SetSourceReplacement(interactionMetadata.ComponentTemplateBase.Model.InternalElement, "this");

        var parameters = requestMapping.MappedEnds
                .Where(m => m.MappingTypeId == "ce70308a-e29d-4644-8410-f9e6bbd214fc") // data mappings
                .Select(map =>
                {
                    // get the . seperated list of elements ids in the source path
                    var joinedPath = string.Join('.', map.SourcePath.Select(s => s.Id));
                    // replace Ids with replacement values
                    var parsedPath = ReplaceKeysInStrings(joinedPath, SourceReplacements);

                    // convert any remaining ids back to the actual resolved names
                    return string.Join(".", parsedPath.Split(".").Where(v => !string.IsNullOrWhiteSpace(v)).Select(id =>
                    {
                        var matchedPath = map.SourcePath.FirstOrDefault(sp => sp.Id == id);

                        if (matchedPath is not null)
                        {
                            return matchedPath.Name.ToCamelCase(true);
                        }

                        return id;
                    }));
                });

        return new GenerateRequestResult(string.Join(", ", parameters), string.Empty);
    }
}
