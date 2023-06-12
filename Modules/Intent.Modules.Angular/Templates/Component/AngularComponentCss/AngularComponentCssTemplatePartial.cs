using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Intent.Angular.Api;
using Intent.Engine;
using Intent.Metadata.Models;
using Intent.Modelers.WebClient.Angular.Api;
using Intent.Modules.Angular.Api;
using Intent.Modules.Angular.Templates.Module.AngularModule;
using Intent.Modules.Common;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.Types.Api;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.ProjectItemTemplate.Partial", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.Component.AngularComponentCss
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class AngularComponentCssTemplate : IntentTemplateBase<ComponentModel>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.Component.AngularComponentCss";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public AngularComponentCssTemplate(IOutputTarget outputTarget, ComponentModel model) : base(TemplateId, outputTarget, model)
        {
        }

        public string ComponentName
        {
            get
            {
                if (Model.Name.EndsWith("Component", StringComparison.InvariantCultureIgnoreCase))
                {
                    return Model.Name.Substring(0, Model.Name.Length - "Component".Length);
                }
                return Model.Name;
            }
        }

        public string ModuleName { get; private set; }

        public override void AfterTemplateRegistration()
        {
            var moduleTemplate = OutputTarget.FindTemplateInstance<AngularModuleTemplate>(AngularModuleTemplate.TemplateId, Model.Module);
            ModuleName = moduleTemplate.ModuleName;
        }

        public override string RunTemplate()
        {
            var meta = GetMetadata();
            var fullFileName = Path.Combine(meta.GetFullLocationPath(), meta.FileNameWithExtension());

            var source = LoadOrCreate(fullFileName);

            return source;
        }

        private string LoadOrCreate(string fullFileName)
        {
            return File.Exists(fullFileName) ? File.ReadAllText(fullFileName) : TransformText();
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            var moduleTemplate = ExecutionContext.FindTemplateInstance<AngularModuleTemplate>(AngularModuleTemplate.TemplateId, Model.Module);
            return new TemplateFileConfig(
                fileName: $"{ComponentName.ToKebabCase()}.component",
                fileExtension: "scss",
                relativeLocation: $"{string.Join("/", Model.GetParentFolderNames())}/{(Model.GetAngularComponentSettings().InOwnFolder() ? $"/{ComponentName.ToKebabCase()}" : "")}"
            );
        }
    }
}