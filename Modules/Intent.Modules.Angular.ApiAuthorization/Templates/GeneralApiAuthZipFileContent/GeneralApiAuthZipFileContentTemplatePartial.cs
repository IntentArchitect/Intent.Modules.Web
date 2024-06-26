using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Intent.Engine;
using Intent.Metadata.Models;
using Intent.Modules.Angular.Templates;
using Intent.Modules.Common;
using Intent.Modules.Common.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.ProjectItemTemplate.Partial", Version = "1.0")]

namespace Intent.Modules.Angular.ApiAuthorization.Templates.GeneralApiAuthZipFileContent
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class GeneralApiAuthZipFileContentTemplate : IntentTemplateBase<object>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.ApiAuthorization.GeneralApiAuthZipFileContent";

        private ZipEntry _zipEntry;

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public GeneralApiAuthZipFileContentTemplate(IOutputTarget outputTarget, object model = null) : base(TemplateId, outputTarget, model)
        {
            _zipEntry = (ZipEntry)model;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            string filename;
            var extension = Path.GetExtension(_zipEntry.FullFileNamePath);
            if (!string.IsNullOrEmpty(extension))
            {
                filename = _zipEntry.FullFileNamePath.Replace(extension, string.Empty);
            }
            else
            {
                filename = _zipEntry.FullFileNamePath;
            }

            return new TemplateFileConfig(
                fileName: filename,
                fileExtension: extension.Replace(".", string.Empty)
            );
        }
        
        public override string GetCorrelationId()
        {
            return $"{Id}#{_zipEntry.FullFileNamePath}";
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public override string TransformText()
        {
            return _zipEntry.Content;
        }
    }
}