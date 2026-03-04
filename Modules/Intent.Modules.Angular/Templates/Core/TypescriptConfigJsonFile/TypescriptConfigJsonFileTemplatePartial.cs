using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Metadata.Models;
using Intent.Modules.Angular.Settings;
using Intent.Modules.Angular.Templates.Core.JsonPatches;
using Intent.Modules.Angular.Templates.Core.JsonPatches.AngularDotJson;
using Intent.Modules.Angular.Templates.Core.JsonPatches.TsConfigJson;
using Intent.Modules.Common;
using Intent.Modules.Common.FileBuilders.DataFileBuilder;
using Intent.Modules.Common.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.ProjectItemTemplate.Partial", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.Core.TypescriptConfigJsonFile
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class TypescriptConfigJsonFileTemplate : IntentTemplateBase<object>, IDataFileBuilderTemplate
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.Core.TypescriptConfigJsonFileTemplate";

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public TypescriptConfigJsonFileTemplate(IOutputTarget outputTarget, object model = null) : base(TemplateId, outputTarget, model)
        {
            DataFile = new DataFile($"TypescriptConfigJsonFile")
                .WithJsonWriter()
                .WithRootObject(this, @object =>
                {
                    @object
                        .WithValue("compileOnSave", false)
                        .WithObject("compilerOptions", options =>
                        {
                            options
                                .WithValue("strict", true)
                                .WithValue("noImplicitOverride", true)
                                .WithValue("noPropertyAccessFromIndexSignature", true)
                                .WithValue("noImplicitReturns", true)
                                .WithValue("noFallthroughCasesInSwitch", true)
                                .WithValue("skipLibCheck", true)
                                .WithValue("isolatedModules", true)
                                .WithValue("experimentalDecorators", true)
                                .WithValue("importHelpers", true)
                                .WithValue("target", "ES2022")
                                .WithValue("module", "preserve");
                        })
                        .WithObject("angularCompilerOptions", compiler =>
                        {
                            compiler
                                .WithValue("enableI18nLegacyMessageIdFormat", false)
                                .WithValue("strictInjectionParameters", true)
                                .WithValue("strictInputAccessModifiers", true)
                                .WithValue("strictTemplates", true);
                        });
                }).OnBuild(file =>
                {
                    PatchTsConfigFile(file);
                });
        }

        private void PatchTsConfigFile(IDataFile file)
        {
            List<IAngularJsonPatch> Patches =
            [
                new Ts19ConfigPatch(ExecutionContext.Settings.GetAngularSettings().AngularVersion().AsEnum()),
                new FilesReferencePatch(ExecutionContext.Settings.GetAngularSettings().AngularVersion().AsEnum()),
                new CompilerHostBindingsPatch(ExecutionContext.Settings.GetAngularSettings().AngularVersion().AsEnum()),
            ];

            foreach (var patch in Patches
                .Where(p => p.Applicable())
                .OrderBy(p => p.Order))
            {
                patch.Apply(file);
            }
        }

        [IntentManaged(Mode.Fully)]
        public IDataFile DataFile { get; }

        [IntentManaged(Mode.Merge)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new TemplateFileConfig(
                fileName: $"tsconfig",
                fileExtension: "json"
            );
        }

        [IntentManaged(Mode.Fully)]
        public override string TransformText() => DataFile.ToString();
    }
}