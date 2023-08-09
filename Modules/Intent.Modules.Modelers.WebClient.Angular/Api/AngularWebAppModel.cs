using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Metadata.Models;
using Intent.Modelers.WebClient.Api;
using Intent.Modules.Common;
using Intent.Modules.Common.Types.Api;
using Intent.RoslynWeaver.Attributes;

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

        [IntentManaged(Mode.Fully)]
        public IList<EnumModel> Enums => UnderlyingPackage.ChildElements
            .GetElementsOfType(EnumModel.SpecializationTypeId)
            .Select(x => new EnumModel(x))
            .ToList();

        [IntentManaged(Mode.Fully)]
        public IList<TypeDefinitionModel> Types => UnderlyingPackage.ChildElements
            .GetElementsOfType(TypeDefinitionModel.SpecializationTypeId)
            .Select(x => new TypeDefinitionModel(x))
            .ToList();

        [IntentManaged(Mode.Fully)]
        public ModuleModel RootModule => UnderlyingPackage.ChildElements
            .GetElementsOfType(ModuleModel.SpecializationTypeId)
            .Select(x => new ModuleModel(x))
            .SingleOrDefault();

    }
}