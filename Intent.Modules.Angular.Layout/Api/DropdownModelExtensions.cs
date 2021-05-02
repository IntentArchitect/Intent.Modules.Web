using Intent.Metadata.Models;

namespace Intent.Angular.Layout.Api
{
    public static class DropdownModelExtensions
    {
        public static bool IsDropdown(this IElement element)
        {
            return element.SpecializationTypeId == DropdownModel.SpecializationTypeId;
        }

        public static DropdownModel AsIsDropdownModel(this IElement element)
        {
            return element.IsDropdown() ? new DropdownModel(element) : null;
        }
    }
}