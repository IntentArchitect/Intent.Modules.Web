using Intent.Engine;
using Intent.Modelers.UI.Api;
using Intent.Modules.Common;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.Types.Api;
using Intent.Modules.Common.TypeScript.Builder;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;
using System;
using System.Collections.Generic;
using System.Linq;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.TypeScript.Templates.TypescriptTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.Component.LayoutComponentTypescript
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public partial class LayoutComponentTypescriptTemplate : TypeScriptTemplateBase<Intent.Modelers.UI.Api.LayoutModel>, ITypescriptFileBuilderTemplate
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.Component.LayoutComponentTypescript";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public LayoutComponentTypescriptTemplate(IOutputTarget outputTarget, Intent.Modelers.UI.Api.LayoutModel model) : base(TemplateId, outputTarget, model)
        {
            TypescriptFile = new TypescriptFile(this.GetFolderPath(), this)
                .AddClass($"{Model.Name}", @class =>
                {
                    @class.AddConstructor(ctor =>
                    {
                        ctor.AddParameter("string", "exampleParam", param =>
                        {
                            param.WithPrivateReadonlyFieldAssignment();
                        });
                    });
                });
        }

        [IntentManaged(Mode.Fully)]
        public TypescriptFile TypescriptFile { get; }

        public string LayoutName
        {
            get
            {
                if (Model.Name.EndsWith("Layout", StringComparison.InvariantCultureIgnoreCase))
                {
                    return Model.Name.Substring(0, Model.Name.Length - "Layout".Length);
                }
                return Model.Name;
            }
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new TypeScriptFileConfig(
                overwriteBehaviour: OverwriteBehaviour.Always,
                fileName: $"{LayoutName.ToKebabCase()}.component",
                relativeLocation: $"{string.Join("/", Model.GetParentFolderNames().Select(f => f.ToKebabCase()))}/{LayoutName.ToKebabCase()}",
                className: $"{LayoutName}Layout"
            );
        }

        [IntentManaged(Mode.Fully)]
        public override string TransformText()
        {
            return TypescriptFile.ToString();
        }
    }
}