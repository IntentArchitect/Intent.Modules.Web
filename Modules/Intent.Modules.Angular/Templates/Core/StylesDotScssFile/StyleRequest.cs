namespace Intent.Modules.Angular.Templates.Core.StylesDotScssFile;

public class StyleRequest
{
    public StyleRequest(string payload)
    {
        Payload = payload;
    }

    public string Payload { get; set; }
}