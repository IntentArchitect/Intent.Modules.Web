using Intent.Metadata.Models;
using Intent.Modelers.Services.Api;
using Intent.Modelers.Services.CQRS.Api;
using Intent.Modules.Angular.HttpClients.ImplementationStrategies.Infrastructure;
using Intent.Modules.Common.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intent.Modules.Angular.HttpClients.ImplementationStrategies;

internal class ObjectParameterStrategy(InteractionMetadata interactionMetadata, IAssociation association) : BaseImplementationStrategy, IImplementationStrategy, IIsSourceStrategy
{
    public bool IsMatch()
    {
        // the operation on the proxy service
        var clientFields = interactionMetadata.ServiceProxyOperation.InternalElement.ChildElements.Where(x => x.IsDTOFieldModel()).ToArray();

        return clientFields.Length > 1 || interactionMetadata.ServiceProxyModel.CreateParameterPerInput;
    }


    public GenerateRequestResult GenerateImplementation()
    {
        var parameterName = interactionMetadata.ServiceProxyOperation.InternalElement.SpecializationTypeId switch
        {
            CommandModel.SpecializationTypeId => "command",
            QueryModel.SpecializationTypeId => "query",
            _ => interactionMetadata.ServiceProxyOperation.InternalElement.Name.ToCamelCase(true)
        };

        var initStatementBuilder = new StringBuilder();
        var requestMapping = association.TargetEnd.Mappings.FirstOrDefault(m => m.TypeId == "e4a4111b-cf13-4efe-8a5d-fea9457ac8ad"); // Call Service Mapping

        foreach (var operation in interactionMetadata.ComponentTemplateBase.Model.Operations)
        {
            SetSourceReplacement([interactionMetadata.ComponentTemplateBase.Model.InternalElement, operation.InternalElement], "");
            SetSourceReplacement(operation.InternalElement, "");
        }
        SetSourceReplacement(interactionMetadata.ComponentTemplateBase.Model.InternalElement, "this");

        var closingBrace = false;
        foreach (var map in requestMapping.MappedEnds)
        {
            var referencedEntities = new List<string>();
            if(map.SourcePath.Any(s => interactionMetadata.ComponentTemplateBase.Model.ModelDefinitions.Select(m => m.Id).Contains(s.Element.TypeReference?.ElementId)))
            {
                referencedEntities.AddRange(interactionMetadata.ComponentTemplateBase.Model.ModelDefinitions.Where(m => map.SourcePath.Select(s => s.Element.TypeReference?.ElementId).Contains(m.Id))
                    .Select(s => s.Name.ToPascalCase()));
            }

            if (map.MappingTypeId == "ab447316-1252-49bc-a695-f34cb00a3cdc") // invocation mapping
            {
                initStatementBuilder.AppendLine($"const {parameterName}: {interactionMetadata.ComponentTemplateBase.GetTypeName(requestMapping.TargetElement as IElement)} = {{");
                closingBrace = true;
                continue;
            }

            // get the . seperated list of elements ids in the source path
            var joinedPath = string.Join('.', map.SourcePath.Select(s => s.Id));
            // replace Ids with replacement values
            var parsedPath = ReplaceKeysInStrings(joinedPath, SourceReplacements);

            // convert any remaining ids back to the actual resolved names
            var sourcePath = string.Join(".", parsedPath.Split(".").Where(v => !string.IsNullOrWhiteSpace(v)).Select(id =>
            {
                var matchedPath = map.SourcePath.FirstOrDefault(sp => sp.Id == id);

                if (matchedPath is not null)
                {
                    return matchedPath.Name.ToCamelCase(true);
                }

                return id;
            }));

            // TODO Fix this
            initStatementBuilder.AppendLine($"{interactionMetadata.ComponentTemplateBuilder.TypescriptFile.Indentation + interactionMetadata.ComponentTemplateBuilder.TypescriptFile.Indentation + interactionMetadata.ComponentTemplateBuilder.TypescriptFile.Indentation}{map.TargetElement.Name.ToCamelCase(true)}: {sourcePath},");
        }

        // -3 to take into account the \r\n at and of string
        if (initStatementBuilder[^3] == ',')
        {
            initStatementBuilder = initStatementBuilder.Remove(initStatementBuilder.Length - 3, 1);
        }

        if (closingBrace)
        {
            initStatementBuilder.AppendLine($"{interactionMetadata.ComponentTemplateBuilder.TypescriptFile.Indentation + interactionMetadata.ComponentTemplateBuilder.TypescriptFile.Indentation}}};");
        }

        initStatementBuilder.Append($@"
{interactionMetadata.ComponentTemplateBuilder.TypescriptFile.Indentation + interactionMetadata.ComponentTemplateBuilder.TypescriptFile.Indentation}");

        return new(parameterName, initStatementBuilder.ToString());
    }
}
