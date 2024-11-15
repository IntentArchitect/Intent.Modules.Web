using System;
using System.Linq;
using Intent.Engine;
using Intent.IArchitect.Agent.Persistence.Model;
using Intent.IArchitect.Agent.Persistence.Model.Common;
using Intent.IArchitect.CrossPlatform.IO;
using Intent.Modules.Angular.Shared;
using Intent.Plugins;

namespace Intent.Modules.Angular.ServiceProxies.Migrations;

public class OnInstallMigration(IApplicationConfigurationProvider configurationProvider) : IModuleOnInstallMigration
{
    public string ModuleId => Constants.ServiceProxiesModuleId;

    public void OnInstall()
    {
        var app = ApplicationPersistable.Load(configurationProvider.GetApplicationConfig().FilePath);
        var designer = app.GetDesigner(Constants.FolderDesignerId);
        var packages = designer.GetPackages();
        var package = packages.FirstOrDefault();

        if (package == null)
        {
            package = new PackageModelPersistable(
                specializationTypeId: Constants.FolderPackageSpecializationTypeId,
                specializationType: Constants.FolderPackageSpecializationType,
                applicationId: app.Id,
                designerId: designer.Id,
                id: Guid.NewGuid().ToString(),
                comment: default,
                name: "root",
                icon: Icon.FolderPackage,
                classes: [],
                associations: [],
                stereotypeDefinitions: []);

            package.AbsolutePath = Path.Join(designer.DirectoryPath, package.Name, $"{package.Name}.{PackageModelPersistable.FileExtension}");

            designer.AddPackageReference(package);
            designer.Save(false);

            packages = [package];
        }

        if (packages.SelectMany(x => x.GetElementsOfType(Constants.RoleElementSpecializationTypeId)).Any(x => x.Name == Constants.RoleElementName))
        {
            return;
        }

        package.AddElement(new ElementPersistable
        {
            Id = Guid.NewGuid().ToString(),
            SpecializationType = Constants.RoleElementSpecializationType,
            SpecializationTypeId = Constants.RoleElementSpecializationTypeId,
            Comment = default,
            Name = Constants.RoleElementName,
            Display = Constants.RoleElementName,
            ExternalReference = default,
            IsAbstract = default,
            IsStatic = default,
            Value = default,
            SortChildren = default,
            SortedBy = default,
            Order = default,
            GenericTypes = [],
            TypeReference = default,
            IsMapped = default,
            Mapping = default,
            ParentFolderId = package.Id,
            PackageId = package.Id,
            PackageName = package.Name,
            Diagram = default,
            Traits = [],
            Stereotypes = [],
            Mappings = [],
            Metadata = default,
            ChildElements = [],
            Package = package
        });

        package.Save(false);
    }
}