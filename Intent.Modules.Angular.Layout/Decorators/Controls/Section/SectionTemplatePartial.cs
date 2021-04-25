using Intent.Angular.Layout.Api;
using Intent.Eventing;
using Intent.Modules.Angular.Templates.Component;
using Intent.Modules.Angular.Templates.Component.AngularComponentHtmlTemplate;
using Intent.Modules.Angular.Templates.Component.Controls;
using Intent.Utils;

namespace Intent.Modules.Angular.Layout.Decorators.Controls.Section
{
    public partial class SectionTemplate : IControl
    {
        public SectionTemplate(SectionModel model, ControlWriter controlWriter)
        {
            Model = model;
            ControlWriter = controlWriter;
        }

        public SectionModel Model { get; }
        public ControlWriter ControlWriter { get; }
    }
}
