using Intent.Engine;
using Intent.Metadata.Models;
using Intent.Modelers.Services.Api;
using Intent.Modelers.Services.CQRS.Api;
using Intent.Modelers.UI.Api;
using Intent.Modules.Angular.HttpClients.Templates.Helper;
using Intent.Modules.Angular.HttpClients.Templates.HttpServiceProxy;
using Intent.Modules.Common;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Intent.Modules.Angular.HttpClients.ImplementationStrategies;

public abstract class BaseImplementationStrategy
{
    internal IApplication _application;
    internal IAssociation _association;

    internal Dictionary<string, string> SourceReplacements = [];
    internal Dictionary<string, string?> TargetReplacements = [];

    internal InteractionMetadata GetInteractionMetadata(ITypescriptFileBuilderTemplate template)
    {
        var metadata = new InteractionMetadata();

        // None of these should ever be null
        var @class = template.TypescriptFile.Classes.FirstOrDefault(c => c.Decorators.Any(d => d.Name == "Component"));
        metadata.Class = @class;

        var ctor = @class.Constructors.FirstOrDefault();
        metadata.Constructor = ctor;

        // get the operation on the component doing the invocation
        var templateOperation = @class.Methods.FirstOrDefault(m => m.Name.Equals(_association.TargetEnd.ParentElement.Name, StringComparison.CurrentCultureIgnoreCase));
        metadata.InvocationMethod = templateOperation;

        // this gets the proxy model which is mapped
        var proxyModel = _application.MetadataManager.GetServiceProxyModels(_application.Id, _application.MetadataManager.UserInterface)
            .FirstOrDefault(p => p.Endpoints.Any(e => e.Id == _association.SourceEnd.ParentElement.Id));
        metadata.ServiceProxyModel = proxyModel;

        var proxyTemplate = _application.FindTemplateInstance<TypeScriptTemplateBase<IServiceProxyModel>>(HttpServiceProxyTemplate.TemplateId, proxyModel);
        metadata.ServiceProxyTemplate = proxyTemplate;

        var serviceproxyOperation = proxyModel.Endpoints.First(e => e.InternalElement.Id == _association.SourceEnd.ParentId);
        metadata.ServiceProxyOperation = serviceproxyOperation;

        metadata.ComponentTemplateBuilder = template;

        return metadata;
    }

    internal static void InjectProxyService(InteractionMetadata templateMetadata)
    {
        if (!templateMetadata.Constructor.Parameters.Any(p => p.Name == templateMetadata.ServiceProxyTemplate.Model.Name.ToCamelCase(true)))
        {
            templateMetadata.Constructor.AddParameter(templateMetadata.ServiceProxyTemplate.Model.Name.ToCamelCase(true), templateMetadata.ComponentTemplateBase.GetTypeName(templateMetadata.ServiceProxyModel.InternalElement), param => param.WithPrivateFieldAssignment());
        }
    }

    internal static void AddServiceInvocationSupportFields(InteractionMetadata templateMetadata)
    {
        // add the field to handle the errors
        if (!templateMetadata.ComponentTemplateBuilder.TypescriptFile.Classes.Any(c => c.Fields.Any(f => f.Name == "serviceError")))
        {
            templateMetadata.Class.AddField("serviceError", "string | null", @field => field.WithValue("null"));
        }

        // add the field to indicate success
        if (!templateMetadata.ComponentTemplateBuilder.TypescriptFile.Classes.Any(c => c.Fields.Any(f => f.Name == "serviceCallSuccess")))
        {
            templateMetadata.Class.AddField("serviceCallSuccess", "boolean", @field => field.WithValue("false"));
        }
    }

    internal string ApplySourceInitializationStatements(InteractionMetadata templateMetadata)
    {
        var requestMapping = _association.TargetEnd.Mappings.FirstOrDefault(m => m.TypeId == "e4a4111b-cf13-4efe-8a5d-fea9457ac8ad"); // Call Service Mapping
        var clientFields = templateMetadata.ServiceProxyOperation.InternalElement.ChildElements.Where(x => x.IsDTOFieldModel()).ToArray();

        var paramString = string.Empty;

        if (clientFields.Length == 0)
        {
            return string.Empty;
        }

        if (clientFields.Length == 1 && !templateMetadata.ServiceProxyModel.CreateParameterPerInput)
        {

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

            return string.Join(", ", parameters);
        }
        else
        {
            var parameterName = templateMetadata.ServiceProxyOperation.InternalElement.SpecializationTypeId switch
            {
                CommandModel.SpecializationTypeId => "command",
                QueryModel.SpecializationTypeId => "query",
                _ => templateMetadata.ServiceProxyOperation.InternalElement.Name.ToCamelCase(true)
            };

            var initStatementBuilder = new StringBuilder();

            var closingBrace = false;
            foreach (var map in requestMapping.MappedEnds)
            {
                if (map.MappingTypeId == "ab447316-1252-49bc-a695-f34cb00a3cdc") // invocation mapping
                {
                    initStatementBuilder.AppendLine($"const {parameterName}: {templateMetadata.ComponentTemplateBase.GetTypeName(requestMapping.TargetElement as IElement)} = {{");
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
                initStatementBuilder.AppendLine($"{templateMetadata.ComponentTemplateBuilder.TypescriptFile.Indentation + templateMetadata.ComponentTemplateBuilder.TypescriptFile.Indentation + templateMetadata.ComponentTemplateBuilder.TypescriptFile.Indentation}{map.TargetElement.Name.ToCamelCase(true)}: {sourcePath},");
            }

            // -3 to take into account the \r\n at and of string
            if (initStatementBuilder[^3] == ',')
            {
                initStatementBuilder = initStatementBuilder.Remove(initStatementBuilder.Length - 3, 1);
            }

            if (closingBrace)
            {
                initStatementBuilder.AppendLine($"{templateMetadata.ComponentTemplateBuilder.TypescriptFile.Indentation + templateMetadata.ComponentTemplateBuilder.TypescriptFile.Indentation}}};");
            }

            templateMetadata.InvocationMethod.AddStatement(initStatementBuilder.ToString());
            templateMetadata.InvocationMethod.AddStatement("");

            return parameterName;
        }
    }

    internal string BuildTargetStatement(InteractionMetadata templateMetadata)
    {
        var responseMapping = _association.TargetEnd.Mappings.FirstOrDefault(m => m.TypeId == "e60890c6-7ce7-47be-a0da-dff82b8adc02"); // Call Service Response Mapping

        if (responseMapping is null)
        {
            return string.Empty;
        }

        var initStatementBuilder = new StringBuilder();

        foreach (var map in responseMapping.MappedEnds)
        {
            // get the . seperated list of elements ids in the source path
            var joinedSourcePath = string.Join('.', map.SourcePath.Select(s => s.Id));
            // replace Ids with replacement values
            var parsedSourcePath = ReplaceKeysInStrings(joinedSourcePath, TargetReplacements);

            // convert any remaining ids back to the actual resolved names
            var sourcePath = string.Join(".", parsedSourcePath.Split(".").Where(v => !string.IsNullOrWhiteSpace(v)).Select(id =>
            {
                var matchedPath = map.SourcePath.FirstOrDefault(sp => sp.Id == id);

                if (matchedPath is not null)
                {
                    return matchedPath.Name.ToCamelCase(true);
                }

                return id;
            }));

            // get the . seperated list of elements ids in the source path
            var joinedTargetPath = string.Join('.', map.TargetPath.Select(s => s.Id));
            // replace Ids with replacement values
            var parsedTargetPath = ReplaceKeysInStrings(joinedTargetPath, TargetReplacements);

            // convert any remaining ids back to the actual resolved names
            var targetPath = string.Join(".", parsedTargetPath.Split(".").Where(v => !string.IsNullOrWhiteSpace(v)).Select(id =>
            {
                var matchedPath = map.TargetPath.FirstOrDefault(sp => sp.Id == id);

                if (matchedPath is not null)
                {
                    return matchedPath.Name.ToCamelCase(true);
                }

                return id;
            }));

            // TODO Fix this
            initStatementBuilder.AppendLine($"{targetPath} = {sourcePath};");
        }

        return initStatementBuilder.ToString();
    }

    internal virtual void SetSourceReplacement(IMetadataModel type, string replacement)
    {
        SourceReplacements.Remove(type.Id);
        SourceReplacements.Add(type.Id, replacement);
    }

    internal virtual void SetSourceReplacement(IMetadataModel[] types, string replacement)
    {
        SourceReplacements.Remove(string.Join(".", types.Select(t => t.Id)));
        SourceReplacements.Add(string.Join(".", types.Select(t => t.Id)), replacement);
    }

    internal virtual void SetTargetReplacement(IMetadataModel type, string replacement)
    {
        TargetReplacements.Remove(type.Id);
        TargetReplacements.Add(type.Id, replacement);
    }

    internal virtual void SetTargetReplacement(IMetadataModel[] types, string replacement)
    {
        TargetReplacements.Remove(string.Join(".", types.Select(t => t.Id)));
        TargetReplacements.Add(string.Join(".", types.Select(t => t.Id)), replacement);
    }
    internal virtual void SetTargetReplacement(string[] types, string replacement)
    {
        TargetReplacements.Remove(string.Join(".", types.Select(t => t ?? "*")));
        TargetReplacements.Add(string.Join(".", types.Select(t => t ?? "*")), replacement);
    }

    internal virtual void SetTargetReplacement(string type, string replacement)
    {
        TargetReplacements.Remove(type);
        TargetReplacements.Add(type, replacement);
    }

    private string ReplaceKeysInStrings(string inputString, Dictionary<string, string> replacements)
    {
        string updated = inputString;

        foreach (var kvp in replacements)
        {
            var value = replacements[kvp.Key];

            // Build regex pattern for wildcard support
            string pattern = Regex.Escape(kvp.Key).Replace("\\*", "[^.]+");

            // Match only full segments: e.g., a.b.c but not xab.c
            pattern = $@"(?<=^|\.|\/){pattern}(?=\.|\/|$)";

            var regex = new Regex(pattern);

            if (regex.IsMatch(updated))
            {
                updated = regex.Replace(updated, value);
            }
        }

        return updated;
    }
}
