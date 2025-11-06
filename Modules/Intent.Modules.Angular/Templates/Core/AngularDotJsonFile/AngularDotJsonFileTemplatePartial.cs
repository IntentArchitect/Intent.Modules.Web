using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Intent.Engine;
using Intent.Metadata.Models;
using Intent.Modules.Common;
using Intent.Modules.Common.FileBuilders.DataFileBuilder;
using Intent.Modules.Common.Templates;
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
                                    .WithObject("schematics", schematics => { })
                                    .WithValue("root", "")
                                    .WithValue("sourceRoot", "src")
                                    .WithValue("prefix", "app")
                                    .WithObject("architect", arch =>
                                    {
                                        arch
                                            .WithObject("build", build =>
                                        {
                                            build
                                                .WithValue("builder", "@angular-devkit/build-angular:application")
                                                .WithObject("options", options =>
                                                {
                                                    options
                                                        .WithValue("outputPath", $"dist/{AppNameKebabCased}")
                                                        .WithValue("index", "src/index.html")
                                                        .WithValue("browser", "src/main.ts")
                                                        .WithArray("polyfills", poly =>
                                                        {
                                                            poly.WithValue("zone.js");
                                                        })
                                                        .WithValue("tsConfig", "tsconfig.app.json")
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
                                                            styles.WithValue("src/styles.css");
                                                        })
                                                        .WithArray("scripts", scripts => { });
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
                                                    .WithValue("builder", "@angular-devkit/build-angular:dev-server")
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
                                            })
                                            .WithObject("extract-i18n", extract =>
                                            {
                                                extract.WithValue("builder", "@angular-devkit/build-angular:extract-i18n");
                                            })
                                            .WithObject("test", test =>
                                            {
                                                test
                                                    .WithValue("builder", "@angular-devkit/build-angular:karma")
                                                    .WithObject("options", options =>
                                                    {
                                                        options.WithArray("polyfills", poly =>
                                                        {
                                                            poly
                                                                .WithValue("zone.js")
                                                                .WithValue("zone.js/testing");
                                                        })
                                                        .WithValue("tsConfig", "tsconfig.spec.json")
                                                        .WithArray("assets", assets =>
                                                        {
                                                            assets.WithObject(assetsObj =>
                                                            {
                                                                assetsObj
                                                                    .WithValue("glob", "**/*")
                                                                    .WithValue("input", "public");
                                                            });
                                                        }).
                                                        WithArray("styles", styles =>
                                                        {
                                                            styles.WithValue("src/styles.css");
                                                        })
                                                        .WithArray("scripts", scripts => { });
                                                    });
                                            });
                                    })
                                ;
                            });
                        })
                        ;
                });
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