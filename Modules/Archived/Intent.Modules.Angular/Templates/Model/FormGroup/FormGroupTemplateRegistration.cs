using System.Collections.Generic;
using Intent.Engine;
using Intent.Modelers.WebClient.Angular.Api;
using Intent.Modules.Common.Registrations;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.TemplateRegistration.FilePerModel", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.Model.FormGroup
{
    [IntentManaged(Mode.Merge, Body = Mode.Merge, Signature = Mode.Fully)]
    public class FormGroupTemplateRegistration : FilePerModelTemplateRegistration<FormGroupDefinitionModel>
    {
        private readonly IMetadataManager _metadataManager;

        public FormGroupTemplateRegistration(IMetadataManager metadataManager)
        {
            _metadataManager = metadataManager;
        }

        public override string TemplateId => FormGroupTemplate.TemplateId;

        [IntentManaged(Mode.Fully)]
        public override ITemplate CreateTemplateInstance(IOutputTarget outputTarget, FormGroupDefinitionModel model)
        {
            return new FormGroupTemplate(outputTarget, model);
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override IEnumerable<FormGroupDefinitionModel> GetModels(IApplication application)
        {
            return Intent.Modelers.UI.Api.ApiMetadataDesignerExtensions.UserInterface(_metadataManager, application).GetFormGroupDefinitionModels();
        }
    }
}