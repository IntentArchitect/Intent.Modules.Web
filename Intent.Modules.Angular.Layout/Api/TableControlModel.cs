using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Metadata.Models;
using Intent.Modules.Common;
using Intent.Modules.Common.Templates;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.Api.ApiElementModel", Version = "1.0")]

namespace Intent.Angular.Layout.Api
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class TableControlModel : IMetadataModel, IHasStereotypes, IHasName
    {
        [IntentManaged(Mode.Fully)] public const string SpecializationType = "Table Control";
        protected readonly IElement _element;

        [IntentManaged(Mode.Ignore)]
        public TableControlModel(IElement element, string requiredType = SpecializationType)
        {
            if (!requiredType.Equals(element.SpecializationType, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new Exception($"Cannot create a '{GetType().Name}' from element with specialization type '{element.SpecializationType}'. Must be of type '{SpecializationType}'");
            }
            _element = element;
            if (Mapping != null)
            {
                DataModelPath = string.Join("?.", Mapping.Path.Select(x => x.Name.ToCamelCase()));
                //if (Mapping.Element.TypeReference.IsCollection)
                //{
                //    DataModel = (IElement)Mapping.Element.TypeReference.Element;
                //}
                //else
                //{
                //    foreach (var childElement in ((IElement)Mapping.Element.TypeReference.Element).ChildElements)
                //    {
                //        if (childElement.TypeReference.IsCollection)
                //        {
                //            DataModelPath += "?." + childElement.Name.ToCamelCase();
                //            // Not robust:
                //            if (childElement.TypeReference.Element.SpecializationType == "Generic Type")
                //            {
                //                DataModel = (IElement)Mapping.Element.TypeReference.GenericTypeParameters.First().Element;
                //            }
                //            else
                //            {
                //                DataModel = (IElement)childElement.TypeReference.Element;
                //            }
                //            break;
                //        }
                //    }
                //}
            }
        }

        [IntentManaged(Mode.Ignore)]
        public string DataModelPath { get; }

        public bool IsValid()
        {
            return DataModelPath != null;
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
        public bool Equals(TableControlModel other)
        {
            return Equals(_element, other?._element);
        }

        [IntentManaged(Mode.Fully)]
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TableControlModel)obj);
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
        public const string SpecializationTypeId = "e302d9ca-268e-4a98-ad8d-4434aefb9903";

        public IList<TableColumnModel> Columns => _element.ChildElements
                    .Where(x => x.SpecializationType == TableColumnModel.SpecializationType)
                    .Select(x => new TableColumnModel(x))
                    .ToList();

        public string Comment => _element.Comment;
    }

    [IntentManaged(Mode.Fully)]
    public static class TableControlModelExtensions
    {

        public static bool IsTableControlModel(this ICanBeReferencedType type)
        {
            return type != null && type is IElement element && element.SpecializationTypeId == TableControlModel.SpecializationTypeId;
        }

        public static TableControlModel AsTableControlModel(this ICanBeReferencedType type)
        {
            return type.IsTableControlModel() ? new TableControlModel((IElement)type) : null;
        }

        public static bool HasNewMappingSettingsMapping(this TableControlModel type)
        {
            return type.Mapping?.MappingSettingsId == "510bf1dc-08d2-4184-9358-499b96e41586";
        }

        public static IElementMapping GetNewMappingSettingsMapping(this TableControlModel type)
        {
            return type.HasNewMappingSettingsMapping() ? type.Mapping : null;
        }
    }
}