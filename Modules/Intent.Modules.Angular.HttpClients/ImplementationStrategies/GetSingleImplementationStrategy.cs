using Intent.Engine;
using Intent.Metadata.Models;
using Intent.Modelers.Services.Api;
using Intent.Modelers.Services.CQRS.Api;
using Intent.Modelers.UI.Api;
using Intent.Modules.Angular.HttpClients.Templates.Helper;
using Intent.Modules.Angular.HttpClients.Templates.HttpServiceProxy;
using Intent.Modules.Common;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.TypeScript.Builder;
using Intent.Modules.Common.TypeScript.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Intent.Modules.Angular.HttpClients.ImplementationStrategies;

public class GetSingleImplementationStrategy : BaseImplementationStrategy, IImplementationStrategy
{
    public GetSingleImplementationStrategy(IApplication application, IAssociation association)
    {
        _application = application;
        _association = association;
    }

    public bool IsMatch()
    {
        var sourceEnd = _association.SourceEnd;
        // this gets the proxy model which is mapped
        var proxyModel = _application.MetadataManager.GetServiceProxyModels(_application.Id, _application.MetadataManager.UserInterface)
            .FirstOrDefault(p => p.Endpoints.Any(e => e.Id == sourceEnd.ParentElement.Id));
        // get the specific endpoint
        var endpoint = proxyModel.Endpoints.First(e => e.Id == sourceEnd.ParentElement.Id);

        return endpoint.Verb == Metadata.WebApi.Models.HttpVerb.Get && !endpoint.ReturnType.IsCollection;
    }

    public void BindToTemplate(ITypescriptFileBuilderTemplate template)
    {
        template.TypescriptFile.AfterBuild(_ => ApplyStrategy(template));
    }

    public void ApplyStrategy(ITypescriptFileBuilderTemplate template)
    {
        var templateMetadata = GetInteractionMetadata(template);
        templateMetadata.ComponentTemplateBase.AddTypeSource(HttpServiceProxyTemplate.TemplateId);

        InjectProxyService(templateMetadata);
        AddServiceInvocationSupportFields(templateMetadata);

        var responseMapping = _association.TargetEnd.Mappings.FirstOrDefault(m => m.TypeId == "e60890c6-7ce7-47be-a0da-dff82b8adc02"); // Call Service Response Mapping
        
        var replacements = new Dictionary<string, string>();
        replacements.TryAdd($"{templateMetadata.ComponentTemplateBase.Model.Name}.{templateMetadata.InvocationMethod.Name}.", "");
        replacements.TryAdd($"{templateMetadata.ComponentTemplateBase.Model.Name.ToCamelCase(true)}.{templateMetadata.InvocationMethod.Name.ToCamelCase(true)}.", "");
        replacements.TryAdd($"{templateMetadata.ComponentTemplateBase.Model.Name}.", "this.");
        replacements.TryAdd($"{templateMetadata.ComponentTemplateBase.Model.Name.ToCamelCase(true)}.", "this.");
        replacements.TryAdd($"{templateMetadata.InvocationMethod.Name}.", "");
        replacements.TryAdd($"{templateMetadata.InvocationMethod.Name.ToCamelCase(true)}.", "");

        // this will be the main entity which is mapped
        var targetModel = responseMapping.MappedEnds.First().TargetElement;
        var requestName = ApplySourceInitializationStatements(templateMetadata);

        templateMetadata.InvocationMethod.AddStatement(new TypescriptStatement($@"this.{templateMetadata.ServiceProxyTemplate.Model.Name.ToCamelCase(true)}.{templateMetadata.ServiceProxyOperation.Name.ToCamelCase(true)}({requestName}).subscribe({{
      next: (data) => {{
        this.{targetModel.Name.ToCamelCase(true)} = data;
        this.serviceError  = null;
      }},
      error: (err) => {{
        const message = err?.error?.message || err.message || 'Unknown error';
        this.serviceError = `Failed to call service: ${{message}}`;
        console.error('Failed to call service:', err);
      }}
    }});"));
    }
}
