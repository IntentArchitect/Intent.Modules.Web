using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Modelers.WebClient.Angular.Api;
using Intent.Modules.Angular.Shared;
using Intent.Modules.Angular.Templates.Model.Model;
using Intent.Modules.Common;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.Types.Api;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.TypeScript.Templates.TypescriptTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.Module.AngularResolver
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class AngularResolverTemplate : TypeScriptTemplateBase<Intent.Modelers.WebClient.Angular.Api.ResolverModel>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.Module.AngularResolver";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public AngularResolverTemplate(IOutputTarget outputTarget, Intent.Modelers.WebClient.Angular.Api.ResolverModel model) : base(TemplateId, outputTarget, model)
        {
            AddTypeSource(ModelTemplate.TemplateId);
            AddTypeSource("Intent.Angular.ServiceProxies.Proxies.AngularDTO");
            AddTypeSource("Intent.Angular.ServiceProxies.Proxies.AngularServiceProxy");
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new TypeScriptFileConfig(
                className: $"{Model.Name}",
                fileName: $"{Model.Name.ToKebabCase()}",
                relativeLocation: $"{string.Join("/", Model.Module.GetParentFolderNames().Concat(new[] { Model.Module.GetModuleName().ToKebabCase() }))}"
            );
        }

        public override void BeforeTemplateExecution()
        {
            ExecutionContext.EventDispatcher.PublishAngularServiceProxyCreatedEvent(
                templateId: TemplateId,
                modelId: Model.Id,
                moduleId: Model.Module.Id);
        }
    }
}