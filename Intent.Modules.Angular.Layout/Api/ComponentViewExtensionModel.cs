using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Angular.Api;
using Intent.Metadata.Models;
using Intent.RoslynWeaver.Attributes;
using Intent.Modules.Angular.Api;
using Intent.Modules.Common;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.Api.ApiElementExtensionModel", Version = "1.0")]

namespace Intent.Angular.Layout.Api
{
    [IntentManaged(Mode.Merge)]
    public class ComponentViewExtensionModel : ComponentViewModel
    {
        [IntentManaged(Mode.Ignore)]
        public ComponentViewExtensionModel(IElement element) : base(element)
        {
        }

        public IList<TableControlModel> TableControls => _element.ChildElements
            .GetElementsOfType(TableControlModel.SpecializationTypeId)
            .Select(x => new TableControlModel(x))
            .ToList();

        public IList<PaginationControlModel> PaginationControls => _element.ChildElements
            .GetElementsOfType(PaginationControlModel.SpecializationTypeId)
            .Select(x => new PaginationControlModel(x))
            .ToList();

        public IList<FormModel> Forms => _element.ChildElements
            .GetElementsOfType(FormModel.SpecializationTypeId)
            .Select(x => new FormModel(x))
            .ToList();

        public IList<ButtonControlModel> ButtonControls => _element.ChildElements
            .GetElementsOfType(ButtonControlModel.SpecializationTypeId)
            .Select(x => new ButtonControlModel(x))
            .ToList();

        public IList<SectionModel> Sections => _element.ChildElements
            .GetElementsOfType(SectionModel.SpecializationTypeId)
            .Select(x => new SectionModel(x))
            .ToList();

        public IList<TabsModel> Tabses => _element.ChildElements
            .GetElementsOfType(TabsModel.SpecializationTypeId)
            .Select(x => new TabsModel(x))
            .ToList();

    }
}