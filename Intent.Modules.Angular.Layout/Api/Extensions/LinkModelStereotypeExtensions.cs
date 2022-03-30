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
    public static class LinkModelStereotypeExtensions
    {
        public static LinkSettings GetLinkSettings(this LinkModel model)
        {
            var stereotype = model.GetStereotype("Link Settings");
            return stereotype != null ? new LinkSettings(stereotype) : null;
        }

        public static bool HasLinkSettings(this LinkModel model)
        {
            return model.HasStereotype("Link Settings");
        }


        public class LinkSettings
        {
            private IStereotype _stereotype;

            public LinkSettings(IStereotype stereotype)
            {
                _stereotype = stereotype;
            }

            public string Name => _stereotype.Name;

            public string RouterLink()
            {
                return _stereotype.GetProperty<string>("Router Link");
            }

        }

    }
}