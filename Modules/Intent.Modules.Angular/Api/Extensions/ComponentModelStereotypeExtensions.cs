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
    public static class ComponentModelStereotypeExtensions
    {
        public static AngularComponentSettings GetAngularComponentSettings(this ComponentModel model)
        {
            var stereotype = model.GetStereotype(AngularComponentSettings.DefinitionId);
            return stereotype != null ? new AngularComponentSettings(stereotype) : null;
        }

        public static bool HasAngularComponentSettings(this ComponentModel model)
        {
            return model.HasStereotype(AngularComponentSettings.DefinitionId);
        }

        public static bool TryGetAngularComponentSettings(this ComponentModel model, out AngularComponentSettings stereotype)
        {
            if (!HasAngularComponentSettings(model))
            {
                stereotype = null;
                return false;
            }

            stereotype = new AngularComponentSettings(model.GetStereotype(AngularComponentSettings.DefinitionId));
            return true;
        }


        public class AngularComponentSettings
        {
            private IStereotype _stereotype;
            public const string DefinitionId = "ded6918f-81c2-4097-bdbe-48ec0fb84b7b";

            public AngularComponentSettings(IStereotype stereotype)
            {
                _stereotype = stereotype;
            }

            public string Name => _stereotype.Name;

            public string Selector()
            {
                return _stereotype.GetProperty<string>("Selector");
            }

            public IElement[] InjectServices()
            {
                return _stereotype.GetProperty<IElement[]>("Inject Services") ?? new IElement[0];
            }

            public bool InOwnFolder()
            {
                return _stereotype.GetProperty<bool>("In Own Folder");
            }

        }

    }
}