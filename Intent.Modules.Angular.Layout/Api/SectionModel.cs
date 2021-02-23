using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Metadata.Models;
using Intent.RoslynWeaver.Attributes;
using Intent.Modules.Common;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.Api.ApiElementModel", Version = "1.0")]

namespace Intent.Angular.Layout.Api
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class SectionModel : IMetadataModel, IHasStereotypes, IHasName
    {
        public const string SpecializationType = "Section";
        protected readonly IElement _element;

        [IntentManaged(Mode.Ignore)]
        public SectionModel(IElement element, string requiredType = SpecializationType)
        {
            if (!requiredType.Equals(element.SpecializationType, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new Exception($"Cannot create a '{GetType().Name}' from element with specialization type '{element.SpecializationType}'. Must be of type '{SpecializationType}'");
            }
            _element = element;
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
        public IList<TableControlModel> TableControls => _element.ChildElements
            .GetElementsOfType(TableControlModel.SpecializationTypeId)
            .Select(x => new TableControlModel(x))
            .ToList();

        [IntentManaged(Mode.Fully)]
        public IList<PaginationControlModel> PaginationControls => _element.ChildElements
            .GetElementsOfType(PaginationControlModel.SpecializationTypeId)
            .Select(x => new PaginationControlModel(x))
            .ToList();

        [IntentManaged(Mode.Fully)]
        public IList<FormModel> Forms => _element.ChildElements
            .GetElementsOfType(FormModel.SpecializationTypeId)
            .Select(x => new FormModel(x))
            .ToList();

        [IntentManaged(Mode.Fully)]
        public IList<ButtonControlModel> ButtonControls => _element.ChildElements
            .GetElementsOfType(ButtonControlModel.SpecializationTypeId)
            .Select(x => new ButtonControlModel(x))
            .ToList();

        [IntentManaged(Mode.Fully)]
        public IList<SectionModel> Sections => _element.ChildElements
            .GetElementsOfType(SectionModel.SpecializationTypeId)
            .Select(x => new SectionModel(x))
            .ToList();

        [IntentManaged(Mode.Fully)]
        public override string ToString()
        {
            return _element.ToString();
        }

        [IntentManaged(Mode.Fully)]
        public bool Equals(SectionModel other)
        {
            return Equals(_element, other?._element);
        }

        [IntentManaged(Mode.Fully)]
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SectionModel)obj);
        }

        [IntentManaged(Mode.Fully)]
        public override int GetHashCode()
        {
            return (_element != null ? _element.GetHashCode() : 0);
        }
        public const string SpecializationTypeId = "45072fbf-b34a-4625-b7ea-d5b1962a5b60";
    }
}