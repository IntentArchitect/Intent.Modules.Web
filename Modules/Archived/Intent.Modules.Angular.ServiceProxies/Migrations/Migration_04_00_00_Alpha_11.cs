using Intent.Engine;
using Intent.IArchitect.Agent.Persistence.Model;
using Intent.Modules.Angular.Shared;
using Intent.Plugins;

namespace Intent.Modules.Angular.ServiceProxies.Migrations;

/// <summary>
/// The template IDs were updated so this migration updates the template outputs in advance so they don't get moved by the module installation process.
/// </summary>
/// <param name="configurationProvider"></param>
public class Migration_04_00_00_Alpha_11(IApplicationConfigurationProvider configurationProvider) : IModuleMigration
{
    public string ModuleId => Constants.ServiceProxiesModuleId;

    public string ModuleVersion => "4.0.0-alpha.11";

    public void Up()
    {
        var app = ApplicationPersistable.Load(configurationProvider.GetApplicationConfig().FilePath);
        var designer = app.GetDesigner(Constants.FolderDesignerId);
        var packages = designer.GetPackages();

        foreach (var package in packages)
        {
            var hasChange = false;

            foreach (var element in package.GetElementsOfType(Constants.TemplateOutputElementSpecializationTypeId))
            {
                if (!element.Name.StartsWith("Intent.Angular.ServiceProxies.Proxies."))
                {
                    continue;
                }

                element.Name = element.Name.Replace("Intent.Angular.ServiceProxies.Proxies.", "Intent.Angular.ServiceProxies."); ;
                hasChange = true;
            }

            if (!hasChange)
            {
                continue;
            }

            package.Save(false);
        }
    }

    public void Down()
    {
        var app = ApplicationPersistable.Load(configurationProvider.GetApplicationConfig().FilePath);
        var designer = app.GetDesigner(Constants.FolderDesignerId);
        var packages = designer.GetPackages();

        foreach (var package in packages)
        {
            var hasChange = false;

            foreach (var element in package.GetElementsOfType(Constants.TemplateOutputElementSpecializationTypeId))
            {
                if (!element.Name.StartsWith("Intent.Angular.ServiceProxies."))
                {
                    continue;
                }

                element.Name = element.Name.Replace("Intent.Angular.ServiceProxies.", "Intent.Angular.ServiceProxies.Proxies.");
                hasChange = true;
            }

            if (!hasChange)
            {
                continue;
            }

            package.Save(false);
        }
    }
}