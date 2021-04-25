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

        public IList<EnumModel> Enums => UnderlyingPackage.ChildElements
            .GetElementsOfType(EnumModel.SpecializationTypeId)
            .Select(x => new EnumModel(x))
            .ToList();

        public IList<TypeDefinitionModel> TypeDefinitions => UnderlyingPackage.ChildElements
            .GetElementsOfType(TypeDefinitionModel.SpecializationTypeId)
            .Select(x => new TypeDefinitionModel(x))
            .ToList();

        public ModuleModel RootModule => UnderlyingPackage.ChildElements
            .GetElementsOfType(ModuleModel.SpecializationTypeId)
            .Select(x => new ModuleModel(x))
            .SingleOrDefault();

    }
}