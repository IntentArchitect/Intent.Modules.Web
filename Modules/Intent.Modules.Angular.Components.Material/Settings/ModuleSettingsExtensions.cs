using System;
using Intent.Configuration;
using Intent.Engine;
using Intent.Modules.Angular.Settings;
using Intent.Modules.Common.Templates;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.Settings.ModuleSettingsExtensions", Version = "1.0")]

namespace Intent.Modules.Angular.Components.Material.Settings
{

    public static class AngularSettingsExtensions
    {

        public static MaterialThemeOptions MaterialTheme(this AngularSettings groupSettings) => new MaterialThemeOptions(groupSettings.GetSetting("bbed95c0-28a3-487c-a72f-2f867f0de0e6")?.Value);

        public class MaterialThemeOptions
        {
            public readonly string Value;

            public MaterialThemeOptions(string value)
            {
                Value = value;
            }

            public MaterialThemeOptionsEnum AsEnum()
            {
                return Value switch
                {
                    "indigo-pink" => MaterialThemeOptionsEnum.IndigoPink,
                    "deeppurple-amber" => MaterialThemeOptionsEnum.DeeppurpleAmber,
                    "pink-bluegrey" => MaterialThemeOptionsEnum.PinkBluegrey,
                    "purple-green" => MaterialThemeOptionsEnum.PurpleGreen,
                    "custom" => MaterialThemeOptionsEnum.Custom,
                    _ => throw new ArgumentOutOfRangeException(nameof(Value), $"{Value} is out of range")
                };
            }

            public bool IsIndigoPink()
            {
                return Value == "indigo-pink";
            }

            public bool IsDeeppurpleAmber()
            {
                return Value == "deeppurple-amber";
            }

            public bool IsPinkBluegrey()
            {
                return Value == "pink-bluegrey";
            }

            public bool IsPurpleGreen()
            {
                return Value == "purple-green";
            }

            public bool IsCustom()
            {
                return Value == "custom";
            }
        }

        public enum MaterialThemeOptionsEnum
        {
            IndigoPink,
            DeeppurpleAmber,
            PinkBluegrey,
            PurpleGreen,
            Custom,
        }
    }
}