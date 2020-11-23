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

    public static class AngularComponentCreatedEvent
    {
        public const string EventId = "AngularComponentCreatedEvent";
        public const string ModelId = "ModelId";
        public const string ModuleId = "ModuleId";
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

    public static class AngularImportDependencyRequiredEvent
    {
        public const string EventId = "ModuleProviderDependencyEvent";
        public const string ModuleId = "ModuleId";
        public const string Dependency = "Dependency";
        public const string Import = "Import";
    }

    public static class AngularConfigVariableRequiredEvent
    {
        public const string EventId = nameof(AngularConfigVariableRequiredEvent);
        public const string VariableId = nameof(VariableId);
        public const string DefaultValueId = nameof(DefaultValueId);
    }
}