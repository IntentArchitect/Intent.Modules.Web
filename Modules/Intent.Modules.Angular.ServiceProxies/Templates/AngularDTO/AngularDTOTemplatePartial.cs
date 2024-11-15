using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Modelers.Services.Api;
using Intent.Modules.Common;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.TypeScript.Builder;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.TypeScript.Templates.TypescriptTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Angular.ServiceProxies.Templates.AngularDTO
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public partial class AngularDTOTemplate : TypeScriptTemplateBase<Intent.Modelers.Services.Api.DTOModel>, ITypescriptFileBuilderTemplate
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.ServiceProxies.AngularDTO";

        [IntentManaged(Mode.Ignore, Signature = Mode.Merge)]
        public AngularDTOTemplate(IOutputTarget outputTarget, Intent.Modelers.Services.Api.DTOModel model) : base(TemplateId, outputTarget, model)
        {
            TypescriptFile = new TypescriptFile(this.GetFolderPath())
                .AddInterface($"{Model.Name}", @interface =>
                {
                    AddTypeSource(TemplateId);

                    @interface.Export();

                    foreach (var field in Model.Fields)
                    {
                        var type = GetTypeName(field.TypeReference);
                        var nullable = field.TypeReference.IsNullable ? "?" : "";

                        @interface.AddField($"{field.Name.ToCamelCase(true)}{nullable}", type);
                    }
                });
        }

        public string GenericTypes => Model.GenericTypes.Any() ? $"<{string.Join(", ", Model.GenericTypes)}>" : "";

        [IntentManaged(Mode.Fully)]
        public TypescriptFile TypescriptFile { get; }

        [IntentManaged(Mode.Ignore)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new TypeScriptFileConfig(
                overwriteBehaviour: OverwriteBehaviour.Always,
                fileName: $"{Model.Name.ToKebabCase().RemoveSuffix("-dto")}.dto",
                relativeLocation: this.GetPackageBasedRelativeLocation(["models"]),
                className: $"{Model.Name}"
            );
        }

        [IntentManaged(Mode.Fully)]
        public override string TransformText()
        {
            return TypescriptFile.ToString();
        }
    }
}