﻿using Intent.Angular.Layout.Api;
using Intent.Engine;
using Intent.Eventing;
using Intent.Modules.Angular.Templates;
using Intent.Modules.Angular.Templates.Component.Controls;

namespace Intent.Modules.Angular.Layout.Decorators.Controls.Navbar
{
    public partial class NavbarTemplate : IControl
    {
        public NavbarTemplate(NavbarModel model, IApplication application)
        {
            Model = model;
            Application = application;
            application.EventDispatcher.Publish(new AngularImportDependencyRequiredEvent(
                moduleId: "AppModule",
                dependency: "CollapseModule.forRoot()",
                import: "import { CollapseModule } from 'ngx-bootstrap/collapse';"));
        }

        public NavbarModel Model { get; }
        public IApplication Application { get; }
    }
}
