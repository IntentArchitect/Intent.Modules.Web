using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Intent.Engine;
using Intent.Modelers.WebClient.Angular.Api;
using Intent.Modules.Angular.Shared;
using Intent.Modules.Angular.Templates.Component.AngularComponentTs;
using Intent.Modules.Angular.Templates.Core.CoreModule;
using Intent.Modules.Angular.Templates.Module.AngularRoutingModule;
using Intent.Modules.Common;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.Types.Api;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.TypeScript.Templates.TypescriptTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.Module.AngularModule
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class AngularModuleTemplate : TypeScriptTemplateBase<Intent.Modelers.WebClient.Angular.Api.ModuleModel>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.Module.AngularModule";
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

            ExecutionContext.EventDispatcher.SubscribeToAngularServiceProxyCreatedEvent((templateId, modelId, moduleId) =>
            {
                if (moduleId != Model.Id)
                {
                    return;
                }

                var templateClassName = GetTypeName(templateId, modelId);
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

        public string GetComponents()
        {
            if (!_components.Any())
            {
                return "";
            }

            return $"{System.Environment.NewLine}    " + string.Join($",{System.Environment.NewLine}    ", _components) + $"{System.Environment.NewLine}  ";
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
                _angularImports.Add(GetTypeName(AngularRoutingModuleTemplate.TemplateId, Model.Routing));
            }

            if (!_angularImports.Any())
            {
                return "";
            }
            return @"
    " + string.Join(@",
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

    public static class TypeScriptTemplateExtensions
    {
        public static string UseType<T>(this TypeScriptTemplateBase<T> template, string type, string location)
        {
            template.AddImport(type, location);
            return type;
        }
    }
}