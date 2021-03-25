using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HtmlAgilityPack;
using Intent.Engine;
using Intent.Metadata.Models;
using Intent.Modules.Angular.Layout.Html;
using Intent.Modules.Angular.Templates;
using Intent.Modules.Angular.Templates.Component.AngularComponentTsTemplate;
using Intent.Modules.Common;
using Intent.Modules.Common.Html.Templates;
using Intent.Modules.Common.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;


[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Html.Templates.HtmlFileTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Angular.Layout.Templates.Shared.Header.HeaderComponentHtmlTemplate
{
    [IntentManaged(Mode.Merge)]
    partial class HeaderComponentHtmlTemplate : HtmlTemplateBase<object>
    {
        private readonly List<ModuleRoute> _mainRoutes = new List<ModuleRoute>();

        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.Layout.Shared.Header.HeaderComponentHtmlTemplate";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public HeaderComponentHtmlTemplate(IOutputTarget outputTarget, object model = null) : base(TemplateId, outputTarget, null)
        {
            outputTarget.Application.EventDispatcher.Subscribe<AngularAppRouteCreatedEvent>(@event =>
           {
               _mainRoutes.Add(new ModuleRoute(@event.ModuleName, @event.Route));
           });
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new HtmlFileConfig(
                overwriteBehaviour: OverwriteBehaviour.Always,
                fileName: "header.component",
                relativeLocation: ""
            );
        }

        private class ModuleRoute
        {
            public ModuleRoute(string moduleName, string route)
            {
                ModuleName = moduleName;
                Route = route;
            }

            public string ModuleName { get; }

            public string Route { get; }
        }
    }
}