using System.Linq;
using Intent.Modules.Common.Templates;

namespace Intent.Modules.Angular.ServiceProxies.Templates;

internal static class InjectableHelpers
{
    /// <summary>
    /// The idea is that when we're installing this module standalone it requires minimal configuration to just work,
    /// but if <c>Intent.Angular</c> is installed that's already got logic to wire this up.
    /// </summary>
    public static string GetOptions(IIntentTemplate template)
    {
        return template.ExecutionContext.InstalledModules.All(x => x.ModuleId != "Intent.Angular")
            ? "{ providedIn: 'root' }"
            : string.Empty;
    }
}