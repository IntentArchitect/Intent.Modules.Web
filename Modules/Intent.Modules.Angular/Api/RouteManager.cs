using Intent.Modelers.UI.Api;
using Intent.Modules.Angular.Templates.Component.ComponentTypeScript;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.TypeScript.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Intent.Modules.Angular.Api;

public class RouteManager
{
    public RouteManager(string route, List<PropertyModel> parameters)
    {
        Route = route;
        Parameters = parameters;
    }

    public string Route { get; private set; }
    public List<PropertyModel> Parameters { get; private set; }

    public bool HasParameterExpression(string parameterName)
    {
        foreach (var expression in GetExpressions(Route))
        {
            var name = Regex.Split(expression, @"[:?]").First();
            if (parameterName.Equals(name, StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    public void ReplaceParameterExpression(string parameterName, string replaceWith)
    {
        foreach (var expression in GetExpressions(Route))
        {
            var name = Regex.Split(expression, @"[:?]").First();
            if (parameterName.Equals(name, StringComparison.InvariantCultureIgnoreCase))
            {
                Route = Route.Replace($"{{{expression}}}", replaceWith);
            }
        }
    }

    private List<string> GetExpressions(string str)
    {
        var result = new List<string>();
        while (str.IndexOf("{", StringComparison.Ordinal) != -1 && str.IndexOf("}", StringComparison.Ordinal) != -1)
        {
            var fromPos = str.IndexOf("{", StringComparison.Ordinal) + 1;
            var toPos = str.IndexOf("}", StringComparison.Ordinal);

            if (fromPos >= toPos)
            {
                throw new FormatException($"Route '{str}' is not formatted corrected. Brace pairing {{ }} is invalid.");
            }

            var expression = str[fromPos..toPos];
            result.Add(expression);
            str = str[(str.IndexOf("}", StringComparison.Ordinal) + 1)..];
        }

        return result;
    }

    public string GetRouteInvocationText(ComponentTypeScriptTemplate template, TypescriptMethod method)
    {
        foreach (var parameter in Parameters)
        {
            method.AddParameter(parameter.Name.ToCamelCase(true), template.GetTypeName(parameter.TypeReference), param =>
            {
                if (parameter.Value != null)
                {
                    param.WithDefaultValue(parameter.Value);
                }
            });

            if (HasParameterExpression(parameter.Name))
            {
                ReplaceParameterExpression(parameter.Name, $"{{{parameter.Name.ToCamelCase(true)}}}");
            }
        }

        var segments = GetRouteSegments(Route);
        for(int position = 0; position < segments.Length; position++)
        {
            var parameter = Parameters.FirstOrDefault(p => segments[position].Equals($"{{{p.Name}}}", StringComparison.InvariantCultureIgnoreCase));

            if (parameter is not null)
            {
                segments[position] = parameter.Name.ToCamelCase(true);
            }

            if (position == 0 && !segments[position].StartsWith("'/"))
            {
                // Ensure the first segment starts with a slash
                segments[position] = $"/{segments[position]}";
            }

            if(parameter is null)
            {
                segments[position] = $"'{segments[position]}'";
            }
        }

        // TODO cleanup
        if(Parameters.Any(p => p.HasQueryParameter()))
        {
            var sb = new StringBuilder();
            sb.AppendLine("{");
            sb.AppendLine("      queryParams: {");
            foreach(var queryParam in Parameters.Where(p => p.HasQueryParameter()))
            {
                sb.AppendLine($"        {queryParam.Name.ToCamelCase(true)}: {queryParam.Name.ToCamelCase(true)},");
            }

            sb.Remove(sb.Length - 3, 1); // Remove last comma

            sb.AppendLine("      }");
            sb.Append("    }");

            return $"this.router.navigate([{string.Join(", ", segments)}], {sb});";
        }

        return $"this.router.navigate([{string.Join(", ", segments)}]);";
    }

    private static string[] GetRouteSegments(string routeTemplate)
    {
        ArgumentNullException.ThrowIfNull(routeTemplate);

        // Trim whitespace, then split on '/'
        return routeTemplate
            .Trim()
            .Split('/', StringSplitOptions.RemoveEmptyEntries);
    }


}