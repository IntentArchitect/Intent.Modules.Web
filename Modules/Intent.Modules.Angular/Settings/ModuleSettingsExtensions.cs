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
    }
}