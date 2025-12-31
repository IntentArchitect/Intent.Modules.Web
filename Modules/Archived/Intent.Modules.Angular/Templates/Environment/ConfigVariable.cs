namespace Intent.Modules.Angular.Templates.Environment;

internal class ConfigVariable
{
    public ConfigVariable(string name, string defaultValue)
    {
        Name = name;
        DefaultValue = defaultValue;
    }
    public string Name { get; set; }
    public string DefaultValue { get; set; }
}