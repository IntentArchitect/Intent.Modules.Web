using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Intent.Angular.ServiceProxies.Api;
using Intent.Engine;
using Intent.Metadata.Models;
using Intent.Modelers.Services.Api;
using Intent.Modelers.Types.ServiceProxies.Api;
using Intent.Modelers.WebClient.Angular.Api;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.Types.Api;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.TypeScript.Templates.TypescriptTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Angular.ServiceProxies.Templates.Proxies.AngularDTO
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class AngularDTOTemplate : TypeScriptTemplateBase<Intent.Modelers.Services.Api.DTOModel>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.ServiceProxies.Proxies.AngularDTO";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public AngularDTOTemplate(IOutputTarget outputTarget, Intent.Modelers.Services.Api.DTOModel model) : base(TemplateId, outputTarget, model)
        {
            AddTypeSource(TemplateId);
        }

        public string GenericTypes => Model.GenericTypes.Any() ? $"<{string.Join(", ", Model.GenericTypes)}>" : "";

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new TypeScriptFileConfig(
                overwriteBehaviour: OverwriteBehaviour.Always,
                fileName: $"{Model.Name.ToKebabCase().RemoveSuffix("-dto")}.dto",
                relativeLocation: $"{string.Join("/", Model.GetModule().InternalElement.GetFolderPath(Model.GetModule().GetModuleName().ToKebabCase(), "models"))}",
                className: $"{Model.Name}"
            );
        }
    }
}