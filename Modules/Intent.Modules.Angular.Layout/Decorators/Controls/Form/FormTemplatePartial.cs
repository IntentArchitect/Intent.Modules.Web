using System.Collections.Generic;
using System.Linq;
using Intent.Angular.Layout.Api;
using Intent.Eventing;
using Intent.Modules.Angular.Templates;
using Intent.Modules.Angular.Templates.Component;
using Intent.Modules.Angular.Templates.Component.Controls;
using Intent.Modules.Angular.Templates.Core.AngularDotJsonFile;
using Intent.Modules.Angular.Templates.Core.Index;
using Intent.Modules.Angular.Templates.Core.StylesDotScssFile;
using Intent.Modules.Common;
using Intent.Modules.Common.TypeScript.Templates;

namespace Intent.Modules.Angular.Layout.Decorators.Controls.Form
{
    public partial class FormTemplate : IControl
    {
        public FormTemplate(FormModel model, ControlWriter controlWriter, IApplicationEventDispatcher eventDispatcher)
        {
            Model = model;
            ControlWriter = controlWriter;
            eventDispatcher.Publish(new AngularImportDependencyRequiredEvent(
                moduleId: Model.Module.Id, 
                dependency: "ReactiveFormsModule", 
                import: "import { ReactiveFormsModule } from '@angular/forms';"));
            eventDispatcher.Publish(new AngularImportDependencyRequiredEvent(
                moduleId: Model.Module.Id, 
                dependency: "FormsModule", 
                import: "import { FormsModule } from '@angular/forms';"));
            if (Model.FormFields.Any(x => x.TypeReference.Element.Name == "Datepicker"))
            {
                eventDispatcher.Publish(new AngularImportDependencyRequiredEvent(
                    moduleId: Model.Module.Id,
                    dependency: "BsDatepickerModule.forRoot()",
                    import: "import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';"));
            }
            if (Model.FormFields.Any(x => x.TypeReference.Element.Name is "Select" or "Multi-Select"))
            {
                EnsureAngularMaterialInstalled(eventDispatcher);

                eventDispatcher.Publish(new AngularImportDependencyRequiredEvent(
                    moduleId: Model.Module.Id,
                    dependency: "MatSelectModule",
                    import: "import { MatSelectModule } from '@angular/material/select';"));
            }
        }

        private void EnsureAngularMaterialInstalled(IApplicationEventDispatcher eventDispatcher)
        {
            eventDispatcher.Publish(new NpmPackageDependency("@angular/cdk", "^16.2.2"));
            eventDispatcher.Publish(new NpmPackageDependency("@angular/material", "^16.2.2"));
            eventDispatcher.Publish(new StyleRequest("html, body { height: 100%; }"));
            eventDispatcher.Publish(new StyleRequest("body { margin: 0; font-family: Roboto, \"Helvetica Neue\", sans-serif; }"));
            eventDispatcher.Publish(new AngularDotJsonStyleRequired("@angular/material/prebuilt-themes/indigo-pink.css"));
            eventDispatcher.Publish(new IndexHeaderLinkRequired(relationship: "preconnect", href: "https://fonts.gstatic.com"));
            eventDispatcher.Publish(new IndexHeaderLinkRequired(relationship: "stylesheet", href: "https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500&display=swap"));
            eventDispatcher.Publish(new IndexHeaderLinkRequired(relationship: "stylesheet", href: "https://fonts.googleapis.com/icon?family=Material+Icons"));

            // TODO JL: Validate this is working correctly
            eventDispatcher.Publish(new AngularImportDependencyRequiredEvent(
                moduleId: Model.Module.Id,
                dependency: "BrowserAnimationsModule",
                import: "import { BrowserAnimationsModule } from \"@angular/platform-browser/animations\""));
        }

        public FormModel Model { get; }
        public ControlWriter ControlWriter { get; }

        private static string GetSelectItemsModel(FormFieldModel field)
        {
            return field.GetSelectControlSettings()?.OptionsSource().Name ?? "[]";
        }

        private static string GetSelectValueField(FormFieldModel field)
        {
            return field.GetSelectControlSettings()?.OptionValueField() ?? "id";
        }

        private static string GetSelectTextField(FormFieldModel field)
        {
            return field.GetSelectControlSettings()?.OptionTextField() ?? "description";
        }
    }
}
