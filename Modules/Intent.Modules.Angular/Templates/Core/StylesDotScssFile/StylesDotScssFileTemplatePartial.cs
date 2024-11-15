using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Metadata.Models;
using Intent.Modules.Common;
using Intent.Modules.Common.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.ProjectItemTemplate.Partial", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.Core.StylesDotScssFile
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class StylesDotScssFileTemplate : IntentTemplateBase<object>
    {
        private readonly List<string> _requests = new();

        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.Core.StylesDotScssFile";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public StylesDotScssFileTemplate(IOutputTarget outputTarget, object model = null) : base(TemplateId, outputTarget, model)
        {
            ExecutionContext.EventDispatcher.Subscribe<StyleRequest>(request => _requests.Add(request.Payload));
        }

        public override string RunTemplate()
        {
            if (!TryGetExistingFileContent(out var content))
            {
                content = TransformText();
            }

            var lines = content.ReplaceLineEndings().Split(System.Environment.NewLine).ToList();

            int trailingEmptyLineCount;
            for (trailingEmptyLineCount = 0; trailingEmptyLineCount < lines.Count; trailingEmptyLineCount++)
            {
                if (!string.IsNullOrWhiteSpace(lines[^(trailingEmptyLineCount + 1)]))
                {
                    break;
                }
            }

            var existing = new HashSet<string>(lines.Select(x => x.Trim().ToLowerInvariant()));
            foreach (var request in _requests)
            {
                if (!existing.Add(request.Trim().ToLowerInvariant()))
                {
                    continue;
                }

                lines.Insert(lines.Count - trailingEmptyLineCount, request);
            }

            content = string.Join(System.Environment.NewLine, lines);

            return content;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new TemplateFileConfig(
                fileName: $"styles",
                fileExtension: "scss"
            );
        }
    }
}