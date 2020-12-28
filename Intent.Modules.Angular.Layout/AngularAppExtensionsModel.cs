using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Metadata.Models;
using Intent.Modelers.WebClient.Api;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.Api.ApiPackageExtensionModel", Version = "1.0")]

namespace Intent.Angular.Layout.Api
{
    [IntentManaged(Mode.Merge)]
    public class AngularAppExtensionsModel : WebClientModel
    {
        [IntentManaged(Mode.Ignore)]
        public AngularAppExtensionsModel(IPackage package) : base(package)
        {
        }

        public IList<FormControlModel> FormControlTypes => UnderlyingPackage.ChildElements
            .Where(x => x.SpecializationType == FormControlModel.SpecializationType)
            .Select(x => new FormControlModel(x))
            .ToList();
    }
}