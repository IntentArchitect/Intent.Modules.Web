using System.Collections.Generic;
using Intent.Eventing;

namespace Intent.Modules.Angular.Shared;

/// <summary>
/// This allows use of essentially "typed" events, without a hard dependency.
/// </summary>
internal static class EventingDispatcherExtensions
{
    #region AngularConfigVariableRequiredEvent

    public delegate void HandleAngularConfigVariableRequiredEvent(string variableKey, string defaultValue);

    public static void PublishAngularConfigVariableRequiredEvent(
        this IApplicationEventDispatcher eventDispatcher,
        string variableKey,
        string defaultValue)
    {
        eventDispatcher.Publish("AngularConfigVariableRequiredEvent", new Dictionary<string, string>
        {
            ["VariableKey"] = variableKey,
            ["DefaultValue"] = defaultValue
        });
    }

    public static void SubscribeToAngularConfigVariableRequiredEvent(
        this IApplicationEventDispatcher eventDispatcher,
        HandleAngularConfigVariableRequiredEvent handle)
    {
        eventDispatcher.Subscribe("AngularConfigVariableRequiredEvent", @event => handle(
            variableKey: @event.GetValue("VariableKey"),
            defaultValue: @event.GetValue("DefaultValue")));
    }

    #endregion

    #region AngularServiceProxyCreatedEvent

    public delegate void HandleAngularServiceProxyCreatedEvent(string templateId, string modelId, string moduleId);

    public static void PublishAngularServiceProxyCreatedEvent(
        this IApplicationEventDispatcher eventDispatcher,
        string templateId,
        string modelId,
        string moduleId)
    {
        eventDispatcher.Publish("AngularServiceProxyCreatedEvent", new Dictionary<string, string>
        {
            ["ModelId"] = modelId,
            ["ModuleId"] = moduleId,
            ["TemplateId"] = templateId
        });
    }

    public static void SubscribeToAngularServiceProxyCreatedEvent(
        this IApplicationEventDispatcher eventDispatcher,
        HandleAngularServiceProxyCreatedEvent handle)
    {
        eventDispatcher.Subscribe("AngularServiceProxyCreatedEvent", @event => handle(
            templateId: @event.GetValue("TemplateId"),
            modelId: @event.GetValue("ModelId"),
            moduleId: @event.GetValue("ModuleId")));
    }

    #endregion
}