using Intent.Modelers.WebClient.Angular.Api;

namespace Intent.Modules.Angular.Templates.Component.Controls.RouterOutlet
{
    public class RouterOutletTemplate : IControl
    {

        public RouterOutletTemplate(RouterOutletModel model)
        {
        }

        public string TransformText()
        {
            return @"<router-outlet></router-outlet>";
        }
    }
}
