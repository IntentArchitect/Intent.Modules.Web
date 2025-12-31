using System.Collections.Generic;
using Intent.Configuration;
using Intent.Engine;
using Intent.Modules.AI.Prompts.Tasks;
using Intent.Modules.Common.AI.Configuration;
using Intent.Modules.Common.Templates.StaticContent;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.StaticContentTemplateRegistration", Version = "1.0")]

namespace Intent.Modules.AI.Angular.Templates.StaticContentTemplateRegistrations
{
    [IntentMerge]
    public class AngularAIPromptTemplatesStaticContentTemplateRegistration : StaticContentTemplateRegistration
    {
        private readonly ISolutionConfig _solutionConfig;
        private readonly IApplicationConfigurationProvider _applicationConfigurationProvider;

        public new const string TemplateId = "Intent.Modules.AI.Angular.Templates.StaticContentTemplateRegistrations.AngularAIPromptTemplatesStaticContentTemplateRegistration";

        [IntentMerge]
        public AngularAIPromptTemplatesStaticContentTemplateRegistration(ISolutionConfig solutionConfig, IApplicationConfigurationProvider applicationConfigurationProvider) : base(TemplateId)
        {
            _solutionConfig = solutionConfig;
            _applicationConfigurationProvider = applicationConfigurationProvider;
        }

        public override string ContentSubFolder => "AngularAIPromptTemplates";


        public override string[] BinaryFileGlobbingPatterns => new string[] { "*.jpg", "*.png", "*.xlsx", "*.ico", "*.pdf" };


        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public override IReadOnlyDictionary<string, string> Replacements(IOutputTarget outputTarget) => new Dictionary<string, string>
        {
        };

        public override string RelativeOutputPathPrefix => PromptConfig.GetTemplatePromptPath(_solutionConfig.SolutionRootLocation, _applicationConfigurationProvider.GetApplicationConfig().Name, GenerateAngularWithAITask.TaskTypeId);

    }
}