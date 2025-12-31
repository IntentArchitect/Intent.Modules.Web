using System;
using System.Linq;
using Intent.Modelers.UI.Api;
using Intent.Modules.Common.Typescript.Mapping;
using Intent.Modules.Common.TypeScript.Builder;
using Intent.Modules.Common.TypeScript.Templates;

namespace Intent.Modules.Angular.Api.Mappings;

public class TypescriptTextDirectiveMapping : TypescriptMappingBase
{
    public TypescriptTextDirectiveMapping(MappingModel model, ITypescriptTemplate template) : base(model, template)
    {
    }

    public override TypescriptStatement GetSourceStatement(bool? targetIsNullable = default)
    {
        var result = Mapping?.MappingExpression ?? throw new Exception($"Could not resolve source path. Mapping expected on '{Model.DisplayText ?? Model.Name}' [{Model.SpecializationType}]. Check that you have a MappingTypeResolver that addresses this scenario.");
        foreach (var map in GetParsedExpressionMap(Mapping?.MappingExpression, path => GetSourcePathText(Mapping.GetSource(path).Path, true)))
        {
            result = result.Replace(map.Key, map.Value.Contains(' ') ? $"@({map.Value})" : $"@{map.Value}");
        }
        return result;
    }
}

public class TypescriptPropertyBindingMapping : TypeConvertingTypescriptMapping
{
    public TypescriptPropertyBindingMapping(MappingModel model, ITypescriptTemplate template) : base(model, template)
    {
    }

    public override TypescriptStatement GetSourceStatement(bool? targetIsNullable = null)
    {
        return GetTypeConvertedSourceStatement();
    }
}

