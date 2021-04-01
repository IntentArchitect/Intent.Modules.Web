using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Metadata.Models;
using Intent.Modelers.WebClient.Api;
using Intent.RoslynWeaver.Attributes;
using Intent.Modules.Common;
using Intent.Modules.Common.Types.Api;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.Api.ApiPackageExtensionModel", Version = "1.0")]

namespace Intent.Modelers.WebClient.Angular.Api
{
    [IntentManaged(Mode.Merge)]
    public class AngularWebAppModel : WebClientModel
    {
        [IntentManaged(Mode.Ignore)]
        public AngularWebAppModel(IPackage element) : base(element)
        {
        }

        public RoutingModel Routing => UnderlyingPackage.ChildElements
            .GetElementsOfType(RoutingModel.SpecializationTypeId)
            .Select(x => new RoutingModel(x))
            .SingleOrDefault();

        public IList<ModuleModel> Modules => UnderlyingPackage.ChildElements
            .GetElementsOfType(ModuleModel.SpecializationTypeId)
            .Select(x => new ModuleModel(x))
            .ToList();

        public IList<ModelDefinitionModel> ModelDefinitions => UnderlyingPackage.ChildElements
            .GetElementsOfType(ModelDefinitionModel.SpecializationTypeId)
            .Select(x => new ModelDefinitionModel(x))
            .ToList();

        public IList<FolderModel> Folders => UnderlyingPackage.ChildElements
            .GetElementsOfType(FolderModel.SpecializationTypeId)
            .Select(x => new FolderModel(x))
            .ToList();

        public IList<AngularServiceModel> Services => UnderlyingPackage.ChildElements
            .GetElementsOfType(AngularServiceModel.SpecializationTypeId)
            .Select(x => new AngularServiceModel(x))
            .ToList();

        public IList<EnumModel> Enums => UnderlyingPackage.ChildElements
            .GetElementsOfType(EnumModel.SpecializationTypeId)
            .Select(x => new EnumModel(x))
            .ToList();

        public IList<TypeDefinitionModel> TypeDefinitions => UnderlyingPackage.ChildElements
            .GetElementsOfType(TypeDefinitionModel.SpecializationTypeId)
            .Select(x => new TypeDefinitionModel(x))
            .ToList();

    }
}