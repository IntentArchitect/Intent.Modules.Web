using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Metadata.Models;
using Intent.Modelers.UI.Api;
using Intent.Modules.Common;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.Api.ApiPackageExtensionModel", Version = "1.0")]

namespace Intent.Angular.Layout.Api
{
    [IntentManaged(Mode.Fully)]
    public class AngularAppExtensionsModel : UserInterfacePackageModel
    {
        [IntentManaged(Mode.Ignore)]
        public AngularAppExtensionsModel(IPackage package) : base(package)
        {
        }

        [IntentManaged(Mode.Fully)]
        public IList<FormControlModel> FormControlTypes => UnderlyingPackage.ChildElements
            .GetElementsOfType(FormControlModel.SpecializationTypeId)
            .Select(x => new FormControlModel(x))
            .ToList();
    }
}