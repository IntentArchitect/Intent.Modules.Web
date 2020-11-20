using Intent.Engine;
using Intent.Metadata.Models;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("ModuleBuilder.Templates.Api.ApiMetadataDesignerExtensions", Version = "1.0")]

namespace Intent.Modelers.WebClient.Api
{
    public static class ApiMetadataDesignerExtensions
    {
        public static IDesigner WebClient(this IMetadataManager metadataManager, IApplication application)
        {
            return metadataManager.GetDesigner(application.Id, "Web Client");
        }

    }
}