using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Intent.Engine;
using Intent.Metadata.Models;
using Intent.Modules.Angular.ApiAuthorization.Resources;
using Intent.Modules.Common;
using Intent.Modules.Common.Registrations;
using Intent.Registrations;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

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

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public void DoRegistration(ITemplateInstanceRegistry registry, IApplication applicationManager)
        {
            ResourceHelper.ApiAuthFileContents(archive =>
            {
                foreach (var entry in archive.Entries.Where(p => p.Name != string.Empty
                    && Path.GetExtension(p.Name) != ".ts"))
                {
                    registry.RegisterTemplate(TemplateId, project => new GeneralApiAuthZipFileContentTemplate(
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