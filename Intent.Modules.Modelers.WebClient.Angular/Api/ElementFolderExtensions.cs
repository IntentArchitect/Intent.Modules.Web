using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Intent.Metadata.Models;
using Intent.Modelers.WebClient.Angular.Api;
using Intent.Modules.Common;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.Types.Api;

namespace Intent.Modelers.WebClient.Angular.Api
{
    public static class ElementFolderExtensions
    {
        public static IFolder GetParentFolder(this IElement element)
        {
            return element.ParentElement?.AsFolder();
        }

        public static IFolder AsFolder(this IElement element)
        {
            switch (element.SpecializationTypeId)
            {
                case FolderModel.SpecializationTypeId:
                    return new FolderModel(element);
                case Intent.Modules.Common.Types.Api.FolderModel.SpecializationTypeId:
                    return new Intent.Modules.Common.Types.Api.FolderModel(element);
                case ModuleModel.SpecializationTypeId:
                    return new ModuleModel(element);
            }

            return null;
        }

        public static IList<IFolder> GetParentFolders(this IElement model)
        {
            List<IFolder> result = new List<IFolder>();

            IFolder current = model.ParentElement?.AsFolder();
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

        public static ModuleModel GetModule(this IElement element)
        {
            var found = element.GetParentPath().Reverse().FirstOrDefault(x => x.SpecializationTypeId == ModuleModel.SpecializationTypeId);
            if (found != null)
            {
                var module = new ModuleModel(found);
                return module;
            }
            return null;
        }
    }
}
