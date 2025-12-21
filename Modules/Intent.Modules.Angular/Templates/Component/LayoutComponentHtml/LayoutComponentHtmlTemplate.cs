using System;
using System.Collections.Generic;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.FileTemplateStringInterpolation", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.Component.LayoutComponentHtml
{


    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public partial class LayoutComponentHtmlTemplate
    {

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public override string TransformText()
        {
            // the content can be set from another template or factory extension
            if (!string.IsNullOrWhiteSpace(_content))
            {
                return _content;
            }

            return @$"<!-- Replace this with your HTML template -->";
        }
    }
}
