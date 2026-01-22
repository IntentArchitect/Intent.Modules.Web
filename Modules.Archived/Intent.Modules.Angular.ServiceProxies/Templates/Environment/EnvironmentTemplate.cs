using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.TypeScript.Templates.TypescriptTemplateStringInterpolation", Version = "1.0")]

namespace Intent.Modules.Angular.ServiceProxies.Templates.Environment
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public partial class EnvironmentTemplate
    {
        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public override string TransformText()
        {
            var sb = new StringBuilder();

            sb.AppendLine("//@IntentCanAdd()");
            sb.AppendLine($"export const {ClassName} = {{");

            if (_configVariables.Count > 0)
            {
                foreach (var (key, defaultValue) in _configVariables)
                {
                    sb.AppendLine($"  {key}: {defaultValue},");
                }

                // Remove trailing comma
                sb.Length -= System.Environment.NewLine.Length + 1;
                sb.AppendLine();
            }

            sb.AppendLine("};");

            return sb.ToString();
        }
    }
}