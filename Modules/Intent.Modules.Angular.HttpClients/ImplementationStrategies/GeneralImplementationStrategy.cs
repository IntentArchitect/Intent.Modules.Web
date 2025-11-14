using Intent.Engine;
using Intent.Metadata.Models;
using Intent.Modelers.UI.Api;
using Intent.Modules.Angular.HttpClients.Templates.Helper;
using Intent.Modules.Angular.HttpClients.Templates.HttpServiceProxy;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.TypeScript.Builder;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.Modules.Metadata.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Intent.Modules.Angular.HttpClients.ImplementationStrategies;

public class GeneralImplementationStrategy : BaseImplementationStrategy, IImplementationStrategy
{
    public GeneralImplementationStrategy(IApplication application, IAssociation association)
    {
        _application = application;
        _association = association;
    }

    public bool IsMatch()
    {
        //var sourceEnd = _association.SourceEnd;
        //// this gets the proxy model which is mapped
        //var proxyModel = _application.MetadataManager.GetServiceProxyModels(_application.Id, _application.MetadataManager.UserInterface)
        //    .FirstOrDefault(p => p.Endpoints.Any(e => e.Id == sourceEnd.ParentElement.Id));
        //// get the specific endpoint
        //var endpoint = proxyModel.Endpoints.First(e => e.Id == sourceEnd.ParentElement.Id);

        //return endpoint.Verb != Metadata.WebApi.Models.HttpVerb.Get;

        return true;
    }

    public void BindToTemplate(ITypescriptFileBuilderTemplate template)
    {
        template.TypescriptFile.AfterBuild(_ => ApplyStrategy(template));
    }

    public void ApplyStrategy(ITypescriptFileBuilderTemplate template)
    {
        // get general metadata for ease of use
        var templateMetadata = GetInteractionMetadata(template);
        templateMetadata.ComponentTemplateBase.AddTypeSource(HttpServiceProxyTemplate.TemplateId);

        // some basic setup on the component
        InjectProxyService(templateMetadata);
        AddServiceInvocationSupportFields(templateMetadata);

        // handle source replacements
        var sourceReplacements = new Dictionary<string, string>
        {
            { templateMetadata.ComponentTemplateBase.Model.InternalElement.Id, "" }
        };
        foreach(var operation in templateMetadata.ComponentTemplateBase.Model.Operations)
        {
            sourceReplacements.Add(operation.InternalElement.Id, "");
        }

        var requestName = ApplySourceInitialization(templateMetadata, sourceReplacements);


        var responseMapping = _association.TargetEnd.Mappings.FirstOrDefault(m => m.TypeId == "e60890c6-7ce7-47be-a0da-dff82b8adc02"); // Call Service Response Mapping
        // could be null if service returns void
        var targetModel = responseMapping?.MappedEnds?.First()?.TargetElement;

        var serviceProxyVariableName = templateMetadata.ServiceProxyTemplate.Model.Name.ToCamelCase(true);
        var serviceProxyOperationName = templateMetadata.ServiceProxyOperation.Name.ToCamelCase(true);
        
        var responseVariableName = targetModel is not null ? "data" : "";
        var localResponseAssignmentStatement= targetModel is not null ? @$"{Environment.NewLine}
        this.{targetModel.Name.ToCamelCase(true)} = data;" : "";

        templateMetadata.InvocationMethod.AddStatement(new TypescriptStatement($@"this.{serviceProxyVariableName}.{serviceProxyOperationName}({requestName}).subscribe({{
      next: ({responseVariableName}) => {{
        this.serviceError  = null;
        this.serviceCallSuccess = true;{localResponseAssignmentStatement}
      }},
      error: (err) => {{
        this.serviceCallSuccess = false;
        const message = err?.error?.message || err.message || 'Unknown error';
        this.serviceError = `Failed to call service: ${{message}}`;

        console.error('Failed to call service:', err);
      }}
    }});"));
    }
}
