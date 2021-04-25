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
        public FormTemplate(FormModel model, IApplicationEventDispatcher eventDispatcher)
        {
            Model = model;
            eventDispatcher.Publish(AngularImportDependencyRequiredEvent.EventId, new Dictionary<string, string>()
            {
                { AngularImportDependencyRequiredEvent.ModuleId, Model.Module.Id},
                { AngularImportDependencyRequiredEvent.Dependency, "ReactiveFormsModule"},
                { AngularImportDependencyRequiredEvent.Import, "import { ReactiveFormsModule } from '@angular/forms';"}
            });
            eventDispatcher.Publish(AngularImportDependencyRequiredEvent.EventId, new Dictionary<string, string>()
            {
                { AngularImportDependencyRequiredEvent.ModuleId, Model.Module.Id},
                { AngularImportDependencyRequiredEvent.Dependency, "FormsModule"},
                { AngularImportDependencyRequiredEvent.Import, "import { FormsModule } from '@angular/forms';"}
            });
            if (Model.FormFields.Any(x => x.TypeReference.Element.Name == "Datepicker"))
            {
                eventDispatcher.Publish(AngularImportDependencyRequiredEvent.EventId, new Dictionary<string, string>()
                {
                    { AngularImportDependencyRequiredEvent.ModuleId, Model.Module.Id},
                    { AngularImportDependencyRequiredEvent.Dependency, "BsDatepickerModule.forRoot()"},
                    { AngularImportDependencyRequiredEvent.Import, "import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';"}
                });
            }
            if (Model.FormFields.Any(x => x.TypeReference.Element.Name == "Select"))
            {
                eventDispatcher.Publish(AngularImportDependencyRequiredEvent.EventId, new Dictionary<string, string>()
                {
                    { AngularImportDependencyRequiredEvent.ModuleId, Model.Module.Id},
                    { AngularImportDependencyRequiredEvent.Dependency, "NgxSelectModule"},
                    { AngularImportDependencyRequiredEvent.Import, "import { NgxSelectModule } from 'ngx-select-ex';"}
                });
            }
        }

        public FormModel Model { get; }

        private string GetSelectItemsModel(FormFieldModel field)
        {
            return field.GetSelectControlOptions()?.OptionsSource().Name ?? "[]";
        }

        private string GetSelectValueField(FormFieldModel field)
        {
            return field.GetSelectControlOptions()?.OptionValueField() ?? "id";
        }

        private string GetSelectTextField(FormFieldModel field)
        {
            return field.GetSelectControlOptions()?.OptionTextField() ?? "description";
        }
    }
}
