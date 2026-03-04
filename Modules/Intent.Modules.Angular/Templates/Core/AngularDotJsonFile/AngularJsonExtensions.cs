using Intent.Engine;
using Intent.Modules.Angular.Settings;
using Intent.Modules.Common.FileBuilders.DataFileBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Intent.Modules.Angular.Templates.Core.AngularDotJsonFile;

public static class AngularJsonExtensions
{
    public static IDataFileObjectValue WithCliObject(this IDataFileObjectValue @object, ISoftwareFactoryExecutionContext executionContext)
    {
        if (executionContext.Settings.GetAngularSettings().AngularVersion().AsEnum() != AngularSettings.AngularVersionOptionsEnum._210)
        {
            return @object;
        }

        return @object.WithObject("cli", cli =>
        {
            cli.WithValue("packageManager", "npm");
        });
    }

    public static IDataFileObjectValue WithBuildObject(this IDataFileObjectValue @object, ISoftwareFactoryExecutionContext executionContext)
    {
        if (executionContext.Settings.GetAngularSettings().AngularVersion().AsEnum() != AngularSettings.AngularVersionOptionsEnum._210)
        {
            return @object;
        }

        return @object.WithObject("build", build =>
        {
            build
                .WithValue("builder", GetBuilderModule(executionContext))
                .WithObject("options", options =>
                {
                    //Add(options);

                    options.WithValue("browser", "src/main.ts");


                    options.AddPolyfillsArray(executionContext);

                    options.WithValue("tsConfig", "tsconfig.app.json")
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

                   // AddScriptArray(options);
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
                
        });
    }

    private static string GetBuilderModule(ISoftwareFactoryExecutionContext executionContext) => executionContext.Settings.GetAngularSettings().AngularVersion().AsEnum() switch
    {
        AngularSettings.AngularVersionOptionsEnum._192 => "@angular-devkit/build-angular:application",
        _ => "@angular/build:application"
    };

    private static string GetServeModule(ISoftwareFactoryExecutionContext executionContext) => executionContext.Settings.GetAngularSettings().AngularVersion().AsEnum() switch
    {
        AngularSettings.AngularVersionOptionsEnum._192 => "@angular-devkit/build-angular:dev-server",
        _ => "@angular/build:dev-server"
    };

    private static string GetExtractModule(ISoftwareFactoryExecutionContext executionContext) => executionContext.Settings.GetAngularSettings().AngularVersion().AsEnum() switch
    {
        AngularSettings.AngularVersionOptionsEnum._192 => "@angular-devkit/build-angular:extract-i18n",
        _ => "@angular/build:extract-i18n"
    };

    private static string GetTestModule(ISoftwareFactoryExecutionContext executionContext) => executionContext.Settings.GetAngularSettings().AngularVersion().AsEnum() switch
    {
        AngularSettings.AngularVersionOptionsEnum._192 => "@angular-devkit/build-angular:karma",
        _ => "@angular/build:karma"
    };

    private static IDataFileObjectValue AddPolyfillsArray(this IDataFileObjectValue options, ISoftwareFactoryExecutionContext executionContext)
    {
        if (executionContext.Settings.GetAngularSettings().AngularVersion().AsEnum() != AngularSettings.AngularVersionOptionsEnum._210)
        {
            return options;
        }

        return options.WithArray("polyfills", poly =>
        {
            poly.WithValue("zone.js");
        });
    }

    //private void AddExtractObject(IDataFileObjectValue arch)
    //{
    //    arch.WithObject("extract-i18n", extract =>
    //    {
    //        extract.WithValue("builder", ExtractBuilderModule);
    //    });
    //}



    //private void AddScriptArray(IDataFileObjectValue options)
    //{
    //    if (ExecutionContext.Settings.GetAngularSettings().AngularVersion().AsEnum() != AngularSettings.AngularVersionOptionsEnum._192)
    //    {
    //        return;
    //    }

    //    options.WithArray("scripts", scripts => { });
    //}


    //private static IDataFileObjectValue AddOutputTargetIndex(this IDataFileObjectValue options, ISoftwareFactoryExecutionContext executionContext)
    //{
    //    if (executionContext.Settings.GetAngularSettings().AngularVersion().AsEnum() != AngularSettings.AngularVersionOptionsEnum._192)
    //    {
    //        return options;
    //    }

    //    return options
    //        .WithValue("outputPath", $"dist/{AppNameKebabCased}")
    //        .WithValue("index", "src/index.html");
    //}

    //private static string AppNameCamelCased => OutputTarget.ApplicationName().ToCamelCase();

    //private static string AppNameKebabCased => OutputTarget.ApplicationName().ToKebabCase();

}
