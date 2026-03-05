using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Intent.Engine;
using Intent.Metadata.Models;
using Intent.Modules.Angular.Settings;
using Intent.Modules.Angular.Templates.Core.JsonPatches;
using Intent.Modules.Angular.Templates.Core.JsonPatches.AngularDotJson;
using Intent.Modules.Common;
using Intent.Modules.Common.FileBuilders.DataFileBuilder;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.TypeScript.Events;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.ProjectItemTemplate.Partial", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.Core.AngularDotJsonFile
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class AngularDotJsonFileTemplate : IntentTemplateBase<object>, IDataFileBuilderTemplate
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.Core.AngularDotJsonFileTemplate";

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public AngularDotJsonFileTemplate(IOutputTarget outputTarget, object model = null) : base(TemplateId, outputTarget, model)
        {


            DataFile = new DataFile($"AngularDotJsonFile")
                .WithJsonWriter()
                .WithRootObject(this, @object =>
                {
                    @object
                        .WithValue("$schema", "./node_modules/@angular/cli/lib/config/schema.json")
                        .WithValue("version", 1)
                        .WithValue("newProjectRoot", "projects")
                        .WithObject("projects", projects =>
                        {
                            projects.WithObject(AppNameCamelCased, app =>
                            {
                                app
                                    .WithValue("projectType", "application")
                                    .WithObject("schematics", schematics =>
                                    {
                                        schematics.WithObject("@schematics/angular:component", comp =>
                                        {
                                            comp.WithValue("style", "scss");
                                        });
                                    })
                                    .WithValue("root", "")
                                    .WithValue("sourceRoot", "src")
                                    .WithValue("prefix", "app")
                                    .WithObject("architect", arch =>
                                    {
                                        arch
                                            .WithObject("build", build =>
                                            {
                                                build
                                                    .WithValue("builder", "@angular/build:application")
                                                    .WithObject("options", options =>
                                                    {
                                                        options
                                                            .WithValue("browser", "src/main.ts")
                                                            .WithValue("tsConfig", "tsconfig.app.json")
                                                            .WithValue("inlineStyleLanguage", "scss")
                                                            .WithArray("assets", assets =>
                                                            {
                                                                assets.WithObject(assetObj =>
                                                                {
                                                                    assetObj
                                                                        .WithValue("glob", "**/*")
                                                                        .WithValue("input", "public");
                                                                });
                                                            })
                                                            .WithArray("styles", styles =>
                                                            {
                                                                styles.WithValue("src/styles.scss");
                                                            });
                                                    })
                                                    .WithObject("configurations", config =>
                                                    {
                                                        config.WithObject("production", prod =>
                                                        {
                                                            prod.WithArray("budgets", budgets =>
                                                            {
                                                                budgets.WithObject(budgetObj =>
                                                                {
                                                                    budgetObj
                                                                        .WithValue("type", "initial")
                                                                        .WithValue("maximumWarning", "500kB")
                                                                        .WithValue("maximumError", "1MB");
                                                                });
                                                                budgets.WithObject(budgetObj =>
                                                                {
                                                                    budgetObj
                                                                        .WithValue("type", "anyComponentStyle")
                                                                        .WithValue("maximumWarning", "4kB")
                                                                        .WithValue("maximumError", "8kB");
                                                                });
                                                            })
                                                            .WithValue("outputHashing", "all");
                                                        })
                                                        .WithObject("development", dev =>
                                                        {
                                                            dev
                                                              .WithValue("optimization", false)
                                                              .WithValue("extractLicenses", false)
                                                              .WithValue("sourceMap", true);
                                                        });
                                                    })
                                                    .WithValue("defaultConfiguration", "production");
                                            })
                                            .WithObject("serve", serve =>
                                            {
                                                serve
                                                    .WithValue("builder", "@angular/build:dev-server")
                                                    .WithObject("configurations", config =>
                                                    {
                                                        config.WithObject("production", prod =>
                                                        {
                                                            prod.WithValue("buildTarget", $"{AppNameCamelCased}:build:production");
                                                        });
                                                        config.WithObject("development", dev =>
                                                        {
                                                            dev.WithValue("buildTarget", $"{AppNameCamelCased}:build:development");
                                                        });
                                                    })
                                                    .WithValue("defaultConfiguration", "development");
                                            });
                                    })
                                ;
                            });
                        });
                }).OnBuild(file =>
                {
                    PatchAngularJsonFile(file);
                });
        }

        private void PatchAngularJsonFile(IDataFile file)
        {
            List<IAngularJsonPatch> Patches =
            [
                new CliObjectPatch(ExecutionContext.Settings.GetAngularSettings().AngularVersion().AsEnum()),
                new BuildOptionsPatch(ExecutionContext.Settings.GetAngularSettings().AngularVersion().AsEnum(), AppNameCamelCased, AppNameKebabCased),
                new OptionsPolyFillsPatch(ExecutionContext.Settings.GetAngularSettings().AngularVersion().AsEnum(), AppNameCamelCased),
                new ExtractBuilderApplication(ExecutionContext.Settings.GetAngularSettings().AngularVersion().AsEnum(), AppNameCamelCased),
                new TestBuilderBuildPatch(ExecutionContext.Settings.GetAngularSettings().AngularVersion().AsEnum(), AppNameCamelCased),
                new TestBuilderUnitTestPatch(ExecutionContext.Settings.GetAngularSettings().AngularVersion().AsEnum(), AppNameCamelCased),
            ];

            foreach (var patch in Patches
                .Where(p => p.Applicable())
                .OrderBy(p => p.Order))
            {
                patch.Apply(file);
            }
        }

        private string AppNameCamelCased => OutputTarget.ApplicationName().ToCamelCase();

        private string AppNameKebabCased => OutputTarget.ApplicationName().ToKebabCase();

        [IntentManaged(Mode.Fully)]
        public IDataFile DataFile { get; }

        [IntentManaged(Mode.Merge)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new TemplateFileConfig(
                fileName: $"angular",
                fileExtension: "json"
            );
        }

        [IntentManaged(Mode.Fully)]
        public override string TransformText() => DataFile.ToString();

    }
}