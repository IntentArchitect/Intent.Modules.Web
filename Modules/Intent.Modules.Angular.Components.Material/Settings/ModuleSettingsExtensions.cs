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
        public static PrimaryColorOptions PrimaryColor(this AngularSettings groupSettings) => new PrimaryColorOptions(groupSettings.GetSetting("bbed95c0-28a3-487c-a72f-2f867f0de0e6")?.Value);

        public class PrimaryColorOptions
        {
            public readonly string Value;

            public PrimaryColorOptions(string value)
            {
                Value = value;
            }

            public PrimaryColorOptionsEnum AsEnum()
            {
                return Value switch
                {
                    "amber" => PrimaryColorOptionsEnum.Amber,
                    "blue" => PrimaryColorOptionsEnum.Blue,
                    "blue-grey" => PrimaryColorOptionsEnum.BlueGrey,
                    "brown" => PrimaryColorOptionsEnum.Brown,
                    "cyan" => PrimaryColorOptionsEnum.Cyan,
                    "deep-orange" => PrimaryColorOptionsEnum.DeepOrange,
                    "deep-purple" => PrimaryColorOptionsEnum.DeepPurple,
                    "green" => PrimaryColorOptionsEnum.Green,
                    "grey" => PrimaryColorOptionsEnum.Grey,
                    "indigo" => PrimaryColorOptionsEnum.Indigo,
                    "light-blue" => PrimaryColorOptionsEnum.LightBlue,
                    "light-green" => PrimaryColorOptionsEnum.LightGreen,
                    "lime" => PrimaryColorOptionsEnum.Lime,
                    "orange" => PrimaryColorOptionsEnum.Orange,
                    "pink" => PrimaryColorOptionsEnum.Pink,
                    "purple" => PrimaryColorOptionsEnum.Purple,
                    "red" => PrimaryColorOptionsEnum.Red,
                    "teal" => PrimaryColorOptionsEnum.Teal,
                    "yellow" => PrimaryColorOptionsEnum.Yellow,
                    "custom" => PrimaryColorOptionsEnum.Custom,
                    _ => throw new ArgumentOutOfRangeException(nameof(Value), $"{Value} is out of range")
                };
            }

            public bool IsAmber()
            {
                return Value == "amber";
            }

            public bool IsBlue()
            {
                return Value == "blue";
            }

            public bool IsBlueGrey()
            {
                return Value == "blue-grey";
            }

            public bool IsBrown()
            {
                return Value == "brown";
            }

            public bool IsCyan()
            {
                return Value == "cyan";
            }

            public bool IsDeepOrange()
            {
                return Value == "deep-orange";
            }

            public bool IsDeepPurple()
            {
                return Value == "deep-purple";
            }

            public bool IsGreen()
            {
                return Value == "green";
            }

            public bool IsGrey()
            {
                return Value == "grey";
            }

            public bool IsIndigo()
            {
                return Value == "indigo";
            }

            public bool IsLightBlue()
            {
                return Value == "light-blue";
            }

            public bool IsLightGreen()
            {
                return Value == "light-green";
            }

            public bool IsLime()
            {
                return Value == "lime";
            }

            public bool IsOrange()
            {
                return Value == "orange";
            }

            public bool IsPink()
            {
                return Value == "pink";
            }

            public bool IsPurple()
            {
                return Value == "purple";
            }

            public bool IsRed()
            {
                return Value == "red";
            }

            public bool IsTeal()
            {
                return Value == "teal";
            }

            public bool IsYellow()
            {
                return Value == "yellow";
            }

            public bool IsCustom()
            {
                return Value == "custom";
            }
        }

        public enum PrimaryColorOptionsEnum
        {
            Amber,
            Blue,
            BlueGrey,
            Brown,
            Cyan,
            DeepOrange,
            DeepPurple,
            Green,
            Grey,
            Indigo,
            LightBlue,
            LightGreen,
            Lime,
            Orange,
            Pink,
            Purple,
            Red,
            Teal,
            Yellow,
            Custom,
        }

        public static AccentColorOptions AccentColor(this AngularSettings groupSettings) => new AccentColorOptions(groupSettings.GetSetting("3a30f6a5-b8eb-4e82-87c7-46788f65ad71")?.Value);

        public class AccentColorOptions
        {
            public readonly string Value;

            public AccentColorOptions(string value)
            {
                Value = value;
            }

            public AccentColorOptionsEnum AsEnum()
            {
                return Value switch
                {
                    "blue" => AccentColorOptionsEnum.Blue,
                    "blue-grey" => AccentColorOptionsEnum.BlueGrey,
                    "brown" => AccentColorOptionsEnum.Brown,
                    "cyan" => AccentColorOptionsEnum.Cyan,
                    "deep-orange" => AccentColorOptionsEnum.DeepOrange,
                    "deep-purple" => AccentColorOptionsEnum.DeepPurple,
                    "green" => AccentColorOptionsEnum.Green,
                    "grey" => AccentColorOptionsEnum.Grey,
                    "indigo" => AccentColorOptionsEnum.Indigo,
                    "light-blue" => AccentColorOptionsEnum.LightBlue,
                    "light-green" => AccentColorOptionsEnum.LightGreen,
                    "lime" => AccentColorOptionsEnum.Lime,
                    "orange" => AccentColorOptionsEnum.Orange,
                    "pink" => AccentColorOptionsEnum.Pink,
                    "purple" => AccentColorOptionsEnum.Purple,
                    "red" => AccentColorOptionsEnum.Red,
                    "teal" => AccentColorOptionsEnum.Teal,
                    "yellow" => AccentColorOptionsEnum.Yellow,
                    "custom" => AccentColorOptionsEnum.Custom,
                    "amber" => AccentColorOptionsEnum.Amber,
                    _ => throw new ArgumentOutOfRangeException(nameof(Value), $"{Value} is out of range")
                };
            }

            public bool IsBlue()
            {
                return Value == "blue";
            }

            public bool IsBlueGrey()
            {
                return Value == "blue-grey";
            }

            public bool IsBrown()
            {
                return Value == "brown";
            }

            public bool IsCyan()
            {
                return Value == "cyan";
            }

            public bool IsDeepOrange()
            {
                return Value == "deep-orange";
            }

            public bool IsDeepPurple()
            {
                return Value == "deep-purple";
            }

            public bool IsGreen()
            {
                return Value == "green";
            }

            public bool IsGrey()
            {
                return Value == "grey";
            }

            public bool IsIndigo()
            {
                return Value == "indigo";
            }

            public bool IsLightBlue()
            {
                return Value == "light-blue";
            }

            public bool IsLightGreen()
            {
                return Value == "light-green";
            }

            public bool IsLime()
            {
                return Value == "lime";
            }

            public bool IsOrange()
            {
                return Value == "orange";
            }

            public bool IsPink()
            {
                return Value == "pink";
            }

            public bool IsPurple()
            {
                return Value == "purple";
            }

            public bool IsRed()
            {
                return Value == "red";
            }

            public bool IsTeal()
            {
                return Value == "teal";
            }

            public bool IsYellow()
            {
                return Value == "yellow";
            }

            public bool IsCustom()
            {
                return Value == "custom";
            }

            public bool IsAmber()
            {
                return Value == "amber";
            }
        }

        public enum AccentColorOptionsEnum
        {
            Blue,
            BlueGrey,
            Brown,
            Cyan,
            DeepOrange,
            DeepPurple,
            Green,
            Grey,
            Indigo,
            LightBlue,
            LightGreen,
            Lime,
            Orange,
            Pink,
            Purple,
            Red,
            Teal,
            Yellow,
            Custom,
            Amber,
        }
    }
}