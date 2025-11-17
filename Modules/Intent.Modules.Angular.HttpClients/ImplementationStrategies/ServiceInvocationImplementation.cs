using Intent.Engine;
using Intent.Metadata.Models;
using Intent.Modelers.UI.Api;
using Intent.Modules.Angular.HttpClients.ImplementationStrategies.Infrastructure;
using Intent.Modules.Angular.HttpClients.Templates.Helper;
using Intent.Modules.Angular.HttpClients.Templates.HttpServiceProxy;
using Intent.Modules.Common;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.TypeScript.Builder;
using Intent.Modules.Common.TypeScript.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intent.Modules.Angular.HttpClients.ImplementationStrategies;

public class ServiceInvocationImplementation
{
    private IImplementationStrategy _sourceStrategy;
    private IImplementationStrategy _targetStrategy;
    private readonly IApplication _application;
    private readonly IAssociation _association;

    public ServiceInvocationImplementation(IApplication application, IAssociation association)
    {
        _application = application;
        _association = association;
    }

    public void SetSourceStrategy(IImplementationStrategy sourceStrategy)
    {
        _sourceStrategy = sourceStrategy;
    }

    public void SetTargetStrategy(IImplementationStrategy targetStrategy)
    {
        _targetStrategy = targetStrategy;
    }

    public void BindToTemplate(ITypescriptFileBuilderTemplate template, InteractionMetadata interactionMetadata)
    {
        template.TypescriptFile.AfterBuild(_ => ApplyStrategy(template, interactionMetadata, _sourceStrategy, _targetStrategy));
    }

    public void ApplyStrategy(ITypescriptFileBuilderTemplate template, InteractionMetadata interactionMetadata,
        IImplementationStrategy sourceStrategy, IImplementationStrategy targetStrategy)
    {
        // get general metadata for ease of use
        interactionMetadata.ComponentTemplateBase.AddTypeSource(HttpServiceProxyTemplate.TemplateId);

        // some basic setup on the component
        InjectProxyService(interactionMetadata);
        AddServiceInvocationSupportFields(interactionMetadata);


        //// Struggled with the replacements bit below. I am sure there is probably a better way.
        //// Basically, will take the Source and Target path and convert to a . seperated list of the Ids (in ApplySourceInitializationStatements and BuildTargetStatement)
        //// The replacement will be applied against this, replaceing the Ids with the "correct"values from the replacement
        //// Then afterwards, any id's left in the string are converted back to the name values
        //foreach (var operation in templateMetadata.ComponentTemplateBase.Model.Operations)
        //{
        //    SetSourceReplacement([templateMetadata.ComponentTemplateBase.Model.InternalElement, operation.InternalElement], "");
        //    SetSourceReplacement(operation.InternalElement, "");
        //}
        //foreach (var modelDef in templateMetadata.ComponentTemplateBase.Model.ModelDefinitions)
        //{
        //    SetSourceReplacement([templateMetadata.ComponentTemplateBase.Model.InternalElement, modelDef.InternalElement], "this");
        //}
        //SetSourceReplacement(templateMetadata.ComponentTemplateBase.Model.InternalElement, "this");
        //var requestName = ApplySourceInitializationStatements(templateMetadata);

        //foreach (var operation in templateMetadata.ComponentTemplateBase.Model.Operations)
        //{
        //    SetTargetReplacement([templateMetadata.ComponentTemplateBase.Model.InternalElement.Id, operation.InternalElement.Id, null, null], "data");
        //}
        //SetTargetReplacement(templateMetadata.ServiceProxyOperation.InternalElement, "");
        //SetTargetReplacement(templateMetadata.ComponentTemplateBase.Model.InternalElement, "this");
        //var targetStatement = BuildTargetStatement(templateMetadata);

        var requestResult = sourceStrategy?.GenerateImplementation() ?? new(string.Empty, string.Empty);
        var targetStatement = "";

        var serviceProxyVariableName = interactionMetadata.ServiceProxyTemplate.Model.Name.ToCamelCase(true);
        var serviceProxyOperationName = interactionMetadata.ServiceProxyOperation.Name.ToCamelCase(true);
        var responseVariableName = !string.IsNullOrWhiteSpace(targetStatement) ? "data" : "";
        var localResponseAssignmentStatement = !string.IsNullOrWhiteSpace(targetStatement) ? @$"{Environment.NewLine}
        {targetStatement}" : "";

        interactionMetadata.InvocationMethod.AddStatement(new TypescriptStatement($@"{requestResult.RequestStatement}this.{serviceProxyVariableName}.{serviceProxyOperationName}({requestResult.RequestName}).subscribe({{
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

    internal static InteractionMetadata GetInteractionMetadata(IAssociation association, IApplication application, ITypescriptFileBuilderTemplate template)
    {
        var metadata = new InteractionMetadata();

        // None of these should ever be null
        var @class = template.TypescriptFile.Classes.FirstOrDefault(c => c.Decorators.Any(d => d.Name == "Component"));
        metadata.Class = @class;

        var ctor = @class.Constructors.FirstOrDefault();
        metadata.Constructor = ctor;

        // get the operation on the component doing the invocation
        var templateOperation = @class.Methods.FirstOrDefault(m => m.Name.Equals(association.TargetEnd.ParentElement.Name, StringComparison.CurrentCultureIgnoreCase));
        metadata.InvocationMethod = templateOperation;

        // this gets the proxy model which is mapped
        var proxyModel = application.MetadataManager.GetServiceProxyModels(application.Id, application.MetadataManager.UserInterface)
            .FirstOrDefault(p => p.Endpoints.Any(e => e.Id == association.SourceEnd.ParentElement.Id));
        metadata.ServiceProxyModel = proxyModel;

        var proxyTemplate = application.FindTemplateInstance<TypeScriptTemplateBase<IServiceProxyModel>>(HttpServiceProxyTemplate.TemplateId, proxyModel);
        metadata.ServiceProxyTemplate = proxyTemplate;

        var serviceproxyOperation = proxyModel.Endpoints.First(e => e.InternalElement.Id == association.SourceEnd.ParentId);
        metadata.ServiceProxyOperation = serviceproxyOperation;

        metadata.ComponentTemplateBuilder = template;

        return metadata;
    }

    private static void InjectProxyService(InteractionMetadata templateMetadata)
    {
        if (!templateMetadata.Constructor.Parameters.Any(p => p.Name == templateMetadata.ServiceProxyTemplate.Model.Name.ToCamelCase(true)))
        {
            templateMetadata.Constructor.AddParameter(templateMetadata.ServiceProxyTemplate.Model.Name.ToCamelCase(true), templateMetadata.ComponentTemplateBase.GetTypeName(templateMetadata.ServiceProxyModel.InternalElement), param => param.WithPrivateFieldAssignment());
        }
    }

    private static void AddServiceInvocationSupportFields(InteractionMetadata templateMetadata)
    {
        // add the field to handle the errors
        if (!templateMetadata.ComponentTemplateBuilder.TypescriptFile.Classes.Any(c => c.Fields.Any(f => f.Name == "serviceError")))
        {
            templateMetadata.Class.AddField("serviceError", "string | null", @field => field.WithValue("null"));
        }

        // add the field to indicate success
        if (!templateMetadata.ComponentTemplateBuilder.TypescriptFile.Classes.Any(c => c.Fields.Any(f => f.Name == "serviceCallSuccess")))
        {
            templateMetadata.Class.AddField("serviceCallSuccess", "boolean", @field => field.WithValue("false"));
        }
    }
}
