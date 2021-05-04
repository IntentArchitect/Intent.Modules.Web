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

namespace Intent.Modules.Angular.ApiAuthorization.Templates.ApiAuthZipFileContent
{
    [IntentManaged(Mode.Merge, Body = Mode.Merge, Signature = Mode.Fully)]
    public class ApiAuthZipFileContentTemplateRegistration : ITemplateRegistration
    {
        private readonly IMetadataManager _metadataManager;

        public ApiAuthZipFileContentTemplateRegistration(IMetadataManager metadataManager)
        {
            _metadataManager = metadataManager;
        }

        public string TemplateId => ApiAuthZipFileContentTemplate.TemplateId;

        public void DoRegistration(ITemplateInstanceRegistry registery, IApplication applicationManager)
        {
            ResourceHelper.ApiAuthFileContents((Action<System.IO.Compression.ZipArchive>)(archive =>
            {
                foreach (var entry in archive.Entries.Where(p => p.Name != string.Empty))
                {
                    registery.RegisterTemplate(TemplateId, (Func<IOutputTarget, ITemplate>)(project => new ApiAuthZipFileContentTemplate(
                        outputTarget: (IOutputTarget)project,
                        model: (object)new ZipEntry
                        {
                            FullFileNamePath = entry.FullName,
                            Content = new StreamReader((Stream)entry.Open()).ReadToEnd()
                        })));
                }
            }));
        }
    }
}