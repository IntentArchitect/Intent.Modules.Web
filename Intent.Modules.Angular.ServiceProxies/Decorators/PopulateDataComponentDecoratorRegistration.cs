using System.ComponentModel;
using Intent.Engine;
using Intent.Modules.Angular.Templates.Component.AngularComponentTsTemplate;
using Intent.Modules.Common.Registrations;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateDecoratorRegistration", Version = "1.0")]

namespace Intent.Modules.Angular.ServiceProxies.Decorators
{
    [Description(PopulateDataComponentDecorator.DecoratorId)]
    public class PopulateDataComponentDecoratorRegistration : DecoratorRegistration<AngularComponentTsTemplate, AngularComponentTsDecorator>
    {

        [IntentManaged(Mode.Ignore)]
        public override AngularComponentTsDecorator CreateDecoratorInstance(AngularComponentTsTemplate template, IApplication application)
        {
            return new PopulateDataComponentDecorator(template, application);
        }

        public override string DecoratorId => PopulateDataComponentDecorator.DecoratorId;
    }
}