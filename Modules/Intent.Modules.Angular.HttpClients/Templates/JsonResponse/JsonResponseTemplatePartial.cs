using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Modelers.Types.ServiceProxies.Api;
using Intent.Modelers.UI.Api;
using Intent.Modules.Angular.HttpClients.Templates.Helper;
using Intent.Modules.Common;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.TypeScript.Builder;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.Modules.Metadata.WebApi.Models;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;
using static System.Net.Mime.MediaTypeNames;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.TypeScript.Templates.TypescriptTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Angular.HttpClients.Templates.JsonResponse
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public partial class JsonResponseTemplate : TypeScriptTemplateBase<object>, ITypescriptFileBuilderTemplate
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.HttpClients.JsonResponse";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully, Body = Mode.Ignore)]
        public JsonResponseTemplate(IOutputTarget outputTarget, object model = null) : base(TemplateId, outputTarget, model)
        {
            TypescriptFile = new TypescriptFile(this.GetFolderPath())
                .AddClass($"JsonResponse", @class =>
                {
                    @class.Export();
                    @class.AddField("value", "T");

                    @class.AddGenericParameter("T");
                    @class.AddConstructor(ctor =>
                    {
                        ctor.AddParameter("value", "T");
                        ctor.AddStatement("this.value = value");
                    });
                });
        }

        [IntentManaged(Mode.Fully)]
        public TypescriptFile TypescriptFile { get; }

        [IntentManaged(Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return TypescriptFile.GetConfig($"JsonResponse");
        }

        [IntentManaged(Mode.Fully)]
        public override string TransformText()
        {
            return TypescriptFile.ToString();
        }

        public override bool CanRunTemplate()
        {
            var x = ExecutionContext.MetadataManager.GetServiceProxyModels(
               OutputTarget.Application.Id,
               ExecutionContext.MetadataManager.UserInterface);

            return x.SelectMany(s => s.Endpoints)
                .Any(x => x.MediaType == HttpMediaType.ApplicationJson);
        }
    }
}