using System.Collections.Generic;
using System.Linq;
using Intent.Metadata.Models;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.Types.Api;

namespace Intent.Modules.Angular.Shared;

internal static class ElementFolderExtensions
{
    public static IFolder GetParentFolder(this IElement element)
    {
        return element.ParentElement?.AsFolder();
    }

    public static IFolder AsFolder(this IElement element)
    {
        if (element?.SpecializationTypeId is
            SpecializationTypeIds.WebClient.Angular.Folder or
            SpecializationTypeIds.Common.Types.Folder or
            SpecializationTypeIds.Angular.Module)
        {
            return new ElementAsFolder(element);
        }

        return null;
    }

    public static IList<IFolder> GetParentFolders(this IElement model)
    {
        var result = new List<IFolder>();

        var current = model.ParentElement?.AsFolder();
        while (current != null)
        {
            result.Insert(0, current);
            current = (current as IHasFolder<IFolder>)?.Folder;
        }
        return result;
    }

    public static string GetFolderPath(this IElement model, params string[] additionalFolders)
    {
        return string.Join("/", model.GetParentFolders().Select(x => x.Name.ToKebabCase()).Concat(additionalFolders));
    }

    internal class ElementAsFolder(IElement element) : IFolder, IHasFolder<IFolder>
    {
        public IEnumerable<IStereotype> Stereotypes => element.Stereotypes;
        public string Name => element.Name;
        public IFolder Folder => element.ParentElement.AsFolder();
    }
}