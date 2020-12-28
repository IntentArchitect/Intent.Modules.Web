using System.IO;
using System.Linq;
using Intent.Engine;
using Intent.Modules.Angular.Templates.Core.CoreModuleTemplate;
using Intent.Modules.Common;
using Intent.Modules.Common.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;
using Intent.Metadata.Models;
using System;
using System.Collections.Generic;
using Intent.Modules.Angular.Templates.Shared.IntentDecoratorsTemplate;
using Intent.Modules.Angular.Api;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.Modelers.WebClient.Api;
using Intent.Angular.Api;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.TypeScript.Templates.TypescriptTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.App.AppModuleTemplate
{
    [IntentManaged(Mode.Merge, Signature = Mode.Merge)]
    partial class AppModuleTemplate : TypeScriptTemplateBase<AngularWebAppModel>
    {
        private readonly ISet<string> _components = new HashSet<string>() { "AppComponent" };
        private readonly ISet<string> _providers = new HashSet<string>();
        private readonly ISet<string> _angularImports = new HashSet<string>();
        private readonly ISet<string> _imports = new HashSet<string>();

        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.App.AppModuleTemplate";

        public AppModuleTemplate(IOutputTarget project, AngularWebAppModel model) : base(TemplateId, project, model)
        {
            AddTemplateDependency(IntentDecoratorsTemplate.TemplateId);
            project.Application.EventDispatcher.Subscribe(AngularComponentCreatedEvent.EventId, @event =>
            {
                if (@event.GetValue(AngularComponentCreatedEvent.ModuleId) != ClassName)
                {
                    return;
                }

                _components.Add(GetTypeName(@event.GetValue(AngularComponentCreatedEvent.ModelId)));
            });

            project.Application.EventDispatcher.Subscribe<AngularServiceProxyCreatedEvent>(@event =>
           {
               if (@event.ModuleId != ClassName)
               {
                   return;
               }

               var template = GetTypeName(@event.ModelId);
               _providers.Add(template);
           });

            project.Application.EventDispatcher.Subscribe(AngularImportDependencyRequiredEvent.EventId, @event =>
            {
                if (@event.GetValue(AngularImportDependencyRequiredEvent.ModuleId) != ClassName)
                {
                    return;
                }

                _angularImports.Add(@event.GetValue(AngularImportDependencyRequiredEvent.Dependency));
                _imports.Add(@event.GetValue(AngularImportDependencyRequiredEvent.Import));
            });
        }

        public string AppRoutingModuleClassName => GetTypeName(AppRoutingModuleTemplate.AppRoutingModuleTemplate.TemplateId);
        public string CoreModule => GetTypeName(CoreModuleTemplate.TemplateId);

        public string GetImports()
        {
            if (!_imports.Any())
            {
                return "";
            }
            return $"{System.Environment.NewLine}" + string.Join($"{System.Environment.NewLine}", _imports);
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
            if (!_angularImports.Any())
            {
                return "";
            }
            return $",{System.Environment.NewLine}    " + string.Join($",    {System.Environment.NewLine}    ", _angularImports);
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new TypeScriptFileConfig(
                overwriteBehaviour: OverwriteBehaviour.Always,
                fileName: $"app.module",
                relativeLocation: $"",
                className: "AppModule"
            );
        }
    }
}