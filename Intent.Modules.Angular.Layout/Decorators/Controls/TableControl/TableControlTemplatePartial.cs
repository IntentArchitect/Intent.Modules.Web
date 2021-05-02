using System.Collections.Generic;
using Intent.Angular.Layout.Api;
using Intent.Modules.Angular.Templates.Component;
using Intent.Modules.Angular.Templates.Component.Controls;
using Intent.Modules.Common.Templates;

namespace Intent.Modules.Angular.Layout.Decorators.Controls.TableControl
{
    public partial class TableControlTemplate : IControl
    {
        public TableControlTemplate(TableControlModel model)
        {
            Model = model;
        }

        public TableControlModel Model { get; }

        private string GetTableRowAttributes()
        {
            var attributes = new List<string>();
            if (Model.GetTableSettings().OnSelectRow() != null)
            {
                attributes.Add($"(click)=\"{Model.GetTableSettings().OnSelectRow().Name}(item.id)\"");
            }

            return string.Join(" ", attributes);
        }

        private string GetTableName()
        {
            return Model.Name.Replace(" ", "").ToKebabCase();
        }
    }
}
