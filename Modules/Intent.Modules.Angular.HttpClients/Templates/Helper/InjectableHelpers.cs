using Intent.Modules.Common.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intent.Modules.Angular.HttpClients.Templates.Helper;

public static class InjectableHelpers
{
    public static string GetOptions(IIntentTemplate template)
    {
        return "{ providedIn: 'root' }";
        //return template.ExecutionContext.InstalledModules.All(x => x.ModuleId != "Intent.Angular")
        //    ? "{ providedIn: 'root' }"
        //    : string.Empty;
    }
}
