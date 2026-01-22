using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Modelers.WebClient.Angular.Api;
using Intent.Modules.Common.Registrations;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.TemplateRegistration.FilePerModel", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.Module.AngularRoutingModule
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

        [IntentManaged(Mode.Fully)]
        public override ITemplate CreateTemplateInstance(IOutputTarget outputTarget, RoutingModel model)
        {
            return new AngularRoutingModuleTemplate(outputTarget, model);
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override IEnumerable<RoutingModel> GetModels(IApplication application)
        {
            return Intent.Modelers.UI.Api.ApiMetadataDesignerExtensions.UserInterface(_metadataManager, application).GetModuleModels().Select(x => x.Routing).Where(x => x != null).ToList();
        }
    }
}