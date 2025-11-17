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

public class OldGeneralImplementationStrategy //: BaseImplementationStrategy, IImplementationStrategy
{
    //public OldGeneralImplementationStrategy(IApplication application, IAssociation association)
    //{
    //    _application = application;
    //    _association = association;
    //}

    //public bool IsMatch()
    //{
    //    //var sourceEnd = _association.SourceEnd;
    //    //// this gets the proxy model which is mapped
    //    //var proxyModel = _application.MetadataManager.GetServiceProxyModels(_application.Id, _application.MetadataManager.UserInterface)
    //    //    .FirstOrDefault(p => p.Endpoints.Any(e => e.Id == sourceEnd.ParentElement.Id));
    //    //// get the specific endpoint
    //    //var endpoint = proxyModel.Endpoints.First(e => e.Id == sourceEnd.ParentElement.Id);

    //    //return endpoint.Verb != Metadata.WebApi.Models.HttpVerb.Get;

    //    return true;
    //}

    //public void BindToTemplate(ITypescriptFileBuilderTemplate template)
    //{
    //    template.TypescriptFile.AfterBuild(_ => ApplyStrategy(template));
    //}

    //public void ApplyStrategy(ITypescriptFileBuilderTemplate template)
    //{
    //    // get general metadata for ease of use
    //    var templateMetadata = GetInteractionMetadata(template);
    //    templateMetadata.ComponentTemplateBase.AddTypeSource(HttpServiceProxyTemplate.TemplateId);

    //    // some basic setup on the component
    //    InjectProxyService(templateMetadata);
    //    AddServiceInvocationSupportFields(templateMetadata);


    //    // Struggled with the replacements bit below. I am sure there is probably a better way.
    //    // Basically, will take the Source and Target path and convert to a . seperated list of the Ids (in ApplySourceInitializationStatements and BuildTargetStatement)
    //    // The replacement will be applied against this, replaceing the Ids with the "correct"values from the replacement
    //    // Then afterwards, any id's left in the string are converted back to the name values
    //    foreach (var operation in templateMetadata.ComponentTemplateBase.Model.Operations)
    //    {
    //        SetSourceReplacement([templateMetadata.ComponentTemplateBase.Model.InternalElement, operation.InternalElement], "");
    //        SetSourceReplacement(operation.InternalElement, "");
    //    }
    //    foreach(var modelDef in templateMetadata.ComponentTemplateBase.Model.ModelDefinitions)
    //    {
    //        SetSourceReplacement([templateMetadata.ComponentTemplateBase.Model.InternalElement, modelDef.InternalElement], "this");
    //    }
    //    SetSourceReplacement(templateMetadata.ComponentTemplateBase.Model.InternalElement, "this");
    //    var requestName = ApplySourceInitializationStatements(templateMetadata);

    //    foreach (var operation in templateMetadata.ComponentTemplateBase.Model.Operations)
    //    {
    //        SetTargetReplacement([templateMetadata.ComponentTemplateBase.Model.InternalElement.Id, operation.InternalElement.Id, null, null], "data");
    //    }
    //    SetTargetReplacement(templateMetadata.ServiceProxyOperation.InternalElement, "");
    //    SetTargetReplacement(templateMetadata.ComponentTemplateBase.Model.InternalElement, "this");
    //    var targetStatement = BuildTargetStatement(templateMetadata);

    //    var serviceProxyVariableName = templateMetadata.ServiceProxyTemplate.Model.Name.ToCamelCase(true);
    //    var serviceProxyOperationName = templateMetadata.ServiceProxyOperation.Name.ToCamelCase(true);
    //    var responseVariableName = !string.IsNullOrWhiteSpace(targetStatement) ? "data" : "";
    //    var localResponseAssignmentStatement = !string.IsNullOrWhiteSpace(targetStatement) ? @$"{Environment.NewLine}
    //    {targetStatement}" : "";

    //    templateMetadata.InvocationMethod.AddStatement(new TypescriptStatement($@"this.{serviceProxyVariableName}.{serviceProxyOperationName}({requestName}).subscribe({{
    //  next: ({responseVariableName}) => {{
    //    this.serviceError  = null;
    //    this.serviceCallSuccess = true;{localResponseAssignmentStatement}
    //  }},
    //  error: (err) => {{
    //    this.serviceCallSuccess = false;
    //    const message = err?.error?.message || err.message || 'Unknown error';
    //    this.serviceError = `Failed to call service: ${{message}}`;

    //    console.error('Failed to call service:', err);
    //  }}
    //}});"));
    //}
}
