using System;
using Intent.Configuration;
using Intent.Engine;
using Intent.Modules.Common.Templates;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.Settings.ModuleSettingsExtensions", Version = "1.0")]

namespace Intent.Modules.Angular.Settings
{
    public static class ModuleSettingsExtensions
    {
        public static AngularSettings GetAngularSettings(this IApplicationSettingsProvider settings)
        {
            return new AngularSettings(settings.GetGroup("3697d56e-8390-4e7f-ba44-fee766191e77"));
        }
    }

    public class AngularSettings : IGroupSettings
    {
        private readonly IGroupSettings _groupSettings;

        public AngularSettings(IGroupSettings groupSettings)
        {
            _groupSettings = groupSettings;
        }

        public string Id => _groupSettings.Id;

        public string Title
        {
            get => _groupSettings.Title;
            set => _groupSettings.Title = value;
        }

        public ISetting GetSetting(string settingId)
        {
            return _groupSettings.GetSetting(settingId);
        }
        public AngularVersionOptions AngularVersion() => new AngularVersionOptions(_groupSettings.GetSetting("3dc81a7d-43f2-44bc-8900-da60ceb75059")?.Value);

        public class AngularVersionOptions
        {
            public readonly string Value;

            public AngularVersionOptions(string value)
            {
                Value = value;
            }

            public AngularVersionOptionsEnum AsEnum()
            {
                return Value switch
                {
                    "21" => AngularVersionOptionsEnum._21,
                    "20" => AngularVersionOptionsEnum._20,
                    "19" => AngularVersionOptionsEnum._19,
                    _ => throw new ArgumentOutOfRangeException(nameof(Value), $"{Value} is out of range")
                };
            }

            public bool Is_21()
            {
                return Value == "21";
            }

            public bool Is_20()
            {
                return Value == "20";
            }

            public bool Is_19()
            {
                return Value == "19";
            }
        }

        public enum AngularVersionOptionsEnum
        {
            _21,
            _20,
            _19,
        }
    }
}