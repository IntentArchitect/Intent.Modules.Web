using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Intent.Modelers.Services.Api;
using Intent.Modules.Common;
using Intent.Modules.Common.Registrations;
using Intent.Engine;
using Intent.Metadata.Models;
using Intent.Modules.Common.Types.Api;
using Intent.Modules.Modelers.Types.ServiceProxies.Api;
using Intent.Modules.Modelers.WebClient.Api;
using Intent.Templates;

namespace Intent.Modules.Typescript.ServiceAgent.Contracts.Templates.TypescriptDTO
{
    [Description("Intent Typescript ServiceAgent DTO - Remote")]
    public class TypescriptDtoTemplateRegistration : FilePerModelTemplateRegistration<ServiceProxyDTOModel>
    {
        private readonly IMetadataManager _metadataManager;

        public TypescriptDtoTemplateRegistration(IMetadataManager metadataManager)
        {
            _metadataManager = metadataManager;
        }

        public override string TemplateId => TypescriptDtoTemplate.TemplateId;

        public override ITemplate CreateTemplateInstance(IOutputTarget project, ServiceProxyDTOModel model)
        {
            return new TypescriptDtoTemplate(TemplateId, project, model);
        }

        public override IEnumerable<ServiceProxyDTOModel> GetModels(IApplication application)
        {
            var dtoModels = _metadataManager.WebClient(application).GetServiceProxyDTOModels();

            return dtoModels;
        }
    }
}
