using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Intent.Engine;
using Intent.Metadata.Models;
using Intent.Modules.Common;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.TypeResolution;
using Intent.Modules.Common.TypeScript.Templates;

namespace Intent.Modules.Angular.ServiceProxies.Templates.Proxies.PagedResult;

public static class PagedResultTypeSource
{
    public static void ApplyTo<T>(TypeScriptTemplateBase<T> template, string pagedResultTemplateId)
    {
        template.AddTypeSource(new PagedResultTypeSource<T>(template, pagedResultTemplateId));
    }
}

public class PagedResultTypeSource<T> : ITypeSource
{
    private const string TypeDefinitionElementId = "9204e067-bdc8-45e7-8970-8a833fdc5253";

    private readonly TypeScriptTemplateBase<T> _originalTemplate;
    private readonly string _pagedResultTemplateId;

    public PagedResultTypeSource(TypeScriptTemplateBase<T> originalTemplate, string pagedResultTemplateId)
    {
        _originalTemplate = originalTemplate;
        _pagedResultTemplateId = pagedResultTemplateId;
    }

    private PagedResultTemplate? _template;
    private PagedResultTemplate Template => _template ??= _originalTemplate.ExecutionContext.FindTemplateInstance<PagedResultTemplate>(_pagedResultTemplateId);

    public IResolvedTypeInfo? GetType(ITypeReference? typeInfo)
    {
        if (typeInfo?.Element?.Id == TypeDefinitionElementId)
        {
            _originalTemplate.AddImport(Template.ClassName, _originalTemplate.GetRelativePath(Template));
            return ResolvedTypeInfo.Create(
                name: Template.ClassName,
                isPrimitive: false,
                isNullable: typeInfo.IsNullable,
                isCollection: typeInfo.IsCollection,
                typeReference: typeInfo,
                template: Template,
                nullableFormatter: NullableFormatter,
                collectionFormatter: CollectionFormatter);
        }

        return null;
    }

    public IEnumerable<ITemplateDependency> GetTemplateDependencies() => Enumerable.Empty<ITemplateDependency>();
    public ICollectionFormatter CollectionFormatter => null!;
    public INullableFormatter NullableFormatter => null!;
}