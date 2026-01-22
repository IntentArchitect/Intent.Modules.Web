using System.Collections.Generic;
using Intent.Engine;
using Intent.Modelers.WebClient.Angular.Api;
using Intent.Modules.Common.Registrations;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.TemplateRegistration.FilePerModel", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.Component.AngularComponentTs
{
    [IntentManaged(Mode.Merge, Body = Mode.Merge, Signature = Mode.Fully)]
    public class AngularComponentTsTemplateRegistration : FilePerModelTemplateRegistration<ComponentModel>
    {
        private readonly IMetadataManager _metadataManager;

        public AngularComponentTsTemplateRegistration(IMetadataManager metadataManager)
        {
            _metadataManager = metadataManager;
        }

        public override string TemplateId => AngularComponentTsTemplate.TemplateId;

        [IntentManaged(Mode.Fully)]
        public override ITemplate CreateTemplateInstance(IOutputTarget outputTarget, ComponentModel model)
        {
            return new AngularComponentTsTemplate(outputTarget, model);
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override IEnumerable<ComponentModel> GetModels(IApplication application)
        {
            return Intent.Modelers.UI.Api.ApiMetadataDesignerExtensions.UserInterface(_metadataManager, application).GetComponentModels();
        }
    }
}