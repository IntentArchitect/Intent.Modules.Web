using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Metadata.Models;
using Intent.RoslynWeaver.Attributes;
using Intent.Modules.Common;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.Api.ApiElementModel", Version = "1.0")]

namespace Intent.Modelers.WebClient.Angular.Api
{
    [IntentManaged(Mode.Merge)]
    public class RouteModel : IMetadataModel, IHasStereotypes, IHasName, IHasTypeReference
    {
        public const string SpecializationType = "Route";
        public const string SpecializationTypeId = "9094f807-b330-4af8-be8d-bc7955035b94";
        protected readonly IElement _element;

        [IntentManaged(Mode.Ignore)]
        public RouteModel(IElement element, string requiredType = SpecializationType)
        {
            if (!requiredType.Equals(element.SpecializationType, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new Exception($"Cannot create a '{GetType().Name}' from element with specialization type '{element.SpecializationType}'. Must be of type '{SpecializationType}'");
            }
            _element = element;
        }

        public string Id => _element.Id;

        public string Name => _element.Name;

        public IEnumerable<IStereotype> Stereotypes => _element.Stereotypes;

        public ITypeReference TypeReference => _element.TypeReference;

        public IElement InternalElement => _element;

        [IntentManaged(Mode.Ignore)]
        public bool RoutesToComponent => TypeReference.Element?.SpecializationTypeId == ComponentModel.SpecializationTypeId;

        [IntentManaged(Mode.Ignore)]
        public bool RoutesToModule => TypeReference.Element?.SpecializationTypeId == ModuleModel.SpecializationTypeId;

        public override string ToString()
        {
            return _element.ToString();
        }

        public bool Equals(RouteModel other)
        {
            return Equals(_element, other?._element);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((RouteModel)obj);
        }

        public override int GetHashCode()
        {
            return (_element != null ? _element.GetHashCode() : 0);
        }

        public string Comment => _element.Comment;

        public bool IsMapped => _element.IsMapped;

        public IElementMapping Mapping => _element.MappedElement;

        public IList<RouteParameterModel> Parameters => _element.ChildElements
            .GetElementsOfType(RouteParameterModel.SpecializationTypeId)
            .Select(x => new RouteParameterModel(x))
            .ToList();
    }

    [IntentManaged(Mode.Fully)]
    public static class RouteModelExtensions
    {

        public static bool IsRouteModel(this ICanBeReferencedType type)
        {
            return type != null && type is IElement element && element.SpecializationTypeId == RouteModel.SpecializationTypeId;
        }

        public static RouteModel AsRouteModel(this ICanBeReferencedType type)
        {
            return type.IsRouteModel() ? new RouteModel((IElement)type) : null;
        }
    }
}