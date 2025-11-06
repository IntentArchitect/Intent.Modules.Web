using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Metadata.Models;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.Types.Api;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.TypeScript.Templates.TypescriptTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.Model.Model
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class ModelTemplate : TypeScriptTemplateBase<Intent.Modelers.WebClient.Angular.Api.ModelDefinitionModel>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.Model.Model";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public ModelTemplate(IOutputTarget outputTarget, Intent.Modelers.WebClient.Angular.Api.ModelDefinitionModel model) : base(TemplateId, outputTarget, model)
        {
            AddTypeSource("Intent.Angular.ServiceProxies.Proxies.AngularDTO");
        }

        public string GetGenericParameters()
        {
            return Model.GenericTypes.Any() ? $"<{string.Join(", ", Model.GenericTypes)}>" : "";
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new TypeScriptFileConfig(
                overwriteBehaviour: OverwriteBehaviour.Always,
                fileName: GetFileName(),
                relativeLocation: $"{string.Join("/", Model.GetParentFolderNames().Concat(new[] { "models" }))}",
                className: "${Model.Name}"
            );
        }

        private string GetFileName()
        {
            var modelName = Model.Name.EndsWith("Model") ? Model.Name.Substring(0, Model.Name.Length - "Model".Length) : Model.Name;
            return $"{modelName.ToKebabCase()}.model";
        }

        public string GetPath(IEnumerable<IElementMappingPathTarget> path)
        {
            return string.Join(".", path.Select(x => x.Name.ToCamelCase()));
        }
    }
}