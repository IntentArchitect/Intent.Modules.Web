using System.Collections.Generic;
using System.Linq;
using Intent.Angular.Layout.Api;
using Intent.Eventing;
using Intent.Modules.Angular.Templates;
using Intent.Modules.Angular.Templates.Component;
using Intent.Modules.Angular.Templates.Component.Controls;
using Intent.Modules.Common;

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
            if (Model.FormFields.Any(x => x.TypeReference.Element.Name == "Select" || x.TypeReference.Element.Name == "Multi-Select"))
            {
                //eventDispatcher.Publish(new AngularImportDependencyRequiredEvent(
                //    moduleId: Model.Module.Id,
                //    dependency: "NgxSelectModule",
                //    import: "import { NgxSelectModule } from 'ngx-select-ex';"));
                //eventDispatcher.Publish(new CliInstallationRequest("ngx-select-ex", "3", "--save"));
                eventDispatcher.Publish(new AngularImportDependencyRequiredEvent(
                    moduleId: Model.Module.Id,
                    dependency: "MatSelectModule",
                    import: "import { MatSelectModule } from '@angular/material/select';"));
                eventDispatcher.Publish(new CliInstallationRequest("@angular/material", "ng add @angular/material"));
            }
        }

        public FormModel Model { get; }
        public ControlWriter ControlWriter { get; }

        private string GetSelectItemsModel(FormFieldModel field)
        {
            return field.GetSelectControlSettings()?.OptionsSource().Name ?? "[]";
        }

        private string GetSelectValueField(FormFieldModel field)
        {
            return field.GetSelectControlSettings()?.OptionValueField() ?? "id";
        }

        private string GetSelectTextField(FormFieldModel field)
        {
            return field.GetSelectControlSettings()?.OptionTextField() ?? "description";
        }
    }
}
