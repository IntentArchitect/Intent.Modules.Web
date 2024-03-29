using Intent.Engine;
using Intent.Metadata.Models;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.Api.ApiMetadataDesignerExtensions", Version = "1.0")]

namespace Intent.Modelers.WebClient.Api
{
    public static class ApiMetadataDesignerExtensions
    {
        public static IDesigner WebClient(this IMetadataManager metadataManager, IApplication application)
        {
            return metadataManager.WebClient(application.Id);
        }

        public static IDesigner WebClient(this IMetadataManager metadataManager, string applicationId)
        {
            return metadataManager.GetDesigner(applicationId, "Web Client");
        }

    }
}