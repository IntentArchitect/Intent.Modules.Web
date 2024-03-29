using System;
using System.Collections.Generic;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.FileTemplateStringInterpolation", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.Core.AngularDotJsonFile
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public partial class AngularDotJsonFileTemplate
    {
        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public override string TransformText()
        {
            return @$"{{
  ""$schema"": ""./node_modules/@angular/cli/lib/config/schema.json"",
  ""version"": 1,
  ""cli"": {{
    ""packageManager"": ""npm""
  }},
  ""newProjectRoot"": ""projects"",
  ""projects"": {{
    ""{AppNameCamelCased}"": {{
      ""projectType"": ""application"",
      ""schematics"": {{
        ""@schematics/angular:component"": {{
          ""style"": ""scss""
        }}
      }},
      ""root"": """",
      ""sourceRoot"": ""src"",
      ""prefix"": ""app"",
      ""architect"": {{
        ""build"": {{
          ""builder"": ""@angular-devkit/build-angular:browser"",
          ""options"": {{
            ""outputPath"": ""dist/{AppNameKebabCased}"",
            ""index"": ""src/index.html"",
            ""main"": ""src/main.ts"",
            ""polyfills"": [
              ""zone.js""
            ],
            ""tsConfig"": ""tsconfig.app.json"",
            ""inlineStyleLanguage"": ""scss"",
            ""assets"": [
              ""src/favicon.ico"",
              ""src/assets""
            ],
            ""styles"": [
              ""src/styles.scss""
            ],
            ""scripts"": []
          }},
          ""configurations"": {{
            ""production"": {{
              ""budgets"": [
                {{
                  ""type"": ""initial"",
                  ""maximumWarning"": ""500kb"",
                  ""maximumError"": ""1mb""
                }},
                {{
                  ""type"": ""anyComponentStyle"",
                  ""maximumWarning"": ""2kb"",
                  ""maximumError"": ""4kb""
                }}
              ],
              ""outputHashing"": ""all""
            }},
            ""development"": {{
              ""buildOptimizer"": false,
              ""optimization"": false,
              ""vendorChunk"": true,
              ""extractLicenses"": false,
              ""sourceMap"": true,
              ""namedChunks"": true
            }}
          }},
          ""defaultConfiguration"": ""production""
        }},
        ""serve"": {{
          ""builder"": ""@angular-devkit/build-angular:dev-server"",
          ""configurations"": {{
            ""production"": {{
              ""browserTarget"": ""{AppNameCamelCased}:build:production""
            }},
            ""development"": {{
              ""browserTarget"": ""{AppNameCamelCased}:build:development""
            }}
          }},
          ""defaultConfiguration"": ""development""
        }},
        ""extract-i18n"": {{
          ""builder"": ""@angular-devkit/build-angular:extract-i18n"",
          ""options"": {{
            ""browserTarget"": ""{AppNameCamelCased}:build""
          }}
        }},
        ""test"": {{
          ""builder"": ""@angular-devkit/build-angular:karma"",
          ""options"": {{
            ""polyfills"": [
              ""zone.js"",
              ""zone.js/testing""
            ],
            ""tsConfig"": ""tsconfig.spec.json"",
            ""inlineStyleLanguage"": ""scss"",
            ""assets"": [
              ""src/favicon.ico"",
              ""src/assets""
            ],
            ""styles"": [
              ""src/styles.scss""
            ],
            ""scripts"": []
          }}
        }}
      }}
    }}
  }}
}}
";
        }
    }
}