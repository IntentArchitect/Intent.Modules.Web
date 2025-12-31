using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.FileTemplateStringInterpolation", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.Core.IndexHtml
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public partial class IndexHtmlTemplate
    {
        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public override string TransformText()
        {
            string html;

            if (!TryGetExistingFileContent(out var existingFileContent))
            {
                html = @$"<!doctype html>
<html lang=""en"">
<head>
  <meta charset=""utf-8"" />
  <title>{Title}</title>
  <base href=""/"" />
  <meta name=""viewport"" content=""width=device-width, initial-scale=1"" />
  <link rel=""icon"" type=""image/x-icon"" href=""favicon.ico"" />
</head>
<body>
  <app-main></app-main>
</body>
</html>
";
            }
            else
            {
                html = existingFileContent;
            }

            return InjectClientResources(html);
        }

        // This will inject any published client resource requests into the <head> section of the HTML
        private string InjectClientResources(string html)
        {
            if (string.IsNullOrEmpty(html) || _resourceRequests.Count == 0)
                return html;

            const string headCloseTag = "</head>";
            var headCloseIndex = html.IndexOf(headCloseTag, StringComparison.OrdinalIgnoreCase);
            if (headCloseIndex < 0)
            {
                // No <head> block found; nothing to do
                return html;
            }

            // Only search for existing links in the head section
            var headContent = html[..headCloseIndex];

            // Filter out requests that already exist (matching on href value)
            var requestsToInsert = _resourceRequests
                .Where(req =>
                {
                    if (string.IsNullOrWhiteSpace(req.ResourceValue))
                        return false;

                    // Simple duplicate check: does head content already contain a link with this href?
                    var hrefPattern = $"href=\"{req.ResourceValue}\"";
                    return headContent.IndexOf(hrefPattern, StringComparison.OrdinalIgnoreCase) < 0;
                })
                .ToList();

            if (requestsToInsert.Count == 0)
                return html;

            // Find insertion point: after last <link> in <head>, else after last <meta>, else before </head>
            int searchLimit = headCloseIndex;
            int insertIndex;

            // Try last <link ...> before </head>
            insertIndex = html.LastIndexOf("<link", searchLimit, StringComparison.OrdinalIgnoreCase);
            if (insertIndex >= 0)
            {
                // Move to end of that line / tag
                var lineEnd = html.IndexOf('\n', insertIndex);
                if (lineEnd < 0)
                    lineEnd = html.IndexOf('>', insertIndex);

                insertIndex = lineEnd >= 0 ? lineEnd + 1 : searchLimit;
            }
            else
            {
                // Try last <meta ...> before </head>
                insertIndex = html.LastIndexOf("<meta", searchLimit, StringComparison.OrdinalIgnoreCase);
                if (insertIndex >= 0)
                {
                    var lineEnd = html.IndexOf('\n', insertIndex);
                    if (lineEnd < 0)
                        lineEnd = html.IndexOf('>', insertIndex);

                    insertIndex = lineEnd >= 0 ? lineEnd + 1 : searchLimit;
                }
                else
                {
                    // Fallback: just before </head>
                    insertIndex = headCloseIndex;
                }
            }

            // Build the new <link> tags only for missing resources
            var sb = new StringBuilder();
            foreach (var req in requestsToInsert)
            {
                sb.Append("  <link rel=\"")
                  .Append(req.RelationshipType)
                  .Append("\" href=\"")
                  .Append(req.ResourceValue)
                  .AppendLine("\" />");
            }

            var injection = sb.ToString();

            // Insert and return
            return html.Insert(insertIndex, injection);
        }

    }
}