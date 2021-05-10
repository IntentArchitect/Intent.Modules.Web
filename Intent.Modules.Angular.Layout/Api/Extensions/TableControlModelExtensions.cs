using System;
using Intent.Metadata.Models;
using Intent.Modules.Common;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.Api.ApiElementModelExtensions", Version = "1.0")]

namespace Intent.Angular.Layout.Api
{
    public static class TableControlModelExtensions
    {
        public static TableSettings GetTableSettings(this TableControlModel model)
        {
            var stereotype = model.GetStereotype("Table Settings");
            return stereotype != null ? new TableSettings(stereotype) : null;
        }

        public static bool HasTableSettings(this TableControlModel model)
        {
            return model.HasStereotype("Table Settings");
        }


        public class TableSettings
        {
            private IStereotype _stereotype;

            public TableSettings(IStereotype stereotype)
            {
                _stereotype = stereotype;
            }

            public string Name => _stereotype.Name;

            public ICanBeReferencedType OnSelectRow()
            {
                return _stereotype.GetProperty<ICanBeReferencedType>("On Select Row");
            }

        }

    }
}