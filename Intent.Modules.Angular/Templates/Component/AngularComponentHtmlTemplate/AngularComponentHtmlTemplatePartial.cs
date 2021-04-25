using System;
using System.IO;
using Intent.Metadata.Models;
using Intent.Modules.Common;
using Intent.Modules.Common.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Engine;
using Intent.Templates;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Intent.Modules.Angular.Api;
using Intent.Modules.Common.Html.Templates;
using Intent.Modelers.WebClient.Angular.Api;
using Intent.Modules.Angular.Templates.Component.Controls;
using Intent.Modules.Angular.Templates.Component.Controls.DisplayComponent;
using Intent.Modules.Common.Types.Api;
using Intent.Utils;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Html.Templates.HtmlFileTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.Component.AngularComponentHtmlTemplate
{
    public interface IAngularComponentHtmlDecorator : ITemplateDecorator
    {
        void RegisterControls(ControlWriter controlWriter);
    }

    [IntentManaged(Mode.Merge, Signature = Mode.Merge)]
    partial class AngularComponentHtmlTemplate : HtmlTemplateBase<ComponentModel>, ITemplatePostCreationHook, IHasDecorators<IAngularComponentHtmlDecorator>
    {
        private readonly IList<IAngularComponentHtmlDecorator> _decorators = new List<IAngularComponentHtmlDecorator>();

        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.Component.AngularComponentHtmlTemplate";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public AngularComponentHtmlTemplate(IOutputTarget outputTarget, Intent.Modelers.WebClient.Angular.Api.ComponentModel model) : base(TemplateId, outputTarget, model)
        {
            ControlWriter = new ControlWriter();
            ControlWriter.RegisterControl(DisplayComponentModel.SpecializationTypeId, element => new DisplayComponentTemplate(new DisplayComponentModel(element), this));
            ControlWriter.RegisterControl(RouterOutletModel.SpecializationTypeId, element => new RouterOutletTemplate(new RouterOutletModel(element)));
        }

        public ControlWriter ControlWriter;

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

        public string ModuleName { get; private set; }

        public override void OnCreated()
        {
            base.OnCreated();
            var moduleTemplate = GetTemplate<Module.AngularModuleTemplate.AngularModuleTemplate>(Module.AngularModuleTemplate.AngularModuleTemplate.TemplateId, Model.Module);
            ModuleName = moduleTemplate.ModuleName;
        }

        public override string RunTemplate()
        {
            //var meta = GetMetadata();
            //var fullFileName = Path.Combine(meta.GetFullLocationPath(), meta.FileNameWithExtension());

            //if (File.Exists(fullFileName))
            //{
            //    var source = File.ReadAllText(fullFileName);
            //if (source.StartsWith("<!--IntentManaged-->"))
            //{
            if (Model.View == null || !Model.View.InternalElement.ChildElements.Any())
            {
                return base.RunTemplate();
            }

            var sb = new StringBuilder();
            sb.AppendLine("<div class=\"container-fluid\" intent-manage=\"add remove\" intent-id=\"container\">");
            sb.AppendLine("  " + ControlWriter.WriteControls(Model.View.InternalElement.ChildElements, "  "));
            sb.AppendLine("</div>");
            return sb.ToString();
            //}
            //else
            //{
            //    return source;
            //}
            //}

            //return base.RunTemplate();
        }

        public void AddDecorator(IAngularComponentHtmlDecorator decorator)
        {
            decorator.RegisterControls(ControlWriter);
            _decorators.Add(decorator);
        }

        public IEnumerable<IAngularComponentHtmlDecorator> GetDecorators()
        {
            return _decorators;
        }

        private string LoadOrCreate(string fullFileName)
        {
            return File.Exists(fullFileName) ? File.ReadAllText(fullFileName) : base.RunTemplate();
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new TemplateFileConfig(
                fileName: $"{ComponentName.ToKebabCase()}.component",
                fileExtension: "html",
                relativeLocation: $"{string.Join("/", Model.GetParentFolderNames().Concat(new[] { ComponentName.ToKebabCase() }))}");
        }
    }
}