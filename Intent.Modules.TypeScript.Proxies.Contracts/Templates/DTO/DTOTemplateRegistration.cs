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
using Intent.Modelers.Types.ServiceProxies.Api;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.TemplateRegistration.FilePerModel", Version = "1.0")]

namespace Intent.Modules.TypeScript.Proxies.Contracts.Templates.DTO
{
    [IntentManaged(Mode.Merge, Body = Mode.Merge, Signature = Mode.Fully)]
    public class DTOTemplateRegistration : FilePerModelTemplateRegistration<ServiceProxyDTOModel>
    {
        private readonly IMetadataManager _metadataManager;

        public DTOTemplateRegistration(IMetadataManager metadataManager)
        {
            _metadataManager = metadataManager;
        }

        public override string TemplateId => DTOTemplate.TemplateId;

        public override ITemplate CreateTemplateInstance(IOutputTarget outputTarget, ServiceProxyDTOModel model)
        {
            return new DTOTemplate(outputTarget, model);
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override IEnumerable<ServiceProxyDTOModel> GetModels(IApplication application)
        {
            return _metadataManager.WebClient(application).GetServiceProxyDTOModels();
        }

    }
}