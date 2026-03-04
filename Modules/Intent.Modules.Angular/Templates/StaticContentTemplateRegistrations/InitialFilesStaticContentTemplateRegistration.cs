using System;
using System.Collections.Generic;
using Intent.Engine;
using Intent.Modules.Angular.Settings;
using Intent.Modules.Common;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.Templates.StaticContent;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.StaticContentTemplateRegistration", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.StaticContentTemplateRegistrations
{
    [IntentMerge]
    public class InitialFilesStaticContentTemplateRegistration : StaticContentTemplateRegistration
    {
        public new const string TemplateId = "Intent.Modules.Angular.Templates.StaticContentTemplateRegistrations.InitialFilesStaticContentTemplateRegistration";

        public InitialFilesStaticContentTemplateRegistration() : base(TemplateId)
        {
        }

        public override string ContentSubFolder => "InitialFiles";


        public override string[] BinaryFileGlobbingPatterns => new string[] { "**/*.jpg", "**/*.png", "**/*.xlsx", "**/*.ico", "**/*.pdf" };


        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public override IReadOnlyDictionary<string, string> Replacements(IOutputTarget outputTarget) => new Dictionary<string, string>
        {
            ["AppNamePascalCased"] = outputTarget.ApplicationName().ToPascalCase(),
            ["AppNameCamelCased"] = outputTarget.ApplicationName().ToCamelCase(),
            ["TestRunner"] = GetVersionTestRunner(outputTarget.ExecutionContext.Settings.GetAngularSettings().AngularVersion().AsEnum()),
            ["FilesIncludePath"] = GetFilesIncludePath(outputTarget.ExecutionContext.Settings.GetAngularSettings().AngularVersion().AsEnum()),
            ["AdditionalConfigArrayName"] = GetAdditionalConfigArrayName(outputTarget.ExecutionContext.Settings.GetAngularSettings().AngularVersion().AsEnum()),
            ["AdditionalConfigArrayPath"] = GetAdditionalConfigArrayPath(outputTarget.ExecutionContext.Settings.GetAngularSettings().AngularVersion().AsEnum())
        };

        private string GetVersionTestRunner(AngularSettings.AngularVersionOptionsEnum angularSettings)
        {
            return angularSettings switch
            {
                AngularSettings.AngularVersionOptionsEnum._210 => "[Vitest](https://vitest.dev/)",
                AngularSettings.AngularVersionOptionsEnum._202 => "[Karma](https://karma-runner.github.io)",
                AngularSettings.AngularVersionOptionsEnum._192 => "[Karma](https://karma-runner.github.io)",
                _ => throw new NotSupportedException($"Unsupported Angular version: {angularSettings}")
            };
        }

        private string GetFilesIncludePath(AngularSettings.AngularVersionOptionsEnum angularSettings)
        {
            return angularSettings switch
            {
                AngularSettings.AngularVersionOptionsEnum._210 => "src/**/*.ts",
                AngularSettings.AngularVersionOptionsEnum._202 => "src/**/*.ts",
                AngularSettings.AngularVersionOptionsEnum._192 => "src/**/*.d.ts",
                _ => throw new NotSupportedException($"Unsupported Angular version: {angularSettings}")
            };
        }

        private string GetAdditionalConfigArrayName(AngularSettings.AngularVersionOptionsEnum angularSettings)
        {
            return angularSettings switch
            {
                AngularSettings.AngularVersionOptionsEnum._210 => "exclude",
                AngularSettings.AngularVersionOptionsEnum._202 => "exclude",
                AngularSettings.AngularVersionOptionsEnum._192 => "files",
                _ => throw new NotSupportedException($"Unsupported Angular version: {angularSettings}")
            };
        }

        private string GetAdditionalConfigArrayPath(AngularSettings.AngularVersionOptionsEnum angularSettings)
        {
            return angularSettings switch
            {
                AngularSettings.AngularVersionOptionsEnum._210 => "src/**/*.spec.ts",
                AngularSettings.AngularVersionOptionsEnum._202 => "src/**/*.spec.ts",
                AngularSettings.AngularVersionOptionsEnum._192 => "src/main.ts",
                _ => throw new NotSupportedException($"Unsupported Angular version: {angularSettings}")
            };
        }
    }
}

