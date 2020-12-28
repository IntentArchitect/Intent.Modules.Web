using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Metadata.Models;
using Intent.Modules.Common;
using Intent.Modules.Common.Registrations;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;
using Intent.Modelers.WebClient.Api;
using Intent.Modules.Angular.Api;
using Intent.Angular.Api;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.TemplateRegistration.FilePerModel", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.App.AppModuleTemplate
{
    [IntentManaged(Mode.Merge, Body = Mode.Merge, Signature = Mode.Fully)]
    public class AppModuleTemplateRegistration : FilePerModelTemplateRegistration<AngularWebAppModel>
    {
        public override string TemplateId => App.AppModuleTemplate.AppModuleTemplate.TemplateId;

        public override ITemplate CreateTemplateInstance(IOutputTarget project, AngularWebAppModel model)
        {
            return new App.AppModuleTemplate.AppModuleTemplate(project, null);
        }
        private readonly IMetadataManager _metadataManager;

        public AppModuleTemplateRegistration(IMetadataManager metadataManager)
        {
            _metadataManager = metadataManager;
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override IEnumerable<AngularWebAppModel> GetModels(IApplication application)
        {
            return _metadataManager.WebClient(application).GetAngularWebAppModels();
        }
    }
}