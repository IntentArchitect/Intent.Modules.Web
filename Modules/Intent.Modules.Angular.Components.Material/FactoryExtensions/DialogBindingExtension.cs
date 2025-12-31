using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using Intent.Engine;
using Intent.Modelers.UI.Api;
using Intent.Modules.Angular.Templates.Component.ComponentTypeScript;
using Intent.Modules.Common;
using Intent.Modules.Common.Plugins;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.TypeScript.Builder;
using Intent.Plugins.FactoryExtensions;
using Intent.RoslynWeaver.Attributes;
using static Intent.Modelers.UI.Api.ComponentModelStereotypeExtensions;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.FactoryExtension", Version = "1.0")]

namespace Intent.Modules.Angular.Components.Material.FactoryExtensions
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class DialogBindingExtension : FactoryExtensionBase
    {
        public override string Id => "Intent.Angular.Components.Material.DialogBindingExtension";

        [IntentManaged(Mode.Ignore)]
        public override int Order => 0;

        protected override void OnAfterTemplateRegistrations(IApplication application)
        {
            // componenents which are NOT pages 
            var componentTemplates = application.FindTemplateInstances<ComponentTypeScriptTemplate>(ComponentTypeScriptTemplate.TemplateId)
                .Where(t => !t.Model.HasPage());

            // this is done here, as each component library handles passing data to a dialog differently
            foreach (var ct in componentTemplates)
            {
                var addDialogDataExtraction = !ct.Model.HasPage() && ct.Model.Properties.Any(p => p.HasRouteParameter() || p.HasBindable());

                if (addDialogDataExtraction)
                {
                    ct.TypescriptFile.AfterBuild(file =>
                    {
                        if (!file.Template.TryGetModel<ComponentModel>(out var model))
                        {
                            return;
                        }

                        var ctor = file.Classes.First(c => c.Constructors.Count != 0).Constructors.First();
                        if (!ctor.Parameters.Any(p => p.Name == "data"))
                        {
                            file.AddImport("Inject", "@angular/core");
                            file.AddImport("MatDialogModule", "@angular/material/dialog");
                            file.AddImport("MatDialogRef", "@angular/material/dialog");
                            file.AddImport("MAT_DIALOG_DATA", "@angular/material/dialog");

                            var paramNameTypes = model.Properties.Where(p => p.HasBindable() || p.HasRouteParameter())
                                .Select(p => $"{p.Name.ToCamelCase(true)}: {file.Template.GetTypeName(p.TypeReference)}{(p.TypeReference.IsNullable ? " | null" : "")}");

                            ctor.AddParameter("data", $"{{ {string.Join(", ", paramNameTypes)} }}", param =>
                            {
                                param.WithFieldAssignment();
                                param.AddDecorator("Inject", decor =>
                                {
                                    decor.AddStatement("MAT_DIALOG_DATA");
                                });
                            });
                        }

                        if (!ctor.Parameters.Any(p => p.Name == "dialogRef"))
                        {
                            ctor.AddParameter("dialogRef", $"MatDialogRef<{ctor.Class.Name}>", param =>
                            {
                                param.WithPrivateFieldAssignment();
                            });
                        }
                        
                        var initMethod = ctor.Class.Methods.First(m => m.Name == "ngOnInit");
                        if (initMethod != null)
                        {
                            foreach (var prop in model.Properties.Where(p => p.HasBindable() || p.HasRouteParameter()))
                            {
                                if (!prop.TypeReference.IsNullable)
                                {
                                    initMethod.AddStatement(@$"if(!this.data?.{prop.Name.ToCamelCase(true)}) {{
      throw new Error(""Expected '{prop.Name.ToCamelCase(true)}' not supplied"");
    }}");
                                }

                                initMethod.AddStatement($"this.{prop.Name.ToCamelCase(true)} = this.data.{prop.Name.ToCamelCase(true)}");
                            }
                        }
                    }, 2000);
                }
            }
        }


    }
}