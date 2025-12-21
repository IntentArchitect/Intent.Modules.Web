using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Intent.Modules.Angular.Components.Material.FactoryExtensions;

public static partial class FactoryRegex
{
    [GeneratedRegex(@"\$my-primary:\s*mat\.m2-define-palette\(mat\.\$m2-[\w-]+-palette\);", RegexOptions.IgnoreCase)]
    public static partial Regex PrimaryPaletteRegex();
    [GeneratedRegex(@"\$my-accent:\s*mat\.m2-define-palette\(mat\.\$m2-[\w-]+-palette\);", RegexOptions.IgnoreCase)]
    public static partial Regex AccentPaletteRegex();
}
