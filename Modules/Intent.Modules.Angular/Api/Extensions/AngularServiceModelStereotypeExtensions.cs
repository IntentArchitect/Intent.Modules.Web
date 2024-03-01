using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Metadata.Models;
using Intent.Modelers.WebClient.Angular.Api;
using Intent.Modules.Common;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.Api.ApiElementModelExtensions", Version = "1.0")]

namespace Intent.Angular.Api
{
    public static class AngularServiceModelStereotypeExtensions
    {
        public static AngularServiceSettings GetAngularServiceSettings(this AngularServiceModel model)
        {
            var stereotype = model.GetStereotype("ede23670-71ce-42ef-ae7c-a37ff7ae46f3");
            return stereotype != null ? new AngularServiceSettings(stereotype) : null;
        }

        public static bool HasAngularServiceSettings(this AngularServiceModel model)
        {
            return model.HasStereotype("ede23670-71ce-42ef-ae7c-a37ff7ae46f3");
        }

        public static bool TryGetAngularServiceSettings(this AngularServiceModel model, out AngularServiceSettings stereotype)
        {
            if (!HasAngularServiceSettings(model))
            {
                stereotype = null;
                return false;
            }

            stereotype = new AngularServiceSettings(model.GetStereotype("ede23670-71ce-42ef-ae7c-a37ff7ae46f3"));
            return true;
        }


        public class AngularServiceSettings
        {
            private IStereotype _stereotype;

            public AngularServiceSettings(IStereotype stereotype)
            {
                _stereotype = stereotype;
            }

            public string Name => _stereotype.Name;

            public string Location()
            {
                return _stereotype.GetProperty<string>("Location");
            }

        }

    }
}