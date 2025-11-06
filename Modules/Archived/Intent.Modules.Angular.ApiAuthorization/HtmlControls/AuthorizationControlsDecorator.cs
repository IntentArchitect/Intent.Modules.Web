using Intent.Angular.ApiAuthorization.Api;
using Intent.Angular.Layout.Api;
using Intent.Modules.Angular.ApiAuthorization.HtmlControls.LoginMenu;
using Intent.Modules.Angular.Layout.Decorators.Controls.Section;
using Intent.Modules.Angular.Templates.Component.AngularComponentHtml;
using Intent.Modules.Angular.Templates.Component.Controls;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.ProjectItemTemplate.Partial", Version = "1.0")]

namespace Intent.Modules.Angular.ApiAuthorization.HtmlControls
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class AuthorizationControlsDecorator : IAngularComponentHtmlDecorator
    {
        public const string DecoratorId = "Intent.Angular.ApiAuthorization.AuthorizationControlsDecorator";
        public readonly AngularComponentHtmlTemplate Template;

        public AuthorizationControlsDecorator(AngularComponentHtmlTemplate template)
        {
            Template = template;
        }

        public void RegisterControls(ControlWriter controlWriter)
        {
            controlWriter.RegisterControl(LoginMenuModel.SpecializationTypeId, control =>  new LoginMenuTemplate(new LoginMenuModel(control)));
        }

        public int Priority { get; } = 0;
    }
}