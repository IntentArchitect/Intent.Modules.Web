using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Modelers.WebClient.Angular.Api;
using Intent.Modules.Common.Registrations;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.TemplateRegistration.FilePerModel", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.Component.AngularComponentCss
{
    [IntentManaged(Mode.Merge, Body = Mode.Merge, Signature = Mode.Fully)]
    public class AngularComponentCssTemplateRegistration : FilePerModelTemplateRegistration<ComponentModel>
    {
        private readonly IMetadataManager _metadataManager;

        public AngularComponentCssTemplateRegistration(IMetadataManager metadataManager)
        {
            _metadataManager = metadataManager;
        }

        public override string TemplateId => AngularComponentCssTemplate.TemplateId;

        [IntentManaged(Mode.Fully)]
        public override ITemplate CreateTemplateInstance(IOutputTarget outputTarget, ComponentModel model)
        {
            return new AngularComponentCssTemplate(outputTarget, model);
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override IEnumerable<ComponentModel> GetModels(IApplication application)
        {
            return Intent.Modelers.UI.Api.ApiMetadataDesignerExtensions.UserInterface(_metadataManager, application).GetModuleModels().SelectMany(x => x.Components);
        }
    }
}