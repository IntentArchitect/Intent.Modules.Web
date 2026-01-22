using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Metadata.Models;
using Intent.Modules.Common;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.Api.ApiElementModelExtensions", Version = "1.0")]

namespace Intent.Angular.Layout.Api
{
    public static class TableControlModelStereotypeExtensions
    {
        public static TableSettings GetTableSettings(this TableControlModel model)
        {
            var stereotype = model.GetStereotype(TableSettings.DefinitionId);
            return stereotype != null ? new TableSettings(stereotype) : null;
        }

        public static bool HasTableSettings(this TableControlModel model)
        {
            return model.HasStereotype(TableSettings.DefinitionId);
        }

        public static bool TryGetTableSettings(this TableControlModel model, out TableSettings stereotype)
        {
            if (!HasTableSettings(model))
            {
                stereotype = null;
                return false;
            }

            stereotype = new TableSettings(model.GetStereotype(TableSettings.DefinitionId));
            return true;
        }


        public class TableSettings
        {
            private IStereotype _stereotype;
            public const string DefinitionId = "df59cc7e-978d-42e3-921f-3f43d120834e";

            public TableSettings(IStereotype stereotype)
            {
                _stereotype = stereotype;
            }

            public string Name => _stereotype.Name;

            public IElement OnSelectRow()
            {
                return _stereotype.GetProperty<IElement>("On Select Row");
            }

        }

    }
}