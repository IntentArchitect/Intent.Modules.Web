using Intent.Metadata.Models;
using Intent.Modelers.UI.Api;
using Intent.Modules.Common;
using Intent.Modules.Common.Html.Templates;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.Types.Api;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intent.Modules.Angular.Templates.Component;

public static class LayoutItemExtensions
{
    public static ITemplateFileConfig GetLayoutItemHtmlFileConfig<TModel>(this IntentTemplateBase<TModel> template) where TModel : IMetadataModel, IHasStereotypes, IHasName, IElementWrapper
    {
        var parentLayout = template.Model.InternalElement.ParentElement.AsLayoutModel();
        var layoutName = GetLayoutName(template.Model.InternalElement);

        string itemType = GetItemType(template);

        return new HtmlFileConfig(
            fileName: $"{layoutName.ToKebabCase()}.{itemType}.component",
            relativeLocation: $"{string.Join("/", parentLayout.GetParentFolderNames().Select(f => f.ToKebabCase()))}/{layoutName.ToKebabCase()}",
            overwriteBehaviour: OverwriteBehaviour.OverwriteDisabled
        );
    }

    public static ITemplateFileConfig GetLayoutItemStyleFileConfig<TModel>(this IntentTemplateBase<TModel> template) where TModel : IMetadataModel, IHasStereotypes, IHasName, IElementWrapper
    {
        var parentLayout = template.Model.InternalElement.ParentElement.AsLayoutModel();
        var layoutName = GetLayoutName(template.Model.InternalElement);

        string itemType = GetItemType(template);

        return new TemplateFileConfig(
            fileName: $"{layoutName.ToKebabCase()}.{itemType}.component",
            fileExtension: "scss",
            relativeLocation: $"{string.Join("/", parentLayout.GetParentFolderNames().Select(f => f.ToKebabCase()))}/{layoutName.ToKebabCase()}",
            overwriteBehaviour: OverwriteBehaviour.OverwriteDisabled
        );
    }

    public static ITemplateFileConfig GetLayoutItemTypescriptFileConfig<TModel>(this IntentTemplateBase<TModel> template) where TModel : IMetadataModel, IHasStereotypes, IHasName, IElementWrapper
    {
        var parentLayout = template.Model.InternalElement.ParentElement.AsLayoutModel();
        var layoutName = GetLayoutName(template.Model.InternalElement);

        string itemType = GetItemType(template);

        return new TypeScriptFileConfig(
            className: $"{layoutName.ToPascalCase()}{itemType.ToPascalCase()}Component",
            fileName: $"{layoutName.ToKebabCase()}.{itemType}.component",
            relativeLocation: $"{string.Join("/", parentLayout.GetParentFolderNames().Select(f => f.ToKebabCase()))}/{layoutName.ToKebabCase()}",
            overwriteBehaviour: OverwriteBehaviour.Always
        );
    }

    public static string GetRootFilename<TModel>(this IntentTemplateBase<TModel> template) where TModel : IMetadataModel, IHasStereotypes, IHasName, IElementWrapper
    {
        var layoutName = GetLayoutName(template.Model.InternalElement);
        string itemType = GetItemType(template);

        return $"{layoutName.ToKebabCase()}.{itemType}.component";
    }

    public static string GetLayoutItemClassName<TModel>(this IntentTemplateBase<TModel> template) where TModel : IMetadataModel, IHasStereotypes, IHasName, IElementWrapper
    {
        var layoutName = GetLayoutName(template.Model.InternalElement);
        string itemType = GetItemType(template);

        return $"{layoutName.ToPascalCase()}{itemType.ToPascalCase()}Component";
    }

    public static string GetLayoutItemClassName<TModel>(this IntentTemplateBase<TModel> template, IElement model) where TModel : IMetadataModel, IHasStereotypes, IHasName, IElementWrapper
    {
        var layoutName = GetLayoutName(model);
        string itemType = GetItemType(model);

        return $"{layoutName.ToPascalCase()}{itemType.ToPascalCase()}Component";
    }

    public static string GetLayoutItemSelector<TModel>(this IntentTemplateBase<TModel> template) where TModel : IMetadataModel, IHasStereotypes, IHasName, IElementWrapper
    {
        var layoutName = GetLayoutName(template.Model.InternalElement);
        var itemType = GetItemType(template);

        return $"app-{layoutName.ToKebabCase()}-{itemType.ToKebabCase()}";
    }

    public static string GetLayoutItemSelector<TModel>(this IntentTemplateBase<TModel> template, IElement model) where TModel : IMetadataModel, IHasStereotypes, IHasName, IElementWrapper
    {
        var layoutName = GetLayoutName(model);
        var itemType = GetItemType(model);

        return $"app-{layoutName.ToKebabCase()}-{itemType.ToKebabCase()}";
    }

    private static string GetItemType<TModel>(IntentTemplateBase<TModel> template) where TModel : IMetadataModel, IHasStereotypes, IHasName, IElementWrapper
    {
        return template.Model switch
        {
            LayoutHeaderModel => "header",
            LayoutFooterModel => "footer",
            LayoutSiderModel => "sider",
            _ => throw new NotSupportedException($"Unsupported layout item type: {template.Model.GetType().Name}")
        };
    }

    private static string GetItemType(IElement element)
    {
        return element.SpecializationType switch
        {
            "Layout Header" => "header",
            "Layout Footer" => "footer",
            "Layout Sider" => "sider",
            _ => throw new NotSupportedException($"Unsupported layout item type: {element.GetType().Name}")
        };
    }

    private static string GetLayoutName(IElement model)
    {
        var parentLayout = model.ParentElement.AsLayoutModel();
        if (parentLayout.Name.EndsWith("Layout", StringComparison.InvariantCultureIgnoreCase))
        {
            return parentLayout.Name[..^"Layout".Length];
        }
        return parentLayout.Name;
    }
}
