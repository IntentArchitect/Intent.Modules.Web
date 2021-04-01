using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Metadata.Models;
using Intent.Modelers.WebClient.Api;
using Intent.Modules.Common;
using Intent.Modules.Common.Registrations;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;
using Intent.Modules.Angular.Api;
using Intent.Modelers.WebClient.Angular.Api;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.TemplateRegistration.FilePerModel", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.App.AppRoutingModuleTemplate
{
    [IntentManaged(Mode.Merge, Body = Mode.Merge, Signature = Mode.Fully)]
    public class AppRoutingModuleTemplateRegistration : FilePerModelTemplateRegistration<AngularWebAppModel>
    {
        public override string TemplateId => App.AppRoutingModuleTemplate.AppRoutingModuleTemplate.TemplateId;

        private readonly IMetadataManager _metadataManager;

        public AppRoutingModuleTemplateRegistration(IMetadataManager metadataManager)
        {
            _metadataManager = metadataManager;
        }

        public override ITemplate CreateTemplateInstance(IOutputTarget project, AngularWebAppModel model)
        {
            return new App.AppRoutingModuleTemplate.AppRoutingModuleTemplate(project, model);
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override IEnumerable<AngularWebAppModel> GetModels(IApplication application)
        {
            return _metadataManager.WebClient(application).GetAngularWebAppModels();
        }
    }
}