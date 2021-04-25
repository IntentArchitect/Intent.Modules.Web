using Intent.Angular.Layout.Api;
using Intent.Modules.Angular.Templates.Component;
using Intent.Modules.Angular.Templates.Component.Controls;

namespace Intent.Modules.Angular.Layout.Decorators.Controls.TableControl
{
    public partial class TableControlTemplate : IControl
    {
        public TableControlTemplate(TableControlModel model)
        {
            Model = model;
        }

        public TableControlModel Model { get; }
    }
}
