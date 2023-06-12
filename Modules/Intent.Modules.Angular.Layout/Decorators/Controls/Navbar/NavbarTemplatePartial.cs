using Intent.Angular.Layout.Api;
using Intent.Engine;
using Intent.Eventing;
using Intent.Modules.Angular.Templates;
using Intent.Modules.Angular.Templates.Component.Controls;

namespace Intent.Modules.Angular.Layout.Decorators.Controls.Navbar
{
    public partial class NavbarTemplate : IControl
    {
        public NavbarTemplate(NavbarModel model, IApplication application, ControlWriter controlWriter)
        {
            Model = model;
            Application = application;
            ControlWriter = controlWriter;
            application.EventDispatcher.Publish(new AngularImportDependencyRequiredEvent(
                moduleId: "AppModule",
                dependency: "CollapseModule.forRoot()",
                import: "import { CollapseModule } from 'ngx-bootstrap/collapse';"));
            application.EventDispatcher.Publish(new AngularComponentFieldRequiredEvent(
                element: model.InternalElement,
                name: "isCollapsed",
                type: "boolean",
                defaultValue: "false"));
        }

        public NavbarModel Model { get; }
        public IApplication Application { get; }
        public ControlWriter ControlWriter { get; }
    }
}
