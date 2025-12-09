using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Modelers.UI.Api;
using Intent.Modules.Angular.Templates.Shared.IntentDecorators;
using Intent.Modules.Common;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.Types.Api;
using Intent.Modules.Common.TypeScript.Builder;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.TypeScript.Templates.TypescriptTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.Component.LayoutComponentTypescript
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public partial class LayoutComponentTypescriptTemplate : TypeScriptTemplateBase<Intent.Modelers.UI.Api.LayoutModel>, ITypescriptFileBuilderTemplate
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.Component.LayoutComponentTypescript";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully, Body = Mode.Ignore)]
        public LayoutComponentTypescriptTemplate(IOutputTarget outputTarget, Intent.Modelers.UI.Api.LayoutModel model) : base(TemplateId, outputTarget, model)
        {
            AddImport("Component", "@angular/core");
            AddImport("OnInit", "@angular/core");

            AddTypeSource("Intent.Angular.HttpClients.DtoContract");
            AddTypeSource("Intent.Angular.HttpClients.PagedResult");
            AddTypeSource("Intent.Angular.HttpClients.HttpServiceProxy");
            AddTypeSource("Intent.Application.Dtos.DtoModel");
            AddTypeSource(TemplateId);

            AddImport("RouterOutlet", "@angular/router");

            TypescriptFile = new TypescriptFile(this.GetFolderPath(), this)
                .AddClass($"{LayoutName}Layout", @class =>
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
                        obj.AddField("selector", $"'app-{LayoutName.ToKebabCase().ToLower()}'");
                        obj.AddField("standalone", "true");
                        obj.AddField("templateUrl", $"'{LayoutName.ToKebabCase()}.component.html'");
                        obj.AddField("styleUrls", $"['{LayoutName.ToKebabCase()}.component.scss']");
                        obj.AddField("imports", $"[RouterOutlet]");

                        component.AddArgument(obj.GetText(""));
                    });
                }).AfterBuild(file =>
                {
                    var intentDecoratorTemplate = GetTemplate<TypeScriptTemplateBase<object>>(IntentDecoratorsTemplate.TemplateId);
                    file.AddImport("IntentIgnoreBody", this.GetRelativePath(intentDecoratorTemplate));
                    file.AddImport("IntentMerge", this.GetRelativePath(intentDecoratorTemplate));
                });
        }

        [IntentManaged(Mode.Fully)]
        public TypescriptFile TypescriptFile { get; }

        public string LayoutName
        {
            get
            {
                if (Model.Name.EndsWith("Layout", StringComparison.InvariantCultureIgnoreCase))
                {
                    return Model.Name.Substring(0, Model.Name.Length - "Layout".Length);
                }
                return Model.Name;
            }
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new TypeScriptFileConfig(
                overwriteBehaviour: OverwriteBehaviour.Always,
                fileName: $"{LayoutName.ToKebabCase()}.component",
                relativeLocation: $"{string.Join("/", Model.GetParentFolderNames().Select(f => f.ToKebabCase()))}/{LayoutName.ToKebabCase()}",
                className: $"{LayoutName}Layout"
            );
        }

        [IntentManaged(Mode.Fully)]
        public override string TransformText()
        {
            return TypescriptFile.ToString();
        }
    }
}