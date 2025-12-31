using Intent.Modelers.WebClient.Angular.Api;
using Intent.Modules.Angular.Templates.Component.AngularComponentHtml;
using Intent.Modules.Angular.Templates.Component.AngularComponentTs;

namespace Intent.Modules.Angular.Templates.Component.Controls.DisplayComponent
{
    public class DisplayComponentTemplate : IControl
    {
        private readonly AngularComponentHtmlTemplate _template;

        public DisplayComponentTemplate(DisplayComponentModel model, AngularComponentHtmlTemplate template)
        {
            _template = template;
            Model = model;
        }

        public DisplayComponentModel Model { get; }

        private string GetSelector()
        {
            var component = _template.GetTemplate<AngularComponentTsTemplate>(AngularComponentTsTemplate.TemplateId, Model.TypeReference.Element);
            return component.GetSelector();
        }

        public string TransformText()
        {
            return $"<{GetSelector()}></{GetSelector()}>";
        }
    }
}
