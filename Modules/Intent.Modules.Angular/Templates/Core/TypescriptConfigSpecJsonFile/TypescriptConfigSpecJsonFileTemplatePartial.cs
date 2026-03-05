using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Metadata.Models;
using Intent.Modules.Angular.Settings;
using Intent.Modules.Angular.Templates.Core.JsonPatches;
using Intent.Modules.Angular.Templates.Core.JsonPatches.TsConfigAppJson;
using Intent.Modules.Angular.Templates.Core.JsonPatches.TsConfigJson;
using Intent.Modules.Common;
using Intent.Modules.Common.FileBuilders.DataFileBuilder;
using Intent.Modules.Common.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.ProjectItemTemplate.Partial", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.Core.TypescriptConfigSpecJsonFile
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class TypescriptConfigSpecJsonFileTemplate : IntentTemplateBase<object>, IDataFileBuilderTemplate
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.Core.TypescriptConfigSpecJsonFileTemplate";

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public TypescriptConfigSpecJsonFileTemplate(IOutputTarget outputTarget, object model = null) : base(TemplateId, outputTarget, model)
        {
            DataFile = new DataFile($"TypescriptConfigAppJsonFile")
                .WithJsonWriter()
                .WithRootObject(this, @object =>
                {
                    @object
                        .WithValue("extends", "./tsconfig.json")
                        .WithObject("compilerOptions", options =>
                        {
                            options
                                .WithValue("outDir", "./out-tsc/spec")
                                .WithArray("types", array => { });
                        })
                        .WithArray("include", array =>
                        {
                            array.WithValue("src/**/*.spec.ts")
                            .WithValue("src/**/*.d.ts");
                        });
                }).OnBuild(file =>
                {
                    PatchTsConfigSpecFile(file);
                });
        }

        private void PatchTsConfigSpecFile(IDataFile file)
        {
            List<IAngularJsonPatch> Patches =
            [
                new OptionTypesJasminePatch(ExecutionContext.Settings.GetAngularSettings().AngularVersion().AsEnum()),
                new OptionTypesViTestPatch(ExecutionContext.Settings.GetAngularSettings().AngularVersion().AsEnum()),
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
                fileName: $"tsconfig.spec",
                fileExtension: "json"
            );
        }

        [IntentManaged(Mode.Fully)]
        public override string TransformText() => DataFile.ToString();
    }
}