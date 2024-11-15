using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Angular.Api;
using Intent.Engine;
using Intent.Metadata.Models;
using Intent.Modelers.WebClient.Angular.Api;
using Intent.Modules.Angular.Templates.Model.FormGroup;
using Intent.Modules.Angular.Templates.Model.Model;
using Intent.Modules.Angular.Templates.Module.AngularModule;
using Intent.Modules.Common;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.Types.Api;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.TypeScript.Templates.TypescriptTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.Component.AngularComponentTs
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class AngularComponentTsTemplate : TypeScriptTemplateBase<Intent.Modelers.WebClient.Angular.Api.ComponentModel, AngularComponentTsDecorator>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.Component.AngularComponentTs";
        private readonly List<(string Name, string Type, string DefaultValue)> _fields = new();

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public AngularComponentTsTemplate(IOutputTarget outputTarget, Intent.Modelers.WebClient.Angular.Api.ComponentModel model) : base(TemplateId, outputTarget, model)
        {
            AddTypeSource(ModelTemplate.TemplateId);
            AddTypeSource(FormGroupTemplate.TemplateId);
            AddTypeSource("Intent.Angular.ServiceProxies.Proxies.AngularDTO");
            AddTypeSource("Intent.Angular.ServiceProxies.Proxies.AngularServiceProxy");
            _injectedServices = Model.GetAngularComponentSettings().InjectServices()?.Where(x => x.SpecializationTypeId == AngularServiceModel.SpecializationTypeId)
                .Select(x => (x.Name.ToCamelCase(), this.UseType(x.Name, new AngularServiceModel(x).GetAngularServiceSettings().Location())))
                .ToList() ?? new List<(string, string)>();
            ExecutionContext.EventDispatcher.Subscribe<AngularComponentFieldRequiredEvent>(OnAngularComponentFieldRequiredEvent);
        }

        private void OnAngularComponentFieldRequiredEvent(AngularComponentFieldRequiredEvent @event)
        {
            var element = @event.Element;
            do
            {
                if (element.Id == Model.Id)
                {
                    break;
                }

                element = element.ParentElement;
                if (element == null)
                {
                    return;
                }
            } while (true);

            _fields.Add((@event.Name, @event.Type, @event.DefaultValue));

        }

        public string ComponentName
        {
            get
            {
                if (Model.Name.EndsWith("Component", StringComparison.InvariantCultureIgnoreCase))
                {
                    return Model.Name.Substring(0, Model.Name.Length - "Component".Length);
                }
                return Model.Name;
            }
        }

        public string ModuleName { get; private set; }

        public override void BeforeTemplateExecution()
        {
            //if (File.Exists(GetMetadata().GetFilePath()))
            //{
            //    return;
            //}

            // New Component:
            ExecutionContext.EventDispatcher.Publish(new AngularComponentCreatedEvent(modelId: Model.Id, moduleId: Model.Module.Id));
        }

        private IList<(string name, string type)> _injectedServices { get; }

        public void InjectService(string name, string type, string location = null)
        {
            _injectedServices.Add((name, location != null ? this.UseType(type, location) : type));
        }

        public string GetImports()
        {
            return "";
            //            if (!_injectedServices.Any())
            //            {
            //                return "";
            //            }
            //            return @"
            //" + string.Join(@"
            //", _injectedServices.Where(x => x.SpecializationType == AngularServiceModel.SpecializationType).Select(x => $"import {{ {x.Name} }} from '{new AngularServiceModel(x).GetAngularServiceSettings().Location()}'"));
        }

        public string GetConstructorParams()
        {
            var services = new List<string>();
            services.AddRange(_injectedServices.Select(x => $"private {x.name}: {x.type}"));
            if ((Model.NavigateToComponents().Any(x => x.IsNavigable) || Model.NavigateBackComponents().Any(x => x.IsNavigable)) && _injectedServices.All(x => x.type != "Router"))
            {
                services.Add($"private router: {this.UseType("Router", "@angular/router")}");
            }
            return string.Join(", ", services);
        }

        public string GetParameters(ComponentCommandModel command)
        {
            return string.Join(", ", command.Parameters.Select(x => x.Name.ToCamelCase() + (x.TypeReference.IsNullable ? "?" : "") + ": " + Types.Get(x.TypeReference, "{0}[]")));
        }

        public string GetReturnType(ComponentCommandModel command)
        {
            return command.ReturnType != null ? GetTypeName(command.ReturnType) : "void";
        }

        public string GetSelector()
        {
            if (!string.IsNullOrWhiteSpace(Model.GetAngularComponentSettings().Selector()))
            {
                return Model.GetAngularComponentSettings().Selector();
            }
            return $"app-{ComponentName.ToKebabCase()}";
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            var moduleTemplate = ExecutionContext.FindTemplateInstance<Module.AngularModule.AngularModuleTemplate>(Module.AngularModule.AngularModuleTemplate.TemplateId, Model.Module);
            return new TypeScriptFileConfig(
                overwriteBehaviour: OverwriteBehaviour.Always,
                fileName: $"{ComponentName.ToKebabCase()}.component",
                relativeLocation: $"{string.Join("/", Model.GetParentFolderNames())}/{(Model.GetAngularComponentSettings().InOwnFolder() ? $"/{ComponentName.ToKebabCase()}" : "")}",
                className: $"{ComponentName}Component"
            );
        }

        private string GetNavigationCommand(NavigationEndModel navigation)
        {
            var route = GetNavigationRoute(navigation);
            //var component = route?.RoutesToComponent == true
            //    ? new ComponentModel((IElement)route.TypeReference.Element)
            //    : null;
            if (route != null)
            {

                //var parameters = string.Join(", ", component.Inputs.Select(x => $"{x.Name}: {GetTypeName(x)}"));
                //var arguments = component.Inputs.Select(x => $"{x.Name}").ToList();
                return $@"
  {navigation.Name}({string.Join(", ", route.Name.Split('/').Where(IsRouteParameter).Select(x => x.Substring(1) + ": any"))}): void {{
    this.router.navigate([{string.Join(", ", route.Name.Split('/').Select(x => IsRouteParameter(x) ? x.Substring(1) : $"\"{x}\""))}]);
  }}";
            }
            return $@"
  {this.IntentIgnoreBodyDecorator()}
  {navigation.Name}(): void {{
    // custom navigation logic...
    // e.g. this.router.navigate([""your-route""]);
  }}";
        }

        private RouteModel GetNavigationRoute(NavigationEndModel navigation)
        {
            var module = new ComponentModel((IElement)navigation.Element).Module;
            var modulesToCheck = new[] { Model.Module, module }.Concat(module.GetParentFolders().OfType<ModuleModel>()).ToList();
            var routes = modulesToCheck.SelectMany(x => x.Routing?.Routes ?? new List<RouteModel>()).ToList();
            var route = routes.FirstOrDefault(x => x.TypeReference.Element.Id == navigation.Element.Id);
            return route;
        }

        private bool IsRouteParameter(string routeElement)
        {
            return routeElement.StartsWith(":");
        }
    }
}