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

        private string GetButtonType()
        {
            if (Model.GetButtonSettings().Type().IsButton())
            {
                return "button";
            }
            if (Model.GetButtonSettings().Type().IsSubmit())
            {
                return "submit";
            }
            if (Model.GetButtonSettings().Type().IsReset())
            {
                return "reset";
            }
            if (Model.GetButtonSettings().Type().IsMenu())
            {
                return "menu";
            }
            return "button";
        }
    }
}
