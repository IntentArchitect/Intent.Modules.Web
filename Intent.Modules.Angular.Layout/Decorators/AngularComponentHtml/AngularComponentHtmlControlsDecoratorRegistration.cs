using Intent.Engine;
using Intent.Modules.Angular.Templates.Component.AngularComponentHtmlTemplate;
using Intent.Modules.Common.Registrations;

namespace Intent.Modules.Angular.Layout.Decorators.AngularComponentHtml
{
    public class AngularComponentHtmlControlsDecoratorRegistration : DecoratorRegistration<AngularComponentHtmlTemplate, IAngularComponentHtmlDecorator>
    {
        public override string DecoratorId => AngularComponentHtmlControlsDecorator.DecoratorId;

        public override IAngularComponentHtmlDecorator CreateDecoratorInstance(AngularComponentHtmlTemplate template, IApplication application)
        {
            return new AngularComponentHtmlControlsDecorator(template);
        }
    }
}