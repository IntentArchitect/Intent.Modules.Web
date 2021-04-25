using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Metadata.Models;
using Intent.Modelers.Services.Api;
using Intent.Modules.Angular.Api;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;
using Intent.Utils;
using Intent.Modelers.WebClient.Angular.Api;
using Intent.Modules.Common.Types.Api;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.TypeScript.Templates.TypescriptTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.Model.FormGroupTemplate
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class FormGroupTemplate : TypeScriptTemplateBase<Intent.Modelers.WebClient.Angular.Api.FormGroupDefinitionModel>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.Model.FormGroupTemplate";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public FormGroupTemplate(IOutputTarget outputTarget, Intent.Modelers.WebClient.Angular.Api.FormGroupDefinitionModel model) : base(TemplateId, outputTarget, model)
        {
            AddTypeSource(FormGroupTemplate.TemplateId);
            // GCB - an interesting dependency predicament. Perhaps there should be a mechanism where a template can declare itself
            //       the representative of a model type, and this would not be necessary. Using the hierarchy of the OutputTargets could
            //       make the discovery of the type predictable. Food for thought.
            //AddTypeSource(AngularDTOTemplate.TemplateId);
            AddTypeSource("Intent.Angular.ServiceProxies.Proxies.AngularDTOTemplate");
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new TypeScriptFileConfig(
                overwriteBehaviour: OverwriteBehaviour.Always,
                fileName: $"{Model.Name.ToKebabCase().RemoveSuffix("-model")}.model",
                relativeLocation: $"{string.Join("/", Model.GetParentFolderNames().Concat(new[] { "models" }))}",
                className: "${Model.Name}"
            );
        }

        public override string RunTemplate()
        {
            ;
            try
            {
                return base.RunTemplate();
            }
            catch (Exception e)
            {
                Logging.Log.Failure(e);
                return TransformText();
            }
        }

        private string GetFieldDefaultValue(FormGroupControlModel field)
        {
            switch (GetTypeName(field.TypeReference))
            {
                case "string":
                    return "\"\"";
                case "boolean":
                    return "false";
                default:
                    return field.TypeReference.IsCollection ? "[]" : "null";
            }
        }

        public string GetPathFromMapping(string prefix, FormGroupControlModel field)
        {
            var path = prefix + string.Join(".", field.InternalElement.MappedElement.Path.Select(x => x.Name.ToCamelCase()));
            if (field.InternalElement.IsMapped && field.TypeReference.Element.SpecializationTypeId == FormGroupDefinitionModel.SpecializationTypeId)
            {
                if (field.TypeReference.IsCollection)
                {
                    path += $".map(x => new {GetTypeName(field).RemoveSuffix("[]")}(x))";
                }
                else
                {
                    path = $"new {GetTypeName(field)}({path})";
                }
            }
            return path;
        }

        private string GetFormFieldType(FormGroupControlModel field)
        {
            return field.TypeReference.IsCollection ? "FormArray" : "FormControl";
        }
    }
}