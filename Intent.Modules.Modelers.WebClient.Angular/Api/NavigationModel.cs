using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Metadata.Models;
using Intent.Modules.Common.Types.Api;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.Api.ApiAssociationModel", Version = "1.0")]

namespace Intent.Modelers.WebClient.Angular.Api
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class NavigationModel : IMetadataModel
    {
        public const string SpecializationType = "Navigation";
        protected readonly IAssociation _association;
        protected NavigationSourceEndModel _sourceEnd;
        protected NavigationTargetEndModel _targetEnd;

        public NavigationModel(IAssociation association, string requiredType = SpecializationType)
        {
            if (!requiredType.Equals(association.SpecializationType, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new Exception($"Cannot create a '{GetType().Name}' from association with specialization type '{association.SpecializationType}'. Must be of type '{SpecializationType}'");
            }
            _association = association;
        }

        public static NavigationModel CreateFromEnd(IAssociationEnd associationEnd)
        {
            var association = new NavigationModel(associationEnd.Association);
            return association;
        }

        public string Id => _association.Id;

        public NavigationSourceEndModel SourceEnd => _sourceEnd ?? (_sourceEnd = new NavigationSourceEndModel(_association.SourceEnd, this));

        public NavigationTargetEndModel TargetEnd => _targetEnd ?? (_targetEnd = new NavigationTargetEndModel(_association.TargetEnd, this));

        public IAssociation InternalAssociation => _association;

        public override string ToString()
        {
            return _association.ToString();
        }

        public bool Equals(NavigationModel other)
        {
            return Equals(_association, other?._association);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((NavigationModel)obj);
        }

        public override int GetHashCode()
        {
            return (_association != null ? _association.GetHashCode() : 0);
        }
        public const string SpecializationTypeId = "6d2b2070-c1cb-4cd2-88b4-4e5f8414bd9e";
    }

    [IntentManaged(Mode.Fully)]
    public class NavigationSourceEndModel : NavigationEndModel
    {
        public const string SpecializationTypeId = "97a3de8a-c9bf-4cf2-bc0a-b8692b02211b";

        public NavigationSourceEndModel(IAssociationEnd associationEnd, NavigationModel association) : base(associationEnd, association)
        {
        }
    }

    [IntentManaged(Mode.Fully)]
    public class NavigationTargetEndModel : NavigationEndModel
    {
        public const string SpecializationTypeId = "2b191288-ecae-4743-b069-cbdd927ef349";

        public NavigationTargetEndModel(IAssociationEnd associationEnd, NavigationModel association) : base(associationEnd, association)
        {
        }
    }

    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class NavigationEndModel : ITypeReference, IMetadataModel, IHasName, IHasStereotypes
    {
        protected readonly IAssociationEnd _associationEnd;
        private readonly NavigationModel _association;

        public NavigationEndModel(IAssociationEnd associationEnd, NavigationModel association)
        {
            _associationEnd = associationEnd;
            _association = association;
        }

        public static NavigationEndModel Create(IAssociationEnd associationEnd)
        {
            var association = new NavigationModel(associationEnd.Association);
            return association.TargetEnd.Id == associationEnd.Id ? (NavigationEndModel)association.TargetEnd : association.SourceEnd;
        }

        [IntentManaged(Mode.Ignore)]
        public NavigationEndModel GetOtherEnd()
        {
            return this.Equals(_association.SourceEnd) ? (NavigationEndModel)Association.TargetEnd : Association.SourceEnd;
        }

        public string Id => _associationEnd.Id;
        public string SpecializationType => _associationEnd.SpecializationType;
        public string SpecializationTypeId => _associationEnd.SpecializationTypeId;
        public string Name => _associationEnd.Name;
        public NavigationModel Association => _association;
        public bool IsNavigable => _associationEnd.IsNavigable;
        public bool IsNullable => _associationEnd.TypeReference.IsNullable;
        public bool IsCollection => _associationEnd.TypeReference.IsCollection;
        public ICanBeReferencedType Element => _associationEnd.TypeReference.Element;
        public IEnumerable<ITypeReference> GenericTypeParameters => _associationEnd.TypeReference.GenericTypeParameters;
        public string Comment => _associationEnd.Comment;
        public ITypeReference TypeReference => this;
        public IPackage Package => Element?.Package;
        public IEnumerable<IStereotype> Stereotypes => _associationEnd.Stereotypes;

        public NavigationEndModel OtherEnd()
        {
            return this.Equals(_association.SourceEnd) ? (NavigationEndModel)_association.TargetEnd : (NavigationEndModel)_association.SourceEnd;
        }

        public bool IsTargetEnd()
        {
            return _associationEnd.IsTargetEnd();
        }

        public bool IsSourceEnd()
        {
            return _associationEnd.IsSourceEnd();
        }

        public override string ToString()
        {
            return _associationEnd.ToString();
        }

        public bool Equals(NavigationEndModel other)
        {
            return Equals(_associationEnd, other._associationEnd);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((NavigationEndModel)obj);
        }

        public override int GetHashCode()
        {
            return (_associationEnd != null ? _associationEnd.GetHashCode() : 0);
        }

        [IntentManaged(Mode.Ignore)]
        public RouteModel GetNavigationRoute()
        {
            var module = new ComponentModel((IElement)Element).Module;
            var otherEndModule = new ComponentModel((IElement)_associationEnd.OtherEnd().TypeReference.Element).Module;
            var modulesToCheck = new[] { otherEndModule, module }.Concat(module.GetParentFolders().OfType<ModuleModel>()).ToList();
            var routes = modulesToCheck.SelectMany(x => x.Routing?.Routes ?? new List<RouteModel>()).ToList();
            var route = routes.FirstOrDefault(x => x.TypeReference.Element.Id == Element.Id);
            return route;
        }

        public IAssociation InternalAssociation => _association.InternalAssociation;

        public IAssociationEnd InternalAssociationEnd => _associationEnd;

    }

    [IntentManaged(Mode.Fully)]
    public static class NavigationEndModelExtensions
    {
        public static bool IsNavigationEndModel(this ICanBeReferencedType type)
        {
            return IsNavigationTargetEndModel(type) || IsNavigationSourceEndModel(type);
        }

        public static NavigationEndModel AsNavigationEndModel(this ICanBeReferencedType type)
        {
            return (NavigationEndModel)type.AsNavigationTargetEndModel() ?? (NavigationEndModel)type.AsNavigationSourceEndModel();
        }

        public static bool IsNavigationTargetEndModel(this ICanBeReferencedType type)
        {
            return type != null && type is IAssociationEnd associationEnd && associationEnd.SpecializationTypeId == NavigationTargetEndModel.SpecializationTypeId;
        }

        public static NavigationTargetEndModel AsNavigationTargetEndModel(this ICanBeReferencedType type)
        {
            return type.IsNavigationTargetEndModel() ? new NavigationModel(((IAssociationEnd)type).Association).TargetEnd : null;
        }

        public static bool IsNavigationSourceEndModel(this ICanBeReferencedType type)
        {
            return type != null && type is IAssociationEnd associationEnd && associationEnd.SpecializationTypeId == NavigationSourceEndModel.SpecializationTypeId;
        }

        public static NavigationSourceEndModel AsNavigationSourceEndModel(this ICanBeReferencedType type)
        {
            return type.IsNavigationSourceEndModel() ? new NavigationModel(((IAssociationEnd)type).Association).SourceEnd : null;
        }
    }
}