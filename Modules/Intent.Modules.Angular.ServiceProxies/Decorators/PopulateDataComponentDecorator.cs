using System.Collections.Generic;
using System.IO;
using System.Linq;
using Intent.Engine;
using Intent.Metadata.Models;
using Intent.Metadata.WebApi.Api;
using Intent.Modelers.Types.ServiceProxies.Api;
using Intent.Modelers.WebClient.Angular.Api;
using Intent.Modelers.WebClient.Api;
using Intent.Modules.Angular.ServiceProxies.Templates.Proxies.AngularServiceProxy;
using Intent.Modules.Angular.Templates.Component.AngularComponentTs;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.Types.Api;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateDecorator", Version = "1.0")]

namespace Intent.Modules.Angular.ServiceProxies.Decorators
{
    [IntentManaged(Mode.Merge)]
    public class PopulateDataComponentDecorator : AngularComponentTsDecorator
    {
        [IntentManaged(Mode.Fully)]
        public const string DecoratorId = "Intent.Angular.ServiceProxies.PopulateDataComponentDecorator";

        [IntentManaged(Mode.Fully)]
        private readonly AngularComponentTsTemplate _template;
        [IntentManaged(Mode.Fully)]
        private readonly IApplication _application;
        private readonly IList<ServiceProxyModel> _serviceProxies;
        private List<string> _onInitStatements = new List<string>();

        [IntentManaged(Mode.Merge)]
        public PopulateDataComponentDecorator(AngularComponentTsTemplate template, IApplication application)
        {
            _template = template;
            _application = application;
            _serviceProxies = application.MetadataManager.WebClient(application).GetServiceProxyModels();
            if (!File.Exists(_template.GetMetadata().GetFilePath()))
            {
                foreach (var model in _template.Model.Models)
                {
                    var service = _serviceProxies.FirstOrDefault(x => x.Operations.Any(o => MatchesOperationReturnType(o, model.TypeReference)));
                    var operation = service?.Operations.FirstOrDefault(x => MatchesOperationReturnType(x, model.TypeReference) && x.Parameters.Count == 0);
                    if (service != null && operation != null)
                    {
                        _template.InjectService(service.Name.ToCamelCase(true), _template.GetTypeName(AngularServiceProxyTemplate.TemplateId, service));
                        _onInitStatements.Add($@"this.{service.Name.ToCamelCase(true)}.{operation.Name.ToCamelCase(true)}()
      .subscribe(result => this.{model.Name.ToCamelCase(true)} = result);");
                    }
                }
            }
        }

        public override string OnInit()
        {
            return _onInitStatements.Any() ? @"
    " + string.Join(@"
    ", _onInitStatements) : null;
        }

        private static bool MatchesOperationReturnType(OperationModel o, ITypeReference typeReference)
        {
            return o.ReturnType?.Element.SpecializationTypeId != TypeDefinitionModel.SpecializationTypeId &&
                   o.ReturnType?.Element.SpecializationTypeId != EnumModel.SpecializationTypeId &&
                   o.ReturnType?.Element.SpecializationTypeId == typeReference.Element.SpecializationTypeId &&
                   o.ReturnType?.Element.Id == typeReference.Element.Id &&
                   o.ReturnType?.IsCollection == typeReference.IsCollection &&
                   o.ReturnType?.IsNullable == typeReference.IsNullable;
        }
    }
}