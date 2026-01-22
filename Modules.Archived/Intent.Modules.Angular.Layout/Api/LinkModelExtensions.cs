using Intent.Metadata.Models;

namespace Intent.Angular.Layout.Api
{
    public static class CustomLinkModelExtensions
    {
        public static bool IsLink(this IElement element)
        {
            return element.SpecializationTypeId == LinkModel.SpecializationTypeId;
        }

        public static LinkModel AsLinkModel(this IElement element)
        {
            return element.IsLink() ? new LinkModel(element) : null;
        }
    }
}