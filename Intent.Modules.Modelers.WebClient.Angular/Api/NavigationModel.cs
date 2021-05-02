using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Metadata.Models;
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
    }

    [IntentManaged(Mode.Fully)]
    public class NavigationSourceEndModel : NavigationEndModel
    {
        public NavigationSourceEndModel(IAssociationEnd associationEnd, NavigationModel association) : base(associationEnd, association)
        {
        }
    }

    [IntentManaged(Mode.Fully)]
    public class NavigationTargetEndModel : NavigationEndModel
    {
        public NavigationTargetEndModel(IAssociationEnd associationEnd, NavigationModel association) : base(associationEnd, association)
        {
        }
    }

    [IntentManaged(Mode.Fully)]
    public class NavigationEndModel : IAssociationEnd
    {
        protected readonly IAssociationEnd _associationEnd;
        private readonly NavigationModel _association;

        public NavigationEndModel(IAssociationEnd associationEnd, NavigationModel association)
        {
            _associationEnd = associationEnd;
            _association = association;
        }

        public string Id => _associationEnd.Id;
        public string Name => _associationEnd.Name;
        public NavigationModel Association => _association;
        IAssociation IAssociationEnd.Association => _association.InternalAssociation;
        public bool IsNavigable => _associationEnd.IsNavigable;
        public bool IsNullable => _associationEnd.IsNullable;
        public bool IsCollection => _associationEnd.IsCollection;
        public ICanBeReferencedType Element => _associationEnd.Element;
        public IEnumerable<ITypeReference> GenericTypeParameters => _associationEnd.GenericTypeParameters;
        public string Comment => _associationEnd.Comment;
        public IEnumerable<IStereotype> Stereotypes => _associationEnd.Stereotypes;

        IAssociationEnd IAssociationEnd.OtherEnd()
        {
            return this.Equals(_association.SourceEnd) ? (IAssociationEnd)_association.TargetEnd : (IAssociationEnd)_association.SourceEnd;
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
    }
}