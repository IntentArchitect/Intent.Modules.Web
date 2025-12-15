using Intent.Modules.Common.TypeScript.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intent.Modules.Angular.Templates.Environment;

public static class EnvironmentExtensions
{
    public static IList<EnvironmentRegistrationRequestEvent> GetMergedEvents(this List<EnvironmentRegistrationRequestEvent> events)
    {
        var result = new List<EnvironmentRegistrationRequestEvent>();

        // 1) Merge all Interface kinds that have a TypeName
        var interfaceGroups = events
            .Where(e => e.Kind == EnvironmentTypeKind.Interface && !string.IsNullOrWhiteSpace(e.TypeName))
            .GroupBy(e => e.TypeName!);

        foreach (var group in interfaceGroups)
        {
            var first = group.First();

            // Merge extends
            var extends = group
                .Where(e => e.Extends != null)
                .SelectMany(e => e.Extends!)
                .Distinct()
                .ToList();

            // Merge fields (including nested fields)
            var allFields = group
                .Where(e => e.Fields != null)
                .SelectMany(e => e.Fields!)
                .ToList();

            var mergedFields = MergeFields(allFields);

            // Last non-null DefaultValue wins
            var defaultValue = group
                .Select(e => e.DefaultValue)
                .LastOrDefault(v => !string.IsNullOrWhiteSpace(v));

            result.Add(new EnvironmentRegistrationRequestEvent
            {
                TypeName = first.TypeName,
                Kind = EnvironmentTypeKind.Interface,
                SimpleType = null, // not used for interfaces
                Fields = mergedFields.Count == 0 ? null : mergedFields,
                Extends = extends.Count == 0 ? null : extends,
                DefaultValue = defaultValue,
                Comment = first.Comment,
                EnvironmentName = first.EnvironmentName
            });
        }

        // 2) Everything else (Simple / TypeAlias / Interfaces without TypeName)
        //    is added as-is (no merging).
        var nonMerged = events.Where(e =>
            !(e.Kind == EnvironmentTypeKind.Interface && !string.IsNullOrWhiteSpace(e.TypeName)));

        result.AddRange(nonMerged);

        return result;
    }

    private static IList<EnvironmentFieldDescriptor> MergeFields(IList<EnvironmentFieldDescriptor> fields)
    {
        if (fields == null || fields.Count == 0)
        {
            return [];
        }

        var byName = fields.GroupBy(f => f.Name);
        var merged = new List<EnvironmentFieldDescriptor>();

        foreach (var group in byName)
        {
            var name = group.Key;
            var descriptors = group.ToList();
            var first = descriptors.First();

            // Ensure all types for this field name are compatible
            var distinctTypes = descriptors.Select(d => d.Type).Distinct().ToList();
            if (distinctTypes.Count > 1)
            {
                throw new InvalidOperationException(
                    $"Conflicting types for field '{name}': {string.Join(", ", distinctTypes)}");
            }

            // Optional if any descriptor marks it optional
            var isOptional = descriptors.Any(d => d.IsOptional);

            // Default: last non-null wins
            var defaultValue = descriptors
                .Select(d => d.DefaultValue)
                .LastOrDefault(v => !string.IsNullOrWhiteSpace(v));

            merged.Add(new EnvironmentFieldDescriptor
            {
                Name = name,
                Type = first.Type,
                IsOptional = isOptional,
                DefaultValue = defaultValue
            });
        }

        return [.. merged.OrderBy(f => f.Name)];
    }
}
