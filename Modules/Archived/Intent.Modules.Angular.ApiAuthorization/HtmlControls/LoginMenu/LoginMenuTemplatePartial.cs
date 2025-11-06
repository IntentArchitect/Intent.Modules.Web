using Intent.Angular.ApiAuthorization.Api;
using Intent.Angular.Layout.Api;
using Intent.Modules.Angular.Templates.Component.Controls;

namespace Intent.Modules.Angular.ApiAuthorization.HtmlControls.LoginMenu
{
    public partial class LoginMenuTemplate : IControl
    {
        public LoginMenuTemplate(LoginMenuModel model)
        {
            Model = model;
        }

        public LoginMenuModel Model { get; }
    }
}
