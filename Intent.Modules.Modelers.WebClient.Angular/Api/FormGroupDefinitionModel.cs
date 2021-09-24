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
    public class FormGroupDefinitionModel : IMetadataModel, IHasStereotypes, IHasName, IHasFolder<IFolder>
    {
        public const string SpecializationType = "Form Group Definition";
        protected readonly IElement _element;

        [IntentManaged(Mode.Ignore)]
        public FormGroupDefinitionModel(IElement element, string requiredType = SpecializationType)
        {
            if (!requiredType.Equals(element.SpecializationType, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new Exception($"Cannot create a '{GetType().Name}' from element with specialization type '{element.SpecializationType}'. Must be of type '{SpecializationType}'");
            }
            _element = element;
        }

        [IntentManaged(Mode.Ignore)]
        public IFolder Folder => InternalElement.GetParentFolder();

        public ModuleModel Module => new ModuleModel(_element.GetParentPath().Reverse().First(x => x.SpecializationType == ModuleModel.SpecializationType));

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
        public bool Equals(FormGroupDefinitionModel other)
        {
            return Equals(_element, other?._element);
        }

        [IntentManaged(Mode.Fully)]
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((FormGroupDefinitionModel)obj);
        }

        [IntentManaged(Mode.Fully)]
        public override int GetHashCode()
        {
            return (_element != null ? _element.GetHashCode() : 0);
        }

        [IntentManaged(Mode.Fully)]
        public IList<FormGroupControlModel> Fields => _element.ChildElements
            .GetElementsOfType(FormGroupControlModel.SpecializationTypeId)
            .Select(x => new FormGroupControlModel(x))
            .ToList();

        [IntentManaged(Mode.Fully)]
        public bool IsMapped => _element.IsMapped;

        [IntentManaged(Mode.Fully)]
        public IElementMapping Mapping => _element.MappedElement;
        public const string SpecializationTypeId = "71142b08-4d10-405b-a5bd-0620d329a992";

        public string Comment => _element.Comment;
    }

    [IntentManaged(Mode.Fully)]
    public static class FormGroupDefinitionModelExtensions
    {

        public static bool IsFormGroupDefinitionModel(this ICanBeReferencedType type)
        {
            return type != null && type is IElement element && element.SpecializationTypeId == FormGroupDefinitionModel.SpecializationTypeId;
        }

        public static FormGroupDefinitionModel AsFormGroupDefinitionModel(this ICanBeReferencedType type)
        {
            return type.IsFormGroupDefinitionModel() ? new FormGroupDefinitionModel((IElement)type) : null;
        }
    }
}