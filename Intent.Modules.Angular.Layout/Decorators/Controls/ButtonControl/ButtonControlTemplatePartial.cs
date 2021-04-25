using Intent.Angular.Layout.Api;
using Intent.Modules.Angular.Templates.Component;
using Intent.Modules.Angular.Templates.Component.Controls;

namespace Intent.Modules.Angular.Layout.Decorators.Controls.ButtonControl
{
    public partial class ButtonControlTemplate : IControl
    {
        public ButtonControlTemplate(ButtonControlModel model)
        {
            Model = model;
        }

        public ButtonControlModel Model { get; }
    }
}
