using Intent.Engine;
using Intent.Modules.Angular.ApiAuthorization.Resources;
using Intent.Registrations;
using Intent.RoslynWeaver.Attributes;
using System.IO;
using System.Linq;
using Intent.Metadata.Models;
using Intent.Modules.Common;
using Intent.Modules.Common.Registrations;
using Intent.Templates;
using System;
using System.Collections.Generic;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.TemplateRegistration.Custom", Version = "1.0")]

namespace Intent.Modules.Angular.ApiAuthorization.Templates.GeneralApiAuthZipFileContent
{
    [IntentManaged(Mode.Merge, Body = Mode.Merge, Signature = Mode.Fully)]
    public class GeneralApiAuthZipFileContentTemplateRegistration : ITemplateRegistration
    {
        private readonly IMetadataManager _metadataManager;

        public GeneralApiAuthZipFileContentTemplateRegistration(IMetadataManager metadataManager)
        {
            _metadataManager = metadataManager;
        }

        public string TemplateId => GeneralApiAuthZipFileContentTemplate.TemplateId;

        public void DoRegistration(ITemplateInstanceRegistry registery, IApplication applicationManager)
        {
            ResourceHelper.ApiAuthFileContents(archive =>
            {
                foreach (var entry in archive.Entries.Where(p => p.Name != string.Empty
                    && Path.GetExtension(p.Name) != ".ts"))
                {
                    registery.RegisterTemplate(TemplateId, project => new GeneralApiAuthZipFileContentTemplate(
                        outputTarget: project,
                        model: new ZipEntry
                        {
                            FullFileNamePath = entry.FullName,
                            Content = new StreamReader(entry.Open()).ReadToEnd()
                        }));
                }
            });
        }
    }
}