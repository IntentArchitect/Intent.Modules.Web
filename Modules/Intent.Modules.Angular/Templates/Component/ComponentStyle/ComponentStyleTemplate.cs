using System;
using System.Collections.Generic;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.FileTemplateStringInterpolation", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.Component.ComponentStyle
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public partial class ComponentStyleTemplate
    {
        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public override string TransformText()
        {
            // the content can be set from another template or factory extension
            if (!string.IsNullOrWhiteSpace(_content))
            {
                return _content;
            }

            return @$"// Place your styling here";
        }
    }
}