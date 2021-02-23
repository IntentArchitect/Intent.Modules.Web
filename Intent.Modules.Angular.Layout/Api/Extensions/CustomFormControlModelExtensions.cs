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
        public static SelectControlOptions GetSelectControlOptions(this FormFieldModel model)
        {
            var stereotype = model.GetStereotype("Select Control Options");
            return stereotype != null ? new SelectControlOptions(stereotype) : null;
        }

        public static bool HasSelectControlOptions(this FormFieldModel model)
        {
            return model.HasStereotype("Select Control Options");
        }


        public class SelectControlOptions
        {
            private IStereotype _stereotype;

            public SelectControlOptions(IStereotype stereotype)
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