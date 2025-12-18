using System;
using System.Collections.Generic;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.FileTemplateStringInterpolation", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.Component.HeaderComponentStyle
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public partial class HeaderComponentStyleTemplate
    {
        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public override string TransformText()
        {
            return @$"// Place your file template logic here
";
        }
    }
}