using System;
using System.Collections.Generic;
using System.Text;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.TypeScript.Templates.TypescriptTemplateStringInterpolation", Version = "1.0")]

namespace Intent.Modules.Angular.ServiceProxies.Templates.EnumContract
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public partial class EnumContractTemplate
    {
        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public override string TransformText()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"export enum {ClassName} {{");

            foreach (var literal in Model.Literals)
            {
                sb.Append($"  {literal.Name}");

                if (!string.IsNullOrWhiteSpace(literal.Value))
                {
                    sb.Append($" = {literal.Value.Trim()}");
                }

                sb.AppendLine(",");
            }

            sb.AppendLine("}");

            return sb.ToString();
        }
    }
}