using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Intent.Engine;
using Intent.Modelers.Services.Api;
using Intent.Modelers.UI.Api;
using Intent.Modules.Angular.HttpClients.ImplementationStrategies;
using Intent.Modules.Angular.HttpClients.ImplementationStrategies.Infrastructure;
using Intent.Modules.Angular.HttpClients.Templates.Helper;
using Intent.Modules.Common;
using Intent.Modules.Common.Plugins;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.TypeScript.Builder;
using Intent.Modules.Common.TypeScript.Events;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.Modules.Modelers.UI.Api;
using Intent.Plugins.FactoryExtensions;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;
using Intent.Utils;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.FactoryExtension", Version = "1.0")]

namespace Intent.Modules.Angular.HttpClients.FactoryExtensions
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class ServiceInvocationFactoryExtension : FactoryExtensionBase
    {
        public override string Id => "Intent.Angular.HttpClients.ServiceInvocationFactoryExtension";

        [IntentManaged(Mode.Ignore)]
        public override int Order => 0;

        /// <summary>
        /// This is an example override which would extend the
        /// <see cref="ExecutionLifeCycleSteps.BeforeTemplateExecution"/> phase of the Software Factory execution.
        /// See <see cref="FactoryExtensionBase"/> for all available overrides.
        /// </summary>
        /// <remarks>
        /// It is safe to update or delete this method.
        /// </remarks>
        protected override void OnBeforeTemplateExecution(IApplication application)
        {
            // Call Service Operation Action Target End associations
            var associations = application.MetadataManager.UserInterface(application).GetAssociationsOfType("fe5a5cd8-aabd-472f-8d42-f5c233e658dc");
            foreach (var association in associations)
            {
                // var componentOperation = association.TargetEnd.ParentElement;
                // var componentModel = new ComponentModel(componentOperation.ParentElement);
                // var template = application.FindTemplateInstance<ITypescriptFileBuilderTemplate>("Intent.Angular.Component.ComponentTypeScript", componentModel);

                // var implementation = new ServiceInvocationImplementation(application, association);
                // var interactionMetadata = ServiceInvocationImplementation.GetInteractionMetadata(association, application, template);

                // var strategies = new List<IImplementationStrategy>
                // {
                //     new NoParameterStrategy(interactionMetadata),
                //     new SingleValueParameterStrategy(interactionMetadata, association),
                //     new ObjectParameterStrategy(interactionMetadata, association)
                //     //new GetAllImplementationStrategy(application, association),
                //     //new GetSingleImplementationStrategy(application, association),
                //     //new OldGeneralImplementationStrategy(application, association)
                // };

                // var sourceStrategy = strategies.Where(s => s is IIsSourceStrategy && s.IsMatch()).ToArray();
                // var targetStrategy = strategies.Where(s => s is IIsTargetStrategy && s.IsMatch()).ToArray();

                // if(sourceStrategy.Length > 1)
                // {
                //     Logging.Log.Warning($@"Multiple source implementation strategies were found that can implement the service operation");
                //     Logging.Log.Debug($@"Source strategies: {string.Join(", ", sourceStrategy.Select(s => s.GetType().Name))}");
                // }

                // if (targetStrategy.Length > 1)
                // {
                //     Logging.Log.Warning($@"Multiple target implementation strategies were found that can implement the service operation");
                //     Logging.Log.Debug($@"Strategies: {string.Join(", ", sourceStrategy.Select(s => s.GetType().Name))}");
                // }

                // implementation.SetSourceStrategy(sourceStrategy.FirstOrDefault());
                // implementation.SetTargetStrategy(targetStrategy.FirstOrDefault());
                // implementation.BindToTemplate(template, interactionMetadata);

                // application.EventDispatcher.Publish(new ServiceConfigurationRequestEvent("provideHttpClient", "@angular/common/http"));
            }
        }
    }
}