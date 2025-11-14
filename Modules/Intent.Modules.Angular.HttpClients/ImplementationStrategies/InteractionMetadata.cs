using Intent.Modelers.UI.Api;
using Intent.Modules.Angular.HttpClients.Templates.Helper;
using Intent.Modules.Common.TypeScript.Builder;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.Modules.Metadata.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intent.Modules.Angular.HttpClients.ImplementationStrategies;

internal class InteractionMetadata
{
    public TypescriptClass Class { get; internal set; }

    public TypescriptConstructor Constructor { get; internal set; }

    public TypescriptMethod InvocationMethod { get; internal set; }

    public IServiceProxyModel ServiceProxyModel { get; internal set; }

    public TypeScriptTemplateBase<IServiceProxyModel> ServiceProxyTemplate { get; internal set; }

    public ITypescriptFileBuilderTemplate ComponentTemplateBuilder { get; internal set; }

    public TypeScriptTemplateBase<ComponentModel> ComponentTemplateBase => ComponentTemplateBuilder as TypeScriptTemplateBase<ComponentModel>;

    public IHttpEndpointModel ServiceProxyOperation { get; internal set;  }
}
