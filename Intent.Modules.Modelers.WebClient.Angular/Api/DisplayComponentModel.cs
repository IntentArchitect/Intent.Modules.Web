using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Metadata.Models;
using Intent.Modules.Common;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.Api.ApiElementModel", Version = "1.0")]

namespace Intent.Modelers.WebClient.Angular.Api
{
    [IntentManaged(Mode.Merge)]
    public class DisplayComponentModel : IMetadataModel, IHasStereotypes, IHasName, IHasTypeReference
    {
        public const string SpecializationType = "Display Component";
        public const string SpecializationTypeId = "866a90f7-4044-43b9-bb05-7270c7889796";

        [IntentManaged(Mode.Ignore)]
        public DisplayComponentModel(IElement element, string requiredType = SpecializationType)
        {
            if (!requiredType.Equals(element.SpecializationType, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new Exception($"Cannot create a '{GetType().Name}' from element with specialization type '{element.SpecializationType}'. Must be of type '{SpecializationType}'");
            }
            _element = element;
        }

        public string Id => _element.Id;

        public override string ToString()
        {
            return _element.ToString();
        }

        public bool Equals(DisplayComponentModel other)
        {
            return Equals(_element, other?._element);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DisplayComponentModel)obj);
        }

        public override int GetHashCode()
        {
            return (_element != null ? _element.GetHashCode() : 0);
        }
        protected readonly IElement _element;

        public string Comment => _element.Comment;

        public IElement InternalElement => _element;

        public string Name => _element.Name;

        public IEnumerable<IStereotype> Stereotypes => _element.Stereotypes;

        public ITypeReference TypeReference => _element.TypeReference;
    }

    [IntentManaged(Mode.Fully)]
    public static class DisplayComponentModelExtensions
    {

        public static bool IsDisplayComponentModel(this ICanBeReferencedType type)
        {
            return type != null && type is IElement element && element.SpecializationTypeId == DisplayComponentModel.SpecializationTypeId;
        }

        public static DisplayComponentModel AsDisplayComponentModel(this ICanBeReferencedType type)
        {
            return type.IsDisplayComponentModel() ? new DisplayComponentModel((IElement)type) : null;
        }
    }
}