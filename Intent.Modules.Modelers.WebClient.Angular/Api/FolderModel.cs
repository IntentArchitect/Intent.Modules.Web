using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Metadata.Models;
using Intent.RoslynWeaver.Attributes;
using Intent.Modules.Common;
using Intent.Modules.Common.Types.Api;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.Api.ApiElementModel", Version = "1.0")]

namespace Intent.Modelers.WebClient.Angular.Api
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class FolderModel : IMetadataModel, IHasStereotypes, IHasName, IFolder, IHasFolder<IFolder>
    {
        public const string SpecializationType = "Folder";
        protected readonly IElement _element;

        [IntentManaged(Mode.Ignore)]
        public FolderModel(IElement element, string requiredType = SpecializationType)
        {
            if (!requiredType.Equals(element.SpecializationType, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new Exception($"Cannot create a '{GetType().Name}' from element with specialization type '{element.SpecializationType}'. Must be of type '{SpecializationType}'");
            }
            _element = element;
        }

        [IntentManaged(Mode.Ignore)]
        public IFolder Folder => InternalElement.GetParentFolder();

        [IntentManaged(Mode.Fully)]
        public string Id => _element.Id;

        [IntentManaged(Mode.Fully)]
        public string Name => _element.Name;

        [IntentManaged(Mode.Fully)]
        public IEnumerable<IStereotype> Stereotypes => _element.Stereotypes;

        [IntentManaged(Mode.Fully)]
        public IElement InternalElement => _element;

        [IntentManaged(Mode.Fully)]
        public IList<ModuleModel> Modules => _element.ChildElements
            .GetElementsOfType(ModuleModel.SpecializationTypeId)
            .Select(x => new ModuleModel(x))
            .ToList();

        [IntentManaged(Mode.Fully)]
        public IList<ModelDefinitionModel> ModelDefinitions => _element.ChildElements
            .GetElementsOfType(ModelDefinitionModel.SpecializationTypeId)
            .Select(x => new ModelDefinitionModel(x))
            .ToList();

        [IntentManaged(Mode.Fully)]
        public IList<FolderModel> Folders => _element.ChildElements
            .GetElementsOfType(FolderModel.SpecializationTypeId)
            .Select(x => new FolderModel(x))
            .ToList();

        [IntentManaged(Mode.Fully)]
        public override string ToString()
        {
            return _element.ToString();
        }

        [IntentManaged(Mode.Fully)]
        public bool Equals(FolderModel other)
        {
            return Equals(_element, other?._element);
        }

        [IntentManaged(Mode.Fully)]
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((FolderModel)obj);
        }

        [IntentManaged(Mode.Fully)]
        public override int GetHashCode()
        {
            return (_element != null ? _element.GetHashCode() : 0);
        }

        [IntentManaged(Mode.Fully)]
        public IList<AngularServiceModel> Services => _element.ChildElements
            .GetElementsOfType(AngularServiceModel.SpecializationTypeId)
            .Select(x => new AngularServiceModel(x))
            .ToList();
        public const string SpecializationTypeId = "0b1c1dcb-8c31-4294-883c-a130345730d2";

        public string Comment => _element.Comment;

        public IList<EnumModel> Enums => _element.ChildElements
                    .GetElementsOfType(EnumModel.SpecializationTypeId)
                    .Select(x => new EnumModel(x))
                    .ToList();

        public IList<TypeDefinitionModel> TypeDefinitions => _element.ChildElements
                    .GetElementsOfType(TypeDefinitionModel.SpecializationTypeId)
                    .Select(x => new TypeDefinitionModel(x))
                    .ToList();

        public IList<ComponentModel> Components => _element.ChildElements
                    .GetElementsOfType(ComponentModel.SpecializationTypeId)
                    .Select(x => new ComponentModel(x))
                    .ToList();

    }
}