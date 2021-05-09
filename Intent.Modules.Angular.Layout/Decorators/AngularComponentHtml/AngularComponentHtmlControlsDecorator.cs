using System.Linq;
using System.Text;
using Intent.Angular.Layout.Api;
using Intent.Engine;
using Intent.Modules.Angular.Layout.Decorators.Controls.ButtonControl;
using Intent.Modules.Angular.Layout.Decorators.Controls.Form;
using Intent.Modules.Angular.Layout.Decorators.Controls.Navbar;
using Intent.Modules.Angular.Layout.Decorators.Controls.PaginationControl;
using Intent.Modules.Angular.Layout.Decorators.Controls.Section;
using Intent.Modules.Angular.Layout.Decorators.Controls.TableControl;
using Intent.Modules.Angular.Templates.Component.AngularComponentHtmlTemplate;
using Intent.Modules.Angular.Templates.Component.Controls;
using Intent.RoslynWeaver.Attributes;
using Intent.Utils;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.ProjectItemTemplate.Partial", Version = "1.0")]

namespace Intent.Modules.Angular.Layout.Decorators.AngularComponentHtml
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class AngularComponentHtmlControlsDecorator : IAngularComponentHtmlDecorator
    {
        public const string DecoratorId = "Angular.Layout.AngularComponentHtmlDecorator.Controls";
        public readonly AngularComponentHtmlTemplate Template;

        public AngularComponentHtmlControlsDecorator(AngularComponentHtmlTemplate template)
        {
            Template = template;
        }

        public void RegisterControls(ControlWriter controlWriter)
        {
            controlWriter.RegisterControl(SectionModel.SpecializationTypeId, control =>  new SectionTemplate(new SectionModel(control), controlWriter));
            controlWriter.RegisterControl(ButtonControlModel.SpecializationTypeId, control =>  new ButtonControlTemplate(new ButtonControlModel(control)));
            controlWriter.RegisterControl(TableControlModel.SpecializationTypeId, control =>  new TableControlTemplate(new TableControlModel(control)));
            controlWriter.RegisterControl(PaginationControlModel.SpecializationTypeId, control =>  new PaginationControlTemplate(new PaginationControlModel(control), Template.ExecutionContext.EventDispatcher));
            controlWriter.RegisterControl(FormModel.SpecializationTypeId, control =>  new FormTemplate(new FormModel(control), Template.ExecutionContext.EventDispatcher));
            controlWriter.RegisterControl(NavbarModel.SpecializationTypeId, control =>  new NavbarTemplate(new NavbarModel(control), (IApplication)Template.ExecutionContext));
        }

        public int Priority { get; } = 0;
    }
}