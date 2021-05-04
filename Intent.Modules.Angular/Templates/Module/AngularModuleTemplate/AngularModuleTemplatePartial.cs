using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Intent.Engine;
using Intent.Metadata.Models;
using Intent.Modules.Angular.Api;
using Intent.Modules.Angular.Templates.Component.AngularComponentTsTemplate;
using Intent.Modules.Common;
using Intent.Modules.Common.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.Modelers.WebClient.Angular.Api;
using Intent.Modules.Angular.Templates.Core.CoreModuleTemplate;
using Intent.Modules.Common.Types.Api;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.TypeScript.Templates.TypescriptTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.Module.AngularModuleTemplate
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class AngularModuleTemplate : TypeScriptTemplateBase<Intent.Modelers.WebClient.Angular.Api.ModuleModel>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.Module.AngularModuleTemplate";
        private readonly ISet<string> _components = new HashSet<string>();
        private readonly ISet<string> _providers = new HashSet<string>();
        private readonly ISet<string> _angularImports = new HashSet<string>();
        private readonly ISet<string> _imports = new HashSet<string>();

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public AngularModuleTemplate(IOutputTarget outputTarget, Intent.Modelers.WebClient.Angular.Api.ModuleModel model) : base(TemplateId, outputTarget, model)
        {
            if (Model.IsRootModule())
            {
                _angularImports.Add(this.UseType("BrowserModule", "@angular/platform-browser"));
                _angularImports.Add(this.UseType("BrowserAnimationsModule", "@angular/platform-browser/animations"));
                _components.Add(this.UseType("AppComponent", "./app.component"));
            }
            else
            {
                _angularImports.Add(this.UseType("CommonModule", "@angular/common"));
            }

            ExecutionContext.EventDispatcher.Subscribe<AngularComponentCreatedEvent>(@event =>
                {
                    if (@event.ModuleId == ClassName)
                    {
                        _components.Add(GetTypeName(@event.ModelId));
                    }
                    else if (@event.ModuleId == Model.Id)
                    {
                        _components.Add(GetTypeName(AngularComponentTsTemplate.TemplateId, @event.ModelId));
                    }
                });

            ExecutionContext.EventDispatcher.Subscribe<AngularServiceProxyCreatedEvent>(@event =>
            {
                if (@event.ModuleId != Model.Id)
                {
                    return;
                }

                var templateClassName = GetTypeName(@event.TemplateId, @event.ModelId);
                _providers.Add(templateClassName);
            });

            ExecutionContext.EventDispatcher.Subscribe<AngularImportDependencyRequiredEvent>(@event =>
            {
                if (@event.ModuleId != Model.Id && @event.ModuleId != ClassName)
                {
                    return;
                }

                _angularImports.Add(@event.Dependency);
                _imports.Add(@event.Import);
            });

            ExecutionContext.EventDispatcher.Subscribe<AngularCustomProviderRequiredEvent>(@event =>
            {
                if (@event.ModuleId != Model.Id && @event.ModuleId != ClassName)
                {
                    return;
                }

                _providers.Add($"{{ provide: {@event.Provide}, useClass: {@event.UseClass}, multi: {@event.Multi.ToString().ToLower()} }}");
                _imports.Add(@event.Import);
            });
        }

        public string ModuleName => Model.GetModuleName();

        public string GetImports()
        {
            if (!_imports.Any())
            {
                return "";
            }
            return $"{System.Environment.NewLine}" + string.Join($"{System.Environment.NewLine}", _imports) + $"{System.Environment.NewLine}  ";
        }

        public bool HasComponents()
        {
            return _components.Any();
        }

        public string GetComponents()
        {
            if (!_components.Any())
            {
                return "";
            }
            return $"{System.Environment.NewLine}    " + string.Join($",{System.Environment.NewLine}    ", _components) + $"{System.Environment.NewLine}  ";
        }

        public bool HasProviders()
        {
            return _providers.Any();
        }

        public string GetProviders()
        {
            if (!_providers.Any())
            {
                return "";
            }
            return $"{System.Environment.NewLine}    " + string.Join($",{System.Environment.NewLine}    ", _providers) + $"{System.Environment.NewLine}  ";
        }

        public string GetAngularImports()
        {
            if (Model.IsRootModule())
            {
                _angularImports.Add(GetTypeName(CoreModuleTemplate.TemplateId));
            }

            foreach (var subModule in Model.Modules)
            {
                _angularImports.Add(GetTypeName(TemplateId, subModule));
            }

            if (Model.Routing != null)
            {
                _angularImports.Add(GetTypeName(AngularRoutingModuleTemplate.AngularRoutingModuleTemplate.TemplateId, Model.Routing));
            }

            if (!_angularImports.Any())
            {
                return "";
            }
            return @"
    " + string.Join($@",
    ", _angularImports) + @"
  ";
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new TypeScriptFileConfig(
                overwriteBehaviour: OverwriteBehaviour.Always,
                fileName: $"{ModuleName.ToKebabCase()}.module",
                relativeLocation: $"{string.Join("/", Model.GetParentFolderNames().Concat(new[] { ModuleName.ToKebabCase() }))}",
                className: "${ModuleName}Module");
        }
    }

    internal class AngularComponentInfo
    {
        public AngularComponentInfo(string componentName, string location)
        {
            ComponentName = componentName;
            Location = location;
        }

        public string ComponentName { get; set; }
        public string Location { get; set; }

        public override string ToString()
        {
            return $"Component: {ComponentName} - {Location}";
        }
    }

    internal class AngularProviderInfo
    {
        public AngularProviderInfo(string providerName, string location)
        {
            ProviderName = providerName;
            Location = location;
        }

        public string ProviderName { get; set; }
        public string Location { get; set; }

        public override string ToString()
        {
            return $"Provider: {ProviderName} - {Location}";
        }
    }

    public static class TypeScriptTemplateExtensions
    {
        public static string UseType<T>(this TypeScriptTemplateBase<T> template, string type, string location)
        {
            template.AddImport(type, location);
            return type;
        }
    }
}