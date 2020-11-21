using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Metadata.Models;
using Intent.Modelers.Types.ServiceProxies.Api;
using Intent.RoslynWeaver.Attributes;
using Intent.Modules.Angular.Api;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("ModuleBuilder.Templates.Api.ApiElementExtensionModel", Version = "1.0")]

namespace Intent.Angular.ServiceProxies.Api
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class ModuleExtensionModel : ModuleModel
    {
        [IntentManaged(Mode.Ignore)]
        public ModuleExtensionModel(IElement element) : base(element)
        {
        }

        public IList<ServiceProxyModel> ServiceProxies => _element.ChildElements
            .Where(x => x.SpecializationType == ServiceProxyModel.SpecializationType)
            .Select(x => new ServiceProxyModel(x))
            .ToList();

    }
}