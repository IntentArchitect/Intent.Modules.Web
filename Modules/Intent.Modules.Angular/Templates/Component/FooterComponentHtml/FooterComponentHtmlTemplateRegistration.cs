using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Metadata.Models;
using Intent.Modelers.UI.Api;
using Intent.Modules.Common;
using Intent.Modules.Common.Registrations;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.TemplateRegistration.FilePerModel", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.Component.FooterComponentHtml
{
    [IntentManaged(Mode.Merge, Body = Mode.Merge, Signature = Mode.Fully)]
    public class FooterComponentHtmlTemplateRegistration : FilePerModelTemplateRegistration<LayoutFooterModel>
    {
        private readonly IMetadataManager _metadataManager;

        public FooterComponentHtmlTemplateRegistration(IMetadataManager metadataManager)
        {
            _metadataManager = metadataManager;
        }

        public override string TemplateId => FooterComponentHtmlTemplate.TemplateId;

        [IntentManaged(Mode.Fully)]
        public override ITemplate CreateTemplateInstance(IOutputTarget outputTarget, LayoutFooterModel model)
        {
            return new FooterComponentHtmlTemplate(outputTarget, model);
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override IEnumerable<LayoutFooterModel> GetModels(IApplication application)
        {
            return _metadataManager.UserInterface(application).GetLayoutModels().Select(l => l.Footer).Where(f => f is not null);
        }
    }
}