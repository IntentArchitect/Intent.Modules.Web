using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Metadata.Models;
using Intent.Modelers.WebClient.Api;
using Intent.Modules.Angular.Api;
using Intent.Modules.Common;
using Intent.Modules.Common.Registrations;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.TemplateRegistration.FilePerModel", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.Module.AngularRoutingModuleTemplate
{
    [IntentManaged(Mode.Merge, Body = Mode.Merge, Signature = Mode.Fully)]
    public class AngularRoutingModuleTemplateRegistration : FilePerModelTemplateRegistration<RoutingModel>
    {
        private readonly IMetadataManager _metadataManager;

        public AngularRoutingModuleTemplateRegistration(IMetadataManager metadataManager)
        {
            _metadataManager = metadataManager;
        }

        public override string TemplateId => AngularRoutingModuleTemplate.TemplateId;

        public override ITemplate CreateTemplateInstance(IOutputTarget project, RoutingModel model)
        {
            return new AngularRoutingModuleTemplate(project, model);
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override IEnumerable<RoutingModel> GetModels(IApplication application)
        {
            return _metadataManager.WebClient(application).GetModuleModels().Select(x => x.Routing).Where(x => x != null).ToList();
        }
    }
}