using Intent.Engine;
using Intent.Persistence;
using Intent.Plugins;
using Intent.RoslynWeaver.Attributes;
using System.Linq;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.Migrations.OnVersionMigration", Version = "1.0")]

namespace Intent.Modules.Angular.Migrations
{
    public class Migration_05_00_00_Beta_02 : IModuleMigration
    {
        private readonly IApplicationConfigurationProvider _configurationProvider;
        private readonly IPersistenceLoader _persistenceLoader;

        public Migration_05_00_00_Beta_02(IApplicationConfigurationProvider configurationProvider, IPersistenceLoader persistenceLoader)
        {
            _configurationProvider = configurationProvider;
            _persistenceLoader = persistenceLoader;
        }

        [IntentFully]
        public string ModuleId => "Intent.Angular";
        [IntentFully]
        public string ModuleVersion => "5.0.0-beta.2";

        public void Up()
        {
            var app = _persistenceLoader.LoadCurrentApplication();

            const string angularGroupId = "3697d56e-8390-4e7f-ba44-fee766191e77";
            const string angularVersionSettingsId = "3dc81a7d-43f2-44bc-8900-da60ceb75059";

            var group = app.ModuleSettingGroups.FirstOrDefault(x => x.Id == angularGroupId);
            group ??= app.ModuleSettingGroups.Add(angularGroupId, "Intent.Angular", "Angular Settings");

            var angSettings = group.Settings.FirstOrDefault(x => x.Id == angularVersionSettingsId);
            angSettings ??= group.Settings.Add(angularVersionSettingsId, ModuleSettingControlType.Select, "Angular Version", "Intent.Angular", "19.2");

            app.SaveAllChanges();
        }

        public void Down()
        {
        }
    }
}