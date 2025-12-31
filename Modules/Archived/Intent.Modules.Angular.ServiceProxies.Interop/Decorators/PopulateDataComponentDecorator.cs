using System.IO;
using System.Linq;
using System.Text;
using Intent.Engine;
using Intent.Metadata.Models;
using Intent.Modelers.Types.ServiceProxies.Api;
using Intent.Modelers.UI.Api;
using Intent.Modules.Angular.ServiceProxies.Templates.AngularServiceProxy;
using Intent.Modules.Angular.Templates.Component.AngularComponentTs;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.Types.Api;
using Intent.RoslynWeaver.Attributes;
using OperationModel = Intent.Modelers.Types.ServiceProxies.Api.OperationModel;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateDecorator", Version = "1.0")]

namespace Intent.Modules.Angular.ServiceProxies.Interop.Decorators
{
    [IntentManaged(Mode.Merge)]
    public class PopulateDataComponentDecorator : AngularComponentTsDecorator
    {
        [IntentManaged(Mode.Fully)]
        public const string DecoratorId = "Intent.Angular.ServiceProxies.Interop.PopulateDataComponentDecorator";

        [IntentManaged(Mode.Fully)]
        private readonly AngularComponentTsTemplate _template;
        [IntentManaged(Mode.Fully)]
        private readonly IApplication _application;

        private readonly StringBuilder _onInitStatements = new();

        [IntentManaged(Mode.Merge)]
        public PopulateDataComponentDecorator(AngularComponentTsTemplate template, IApplication application)
        {
            _template = template;
            _application = application;

            if (File.Exists(_template.GetMetadata().GetFilePath()))
            {
                return;
            }

            var serviceProxies = Enumerable.Empty<ServiceProxyModel>()
                //.Concat(application.MetadataManager.WebClient(application).GetServiceProxyModels()) // because .UserInterface(...) it's fetching by designer's id.
                .Concat(application.MetadataManager.UserInterface(application).GetServiceProxyModels()) 
                .ToArray();

            foreach (var model in _template.Model.Models)
            {
                var service = serviceProxies.FirstOrDefault(x => x.Operations.Any(o => MatchesOperationReturnType(o, model.TypeReference)));
                var operation = service?.Operations.FirstOrDefault(x => MatchesOperationReturnType(x, model.TypeReference) && x.Parameters.Count == 0);
                if (service == null || operation == null)
                {
                    continue;
                }

                _template.InjectService(service.Name.ToCamelCase(true), _template.GetTypeName(AngularServiceProxyTemplate.TemplateId, service));

                _onInitStatements.AppendLine($"    this.{service.Name.ToCamelCase(true)}.{operation.Name.ToCamelCase(true)}()");
                _onInitStatements.AppendLine($"      .subscribe(result => this.{model.Name.ToCamelCase(true)} = result);");
            }
        }

        public override string OnInit()
        {
            if (_onInitStatements.Length == 0)
            {
                return null;
            }

            return _onInitStatements.ToString();
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