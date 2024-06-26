using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Metadata.Models;
using Intent.Modules.Common;
using Intent.Modules.Common.Types.Api;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.Api.ApiElementModel", Version = "1.0")]

namespace Intent.Modelers.WebClient.Angular.Api
{
    [IntentManaged(Mode.Merge, Signature = Mode.Merge)]
    public class ModelDefinitionModel : IMetadataModel, IHasStereotypes, IHasName, IHasFolder<IFolder>, IElementWrapper
    {
        public const string SpecializationType = "Model Definition";
        protected readonly IElement _element;

        [IntentManaged(Mode.Ignore)]
        public ModelDefinitionModel(IElement element, string requiredType = SpecializationType)
        {
            if (!requiredType.Equals(element.SpecializationType, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new Exception($"Cannot create a '{GetType().Name}' from element with specialization type '{element.SpecializationType}'. Must be of type '{SpecializationType}'");
            }
            _element = element;
        }

        public ModuleModel Module => new ModuleModel(_element.GetParentPath().Reverse().First(x => x.SpecializationType == ModuleModel.SpecializationType));

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
        public override string ToString()
        {
            return _element.ToString();
        }

        [IntentManaged(Mode.Fully)]
        public bool Equals(ModelDefinitionModel other)
        {
            return Equals(_element, other?._element);
        }

        [IntentManaged(Mode.Fully)]
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ModelDefinitionModel)obj);
        }

        [IntentManaged(Mode.Fully)]
        public override int GetHashCode()
        {
            return (_element != null ? _element.GetHashCode() : 0);
        }

        [IntentManaged(Mode.Fully)]
        public IList<ModelDefinitionFieldModel> Fields => _element.ChildElements
            .GetElementsOfType(ModelDefinitionFieldModel.SpecializationTypeId)
            .Select(x => new ModelDefinitionFieldModel(x))
            .ToList();

        [IntentManaged(Mode.Fully)]
        public bool IsMapped => _element.IsMapped;

        [IntentManaged(Mode.Fully)]
        public IElementMapping Mapping => _element.MappedElement;

        [IntentManaged(Mode.Fully)]
        public IEnumerable<string> GenericTypes => _element.GenericTypes.Select(x => x.Name);
        public const string SpecializationTypeId = "bd3941b5-e3b3-4a40-96e6-b9c87cea0101";

        public string Comment => _element.Comment;
    }

    [IntentManaged(Mode.Fully)]
    public static class ModelDefinitionModelExtensions
    {

        public static bool IsModelDefinitionModel(this ICanBeReferencedType type)
        {
            return type != null && type is IElement element && element.SpecializationTypeId == ModelDefinitionModel.SpecializationTypeId;
        }

        public static ModelDefinitionModel AsModelDefinitionModel(this ICanBeReferencedType type)
        {
            return type.IsModelDefinitionModel() ? new ModelDefinitionModel((IElement)type) : null;
        }

        public static bool HasNewMappingSettingsMapping(this ModelDefinitionModel type)
        {
            return type.Mapping?.MappingSettingsId == "31b3d3a7-bf3c-4bb4-8b1d-9e18b6a8bcdd";
        }

        public static IElementMapping GetNewMappingSettingsMapping(this ModelDefinitionModel type)
        {
            return type.HasNewMappingSettingsMapping() ? type.Mapping : null;
        }
    }
}