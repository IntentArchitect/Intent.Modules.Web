using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Metadata.Models;
using Intent.Modelers.WebClient.Angular.Api;
using Intent.Modules.Common;
using Intent.Modules.Common.Templates;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.Api.ApiElementModel", Version = "1.0")]

namespace Intent.Angular.Layout.Api
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class ButtonControlModel : IMetadataModel, IHasStereotypes, IHasName, IElementWrapper
    {
        public const string SpecializationType = "Button Control";
        protected readonly IElement _element;

        [IntentManaged(Mode.Ignore)]
        public ButtonControlModel(IElement element, string requiredType = SpecializationType)
        {
            if (!requiredType.Equals(element.SpecializationType, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new Exception($"Cannot create a '{GetType().Name}' from element with specialization type '{element.SpecializationType}'. Must be of type '{SpecializationType}'");
            }
            _element = element;
            if (InternalElement.IsMapped)
            {
                ClickCommandPath = string.Join(".", InternalElement.MappedElement.Path.Select(x => x.Name.ToCamelCase()));
                if (InternalElement.MappedElement.Element.SpecializationTypeId == NavigationTargetEndModel.SpecializationTypeId ||
                    InternalElement.MappedElement.Element.SpecializationTypeId == NavigationSourceEndModel.SpecializationTypeId)
                {
                    // This is a best-attempt hack.
                    var associationEnd = (IAssociationEnd)InternalElement.MappedElement.Element;
                    var navigationModel = new NavigationModel(associationEnd.Association);
                    var navigationEndModel = associationEnd.IsTargetEnd() ? (NavigationEndModel)navigationModel.TargetEnd : navigationModel.SourceEnd;
                    var component = new ComponentModel((IElement)navigationEndModel.Element);
                    var sourceComponent = new ComponentModel((IElement)navigationEndModel.GetOtherEnd().Element);
                    ClickCommandPath += $"({string.Join(", ", component.Inputs.Select(x => sourceComponent.Models.Any(m => m.Name == x.Name) || sourceComponent.Inputs.Any(m => m.Name == x.Name) ? x.Name : x.Name.ToDotCase()))})";
                }
                else
                {
                    ClickCommandPath += "()";
                }
            }
        }

        [IntentManaged(Mode.Ignore)]
        public string ClickCommandPath { get; }

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
        public bool Equals(ButtonControlModel other)
        {
            return Equals(_element, other?._element);
        }

        [IntentManaged(Mode.Fully)]
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ButtonControlModel)obj);
        }

        [IntentManaged(Mode.Fully)]
        public override int GetHashCode()
        {
            return (_element != null ? _element.GetHashCode() : 0);
        }

        [IntentManaged(Mode.Fully)]
        public bool IsMapped => _element.IsMapped;

        [IntentManaged(Mode.Fully)]
        public IElementMapping Mapping => _element.MappedElement;
        public const string SpecializationTypeId = "13eca516-0628-4085-91c3-36d5eddda6c9";

        public string Comment => _element.Comment;
    }

    [IntentManaged(Mode.Fully)]
    public static class ButtonControlModelExtensions
    {

        public static bool IsButtonControlModel(this ICanBeReferencedType type)
        {
            return type != null && type is IElement element && element.SpecializationTypeId == ButtonControlModel.SpecializationTypeId;
        }

        public static ButtonControlModel AsButtonControlModel(this ICanBeReferencedType type)
        {
            return type.IsButtonControlModel() ? new ButtonControlModel((IElement)type) : null;
        }

        public static bool HasNewMappingSettingsMapping(this ButtonControlModel type)
        {
            return type.Mapping?.MappingSettingsId == "67e826e4-cf1c-4554-ab6f-be922c2d6a9f";
        }

        public static IElementMapping GetNewMappingSettingsMapping(this ButtonControlModel type)
        {
            return type.HasNewMappingSettingsMapping() ? type.Mapping : null;
        }
    }
}