using Intent.Engine;
using Intent.Modules.Common.TypeScript.Events;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Intent.Modules.Angular.Templates.Environment;

public abstract partial class EnvironmentTemplateBase : TypeScriptTemplateBase<object>
{
    private readonly IList<ConfigurationVariableRequiredEvent> _environmentVariables = [];

    protected EnvironmentTemplateBase(string templateId, IOutputTarget outputTarget, object model) : base(templateId, outputTarget, model)
    {
    }

    protected string GenerateFile()
    {
        if (TryGetExistingFileContent(out var content))
        {
            // Parse existing environment keys
            var existingKeys = ParseExistingEnvironmentKeys(content);

            // Add new keys from _environments that don't already exist
            var newKeys = new Dictionary<string, string>();
            foreach (var env in _environmentVariables)
            {
                if (!existingKeys.ContainsKey(env.Key))
                {
                    newKeys[env.Key] = env.DefaultValue;
                }
            }

            // If there are new keys to add, update the content
            if (newKeys.Count != 0)
            {
                content = UpdateEnvironmentContent(content, newKeys);
            }

            return content;
        }

        // No existing file, create new one with all environment variables
        return GenerateEnvironmentFile();
    }

    private static Dictionary<string, string> ParseExistingEnvironmentKeys(string content)
    {
        var keys = new Dictionary<string, string>();

        // Find the environment object declaration
        var envMatch = EnvironmentFileRegex().Match(content);

        if (!envMatch.Success)
        {
            return keys;
        }

        // Find the matching closing brace by counting braces (same logic as UpdateEnvironmentContent)
        int braceCount = 1;
        int startIndex = envMatch.Index + envMatch.Length;
        int closingBraceIndex = -1;

        for (int i = startIndex; i < content.Length; i++)
        {
            if (content[i] == '{')
            {
                braceCount++;
            }
            else if (content[i] == '}')
            {
                braceCount--;
                if (braceCount == 0)
                {
                    closingBraceIndex = i;
                    break;
                }
            }
        }

        if (closingBraceIndex == -1)
        {
            return keys;
        }

        var objectContent = content.Substring(startIndex, closingBraceIndex - startIndex);

        // Parse only top-level key-value pairs by tracking brace depth
        int depth = 0;
        int keyStart = -1;

        for (int i = 0; i < objectContent.Length; i++)
        {
            char c = objectContent[i];

            if (c == '{')
            {
                depth++;
            }
            else if (c == '}')
            {
                depth--;
            }
            else if (depth == 0 && char.IsLetter(c) && keyStart == -1)
            {
                // Start of a potential key at depth 0
                keyStart = i;
            }
            else if (depth == 0 && keyStart != -1 && c == ':')
            {
                // Found the colon for a top-level key
                var key = objectContent.Substring(keyStart, i - keyStart).Trim();
                keys[key] = string.Empty; // We don't care about the value
                keyStart = -1;
            }
            else if (depth == 0 && (c == ',' || c == '\n' || c == '\r'))
            {
                // Reset key tracking at separators
                keyStart = -1;
            }
        }

        return keys;
    }

    private static string UpdateEnvironmentContent(string content, Dictionary<string, string> newKeys)
    {
        // Find the environment object declaration
        var envMatch = EnvironmentFileRegex().Match(content);

        if (!envMatch.Success)
        {
            return content;
        }

        // Find the matching closing brace by counting braces
        int braceCount = 1;
        int startIndex = envMatch.Index + envMatch.Length;
        int closingBraceIndex = -1;

        for (int i = startIndex; i < content.Length; i++)
        {
            if (content[i] == '{')
            {
                braceCount++;
            }
            else if (content[i] == '}')
            {
                braceCount--;
                if (braceCount == 0)
                {
                    closingBraceIndex = i;
                    break;
                }
            }
        }

        if (closingBraceIndex == -1)
        {
            return content;
        }

        // Find the last non-whitespace character before the closing brace
        int lastCharIndex = closingBraceIndex - 1;
        while (lastCharIndex >= 0 && char.IsWhiteSpace(content[lastCharIndex]))
        {
            lastCharIndex--;
        }

        var needsComma = lastCharIndex >= 0 && content[lastCharIndex] != ',' && content[lastCharIndex] != '{';

        var afterClosingBrace = content.Substring(closingBraceIndex);

        // Build new entries
        var newEntries = string.Join(", ", newKeys.Select(kvp => $"{kvp.Key}: {FormatValue(kvp.Value)}"));

        // If we need a comma, add it right after the last character, then add newline and new entries
        if (needsComma)
        {
            // Insert comma after the last non-whitespace character
            var beforeLastChar = content.Substring(0, lastCharIndex + 1);
            return $"{beforeLastChar},\n  {newEntries}\n{afterClosingBrace}";
        }
        else
        {
            var beforeLastChar = content.Substring(0, lastCharIndex + 1);
            return $"{beforeLastChar}\n  {newEntries}\n{afterClosingBrace}";
        }
    }

    private string GenerateEnvironmentFile()
    {
        if (_environmentVariables.Count == 0)
        {
            return "export const environment = { };";
        }

        var entries = string.Join(",\n  ", _environmentVariables.Select(env => $"{env.Key}: {FormatValue(env.DefaultValue)}"));
        return $@"export const environment = {{
    {entries}
}};";
    }

    private static string FormatValue(string value)
    {
        // If value is already quoted or is an object/array, return as-is
        if (value.StartsWith("'") || value.StartsWith("\"") ||
            value.StartsWith("{") || value.StartsWith("[") ||
            bool.TryParse(value, out _) ||
            int.TryParse(value, out _) ||
            double.TryParse(value, out _))
        {
            return value;
        }

        // Otherwise, wrap in single quotes
        return $"'{value}'";
    }

    [GeneratedRegex(@"export\s+const\s+environment\s*=\s*\{", RegexOptions.Singleline)]
    private static partial Regex EnvironmentFileRegex();
}
