using System;
using Intent.Metadata.Models;
using Intent.Modules.Common;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.Api.ApiElementModelExtensions", Version = "1.0")]

namespace Intent.Angular.Layout.Api
{
    public static class CustomFormControlModelExtensions
    {
        public static SelectControlSettings GetSelectControlSettings(this FormFieldModel model)
        {
            var stereotype = model.GetStereotype("Select Control Settings");
            return stereotype != null ? new SelectControlSettings(stereotype) : null;
        }

        public static bool HasSelectControlSettings(this FormFieldModel model)
        {
            return model.HasStereotype("Select Control Settings");
        }


        public class SelectControlSettings
        {
            private IStereotype _stereotype;

            public SelectControlSettings(IStereotype stereotype)
            {
                _stereotype = stereotype;
            }

            public string Name => _stereotype.Name;

            public IElement OptionsSource()
            {
                return _stereotype.GetProperty<IElement>("Options Source");
            }

            public string OptionValueField()
            {
                return _stereotype.GetProperty<string>("Option Value Field");
            }

            public string OptionTextField()
            {
                return _stereotype.GetProperty<string>("Option Text Field");
            }

        }

    }
}