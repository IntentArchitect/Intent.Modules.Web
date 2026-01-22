using System;
using System.Collections.Generic;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.FileTemplateStringInterpolation", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.Core.StylesDotScssFile
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public partial class StylesDotScssFileTemplate
    {
        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public override string TransformText()
        {
            return $"/* You can add global styles to this file, and also import other style files */{System.Environment.NewLine}";
        }
    }
}