using Intent.Metadata.Models;

namespace Intent.Angular.Layout.Api
{
    public static class DropdownModelStereotypeExtensions
    {
        public static bool IsDropdown(this IElement element)
        {
            return element.SpecializationTypeId == DropdownModel.SpecializationTypeId;
        }

        public static DropdownModel AsDropdownModel(this IElement element)
        {
            return element.IsDropdown() ? new DropdownModel(element) : null;
        }
    }
}