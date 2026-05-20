using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using Intent.AI;
using Intent.Engine;
using Intent.Modelers.UI.Api;
using Intent.Modelers.UI.Core.Api;
using Intent.Modules.Angular.Templates.Component.ComponentHtml;
using Intent.Modules.Angular.Templates.Component.ComponentStyle;
using Intent.Modules.Angular.Templates.Component.ComponentTypeScript;
using Intent.Modules.Angular.Templates.Component.LayoutComponentHtml;
using Intent.Modules.Common;
using Intent.Modules.Common.Plugins;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.Plugins.FactoryExtensions;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.FactoryExtension", Version = "1.0")]

namespace Intent.Modules.Angular.FactoryExtensions
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class AITasksFactoryExtension : FactoryExtensionBase
    {
        public override string Id => "Intent.Angular.AITasksFactoryExtension";

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
            application.AITaskManager.RegisterTaskProvider(new TemplateAITaskProvider(application, GetTasks));
        }

        public IAITask[] GetTasks(IChange[] changes, IOutputFile[] outputFiles, IApplication application)
        {
            var tasks = new List<IAITask>();

            tasks.AddRange(GetAngularComponentImplementationTasks(changes, application));

            return [.. tasks];
        }

        private IEnumerable<IAITask> GetAngularComponentImplementationTasks(IChange[] changes, IApplication application)
        {
            var relevantChangeTypes = new ChangeType[] { ChangeType.Create };

            var handlerChanges = changes.Where(c =>
                c.Template?.Id == ComponentTypeScriptTemplate.TemplateId &&
                relevantChangeTypes.Contains(c.ChangeType) &&
                !c.IsIgnored);

            foreach (var change in handlerChanges)
            {
                if (!change.Template.TryCastTemplate<ITypescriptFileBuilderTemplate, ComponentModel>(out var template, out var model))
                {
                    continue;
                }

                yield return CreateGenerateComponentAITask(application, template, model, change);
            }
        }

        private IAITask CreateGenerateComponentAITask(IApplication application, ITypescriptFileBuilderTemplate template, ComponentModel model, IChange change)
        {
            var intention = new StringBuilder();
            var templateInstructionExtension = "";

            var (LayoutTemplates, Instructions) = AddLayoutComponentInstructions(template, model, change, intention);
            templateInstructionExtension += Instructions;

            AddNavigatesToContext(model, intention);
            var fromTemplates = AddNavigatesFromContext(model, intention, template);
            AddShowDialogContext(model, intention);

            // get the html and stylesheet for the component
            var componentHtmlTemplate = template.ExecutionContext.FindTemplateInstance(ComponentHtmlTemplate.TemplateId, model.Id);
            var componentScssTemplate = template.ExecutionContext.FindTemplateInstance(ComponentStyleTemplate.TemplateId, model.Id);

            var relatedTemplates = new[]
            {
                componentHtmlTemplate,
                componentScssTemplate
            }
            .Where(t => t is not null)
            .Concat(LayoutTemplates
                .Where(t => t is not null))
            .Concat(fromTemplates
                .Where(t => t is not null));

            return new TemplateAITask(template, [.. relatedTemplates])
            {
                Type = "Implement Angular Component",
                Title = $"Implement Angular Component: {model.Name}",
                Context = @$"""
                                    ## User has modeled the following intentions:
                                    {intention}
                                """,
                Instructions =
                        $"""Implement the {model.Name} Angular {templateInstructionExtension}component using the appropriate skill(s)."""
            };
        }

        // Add context about which other pages this component navigates to
        private static void AddNavigatesToContext(ComponentModel model, StringBuilder intention)
        {
            foreach (var navigation in model.InternalElement.AssociatedElements.Where(e => e.IsNavigationEndModel() && e.IsNavigable))
            {
                var navEndModel = navigation.AsNavigationEndModel();
                intention.AppendLine($"- This page navigates to the {navEndModel.TypeReference.Element.Name} component");
            }
        }

        private static IEnumerable<ITemplate> AddNavigatesFromContext(ComponentModel model, StringBuilder intention, ITypescriptFileBuilderTemplate template)
        {
            foreach (var navigation in model.InternalElement.AssociatedElements.Where(e => e.IsNavigationSourceEndModel() && e.IsNavigable))
            {
                var navEndModel = navigation.AsNavigationEndModel();
                intention.AppendLine($"- This page is navigates to from the {navEndModel.TypeReference.Element.Name} component. You MUST read the {navEndModel.TypeReference.Element.Name} component (HTML and ts file) and update it toensure  it has the required implementation (buttons, actions etc) to navigate to this component. If it does not contain the necessary implementation, you MUST update it accordingly.");

                if (navEndModel.TypeReference.Element.IsComponentModel())
                {
                    var fromModel = navEndModel.TypeReference.Element.AsComponentModel();
                    var fromHtmlPage = template.ExecutionContext.FindTemplateInstance(ComponentHtmlTemplate.TemplateId, fromModel.Id);

                    if (fromHtmlPage is not null)
                    {
                        yield return fromHtmlPage;
                    }
                }
            }
        }

        // Add context about which dialogs this component shows
        private static void AddShowDialogContext(ComponentModel model, StringBuilder intention)
        {
            // Show Dialog associations
            foreach (var operation in model.Operations.Where(o => o.InternalElement.AssociatedElements.Any(e => e.IsShowDialogTargetEndModel())))
            {
                foreach (var association in operation.InternalElement.AssociatedElements.Where(e => e.IsShowDialogTargetEndModel()))
                {
                    var dialogTargetEnd = association.AsShowDialogTargetEndModel();
                    intention.AppendLine($"- The {operation.Name} operation opens a dialog to show the {dialogTargetEnd.TypeReference.Element.Name} component");
                }
            }
        }

        // If this component is being navigated to from a menu item, we want to include the layout component in the task so that both are generated together and the user doesn't have to wait for two separate tasks.
        // We can identify this by looking for any navigation source associations where the other end is not navigable (i.e. it's a menu item).
        private static (List<ITemplate> LayoutTemplates, string Instructions) AddLayoutComponentInstructions(ITypescriptFileBuilderTemplate template, ComponentModel model, IChange change, StringBuilder intention)
        {
            List<ITemplate> menuTemplates = [];
            string templateInstructionExtension = "";

            foreach (var associationEnd in model.InternalElement.AssociatedElements.Where(a => a.IsNavigationSourceEndModel() && !a.IsNavigable))
            {
                intention.AppendLine($"- This pages is navigated to from a {associationEnd.TypeReference.Element.Name} menu item");

                // we only want to add the menu template when the item is being created
                if (change.ChangeType == ChangeType.Create)
                {
                    var layoutTemplate = template.ExecutionContext.FindTemplateInstance(LayoutComponentHtmlTemplate.TemplateId, associationEnd.TypeReference.Element.Id);
                    menuTemplates.Add(layoutTemplate);
                    templateInstructionExtension = $"as well as the {associationEnd.TypeReference.Element.Name} Layout ";
                }
            }

            return (LayoutTemplates: menuTemplates, Instructions: templateInstructionExtension);
        }
    }
}