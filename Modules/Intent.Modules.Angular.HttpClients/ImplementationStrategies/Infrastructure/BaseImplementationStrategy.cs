using Intent.Metadata.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Intent.Modules.Angular.HttpClients.ImplementationStrategies.Infrastructure;

internal class BaseImplementationStrategy
{
    internal Dictionary<string, string> SourceReplacements = [];
    internal Dictionary<string, string?> TargetReplacements = [];

    internal virtual void SetSourceReplacement(IMetadataModel type, string replacement)
    {
        SourceReplacements.Remove(type.Id);
        SourceReplacements.Add(type.Id, replacement);
    }

    internal virtual void SetSourceReplacement(IMetadataModel[] types, string replacement)
    {
        SourceReplacements.Remove(string.Join(".", types.Select(t => t.Id)));
        SourceReplacements.Add(string.Join(".", types.Select(t => t.Id)), replacement);
    }

    internal virtual void SetTargetReplacement(IMetadataModel type, string replacement)
    {
        TargetReplacements.Remove(type.Id);
        TargetReplacements.Add(type.Id, replacement);
    }

    internal virtual void SetTargetReplacement(IMetadataModel[] types, string replacement)
    {
        TargetReplacements.Remove(string.Join(".", types.Select(t => t.Id)));
        TargetReplacements.Add(string.Join(".", types.Select(t => t.Id)), replacement);
    }

    internal virtual void SetTargetReplacement(string[] types, string replacement)
    {
        TargetReplacements.Remove(string.Join(".", types.Select(t => t ?? "*")));
        TargetReplacements.Add(string.Join(".", types.Select(t => t ?? "*")), replacement);
    }

    internal virtual void SetTargetReplacement(string type, string replacement)
    {
        TargetReplacements.Remove(type);
        TargetReplacements.Add(type, replacement);
    }

    internal string ReplaceKeysInStrings(string inputString, Dictionary<string, string> replacements)
    {
        string updated = inputString;

        foreach (var kvp in replacements)
        {
            var value = replacements[kvp.Key];

            // Build regex pattern for wildcard support
            string pattern = Regex.Escape(kvp.Key).Replace("\\*", "[^.]+");

            // Match only full segments: e.g., a.b.c but not xab.c
            pattern = $@"(?<=^|\.|\/){pattern}(?=\.|\/|$)";

            var regex = new Regex(pattern);

            if (regex.IsMatch(updated))
            {
                updated = regex.Replace(updated, value);
            }
        }

        return updated;
    }
}
