using Intent.SoftwareFactory;
using Intent.Engine;
using Intent.Templates;

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Intent.Modelers.Services;
using Intent.Modelers.Services.Api;
using Intent.Modules.Common;
using Intent.Modules.Common.Registrations;
using Intent.Modules.Modelers.Types.ServiceProxies.Api;
using Intent.Modules.Modelers.WebClient.Api;

namespace Intent.Modules.Typescript.ServiceAgent.AngularJs.Templates.ServiceProxy
{
    [Description("Intent Typescript ServiceAgent Proxy - Local Server")]
    public class LocalRegistrations : FilePerModelTemplateRegistration<ServiceProxyModel>
    {
        private readonly IMetadataManager _metadataManager;

        public LocalRegistrations(IMetadataManager metadataManager)
        {
            _metadataManager = metadataManager;
        }

        public override string TemplateId => TypescriptWebApiClientServiceProxyTemplate.LocalIdentifier;

        public override ITemplate CreateTemplateInstance(IOutputTarget project, ServiceProxyModel model)
        {
            return new TypescriptWebApiClientServiceProxyTemplate(TypescriptWebApiClientServiceProxyTemplate.LocalIdentifier, project, model, project.Application.EventDispatcher);
        }

        public override IEnumerable<ServiceProxyModel> GetModels(IApplication application)
        {
            var serviceModels = _metadataManager.WebClient(application).GetServiceProxyModels();

            // TODO JL: Temp, filter out ones for server only, will ultimately get replaced with concept of client applications in the future
            //serviceModels = serviceModels.Where(x => x.Stereotypes.All(s => s.Name != "ServerOnly")).ToList();

            return serviceModels;
        }
    }
}