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
    public class PaginationControlModel : IMetadataModel, IHasStereotypes, IHasName, IElementWrapper
    {
        [IntentManaged(Mode.Fully)] public const string SpecializationType = "Pagination Control";
        protected readonly IElement _element;

        [IntentManaged(Mode.Ignore)]
        public PaginationControlModel(IElement element, string requiredType = SpecializationType)
        {
            if (!requiredType.Equals(element.SpecializationType, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new Exception($"Cannot create a '{GetType().Name}' from element with specialization type '{element.SpecializationType}'. Must be of type '{SpecializationType}'");
            }
            _element = element;
            if (Mapping != null)
            {
                DataModelPath = Mapping.Element.Name.ToCamelCase();
                TotalItemsPath = ((IElement)Mapping.Element.TypeReference.Element).ChildElements
                    .FirstOrDefault(x => x.Name.Contains("Total", StringComparison.InvariantCultureIgnoreCase))?.Name.ToCamelCase();
                PageNumberPath = ((IElement)Mapping.Element.TypeReference.Element).ChildElements
                    .FirstOrDefault(x => x.Name.Contains("Number", StringComparison.InvariantCultureIgnoreCase))?.Name.ToCamelCase();
            }
        }

        [IntentManaged(Mode.Ignore)]
        public ModuleModel Module => new ModuleModel(_element.GetParentPath().Reverse().First(x => x.SpecializationType == ModuleModel.SpecializationType));

        [IntentManaged(Mode.Ignore)]
        public string DataModelPath { get; }

        [IntentManaged(Mode.Ignore)]
        public string TotalItemsPath { get; set; }

        [IntentManaged(Mode.Ignore)]
        public string PageNumberPath { get; set; }

        [IntentManaged(Mode.Ignore)]
        public bool IsValid()
        {
            return DataModelPath != null && TotalItemsPath != null && PageNumberPath != null;
        }

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
        public bool Equals(PaginationControlModel other)
        {
            return Equals(_element, other?._element);
        }

        [IntentManaged(Mode.Fully)]
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PaginationControlModel)obj);
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
        public const string SpecializationTypeId = "3faee16c-8605-4bb4-8566-06a555c929a3";

        public string Comment => _element.Comment;

    }

    [IntentManaged(Mode.Fully)]
    public static class PaginationControlModelExtensions
    {

        public static bool IsPaginationControlModel(this ICanBeReferencedType type)
        {
            return type != null && type is IElement element && element.SpecializationTypeId == PaginationControlModel.SpecializationTypeId;
        }

        public static PaginationControlModel AsPaginationControlModel(this ICanBeReferencedType type)
        {
            return type.IsPaginationControlModel() ? new PaginationControlModel((IElement)type) : null;
        }

        public static bool HasMapFromModelMapping(this PaginationControlModel type)
        {
            return type.Mapping?.MappingSettingsId == "74ef00c1-212d-44a8-aa30-2ff44eba556e";
        }

        public static IElementMapping GetMapFromModelMapping(this PaginationControlModel type)
        {
            return type.HasMapFromModelMapping() ? type.Mapping : null;
        }
    }
}