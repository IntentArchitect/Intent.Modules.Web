using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Metadata.Models;
using Intent.Modules.Common;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.Api.ApiElementModel", Version = "1.0")]

namespace Intent.Modelers.WebClient.Angular.Api
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class FormGroupControlModel : IMetadataModel, IHasStereotypes, IHasName, IHasTypeReference
    {
        public const string SpecializationType = "Form Group Control";
        protected readonly IElement _element;

        [IntentManaged(Mode.Ignore)]
        public FormGroupControlModel(IElement element, string requiredType = SpecializationType)
        {
            if (!requiredType.Equals(element.SpecializationType, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new Exception($"Cannot create a '{GetType().Name}' from element with specialization type '{element.SpecializationType}'. Must be of type '{SpecializationType}'");
            }
            _element = element;
        }

        [IntentManaged(Mode.Fully)]
        public string Id => _element.Id;

        [IntentManaged(Mode.Fully)]
        public string Name => _element.Name;

        [IntentManaged(Mode.Fully)]
        public IEnumerable<IStereotype> Stereotypes => _element.Stereotypes;

        [IntentManaged(Mode.Fully)]
        public ITypeReference TypeReference => _element.TypeReference;

        [IntentManaged(Mode.Fully)]
        public IElement InternalElement => _element;

        [IntentManaged(Mode.Fully)]
        public override string ToString()
        {
            return _element.ToString();
        }

        [IntentManaged(Mode.Fully)]
        public bool Equals(FormGroupControlModel other)
        {
            return Equals(_element, other?._element);
        }

        [IntentManaged(Mode.Fully)]
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((FormGroupControlModel)obj);
        }

        [IntentManaged(Mode.Fully)]
        public override int GetHashCode()
        {
            return (_element != null ? _element.GetHashCode() : 0);
        }
        public const string SpecializationTypeId = "b4e29ea5-b9a4-47eb-8214-c4cc5b46ee91";

        public string Comment => _element.Comment;
    }

    [IntentManaged(Mode.Fully)]
    public static class FormGroupControlModelExtensions
    {

        public static bool IsFormGroupControlModel(this ICanBeReferencedType type)
        {
            return type != null && type is IElement element && element.SpecializationTypeId == FormGroupControlModel.SpecializationTypeId;
        }

        public static FormGroupControlModel AsFormGroupControlModel(this ICanBeReferencedType type)
        {
            return type.IsFormGroupControlModel() ? new FormGroupControlModel((IElement)type) : null;
        }
    }
}