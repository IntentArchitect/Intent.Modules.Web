using Intent.Engine;
using Intent.Modules.Angular.Templates.Component.AngularComponentHtml;
using Intent.Modules.Common.Registrations;

namespace Intent.Modules.Angular.ApiAuthorization.HtmlControls
{
    public class AuthorizationControlsDecoratorRegistration : DecoratorRegistration<AngularComponentHtmlTemplate, IAngularComponentHtmlDecorator>
    {
        public override string DecoratorId => AuthorizationControlsDecorator.DecoratorId;

        public override IAngularComponentHtmlDecorator CreateDecoratorInstance(AngularComponentHtmlTemplate template, IApplication application)
        {
            return new AuthorizationControlsDecorator(template);
        }
    }
}