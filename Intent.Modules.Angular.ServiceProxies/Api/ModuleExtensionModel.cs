using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Angular.Api;
using Intent.Metadata.Models;
using Intent.Modelers.Types.ServiceProxies.Api;
using Intent.Modelers.WebClient.Angular.Api;
using Intent.Modules.Angular.Api;
using Intent.Modules.Common;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.Api.ApiElementExtensionModel", Version = "1.0")]

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
            .GetElementsOfType(ServiceProxyModel.SpecializationTypeId)
            .Select(x => new ServiceProxyModel(x))
            .ToList();

    }
}