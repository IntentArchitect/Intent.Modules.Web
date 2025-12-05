using System;
using System.Collections.Generic;
using Intent.Engine;
using Intent.Modules.Common;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.Types.Api;
using Intent.Modules.Common.TypeScript.Builder;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.TypeScript.Templates.TypescriptTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Angular.HttpClients.Templates.EnumContract
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public partial class EnumContractTemplate : TypeScriptTemplateBase<Intent.Modules.Common.Types.Api.EnumModel>, ITypescriptFileBuilderTemplate
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.HttpClients.EnumContract";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully, Body = Mode.Ignore)]
        public EnumContractTemplate(IOutputTarget outputTarget, Intent.Modules.Common.Types.Api.EnumModel model) : base(TemplateId, outputTarget, model)
        {
            TypescriptFile = new TypescriptFile(this.GetFolderPath(), this)
                .AddEnum($"{Model.Name}", @enum =>
                {
                    @enum.Export();
                    foreach (var literal in Model.Literals)
                    {
                        @enum.AddLiteral(literal.Name, !string.IsNullOrWhiteSpace(literal.Value) ? literal.Value : null);
                    }
                });
        }

        [IntentManaged(Mode.Fully)]
        public TypescriptFile TypescriptFile { get; }

        [IntentManaged(Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return TypescriptFile.GetConfig($"{Model.Name}");
        }

        [IntentManaged(Mode.Fully)]
        public override string TransformText()
        {
            return TypescriptFile.ToString();
        }
    }
}