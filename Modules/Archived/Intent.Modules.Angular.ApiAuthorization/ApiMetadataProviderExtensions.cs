using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Metadata.Models;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.Api.ApiMetadataProviderExtensions", Version = "1.0")]

namespace Intent.Angular.ApiAuthorization.Api
{
    public static class ApiMetadataProviderExtensions
    {
        public static IList<LoginMenuModel> GetLoginMenuModels(this IDesigner designer)
        {
            return designer.GetElementsOfType(LoginMenuModel.SpecializationTypeId)
                .Select(x => new LoginMenuModel(x))
                .ToList();
        }

    }
}