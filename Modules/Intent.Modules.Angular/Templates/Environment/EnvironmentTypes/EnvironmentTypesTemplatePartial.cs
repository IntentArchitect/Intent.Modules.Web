using Intent.Engine;
using Intent.Modules.Common;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.TypeScript.Builder;
using Intent.Modules.Common.TypeScript.Events;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.TypeScript.Templates.TypescriptTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.Environment.EnvironmentTypes
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public partial class EnvironmentTypesTemplate : TypeScriptTemplateBase<object>, ITypescriptFileBuilderTemplate
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.Environment.EnvironmentTypes";

        private List<EnvironmentRegistrationRequestEvent> _registrationEvents = new List<EnvironmentRegistrationRequestEvent>();

        [IntentManaged(Mode.Merge, Signature = Mode.Ignore)]
        public EnvironmentTypesTemplate(IOutputTarget outputTarget, object model = null) : base(TemplateId, outputTarget, model)
        {
            TypescriptFile = new TypescriptFile(this.GetFolderPath(), this)
                .AddInterface("AppEnvironment", @interface =>
                {
                    @interface.WithComments("// interface for all environment configuration");
                    @interface.WithComments("//@IntentMerge()");
                    @interface.Export();
                    
                });

            TypescriptFile.WithComments("//@IntentMerge()").AfterBuild(file =>
            {
                AddRegistrationRequestTypes(file);
            });

            ExecutionContext.EventDispatcher.Subscribe<EnvironmentRegistrationRequestEvent>(HandleEnvironmentRegistrationRequestEvent);
        }

        private void AddRegistrationRequestTypes(TypescriptFile file)
        {
            var mergedEvents = _registrationEvents.GetMergedEvents();
            var appEnvironmentInterface = file.Interfaces.Single(i => i.Name == "AppEnvironment");

            foreach (var @event in mergedEvents.Where(e => e.Kind == EnvironmentTypeKind.Interface))
            {
                file.AddInterface(@event.TypeName, @interface =>
                {
                    @interface.Export();
                    if (@event.Extends is not null && @event.Extends.Any())
                    {
                        foreach (var extendsInterface in @event.Extends)
                        {
                            @interface.ExtendsInterface(extendsInterface);
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(@event.Comment))
                    {
                        @interface.WithComments($"// {@event.Comment}");
                    }

                    foreach (var @field in @event.Fields)
                    {
                        var fieldName = @field.IsOptional ? $"{field.Name}?" : field.Name;
                        @interface.AddField(fieldName, @field.Type);
                    }
                });

                if(!string.IsNullOrWhiteSpace(@event.EnvironmentName) && !appEnvironmentInterface.Fields.Any(f => f.Name == @event.EnvironmentName))
                {
                    appEnvironmentInterface.AddField(@event.EnvironmentName, @event.TypeName);
                }
            }
        }

        [IntentManaged(Mode.Fully)]
        public TypescriptFile TypescriptFile { get; }

        [IntentManaged(Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return TypescriptFile.GetConfig($"EnvironmentTypes");
        }

        [IntentManaged(Mode.Fully)]
        public override string TransformText()
        {
            return TypescriptFile.ToString();
        }

        public void HandleEnvironmentRegistrationRequestEvent(EnvironmentRegistrationRequestEvent @event)
        {
            _registrationEvents.Add(@event);
        }

        
    }
}