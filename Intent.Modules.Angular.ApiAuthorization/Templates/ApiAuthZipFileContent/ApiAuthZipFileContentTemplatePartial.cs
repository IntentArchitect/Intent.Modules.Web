using System;
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

namespace Intent.Modules.Angular.ApiAuthorization.Templates.ApiAuthZipFileContent
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class ApiAuthZipFileContentTemplate : IntentTemplateBase<object>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.ApiAuthorization.ApiAuthZipFileContent";

        private ZipEntry _zipEntry;

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public ApiAuthZipFileContentTemplate(IOutputTarget outputTarget, object model = null) : base(TemplateId, outputTarget, model)
        {
            _zipEntry = (ZipEntry)model;
        }

        public override void BeforeTemplateExecution()
        {
            OutputTarget.Application.EventDispatcher.Publish(new AngularImportDependencyRequiredEvent(
                moduleId: "AppModule",
                dependency: "ApiAuthorizationModule",
                import: "import { ApiAuthorizationModule } from './api-authorization/api-authorization.module';"));

            OutputTarget.Application.EventDispatcher.Publish(new AngularImportDependencyRequiredEvent(
                moduleId: "AppModule",
                dependency: "HttpClientModule",
                import: "import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';"));

            OutputTarget.Application.EventDispatcher.Publish(new AngularCustomProviderRequiredEvent(
                moduleId: "AppModule",
                provide: "HTTP_INTERCEPTORS",
                useClass: "AuthorizeInterceptor",
                multi: true,
                import: "import { AuthorizeInterceptor } from './api-authorization/authorize.interceptor';"));
        }

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

        public override string TransformText()
        {
            return _zipEntry.Content;
        }
    }
}