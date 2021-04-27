using System.Collections.Generic;
using Intent.Angular.Layout.Api;
using Intent.Eventing;
using Intent.Modules.Angular.Templates;
using Intent.Modules.Angular.Templates.Component;
using Intent.Modules.Angular.Templates.Component.Controls;

namespace Intent.Modules.Angular.Layout.Decorators.Controls.PaginationControl
{
    public partial class PaginationControlTemplate : IControl
    {
        public PaginationControlTemplate(PaginationControlModel model, IApplicationEventDispatcher eventDispatcher)
        {
            Model = model;
            eventDispatcher.Publish(new AngularImportDependencyRequiredEvent(
                moduleId: Model.Module.Id,
                dependency: "PaginationModule.forRoot()",
                import: "import { PaginationModule } from 'ngx-bootstrap/pagination';"));
        }

        public PaginationControlModel Model { get; }
    }
}
