using Intent.Engine;
using Intent.Metadata.Models;
using Intent.Modelers.UI.Api;
using Intent.Modules.Angular.Templates.Shared.IntentDecorators;
using Intent.Modules.Common;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.Types.Api;
using Intent.Modules.Common.TypeScript.Builder;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Text;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.TypeScript.Templates.TypescriptTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.Component.ComponentTypeScript
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public partial class ComponentTypeScriptTemplate : TypeScriptTemplateBase<Intent.Modelers.UI.Api.ComponentModel>, ITypescriptFileBuilderTemplate
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.Component.ComponentTypeScript";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully, Body = Mode.Ignore)]
        public ComponentTypeScriptTemplate(IOutputTarget outputTarget, Intent.Modelers.UI.Api.ComponentModel model) : base(TemplateId, outputTarget, model)
        {
            AddImport("Component", "@angular/core");
            AddImport("OnInit", "@angular/core");

            AddTypeSource("Intent.Angular.HttpClients.DtoContract");
            AddTypeSource("Intent.Angular.HttpClients.PagedResult");

            TypescriptFile = new TypescriptFile(this.GetFolderPath());

            AddModelDefinitions(model);

            TypescriptFile.AddClass($"{ComponentName}Component", @class =>
            {
                @class.Export();
                @class.AddDecorator("IntentMerge");
                @class.AddDecorator("Component", component =>
                {
                    var obj = new TypescriptVariableObject();
                    obj.AddField("selector", $"'app-{ComponentName.ToKebabCase().ToLower()}'");
                    obj.AddField("templateUrl", $"'{ComponentName.ToKebabCase()}.component.html'");
                    obj.AddField("styleUrl", $"'{ComponentName.ToKebabCase()}.component.scss'");

                    //TODO sort out indentation on final line of parameter
                    component.AddArgument(obj.GetText(TypescriptFile.Indentation));
                });

                if (model.Operations.Any(o => o.Name == "ngOnInit"))
                {
                    @class.ImplementsInterface("OnInit");
                }

                @class.AddConstructor(ctor =>
                {
                });

                foreach (var operation in model.Operations)
                {
                    if (operation.Name == "ngOnInit")
                    {
                        AddOnInitOperation(@class);
                        continue;
                    }

                    @class.AddMethod(operation.Name.ToCamelCase(true), operation.ReturnType is null ? "void" : GetTypeName(operation.ReturnType), method =>
                    {
                        foreach (var param in operation.Parameters)
                        {
                            method.AddParameter(param.Name, GetTypeName(param.TypeReference), p =>
                            {
                                p.WithDefaultValue(param.Value);
                            });
                        }
                    });
                }
            }).AfterBuild(file =>
            {
                foreach (var model in Model.Properties)
                {
                    var fieldType = GetTypeName(model.TypeReference);
                    if (model.TypeReference.IsNullable)
                    {
                        fieldType = $"{fieldType} | null";
                    }

                    var @class = file.Classes.FirstOrDefault(c => c.Name == $"{ComponentName}Component");
                    @class.AddField(model.Name.ToCamelCase(true), fieldType, @field =>
                    {
                        if ((model.HasRouteParameter() || model.HasQueryParameter()) && !model.TypeReference.IsNullable)
                        {
                            field.DefinitelyAssigned();
                        }

                        if (model.TypeReference.IsNullable && string.IsNullOrWhiteSpace(model.Value))
                        {
                            @field.WithValue("null");
                        }

                        if (!string.IsNullOrWhiteSpace(model.Value))
                        {
                            field.WithValue(model.Value);
                        }

                        if (!model.TypeReference.IsNullable && string.IsNullOrWhiteSpace(model.Value) && !field.IsDefinitelyAssigned)
                        {
                            field.WithDefaultValue(this, model.TypeReference);
                        }

                    });
                }

                var intentDecoratorTemplate = GetTemplate<TypeScriptTemplateBase<object>>(IntentDecoratorsTemplate.TemplateId);
                file.AddImport("IntentIgnoreBody", this.GetRelativePath(intentDecoratorTemplate));
                file.AddImport("IntentMerge", this.GetRelativePath(intentDecoratorTemplate));
            }, 1000); // this build needs to happen AFTER the "Intent.Angular.HttpClients.DtoContract" template, otherwise PagedResults cannot be resolved
        }

        private void AddModelDefinitions(ComponentModel model)
        {
            foreach (var modelDef in model.ModelDefinitions)
            {
                // the defacto standard should be interfaces, but they cannot contain operations
                // or contructors. So add class if required, but otherwise add interface
                if(modelDef.Constructors.Any() || modelDef.Operations.Any())
                {
                    AddClassModelDefinition(model, modelDef);
                    continue;
                }

                AddInterfaceModelDefinition(model, modelDef);
            }
        }

        private void AddInterfaceModelDefinition(ComponentModel model, ModelDefinitionModel modelDef)
        {
            TypescriptFile.AddInterface(modelDef.Name, @interface =>
            {
                foreach (var propertyModel in modelDef.Properties)
                {
                    var fieldType = GetTypeName(propertyModel.TypeReference);

                    if (propertyModel.TypeReference.IsNullable)
                    {
                        fieldType = $"{fieldType} | null";
                    }

                    @interface.AddField(propertyModel.Name.ToCamelCase(true), fieldType);
                }
            });
        }

        private void AddClassModelDefinition(ComponentModel model, ModelDefinitionModel modelDef)
        {
            TypescriptFile.AddClass(modelDef.Name, @class =>
            {
                foreach (var constructorModel in modelDef.Constructors)
                {
                    @class.AddConstructor(ctor =>
                    {
                        foreach (var parameter in constructorModel.Parameters)
                        {
                            ctor.AddParameter(parameter.Name.ToCamelCase(true), GetTypeName(parameter.TypeReference));
                        }
                    });
                }

                foreach (var propertyModel in modelDef.Properties)
                {
                    var fieldType = GetTypeName(propertyModel.TypeReference);

                    if (propertyModel.TypeReference.IsNullable)
                    {
                        fieldType = $"{fieldType} | null";
                    }

                    @class.AddField(propertyModel.Name.ToCamelCase(true), fieldType, @field =>
                    {
                        if (propertyModel.TypeReference.IsNullable && string.IsNullOrWhiteSpace(propertyModel.Value))
                        {
                            @field.WithValue("null");
                        }

                        if (!string.IsNullOrWhiteSpace(propertyModel.Value))
                        {
                            field.WithValue(propertyModel.Value);
                        }

                    });
                }

                foreach (var operationModel in modelDef.Operations)
                {
                    @class.AddMethod(operationModel.Name.ToCamelCase(true), GetTypeName(operationModel.TypeReference), method =>
                    {
                        foreach (var parameter in operationModel.Parameters)
                        {
                            method.AddParameter(parameter.Name.ToCamelCase(true), GetTypeName(parameter.TypeReference));
                        }
                    });
                }
            });
        }

        private void AddOnInitOperation(TypescriptClass @class)
        {
            @class.AddMethod("ngOnInit", "void", init =>
            {
                init.AddDecorator("IntentMerge");

                if (Model.Properties.Where(p => p.HasRouteParameter()).Any())
                {
                    var ctor = @class.Constructors.First();
                    ctor.AddParameter("route", this.UseType("ActivatedRoute", "@angular/router"), param =>
                    {
                        param.WithPrivateFieldAssignment();
                    });
                }

                foreach (var prop in Model.Properties.Where(p => p.HasRouteParameter() || p.HasQueryParameter()))
                {
                    var postStatement = "";
                    if (!prop.TypeReference.IsNullable)
                    {
                        postStatement = " ?? ''";
                    }

                    if (prop.HasRouteParameter())
                    {
                        init.AddStatement($"this.{prop.Name.ToCamelCase(true)} = this.route.snapshot.paramMap.get('{prop.Name.ToCamelCase(true)}'){postStatement};");
                    }

                    if (prop.HasQueryParameter())
                    {
                        init.AddStatement($"this.{prop.Name.ToCamelCase(true)} = this.route.snapshot.queryParamMap.get('{prop.Name.ToCamelCase(true)}'){postStatement};");
                    }
                }
            });
        }

        public override void BeforeTemplateExecution()
        {
            if (Model.HasPage())
            {
                ExecutionContext.EventDispatcher.Publish(new ComponentCreatedEvent($"{ComponentName}Component", $"app-{ComponentName.ToKebabCase().ToLower()}",
                    Model.GetPage().Route(), Model));
            }

            base.BeforeTemplateExecution();
        }

        public string ComponentName
        {
            get
            {
                if (Model.Name.EndsWith("Component", StringComparison.InvariantCultureIgnoreCase))
                {
                    return Model.Name.Substring(0, Model.Name.Length - "Component".Length);
                }
                return Model.Name;
            }
        }

        [IntentManaged(Mode.Fully)]
        public TypescriptFile TypescriptFile { get; }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new TypeScriptFileConfig(
                overwriteBehaviour: OverwriteBehaviour.Always,
                fileName: $"{ComponentName.ToKebabCase()}.component",
                relativeLocation: $"{string.Join("/", Model.GetParentFolderNames().Select(f => f.ToKebabCase()))}/{ComponentName.ToKebabCase()}",
                className: $"{ComponentName}Component"
            );
        }

        [IntentManaged(Mode.Fully)]
        public override string TransformText()
        {
            return TypescriptFile.ToString();
        }
    }
}