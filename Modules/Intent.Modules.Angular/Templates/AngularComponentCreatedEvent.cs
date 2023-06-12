using Intent.Metadata.Models;

namespace Intent.Modules.Angular.Templates
{
    public static class AngularModuleCreatedEvent
    {
        public const string EventId = nameof(AngularModuleCreatedEvent);
        public const string ModelId = "ModelId";
        public const string ModuleId = "ModuleId";
    }

    public class AngularAppRouteCreatedEvent
    {
        public AngularAppRouteCreatedEvent(string moduleName, string route)
        {
            ModuleName = moduleName;
            Route = route;
        }
        public string ModuleName { get; }
        public string Route { get; }
    }

    public class AngularComponentCreatedEvent
    {
        public AngularComponentCreatedEvent(string modelId, string moduleId)
        {
            ModelId = modelId;
            ModuleId = moduleId;
        }

        public string ModelId { get; }
        public string ModuleId { get; }
    }

    public class AngularServiceProxyCreatedEvent
    {
        public AngularServiceProxyCreatedEvent(string templateId, string modelId, string moduleId)
        {
            ModelId = modelId;
            ModuleId = moduleId;
            TemplateId = templateId;
        }

        public string ModelId { get; }
        public string ModuleId { get; }
        public string TemplateId { get; }
    }

    public class AngularImportDependencyRequiredEvent
    {
        public AngularImportDependencyRequiredEvent(string moduleId, string dependency, string import)
        {
            ModuleId = moduleId;
            Dependency = dependency;
            Import = import;
        }
        public string ModuleId { get; }
        public string Dependency { get; }
        public string Import { get; }
    }

    public class AngularCustomProviderRequiredEvent
    {
        public AngularCustomProviderRequiredEvent(string moduleId, string provide, string useClass, bool multi, string import)
        {
            ModuleId = moduleId;
            Provide = provide;
            UseClass = useClass;
            Multi = multi;
            Import = import;
        }

        public string ModuleId { get; }
        public string Provide { get; }
        public string UseClass { get; }
        public bool Multi { get; }
        public string Import { get; }
    }

    public class AngularConfigVariableRequiredEvent
    {
        public AngularConfigVariableRequiredEvent(string variableKey, string defaultValue)
        {
            VariableKey = variableKey;
            DefaultValue = defaultValue;
        }
        public string VariableKey { get; }
        public string DefaultValue { get; }
    }

    public class AngularComponentFieldRequiredEvent
    {
        public AngularComponentFieldRequiredEvent(IElement element, string name, string type, string defaultValue)
        {
            Element = element;
            Name = name;
            Type = type;
            DefaultValue = defaultValue;
        }

        public IElement Element { get; }
        public string Name { get; }
        public string Type { get; }
        public string DefaultValue { get; }
    }
}