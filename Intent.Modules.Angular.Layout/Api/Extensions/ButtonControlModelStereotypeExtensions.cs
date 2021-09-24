using System;
using Intent.Metadata.Models;
using Intent.Modules.Common;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.Api.ApiElementModelExtensions", Version = "1.0")]

namespace Intent.Angular.Layout.Api
{
    public static class ButtonControlModelStereotypeExtensions
    {
        public static ButtonSettings GetButtonSettings(this ButtonControlModel model)
        {
            var stereotype = model.GetStereotype("Button Settings");
            return stereotype != null ? new ButtonSettings(stereotype) : null;
        }

        public static bool HasButtonSettings(this ButtonControlModel model)
        {
            return model.HasStereotype("Button Settings");
        }


        public class ButtonSettings
        {
            private IStereotype _stereotype;

            public ButtonSettings(IStereotype stereotype)
            {
                _stereotype = stereotype;
            }

            public string Name => _stereotype.Name;

            public TypeOptions Type()
            {
                return new TypeOptions(_stereotype.GetProperty<string>("Type"));
            }

            public class TypeOptions
            {
                public readonly string Value;

                public TypeOptions(string value)
                {
                    Value = value;
                }

                public TypeOptionsEnum AsEnum()
                {
                    switch (Value)
                    {
                        case "Button":
                            return TypeOptionsEnum.Button;
                        case "Submit":
                            return TypeOptionsEnum.Submit;
                        case "Reset":
                            return TypeOptionsEnum.Reset;
                        case "Menu":
                            return TypeOptionsEnum.Menu;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                public bool IsButton()
                {
                    return Value == "Button";
                }
                public bool IsSubmit()
                {
                    return Value == "Submit";
                }
                public bool IsReset()
                {
                    return Value == "Reset";
                }
                public bool IsMenu()
                {
                    return Value == "Menu";
                }
            }

            public enum TypeOptionsEnum
            {
                Button,
                Submit,
                Reset,
                Menu
            }
        }

    }
}