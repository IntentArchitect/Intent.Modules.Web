using Intent.Exceptions;
using Intent.Metadata.Models;
using Intent.Modelers.UI.Api;
using Intent.Modelers.UI.Core.Api;
using Intent.Modules.Common;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.TypeResolution;
using Intent.Modules.Common.Typescript.Mapping;
using Intent.Modules.Common.TypeScript.Builder;
using Intent.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Intent.Modules.Angular.Api;

public static class TypescriptFileExtensions
{
    public static List<IElement> GetProcessingActions(this ComponentOperationModel operation)
    {
        var processingActions = operation.InternalElement.ChildElements.ToList();
        foreach (var associationEnd in operation.InternalElement.AssociatedElements.OrderBy(x => x.Order))
        {
            processingActions.Insert(associationEnd.Order, associationEnd);
        }

        return processingActions;
    }

    public static string InjectServiceProperty(this TypescriptClass @class, IntentTemplateBase template, string fullyQualifiedTypeName, string? fieldName = null)
    {
        var type = fullyQualifiedTypeName;
        fieldName ??= type.Length > 2 && type[0] == 'I' && char.IsUpper(type[1]) ? type[1..] : type; // remove 'I' prefix if necessary.

        var ctor = @class.Constructors.FirstOrDefault();

        if(ctor is null)
        {
            return string.Empty;
        }

        if (ctor.Parameters.Any(p => p.Type == type))
        {
            return fieldName.ToCamelCase(true);
        }
        ctor.AddParameter(
            type: type,
            name: fieldName.ToCamelCase(true) ?? type, 
            configure: config =>
            {
                config.WithPrivateReadonlyFieldAssignment();
            });

        return fieldName.ToCamelCase(true);
    }

    public static void InjectServiceUtilityFields(this TypescriptClass @class, string methodName)
    {
        // add the field to handle the errors
        if (!@class.Fields.Any(f => f.Name == "serviceErrors"))
        {
            @class.AddField("serviceErrors", field =>
            {
                field.WithDefaultValue(@$"{{
    {methodName}Error: null as string | null
  }}");
            });
        }
        else
        {
            // TODO: Make this entire way of doing this better and not using strings by Typescript builder type
            var serviceErrorsField = @class.Fields.Single(f => f.Name == "serviceErrors");

            var defaultValue = serviceErrorsField.Value ?? string.Empty;
            var propertyName = $"{methodName}Error";

            // Don't add it twice
            if (!defaultValue.Contains(propertyName, StringComparison.Ordinal))
            {
                // Find the last '}' in the object literal
                var insertPos = defaultValue.LastIndexOf('}');
                if (insertPos >= 0)
                {
                    var before = defaultValue[..insertPos];

                    // Trim trailing whitespace/newlines so the comma sits on the same line
                    var trimmedBefore = before.TrimEnd();

                    // Decide if we need a comma before the new line
                    var needsComma = !trimmedBefore.EndsWith('{');
                    var comma = needsComma ? "," : string.Empty;

                    // Rebuild the whole initializer with a clean closing line
                    var newDefaultValue = $"{trimmedBefore}{comma}\n    {propertyName}: null as string | null\n  }}";

                    serviceErrorsField.WithDefaultValue(newDefaultValue);
                }
            }
        }

        // add the field to handle the errors
        if (!@class.Fields.Any(f => f.Name == "isLoading"))
        {
            @class.AddField("isLoading", @field => field.WithValue("false"));
        }
    }

    public static void InjectNullGuardChecks(this TypescriptMethod @method, ComponentModel model, IElementToElementMapping invocationMapping)
    {
        // get the properties on the model which are nullable
        var nullablePropertyElements = model.Properties.Where(p => p.TypeReference.IsNullable)
                                       .Select(p => p.TypeReference.ElementId)
                                       .ToList();

        // get all the mapping elements
        var distinctSourceElements = invocationMapping.MappedEnds
            .SelectMany(m => m.SourcePath)
            .Select(sp => sp.Element?.TypeReference?.ElementId)
            .Where(s => s is not null)
            .Distinct();

        // get the intersections
        var qualifyingItems = nullablePropertyElements.Intersect(distinctSourceElements);

        if (!qualifyingItems.Any())
        {
            return;
        }

        // add the null guard
        foreach(var item in qualifyingItems )
        {
            var property = model.Properties.First(m => m.TypeReference?.ElementId == item);
            var propertyName = property.Name.ToCamelCase(true);

            method.Statements.Add($@"if(!this.{propertyName}) {{
      this.serviceErrors.{method.Name}Error = ""Property '{propertyName}' cannot be null"";
      this.isLoading = false;
      return;
    }}");

        }
    }

    public static IEnumerable<TypescriptStatement> GetCallServiceOperation(CallServiceOperationActionTargetEndModel serviceCall,
        TypescriptClassMappingManager mappingManager,
        string serviceName,
        TypescriptStatement invocation,
        string currentMethodName)
    {
        var result = new List<TypescriptStatement>();
        if (serviceCall.GetMapResponseMapping() != null)
        {
            var responseStaticElementId = "2f68b1a4-a523-4987-b3da-f35e6e8e146b";
            if (serviceCall.GetMapResponseMapping().MappedEnds.Count == 1 && serviceCall.GetMapResponseMapping().MappedEnds.Single().SourceElement.Id == responseStaticElementId)
            {
                var variableName = serviceCall.TypeReference.Element.TypeReference.IsCollection ? serviceCall.TypeReference.Element.TypeReference.Element.Name.Pluralize().ToCamelCase(true) : serviceCall.TypeReference.Element.TypeReference.Element.Name.ToCamelCase(true);
                mappingManager.SetFromReplacement(new StaticMetadata(responseStaticElementId), variableName);

                var responseObject = serviceCall.GetMapResponseMapping().MappedEnds.Any() ? mappingManager.GenerateTargetStatementForMapping(serviceCall.GetMapResponseMapping(), serviceCall.GetMapResponseMapping().MappedEnds.Single()) : string.Empty;
                var responseText = responseObject.GetText("");

                result.Add(new TypescriptStatement(@$"this.{serviceName}.{invocation}
    .pipe(
        finalize(() => {{
          this.isLoading = false; 
        }})
     )
    .subscribe({{{(!string.IsNullOrWhiteSpace(responseText) ? $@"
      next: (data) => {{
        this.{responseObject} = data;
      }},": string.Empty)}
      {GetErrorBlock(currentMethodName)}
    }});"));

            }
            else
            {
                mappingManager.SetFromReplacement(new StaticMetadata(responseStaticElementId), "data");
                var responses = mappingManager.GenerateUpdateStatements(serviceCall.GetMapResponseMapping());

                var sb = new StringBuilder();
                sb.Append(@$"this.{serviceName}.{invocation}
    .pipe(
      finalize(() => {{
        this.isLoading = false; 
      }})
    ).subscribe({{");

                if(responses.Any())
                {
                    sb.AppendLine($@"
      next: (data) => {{");

                    foreach (var response in responses)
                    {
                        sb.AppendLine($"        {response.ToString()}");
                    }

                    sb.Append(@$"      }},");
                }

                sb.Append(@$"
      {GetErrorBlock(currentMethodName)}
    }});");

                result.Add(sb.ToString());
            }
        }
        else
        {
            result.Add(new TypescriptStatement(@$"this.{serviceName}.{invocation}
    .pipe(
        finalize(() => {{
          this.isLoading = false; 
        }})
    )
    .subscribe({{
      {GetErrorBlock(currentMethodName)}
    }});"));

        }
        return result;
    }

    public static void InjectVariableInitStatements(this TypescriptMethod method)
    {
        method.AddStatement($"this.serviceErrors.{method.Name}Error = null;");
        method.AddStatement($"this.isLoading = true;");
        method.AddStatement("");
    }

    private static string GetErrorBlock(string methodName)
    {
        return @$"error: (err) => {{
        const message = err?.error?.message || err.message || 'Unknown error';
        this.serviceErrors.{methodName}Error = `Failed to call service: ${{message}}`;

        console.error('Failed to call service:', err);
      }}";
    }
}

public record StaticMetadata(string id) : IMetadataModel
{
    public string Id { get; } = id;
}