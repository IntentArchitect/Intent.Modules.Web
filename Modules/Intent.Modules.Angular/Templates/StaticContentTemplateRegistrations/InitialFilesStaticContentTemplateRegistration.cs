using System.Collections.Generic;
using Intent.Engine;
using Intent.Modules.Common;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.Templates.StaticContent;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.StaticContentTemplateRegistration", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.StaticContentTemplateRegistrations
{
    public class InitialFilesStaticContentTemplateRegistration : StaticContentTemplateRegistration
    {
        public new const string TemplateId = "Intent.Modules.Angular.Templates.StaticContentTemplateRegistrations.InitialFilesStaticContentTemplateRegistration";

        public InitialFilesStaticContentTemplateRegistration() : base(TemplateId)
        {
        }

        public override string ContentSubFolder => "InitialFiles";


        public override string[] BinaryFileGlobbingPatterns => new string[] { "*.jpg", "*.png", "*.xlsx", "*.ico", "*.pdf" };


        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public override IReadOnlyDictionary<string, string> Replacements(IOutputTarget outputTarget) => new Dictionary<string, string>
        {
            ["AppNamePascalCased"] = outputTarget.ApplicationName().ToPascalCase(),
            ["AppNameCamelCased"] = outputTarget.ApplicationName().ToCamelCase(),
            ["AppNameKebabCased"] = outputTarget.ApplicationName().ToKebabCase(),
        };
    }
}