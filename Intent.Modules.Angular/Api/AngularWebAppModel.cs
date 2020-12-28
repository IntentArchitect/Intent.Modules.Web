using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Metadata.Models;
using Intent.Modelers.WebClient.Api;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.Api.ApiPackageExtensionModel", Version = "1.0")]

namespace Intent.Angular.Api
{
    [IntentManaged(Mode.Merge)]
    public class AngularWebAppModel : WebClientModel
    {
        [IntentManaged(Mode.Ignore)]
        public AngularWebAppModel(IPackage element) : base(element)
        {
        }

        public RoutingModel Routing => UnderlyingPackage.ChildElements
            .Where(x => x.SpecializationType == RoutingModel.SpecializationType)
            .Select(x => new RoutingModel(x))
            .SingleOrDefault();

        public IList<ModuleModel> Modules => UnderlyingPackage.ChildElements
            .Where(x => x.SpecializationType == ModuleModel.SpecializationType)
            .Select(x => new ModuleModel(x))
            .ToList();

        public IList<ModelDefinitionModel> ModelDefinitions => UnderlyingPackage.ChildElements
            .Where(x => x.SpecializationType == ModelDefinitionModel.SpecializationType)
            .Select(x => new ModelDefinitionModel(x))
            .ToList();

        public IList<TypeDefinitionModel> TypeDefinitions => UnderlyingPackage.ChildElements
            .Where(x => x.SpecializationType == TypeDefinitionModel.SpecializationType)
            .Select(x => new TypeDefinitionModel(x))
            .ToList();

        public IList<EnumModel> Enums => UnderlyingPackage.ChildElements
            .Where(x => x.SpecializationType == EnumModel.SpecializationType)
            .Select(x => new EnumModel(x))
            .ToList();

        public IList<FolderModel> Folders => UnderlyingPackage.ChildElements
            .Where(x => x.SpecializationType == FolderModel.SpecializationType)
            .Select(x => new FolderModel(x))
            .ToList();

        public IList<AngularServiceModel> Services => UnderlyingPackage.ChildElements
            .Where(x => x.SpecializationType == AngularServiceModel.SpecializationType)
            .Select(x => new AngularServiceModel(x))
            .ToList();

    }
}