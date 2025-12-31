using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Metadata.Models;
using Intent.Modules.Common;
using Intent.Modules.Common.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.ProjectItemTemplate.Partial", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.Core.AngularDotJsonFile
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class AngularDotJsonFileTemplate : IntentTemplateBase<object>
    {
        private readonly HashSet<string> _requiredStyles = new();

        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.Core.AngularDotJsonFile";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public AngularDotJsonFileTemplate(IOutputTarget outputTarget, object model = null) : base(TemplateId, outputTarget, model)
        {
            ExecutionContext.EventDispatcher.Subscribe<AngularDotJsonStyleRequired>(x => _requiredStyles.Add(x.Payload));
        }

        private string AppNameCamelCased => OutputTarget.ApplicationName().ToCamelCase();
        private string AppNameKebabCased => OutputTarget.ApplicationName().ToKebabCase();

        public override string RunTemplate()
        {
            if (!TryGetExistingFileContent(out var content))
            {
                content = TransformText();
            }

            if (!_requiredStyles.Any())
            {
                return content;
            }

            var json = JsonConvert.DeserializeObject<JObject>(content)!;
            if (json["projects"] == null)
            {
                return content;
            }

            var paths = new[]
            {
                new[] {"architect", "build", "options" },
                new[] {"architect", "test", "options" }
            };

            foreach (var project in json["projects"]!.Children())
            {
                foreach (var path in paths)
                {
                    var currentToken = project.Children().First();
                    foreach (var field in path)
                    {
                        currentToken = currentToken[field] ??= new JObject();
                    }

                    var stylesToken = (JArray)(currentToken["styles"] ??= new JArray());

                    var missingStyles = _requiredStyles
                        .Where(requiredStyle => !stylesToken.Contains(requiredStyle))
                        .ToArray();
                    if (!missingStyles.Any())
                    {
                        continue;
                    }

                    var styles = stylesToken
                        .Select(x => x.Value<string>())
                        .Union(missingStyles)
                        .OrderBy(x => x)
                        .ToArray();

                    stylesToken.Clear();

                    foreach (var style in styles.OrderBy(x => x))
                    {
                        stylesToken.Add(style);
                    }
                }
            }

            content = JsonConvert.SerializeObject(json, Formatting.Indented);

            return content;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new TemplateFileConfig(
                fileName: $"angular",
                fileExtension: "json"
            );
        }
    }
}