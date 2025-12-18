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

namespace Intent.Modules.Angular.Templates.Component.HeaderComponentHtml
{
    [IntentManaged(Mode.Merge, Body = Mode.Merge, Signature = Mode.Fully)]
    public class HeaderComponentHtmlTemplateRegistration : FilePerModelTemplateRegistration<LayoutHeaderModel>
    {
        private readonly IMetadataManager _metadataManager;

        public HeaderComponentHtmlTemplateRegistration(IMetadataManager metadataManager)
        {
            _metadataManager = metadataManager;
        }

        public override string TemplateId => HeaderComponentHtmlTemplate.TemplateId;

        [IntentManaged(Mode.Fully)]
        public override ITemplate CreateTemplateInstance(IOutputTarget outputTarget, LayoutHeaderModel model)
        {
            return new HeaderComponentHtmlTemplate(outputTarget, model);
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override IEnumerable<LayoutHeaderModel> GetModels(IApplication application)
        {
            return _metadataManager.UserInterface(application).GetLayoutModels().Select(l => l.Header).Where(h => h is not null);
        }
    }
}