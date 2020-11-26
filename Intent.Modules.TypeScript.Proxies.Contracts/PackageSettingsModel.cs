using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Metadata.Models;
using Intent.Modelers.Types.ServiceProxies.Api;
using Intent.Modelers.WebClient.Api;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("ModuleBuilder.Templates.Api.ApiPackageExtensionModel", Version = "1.0")]

namespace Intent.Modules.TypeScript.Proxies.Contracts
{
    [IntentManaged(Mode.Merge)]
    public class PackageSettingsModel : WebClientModel
    {
        [IntentManaged(Mode.Ignore)]
        public PackageSettingsModel(IPackage package) : base(package)
        {
        }

        public IList<ServiceProxyModel> ServiceProxies => UnderlyingPackage.ChildElements
            .Where(x => x.SpecializationType == ServiceProxyModel.SpecializationType)
            .Select(x => new ServiceProxyModel(x))
            .ToList();

    }
}