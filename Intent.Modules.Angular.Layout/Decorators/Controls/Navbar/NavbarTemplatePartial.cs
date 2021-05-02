using Intent.Angular.Layout.Api;
using Intent.Modules.Angular.Templates.Component.Controls;

namespace Intent.Modules.Angular.Layout.Decorators.Controls.Navbar
{
    public partial class NavbarTemplate : IControl
    {
        public NavbarTemplate(NavbarModel model)
        {
            Model = model;
        }

        public NavbarModel Model { get; }
    }
}
