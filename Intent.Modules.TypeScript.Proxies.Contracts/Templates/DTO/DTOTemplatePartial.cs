using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.Modules.Modelers.WebClient.Proxies.Api;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("ModuleBuilder.TypeScript.Templates.TypescriptTemplatePartial", Version = "1.0")]

namespace Intent.Modules.TypeScript.Proxies.Contracts.Templates.DTO
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class DTOTemplate : TypeScriptTemplateBase<ServiceProxyDTOModel>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "TypeScript.Proxies.Contracts.DTO";

        public DTOTemplate(IOutputTarget outputTarget, ServiceProxyDTOModel model) : base(TemplateId, outputTarget, model)
        {
            AddTypeSource(TemplateId);
        }

        public string GenericTypes => Model.GenericTypes.Any() ? $"<{ string.Join(", ", Model.GenericTypes) }>" : "";

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            //var moduleTemplate = Project.FindTemplateInstance<AngularModuleTemplate.AngularModuleTemplate>(AngularModuleTemplate.AngularModuleTemplate.TemplateId, Model.Module);
            return new TypeScriptFileConfig(
                className: $"{Model.Name}",
                fileName: $"{Model.Name.ToKebabCase()}",
                relativeLocation: ""
            );
        }

    }
}