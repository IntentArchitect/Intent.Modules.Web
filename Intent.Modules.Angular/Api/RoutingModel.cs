using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Metadata.Models;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("ModuleBuilder.Templates.Api.ApiElementModel", Version = "1.0")]

namespace Intent.Modules.Angular.Api
{
    [IntentManaged(Mode.Merge)]
    public class RoutingModel : IMetadataModel, IHasStereotypes, IHasName
    {
        public const string SpecializationType = "Routing";
        public const string SpecializationTypeId = "b548c3c2-a27d-4290-8d23-99d158bb8987";
        protected readonly IElement _element;

        [IntentManaged(Mode.Ignore)]
        public RoutingModel(IElement element, string requiredType = SpecializationType)
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

        public IElement InternalElement => _element;

        [IntentManaged(Mode.Ignore)]
        public ModuleModel Module => new ModuleModel(InternalElement.ParentElement);

        public IList<RouteModel> Routes => _element.ChildElements
            .Where(x => x.SpecializationType == RouteModel.SpecializationType)
            .Select(x => new RouteModel(x))
            .ToList();

        public override string ToString()
        {
            return _element.ToString();
        }

        public bool Equals(RoutingModel other)
        {
            return Equals(_element, other?._element);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((RoutingModel)obj);
        }

        public override int GetHashCode()
        {
            return (_element != null ? _element.GetHashCode() : 0);
        }

        public IList<RedirectModel> Redirects => _element.ChildElements
            .Where(x => x.SpecializationType == RedirectModel.SpecializationType)
            .Select(x => new RedirectModel(x))
            .ToList();
    }
}