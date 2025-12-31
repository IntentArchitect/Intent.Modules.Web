namespace Intent.Modules.Angular.Templates.Core.Index;

/// <summary>
/// Use for adding <c>&lt;link rel="…" href="…"&gt;</c> entries to the <c>index.html</c> file.
/// </summary>
public class IndexHeaderLinkRequired
{
    /// <param name="relationship">The value of the <c>rel</c> attribute.</param>
    /// <param name="href">The value of the <c>href</c> attribute.</param>
    public IndexHeaderLinkRequired(string relationship, string href)
    {
        Relationship = relationship;
        Href = href;
    }

    public string Relationship { get; set; }
    public string Href { get; set; }
}