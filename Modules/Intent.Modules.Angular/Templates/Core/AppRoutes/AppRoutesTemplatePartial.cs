using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Intent.Engine;
using Intent.Modules.Angular.Templates.Component.ComponentTypeScript;
using Intent.Modules.Common;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.TypeScript.Builder;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.TypeScript.Templates.TypescriptTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.Core.AppRoutes
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public partial class AppRoutesTemplate : TypeScriptTemplateBase<object>, ITypescriptFileBuilderTemplate
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.Core.AppRoutes";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully, Body = Mode.Ignore)]
        public AppRoutesTemplate(IOutputTarget outputTarget, object model = null) : base(TemplateId, outputTarget, model)
        {
            ExecutionContext.EventDispatcher.Subscribe<ComponentCreatedEvent>(HandleComponenetCreatedEvent);

            TypescriptFile = new TypescriptFile(this.GetFolderPath(), this)
                .AddImport("Routes", "@angular/router")
                .AddVariable("routes", "Routes", @var =>
                {
                    var.Export().Const();
                    var.WithArrayValue();

                    //var.WithArrayValue(arr =>
                    //{
                    //    arr.AddObject(obj =>
                    //    {
                    //        obj.AddProperty("one", "1");
                    //        obj.AddProperty("two", "2");
                    //    });
                    //    arr.AddObject(obj =>
                    //    {
                    //        obj.AddProperty("three", "3");
                    //        obj.AddProperty("four", "4");
                    //    });
                    //});
                });
        }

        private void HandleComponenetCreatedEvent(ComponentCreatedEvent @event)
        {
            var routes = TypescriptFile.Variables.FirstOrDefault(v => v.Name == "routes");

            if (routes is null)
            {
                return;
            }

            // should definitely be an array
            if (routes.Value is TypescriptVariableArray array)
            {
                array.AddObject(obj =>
                {
                    var componentTemplate = this.GetTemplate<ComponentTypeScriptTemplate>(ComponentTypeScriptTemplate.TemplateId, @event.Model);

                    TypescriptFile.AddImport(@event.ComponentName, this.GetRelativePath(componentTemplate));

                    obj.WithFieldsSeparated(TypescriptCodeSeparatorType.None);
                    obj.AddField("path", $"'{ConvertToAngularRouteFormat(@event.Route)}'");
                    obj.AddField("component", @event.ComponentName);
                });
            }
        }

        private string ConvertToAngularRouteFormat(string route)
        {
            if (string.IsNullOrWhiteSpace(route))
            {
                return route;
            }

            var formattedRoute = route;
            if (formattedRoute.StartsWith('/'))
            {
                formattedRoute = formattedRoute[1..];
            }

            // Matches {word} or {number} inside braces
            return RouteConversionRegex().Replace(formattedRoute, match =>
            {
                var original = match.Groups[1].Value;
                var paramNameOnly = original.Split(":").First();
                var camelCase = paramNameOnly.ToCamelCase();
                return $":{camelCase}";
            });
        }

        [IntentManaged(Mode.Fully)]
        public TypescriptFile TypescriptFile { get; }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new TemplateFileConfig(
                fileName: $"app.routes",
                fileExtension: "ts"
            );
        }

        [IntentManaged(Mode.Fully)]
        public override string TransformText()
        {
            return TypescriptFile.ToString();
        }

        [GeneratedRegex(@"\{([^}]+)\}")]
        private static partial Regex RouteConversionRegex();
    }
}