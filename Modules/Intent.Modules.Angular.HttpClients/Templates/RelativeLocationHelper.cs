using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Modelers.Services.Api;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.Types.Api;
using Intent.Modules.Common.TypeScript.Templates;

namespace Intent.Modules.Angular.HttpClients.Templates;

public static class RelativeLocationHelper
{
    public static string GetPackageBasedRelativeLocation(this TypeScriptTemplateBase<EnumModel> template, IEnumerable<string> parentFolders = null)
    {
        return GetPackageBasedRelativeLocation<EnumModel>(template, parentFolders);
    }

    public static string GetPackageBasedRelativeLocation(this TypeScriptTemplateBase<DTOModel> template, IEnumerable<string> parentFolders = null)
    {
        return GetPackageBasedRelativeLocation<DTOModel>(template, parentFolders);
    }

    private static string GetPackageBasedRelativeLocation<T>(TypeScriptTemplateBase<T> template, IEnumerable<string> parentFolders = null)
        where T : IHasFolder
    {
        return string.Join('/', Enumerable.Empty<string>()
            .Concat(parentFolders ?? [])
            .Concat(GetElementPackageParts(template))
            .Concat(GetParentFolders(template.Model))
            .Select(x => x.ToKebabCase())
        );
    }

    private static IEnumerable<string> GetElementPackageParts<T>(TypeScriptTemplateBase<T> template)
    {
        var element = template.Model switch
        {
            EnumModel model => model.InternalElement,
            DTOModel model => model.InternalElement,
            _ => throw new InvalidOperationException()
        };

        var appNameParts = new Queue<string>(template.ExecutionContext.GetApplicationConfig().Name.Split("."));
        var packageParts = new Queue<string>(element.Package.Name.Split("."));

        while (appNameParts.TryPeek(out var appNamePart) &&
               packageParts.TryPeek(out var packagePart) &&
               appNamePart == packagePart)
        {
            appNameParts.Dequeue();
            packageParts.Dequeue();
        }

        return packageParts;
    }

    private static IEnumerable<string> GetParentFolders(IHasFolder hasFolder) => hasFolder
        .GetParentFolders()
        .Select(x => x.Name);

}