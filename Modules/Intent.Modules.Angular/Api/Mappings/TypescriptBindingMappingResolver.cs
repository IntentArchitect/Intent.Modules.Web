using System.Linq;
using Intent.Metadata.Models;
using Intent.Modules.Common;
using Intent.Modules.Common.Types.Api;
using Intent.Modules.Common.Typescript.Mapping;
using Intent.Modules.Common.TypeScript.Builder;
using Intent.Modules.Common.TypeScript.Templates;


namespace Intent.Modules.Angular.Api.Mappings;

public class TypescriptBindingMappingResolver : IMappingTypeResolver
{
    private readonly ITypescriptTemplate _template;

    public TypescriptBindingMappingResolver(ITypescriptTemplate template)
    {
        _template = template;
    }

    public ITypescriptMapping ResolveMappings(MappingModel mappingModel)
    {
        if (mappingModel.Model?.SpecializationType == "Text")
        {
            return new TypescriptTextDirectiveMapping(mappingModel, _template);
        }

        //if (mappingModel.Mapping?.MappingTypeId == "95ce3c3e-ddf4-440b-a9b2-6b152a5fc1b8") // Event Mapping
        //{   
        //    //return new MethodInvocationMapping(mappingModel, _template);
        //    return new RazorEventBindingMapping(mappingModel, _template);
        //}

        //if(mappingModel.Mapping?.MappingTypeId == "e4f0c63b-0f00-42bd-a703-00adf44f3364") // Invokable Mapping
        //{
        //    return new RazorEventBindingMapping(mappingModel, _template);
        //}

        if (mappingModel.Model?.TypeReference?.Element?.IsTypeDefinitionModel() == true
            || mappingModel.Model?.TypeReference?.Element?.IsEnumModel() == true)
        {
            return new TypescriptPropertyBindingMapping(mappingModel, _template);
        }

        return null;
    }
}

