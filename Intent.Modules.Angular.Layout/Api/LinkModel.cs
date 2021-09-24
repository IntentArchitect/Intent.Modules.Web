using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Metadata.Models;
using Intent.Modules.Common;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.Api.ApiElementModel", Version = "1.0")]

namespace Intent.Angular.Layout.Api
{
    [IntentManaged(Mode.Merge)]
    public class LinkModel : IMetadataModel, IHasStereotypes, IHasName
    {
        public const string SpecializationType = "Link";
        public const string SpecializationTypeId = "b0127146-e571-4000-825e-e7a04e4a45d6";
        protected readonly IElement _element;

        [IntentManaged(Mode.Ignore)]
        public LinkModel(IElement element, string requiredType = SpecializationType)
        {
            if (!requiredType.Equals(element.SpecializationType, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new Exception($"Cannot create a '{GetType().Name}' from element with specialization type '{element.SpecializationType}'. Must be of type '{SpecializationType}'");
            }
            _element = element;
        }

        public string Id => _element.Id;

        public string Name => _element.Name;

        public string Comment => _element.Comment;

        public IEnumerable<IStereotype> Stereotypes => _element.Stereotypes;

        public IElement InternalElement => _element;

        public override string ToString()
        {
            return _element.ToString();
        }

        public bool Equals(LinkModel other)
        {
            return Equals(_element, other?._element);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((LinkModel)obj);
        }

        public override int GetHashCode()
        {
            return (_element != null ? _element.GetHashCode() : 0);
        }
    }

    [IntentManaged(Mode.Fully)]
    public static class LinkModelExtensions
    {

        public static bool IsLinkModel(this ICanBeReferencedType type)
        {
            return type != null && type is IElement element && element.SpecializationTypeId == LinkModel.SpecializationTypeId;
        }

        public static LinkModel ToLinkModel(this ICanBeReferencedType type)
        {
            return type.IsLinkModel() ? new LinkModel((IElement)type) : null;
        }
    }
}