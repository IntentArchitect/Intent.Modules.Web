using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Metadata.Models;
using Intent.Modelers.WebClient.Angular.Api;
using Intent.Modules.Common;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.Api.ApiElementExtensionModel", Version = "1.0")]

namespace Intent.Angular.Layout.Api
{
    [IntentManaged(Mode.Merge)]
    public class ModuleExtensionModel : ModuleModel
    {
        [IntentManaged(Mode.Ignore)]
        public ModuleExtensionModel(IElement element) : base(element)
        {
        }

    }
}