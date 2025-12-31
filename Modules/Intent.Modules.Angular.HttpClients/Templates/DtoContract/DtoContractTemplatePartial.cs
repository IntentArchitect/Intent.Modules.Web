using System;
using System.Collections.Generic;
using Intent.Engine;
using Intent.IArchitect.Agent.Persistence;
using Intent.Modelers.Services.Api;
using Intent.Modules.Angular.HttpClients.Templates.EnumContract;
using Intent.Modules.Angular.HttpClients.Templates.Helper;
using Intent.Modules.Angular.HttpClients.Templates.PagedResult;
using Intent.Modules.Angular.ServiceProxies.Templates.Proxies.PagedResult;
using Intent.Modules.Common;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.TypeScript.Builder;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.TypeScript.Templates.TypescriptTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Angular.HttpClients.Templates.DtoContract
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public partial class DtoContractTemplate : TypeScriptTemplateBase<Intent.Modelers.Services.Api.DTOModel>, ITypescriptFileBuilderTemplate
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.HttpClients.DtoContract";

        private static ISet<string> _outboundDtoElementIds = new HashSet<string>();

        [IntentManaged(Mode.Merge, Signature = Mode.Fully, Body = Mode.Ignore)]
        public DtoContractTemplate(IOutputTarget outputTarget, Intent.Modelers.Services.Api.DTOModel model) : base(TemplateId, outputTarget, model)
        {
            TypescriptFile = new TypescriptFile(this.GetFolderPath(), this)
                .AddInterface($"{Model.Name}", @interface =>
                {
                    AddTypeSource(TemplateId);
                    AddTypeSource(EnumContractTemplate.TemplateId);
                    PagedResultTypeSource.ApplyTo(this, PagedResultTemplate.TemplateId);

                    @interface.Export();

                    foreach (var field in Model.Fields)
                    {
                        var type = GetTypeName(field.TypeReference);
                        type = field.TypeReference.IsNullable ? $"{type} | null" : type;

                        @interface.AddField($"{field.Name.ToCamelCase(true)}", type);
                    }
                });
        }

        [IntentManaged(Mode.Fully)]
        public TypescriptFile TypescriptFile { get; }

        [IntentManaged(Mode.Ignore)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new TypeScriptFileConfig(
                overwriteBehaviour: OverwriteBehaviour.Always,
                fileName: $"{Model.Name.ToKebabCase()}",
                relativeLocation: this.GetPackageBasedRelativeLocation([]),
                className: $"{Model.Name}"
            );
        }

        [IntentManaged(Mode.Fully)]
        public override string TransformText()
        {
            return TypescriptFile.ToString();
        }

        internal static void SetOutboundDtoElementIds(ISet<string> outboundDtoElementIds)
        {
            _outboundDtoElementIds = outboundDtoElementIds;
        }
    }
}