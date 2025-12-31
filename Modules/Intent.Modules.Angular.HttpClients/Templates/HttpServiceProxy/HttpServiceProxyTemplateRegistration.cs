using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Metadata.Models;
using Intent.Modelers.Types.ServiceProxies.Api;
using Intent.Modelers.UI.Api;
using Intent.Modules.Angular.HttpClients.Templates.Helper;
using Intent.Modules.Common;
using Intent.Modules.Common.Registrations;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.TemplateRegistration.FilePerModel", Version = "1.0")]

namespace Intent.Modules.Angular.HttpClients.Templates.HttpServiceProxy
{
    [IntentManaged(Mode.Ignore)]
    public class HttpServiceProxyTemplateRegistration : FilePerModelTemplateRegistration<IServiceProxyModel>
    {
        private readonly IMetadataManager _metadataManager;

        public HttpServiceProxyTemplateRegistration(IMetadataManager metadataManager)
        {
            _metadataManager = metadataManager;
        }

        public override string TemplateId => HttpServiceProxyTemplate.TemplateId;

        [IntentManaged(Mode.Fully)]
        public override ITemplate CreateTemplateInstance(IOutputTarget outputTarget, IServiceProxyModel model)
        {
            return new HttpServiceProxyTemplate(outputTarget, model);
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override IEnumerable<IServiceProxyModel> GetModels(IApplication application)
        {
            return _metadataManager.GetServiceProxyModels(
                application.Id,
                _metadataManager.UserInterface);
        }
    }
}