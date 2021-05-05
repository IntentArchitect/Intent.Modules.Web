using System.Collections.Generic;
using System.IO;
using Intent.Engine;
using Intent.Modules.Angular.Templates;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;


[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.TypeScript.Templates.TypescriptTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Angular.ApiAuthorization.Templates.ApiAuthTypescriptZipFileContent
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class ApiAuthTypescriptZipFileContentTemplate : TypeScriptTemplateBase<object>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.ApiAuthorization.ApiAuthTypescriptZipFileContent";

        private ZipEntry _zipEntry;

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public ApiAuthTypescriptZipFileContentTemplate(IOutputTarget outputTarget, object model = null) : base(TemplateId, outputTarget, model)
        {
            _zipEntry = (ZipEntry)model;
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
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

        public override string RunTemplate()
        {
            return _zipEntry.Content;
        }

        private static bool _hasBeenPublished;

        public override void BeforeTemplateExecution()
        {
            if (!_hasBeenPublished)
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

                OutputTarget.Application.EventDispatcher.Publish(
                    eventIdentifier: AngularConfigVariableRequiredEvent.EventId,
                    @event: new Dictionary<string, string>()
                    {
                        { AngularConfigVariableRequiredEvent.VariableId, "auth" },
                        {
                            AngularConfigVariableRequiredEvent.DefaultValueId,
                            @"{
    authority: 'https://localhost:{sts port number}',
    client_id: 'Auth_Code_Client',
    redirect_uri: window.location.origin + '/authentication/login-callback',
    post_logout_redirect_uri: window.location.origin + '/authentication/logout-callback',
    response_type: 'code',
    scope: 'openid profile email api roles'
  }"
                        },
                    });

                _hasBeenPublished = true;
            }
        }
    }
}