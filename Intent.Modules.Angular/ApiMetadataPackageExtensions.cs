using System.Collections.Generic;
using System.Linq;
using System.Net;
using Intent.Metadata.Models;
using Intent.Modelers.WebClient.Api;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("ModuleBuilder.Templates.Api.ApiMetadataPackageExtensions", Version = "1.0")]

namespace Intent.Modules.Angular.Api
{
    public static class ApiMetadataPackageExtensions
    {
        public static IList<AngularWebAppModel> GetAngularWebAppModels(this IDesigner designer)
        {
            return designer.GetPackagesOfType(WebClientModel.SpecializationTypeId)
                .Select(x => new AngularWebAppModel(x))
                .ToList();
        }
    }
}