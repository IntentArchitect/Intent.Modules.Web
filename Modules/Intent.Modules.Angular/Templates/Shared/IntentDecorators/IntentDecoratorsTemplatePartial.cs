using System;
using System.Collections.Generic;
using Intent.Engine;
using Intent.Modules.Angular.Api;
using Intent.Modules.Common;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.TypeScript.Templates.TypescriptTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.Shared.IntentDecorators
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class IntentDecoratorsTemplate : TypeScriptTemplateBase<object>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.Shared.IntentDecorators";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public IntentDecoratorsTemplate(IOutputTarget outputTarget, object model = null) : base(TemplateId, outputTarget, model)
        {
        }

        public string IntentIgnore => "IntentIgnore";
        public string IntentIgnoreBody => "IntentIgnoreBody";
        public string IntentManage => "IntentManage";
        public string IntentMerge => "IntentMerge";
        public string IntentManageClass => "IntentManageClass";

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new TypeScriptFileConfig(
                overwriteBehaviour: OverwriteBehaviour.Always,
                fileName: "intent.decorators",
                relativeLocation: "",
                className: IntentIgnore
            );
        }

    }
}