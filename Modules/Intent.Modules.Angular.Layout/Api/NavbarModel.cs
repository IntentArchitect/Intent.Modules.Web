using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Metadata.Models;
using Intent.Modelers.WebClient.Angular.Api;
using Intent.Modules.Common;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.Api.ApiElementModel", Version = "1.0")]

namespace Intent.Angular.Layout.Api
{
    [IntentManaged(Mode.Merge)]
    public class NavbarModel : IMetadataModel, IHasStereotypes, IHasName, IElementWrapper
    {
        public const string SpecializationType = "Navbar";
        public const string SpecializationTypeId = "990cd6f1-8840-419b-be75-4ec63f03f359";
        protected readonly IElement _element;

        [IntentManaged(Mode.Ignore)]
        public NavbarModel(IElement element, string requiredType = SpecializationType)
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

        [IntentManaged(Mode.Ignore)]
        public ModuleModel Module => InternalElement.GetModule();

        public IEnumerable<IStereotype> Stereotypes => _element.Stereotypes;

        public IElement InternalElement => _element;

        public IList<LinkModel> Links => _element.ChildElements
            .GetElementsOfType(LinkModel.SpecializationTypeId)
            .Select(x => new LinkModel(x))
            .ToList();

        public IList<DropdownModel> Dropdowns => _element.ChildElements
            .GetElementsOfType(DropdownModel.SpecializationTypeId)
            .Select(x => new DropdownModel(x))
            .ToList();

        public override string ToString()
        {
            return _element.ToString();
        }

        public bool Equals(NavbarModel other)
        {
            return Equals(_element, other?._element);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((NavbarModel)obj);
        }

        public override int GetHashCode()
        {
            return (_element != null ? _element.GetHashCode() : 0);
        }
    }

    [IntentManaged(Mode.Fully)]
    public static class NavbarModelExtensions
    {

        public static bool IsNavbarModel(this ICanBeReferencedType type)
        {
            return type != null && type is IElement element && element.SpecializationTypeId == NavbarModel.SpecializationTypeId;
        }

        public static NavbarModel AsNavbarModel(this ICanBeReferencedType type)
        {
            return type.IsNavbarModel() ? new NavbarModel((IElement)type) : null;
        }
    }
}