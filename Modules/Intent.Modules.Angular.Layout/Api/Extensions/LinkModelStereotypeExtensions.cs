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
            var stereotype = model.GetStereotype(LinkSettings.DefinitionId);
            return stereotype != null ? new LinkSettings(stereotype) : null;
        }

        public static bool HasLinkSettings(this LinkModel model)
        {
            return model.HasStereotype(LinkSettings.DefinitionId);
        }

        public static bool TryGetLinkSettings(this LinkModel model, out LinkSettings stereotype)
        {
            if (!HasLinkSettings(model))
            {
                stereotype = null;
                return false;
            }

            stereotype = new LinkSettings(model.GetStereotype(LinkSettings.DefinitionId));
            return true;
        }


        public class LinkSettings
        {
            private IStereotype _stereotype;
            public const string DefinitionId = "83a5537c-2283-450f-99be-640547d0fad0";

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