using System;
using System.Collections.Generic;
using Intent.Engine;
using Intent.Modules.Common;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.TypeScript.Builder;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.TypeScript.Templates.TypescriptTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Angular.HttpClients.Templates.PagedResult
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public partial class PagedResultTemplate : TypeScriptTemplateBase<object>, ITypescriptFileBuilderTemplate
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.HttpClients.PagedResult";

        public const string TypeDefinitionElementId = "9204e067-bdc8-45e7-8970-8a833fdc5253";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully, Body = Mode.Ignore)]
        public PagedResultTemplate(IOutputTarget outputTarget, object model = null) : base(TemplateId, outputTarget, model)
        {
            TypescriptFile = new TypescriptFile(this.GetFolderPath(), this)
                .AddClass($"PagedResult", @class =>
                {
                    @class.Export();
                    @class.AddGenericParameter("T");

                    @class.AddConstructor(ctor =>
                    {
                        ctor.AddParameter("totalCount", "number")
                            .AddParameter("pageCount", "number")
                            .AddParameter("pageSize", "number")
                            .AddParameter("pageNumber", "number")
                            .AddParameter("data", "Array<T>");

                        ctor.AddStatement("this.totalCount = totalCount;");
                        ctor.AddStatement("this.pageCount = pageCount;");
                        ctor.AddStatement("this.pageSize = pageSize;");
                        ctor.AddStatement("this.pageNumber = pageNumber;");
                        ctor.AddStatement("this.data = data;");

                    });

                    @class.AddField("totalCount", "number")
                        .AddField("pageCount", "number")
                        .AddField("pageSize", "number")
                        .AddField("pageNumber", "number")
                        .AddField("data", "Array<T>");
                });
        }

        [IntentManaged(Mode.Fully)]
        public TypescriptFile TypescriptFile { get; }

        [IntentManaged(Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return TypescriptFile.GetConfig($"PagedResult");
        }

        [IntentManaged(Mode.Fully)]
        public override string TransformText()
        {
            return TypescriptFile.ToString();
        }
    }
}