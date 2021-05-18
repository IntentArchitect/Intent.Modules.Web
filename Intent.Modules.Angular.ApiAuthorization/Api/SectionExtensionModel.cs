using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Angular.Layout.Api;
using Intent.Metadata.Models;
using Intent.Modules.Common;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.Api.ApiElementExtensionModel", Version = "1.0")]

namespace Intent.Angular.ApiAuthorization.Api
{
    [IntentManaged(Mode.Merge)]
    public class SectionExtensionModel : SectionModel
    {
        [IntentManaged(Mode.Ignore)]
        public SectionExtensionModel(IElement element) : base(element)
        {
        }

        public IList<LoginMenuModel> LoginMenus => _element.ChildElements
            .GetElementsOfType(LoginMenuModel.SpecializationTypeId)
            .Select(x => new LoginMenuModel(x))
            .ToList();

    }
}