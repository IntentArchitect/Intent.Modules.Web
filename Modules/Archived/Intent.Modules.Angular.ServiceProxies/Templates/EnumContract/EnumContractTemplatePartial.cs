using System;
using System.Collections.Generic;
using Intent.Engine;
using Intent.Modules.Common;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.Types.Api;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.TypeScript.Templates.TypescriptTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Angular.ServiceProxies.Templates.EnumContract
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class EnumContractTemplate : TypeScriptTemplateBase<Intent.Modules.Common.Types.Api.EnumModel>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.ServiceProxies.EnumContract";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public EnumContractTemplate(IOutputTarget outputTarget, Intent.Modules.Common.Types.Api.EnumModel model) : base(TemplateId, outputTarget, model)
        {
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new TypeScriptFileConfig(
                className: $"{Model.Name}",
                fileName: $"{Model.Name.ToKebabCase()}"
            );
        }
    }
}