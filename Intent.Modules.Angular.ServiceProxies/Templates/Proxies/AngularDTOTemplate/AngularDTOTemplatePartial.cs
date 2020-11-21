using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Intent.Angular.ServiceProxies.Api;
using Intent.Engine;
using Intent.Metadata.Models;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.Modelers.Types.ServiceProxies.Api;
using Intent.Modules.Common.Templates;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("ModuleBuilder.TypeScript.Templates.TypescriptTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Angular.ServiceProxies.Templates.Proxies.AngularDTOTemplate
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class AngularDTOTemplate : TypeScriptTemplateBase<ServiceProxyDTOModel>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Angular.ServiceProxies.Proxies.AngularDTOTemplate";

        public AngularDTOTemplate(IOutputTarget project, ServiceProxyDTOModel model) : base(TemplateId, project, model)
        {
            AddTypeSource(TemplateId);
        }

        public string GenericTypes => Model.GenericTypes.Any() ? $"<{ string.Join(", ", Model.GenericTypes) }>" : "";

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new TypeScriptFileConfig(
                overwriteBehaviour: OverwriteBehaviour.Always,
                fileName: $"{Model.Name}",
                relativeLocation: $"{Model.GetModule().GetModuleName().ToKebabCase()}/models",
                className: $"{Model.Name}"
            );
        }
    }
}