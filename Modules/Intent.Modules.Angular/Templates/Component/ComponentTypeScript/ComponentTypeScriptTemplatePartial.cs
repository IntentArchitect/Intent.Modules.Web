using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Text;
using Intent.Engine;
using Intent.Exceptions;
using Intent.Metadata.Models;
using Intent.Modelers.UI.Api;
using Intent.Modules.Angular.Api;
using Intent.Modules.Angular.Api.Mappings;
using Intent.Modules.Angular.Templates.Shared.IntentDecorators;
using Intent.Modules.Common;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.TypeResolution;
using Intent.Modules.Common.Types.Api;
using Intent.Modules.Common.TypeScript.Builder;
using Intent.Modules.Common.Typescript.Mapping;
using Intent.Modules.Common.Typescript.Mapping.Resolvers;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;
using static System.Reflection.Metadata.BlobBuilder;

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

            AddTypeSource("Intent.Angular.HttpClients.EnumContract");
            AddTypeSource("Intent.Angular.HttpClients.DtoContract");
            AddTypeSource("Intent.Angular.HttpClients.PagedResult");
            AddTypeSource("Intent.Angular.HttpClients.HttpServiceProxy");
            AddTypeSource("Intent.Application.Dtos.DtoModel");
            AddTypeSource(TemplateId);

            TypescriptFile = new TypescriptFile(this.GetFolderPath(), this);

            AddModelDefinitions(model);

            TypescriptFile.AddClass($"{ComponentName}Component", @class =>
            {
                @class.Export();
                @class.AddDecorator("IntentMerge");
                @class.AddDecorator("Component", component =>
                {
                    // TODO Clean this up
                    var obj = new TypescriptVariableObject
                    {
                        Indentation = TypescriptFile.Indentation
                    };
                    obj.AddField("selector", $"'app-{ComponentName.ToKebabCase().ToLower()}'");
                    obj.AddField("standalone", "true");
                    obj.AddField("templateUrl", $"'{ComponentName.ToKebabCase()}.component.html'");
                    obj.AddField("styleUrls", $"['{ComponentName.ToKebabCase()}.component.scss']");

                    component.AddArgument(obj.GetText(""));
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
                    @class.AddMethod(operation.Name.ToCamelCase(true), operation.ReturnType is null ? "void" : GetTypeName(operation.ReturnType), method =>
                    {
                        foreach (var param in operation.Parameters)
                        {
                            method.AddParameter(param.Name, GetTypeName(param.TypeReference), p =>
                            {
                                p.WithDefaultValue(param.Value);
                            });
                        }

                        if (operation.Name == "ngOnInit")
                        {
                            ConfigureOnInitOperation(@class, method);
                        }

                        TypescriptFile.AfterBuild(file =>
                        {
                            var mappingManager = CreateMappingManager();
                            mappingManager.SetFromReplacement(operation, null);

                            foreach (var action in operation.GetProcessingActions())
                            {
                                if (action.IsInvocationModel() && action.Mappings.Count() == 1)
                                {
                                    var operationMapping = action.Mappings.Single();
                                    var mappedEnd = operationMapping.MappedEnds.FirstOrDefault(x => x.SourceElement.Id == action.Id) ?? throw new ElementException(action, "Mapping required for this invocation");
                                    var invocation = mappingManager.GenerateSourceStatementForMapping(operationMapping, mappedEnd);
                                    method.AddStatement(invocation);
                                    continue;
                                }

                                if (action.IsCallServiceOperationActionTargetEndModel())
                                {
                                    var serviceCall = action.AsCallServiceOperationActionTargetEndModel();
                                    var parentElement = ((IElement)serviceCall.Element).ParentElement;
                                    var invocationMapping = serviceCall.GetMapInvocationMapping();
                                    var targetElement = (IElement)invocationMapping.TargetElement;

                                    const string commandSpecializationTypeId = "ccf14eb6-3a55-4d81-b5b9-d27311c70cb9";
                                    const string querySpecializationTypeId = "e71b0662-e29d-4db2-868b-8a12464b25d0";
                                    const string dtoFieldTypeId = "7baed1fd-469b-4980-8fd9-4cefb8331eb2";
                                    const string httpSettingsDefinitionId = "b4581ed2-42ec-4ae2-83dd-dcdd5f0837b6";

                                    string? serviceName = null;
                                    TypescriptStatement? invocation = null;

                                    serviceName = parentElement is not null ? @class.InjectServiceProperty(this, GetTypeName(parentElement)) :
                                        @class.InjectServiceProperty(this, GetTypeName(serviceCall.Package.AsTypeReference()));

                                    @class.InjectServiceUtilityFields(method.Name);
                                    this.AddImport("finalize", "rxjs");

                                    if (targetElement.SpecializationTypeId is commandSpecializationTypeId or querySpecializationTypeId)
                                    {
                                        if (!targetElement.HasStereotype(httpSettingsDefinitionId))
                                        {
                                            throw new ElementException(action, "Target CQRS request is not exposed with HTTP");
                                        }

                                        var nameOfMethodToInvoke = (this
                                            .GetAllTypeInfo(parentElement.AsTypeReference()?.Element is null ? serviceCall.Package.AsTypeReference() : parentElement.AsTypeReference())
                                            .Select(x => x.Template)
                                            .OfType<ITypescriptTemplate>()
                                            .FirstOrDefault(x => x.RootCodeContext.TryGetReferenceForModel(targetElement.Id, out _))
                                                ?.RootCodeContext.GetReferenceForModel(targetElement.Id).Name) ?? throw new FriendlyException("Unable to resolve the service type for the service call to `" + targetElement.DisplayText + "`. Try installing a module to realize this service (e.g. `Intent.Blazor.HttpClients`)");

                                        var arguments = new List<TypescriptStatement>();
                                        if (targetElement.ChildElements.Any(x => x.SpecializationTypeId is dtoFieldTypeId))
                                        {
                                            arguments.Add(mappingManager.GenerateCreationStatement(invocationMapping));
                                        }

                                        invocation = new TypescriptStatement($"{nameOfMethodToInvoke}({string.Join(',', arguments)})");
                                    }
                                    else // Operations
                                    {
                                        invocation = mappingManager.GenerateUpdateStatements(invocationMapping).First();
                                    }

                                    method.AddStatements(TypescriptFileExtensions.GetCallServiceOperation(serviceCall, mappingManager, serviceName, invocation, method.Name));

                                    continue;
                                }
                            }
                        });
                    });
                }

                foreach (var navigation in model.InternalElement.AssociatedElements.Where(e => e.IsNavigationEndModel() && e.IsNavigable))
                {
                    var navigationModel = navigation.AsNavigationEndModel();
                    if (!navigationModel.Element.AsComponentModel().HasPage())
                    {
                        throw new ElementException(navigationModel.Element, "Navigation is targeting a Component that isn't a page. Please add the Page stereotype to the targeted Component.");
                    }

                    var toComponent = navigationModel.Element.AsComponentModel();

                    @class.AddMethod(navigation.Name?.ToCamelCase(true) ?? $"navigateTo{toComponent.Name}", "void", method =>
                    {
                        var parameters = toComponent.Properties.Where(x => x.HasRouteParameter() || x.HasQueryParameter());
                        var routeManager = new RouteManager(toComponent.GetPage().Route(), [.. parameters]);

                        this.AddImport("Router", "@angular/router");
                        var ctor = @class.Constructors.First();
                        if (!ctor.Parameters.Any(p => p.Name == "router"))
                        {
                            ctor.AddParameter("router", this.UseType("Router", "@angular/router"), param =>
                            {
                                param.WithPrivateFieldAssignment();
                            });
                        }

                        method.AddStatement(routeManager.GetRouteInvocationText(this, method));
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

                        // TODO cater for default value and add back in IsDefinitelyAssigned
                        if (!model.TypeReference.IsNullable)// && !field.IsDefinitelyAssigned)
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

        public TypescriptClassMappingManager CreateMappingManager()
        {
            var template = (ITypescriptTemplate)this;

            var mappingManager = new TypescriptClassMappingManager(template);
            mappingManager.AddMappingResolver(new CallServiceOperationMappingResolver(template));
            mappingManager.AddMappingResolver(new TypescriptBindingMappingResolver(template));
            mappingManager.AddMappingResolver(new TypeConvertingMappingResolver(template));
            mappingManager.SetFromReplacement(Model, "this");
            mappingManager.SetToReplacement(Model, "this");
            return mappingManager;
        }

        private void AddModelDefinitions(ComponentModel model)
        {
            foreach (var modelDef in model.ModelDefinitions)
            {
                // the defacto standard should be interfaces, but they cannot contain operations
                // or contructors. So add class if required, but otherwise add interface
                if (modelDef.Constructors.Any() || modelDef.Operations.Any())
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
                    var propName = propertyModel.Name.ToCamelCase(true);
                    var fieldType = GetTypeName(propertyModel.TypeReference);

                    if (propertyModel.TypeReference.IsNullable)
                    {
                        fieldType = $"{fieldType} | null";
                        propName = $"{propName}?";
                    }

                    @interface.AddField(propName, fieldType);
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
                    var propName = propertyModel.Name.ToCamelCase(true);
                    var fieldType = GetTypeName(propertyModel.TypeReference);

                    if (propertyModel.TypeReference.IsNullable)
                    {
                        fieldType = $"{fieldType} | null";
                        propName = $"{propName}?";
                    }

                    @class.AddField(propName, fieldType, @field =>
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

        private void ConfigureOnInitOperation(TypescriptClass @class, TypescriptMethod method)
        {

            // TODO. Fix
            //method.AddDecorator("IntentMerge");

            if (Model.Properties.Where(p => p.HasRouteParameter() || p.HasQueryParameter()).Any())
            {
                var ctor = @class.Constructors.First();
                if (!ctor.Parameters.Any(p => p.Name == "route"))
                {
                    ctor.AddParameter("route", this.UseType("ActivatedRoute", "@angular/router"), param =>
                    {
                        param.WithPrivateFieldAssignment();
                    });
                }
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
                    method.AddStatement($"this.{prop.Name.ToCamelCase(true)} = this.route.snapshot.paramMap.get('{prop.Name.ToCamelCase(true)}'){postStatement};");
                }

                if (prop.HasQueryParameter())
                {
                    method.AddStatement($"this.{prop.Name.ToCamelCase(true)} = this.route.snapshot.queryParamMap.get('{prop.Name.ToCamelCase(true)}'){postStatement};");
                }
            }
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