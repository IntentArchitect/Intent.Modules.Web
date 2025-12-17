using Intent.Engine;
using Intent.IArchitect.Agent.Persistence.Model;
using Intent.IArchitect.Agent.Persistence.Model.Common;
using Intent.Metadata.Models;
using Intent.Modules.Angular.HttpClients.Templates.Helper;
using Intent.Modules.AngularConstants;
using Intent.Modules.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intent.Modules.Angular.Templates.Helper;

public static class ProxyUrlHelper
{
    internal static string GetProxyApplicationtUrl(IServiceProxyModel model, ISoftwareFactoryExecutionContext executionContext)
    {
        var url = string.Empty;

        var element = model.InternalElement;
        var package = element?.MappedElement?.Element?.Package;
        package ??= element?.Package;

        if (model.Endpoints.Any() && package is null)
        {
            package = model.Endpoints[0].InternalElement?.Package;
        }

        if (package == null)
        {
            return url;
        }

        var proxyUrl = GetProxyApplicationtUrl(package, executionContext);
        return string.IsNullOrWhiteSpace(proxyUrl) ? "https://localhost:{app_port}/" : proxyUrl;
    }

    public static string GetProxyApplicationtUrl(IPackage package, ISoftwareFactoryExecutionContext executionContext)
    {
        // "Endpoint Settings => Service URL" defined in Intent.Metadata.WebApi
        var serviceUrl = package.GetStereotypeProperty("c06e9978-c271-49fc-b5c9-09833b6b8992", "2164bf84-1db8-42d0-94a6-255d2908b9b5", string.Empty);
        if (!string.IsNullOrWhiteSpace(serviceUrl))
        {
            return serviceUrl;
        }

        // if the Endpoint Settings is not defined, then we need to try get the base URL from the 
        // project in the VS Designer, in the "source" application
        var sourceAppConfig = executionContext.GetSolutionConfig()
            .GetApplicationReferences()
            .Where(a => a.Id == package.ApplicationId)
            .Select(app => executionContext.GetSolutionConfig().GetApplicationConfig(app.Id))
            .FirstOrDefault();

        if (sourceAppConfig is not null)
        {
            IEnumerable<PackageModelPersistable> vsPackages = [];

            try
            {
                var appPersistable = ApplicationPersistable.Load(sourceAppConfig.FilePath);
                var vsDesigner = appPersistable.GetDesigner(Designers.VisualStudioId);

                vsPackages = vsDesigner.GetPackages();
            }
            // not the best way to handle this, but this occurs if there is an problem loading all of the packages
            // in the VS designer
            catch (Exception ex)
            {
                return string.Empty;
            }

            foreach(var vsPackage in vsPackages)
            {
                var launchProject = FindFirstMatchingElementRecursive(vsPackage);

                if (launchProject is not null)
                {
                    // LaunchSettings Id and BaseUrl property
                    var launchSettingsStereotype = launchProject.Stereotypes.FirstOrDefault(s => s.DefinitionId == "fd52e71a-3810-4ae7-ae40-4e1514903d25");

                    if(launchSettingsStereotype is null)
                    {
                        return string.Empty;
                    }

                    var applicationUrlProperty = launchSettingsStereotype.Properties
                        .FirstOrDefault(p => p.DefinitionId == "1b85d04d-e80a-4640-9fee-558711473fa8");

                    if(applicationUrlProperty is null || string.IsNullOrWhiteSpace(applicationUrlProperty.Value))
                    {
                        return string.Empty;
                    }

                    return applicationUrlProperty.Value;
                }
            }
        }

        return string.Empty;
    }

    private static ElementPersistable? FindFirstMatchingElementRecursive(IElementPersistable element)
    {
        // Check current level first
        var match = element.ChildElements
            .FirstOrDefault(c =>
                (c.SpecializationTypeId == "FFD54A85-9362-48AC-B646-C93AB9AC63D2" ||
                 c.SpecializationTypeId == "8e9e6693-2888-4f48-a0d6-0f163baab740") &&
                 // LaunchSettings stereotype
                c.Stereotypes.Any(s => s.DefinitionId == "fd52e71a-3810-4ae7-ae40-4e1514903d25"));

        if (match != null)
        {
            return match; // Found, stop here
        }

        // Recurse into children
        foreach (var child in element.ChildElements)
        {
            var nestedMatch = FindFirstMatchingElementRecursive(child);
            if (nestedMatch != null)
            {
                return nestedMatch; // Propagate first match up
            }
        }

        return null; // No match found
    }
}
