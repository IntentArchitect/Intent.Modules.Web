// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 16.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace Intent.Modules.Angular.Templates.Module.AngularModuleTemplate
{
    using System.Collections.Generic;
    using System.Linq;
    using Intent.Modules.Common;
    using Intent.Modules.Common.TypeScript.Templates;
    using Intent.Modules.Angular.Api;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "C:\Dev\Intent.Modules.Web\Intent.Modules.Angular\Templates\Module\AngularModuleTemplate\AngularModuleTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "16.0.0.0")]
    public partial class AngularModuleTemplate : TypeScriptTemplateBase<Intent.Modelers.WebClient.Angular.Api.ModuleModel>
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public override string TransformText()
        {
            this.Write("import { NgModule } from \'@angular/core\';\r\nimport { CommonModule } from \'@angular" +
                    "/common\';");
            
            #line 9 "C:\Dev\Intent.Modules.Web\Intent.Modules.Angular\Templates\Module\AngularModuleTemplate\AngularModuleTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GetImports()));
            
            #line default
            #line hidden
            this.Write("\r\n\r\n");
            
            #line 11 "C:\Dev\Intent.Modules.Web\Intent.Modules.Angular\Templates\Module\AngularModuleTemplate\AngularModuleTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(this.IntentMergeDecorator()));
            
            #line default
            #line hidden
            this.Write("\r\n@NgModule({\r\n  declarations: [");
            
            #line 13 "C:\Dev\Intent.Modules.Web\Intent.Modules.Angular\Templates\Module\AngularModuleTemplate\AngularModuleTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GetComponents()));
            
            #line default
            #line hidden
            this.Write("],\r\n");
            
            #line 14 "C:\Dev\Intent.Modules.Web\Intent.Modules.Angular\Templates\Module\AngularModuleTemplate\AngularModuleTemplate.tt"
  if (HasProviders()) { 
            
            #line default
            #line hidden
            this.Write("  providers: [");
            
            #line 15 "C:\Dev\Intent.Modules.Web\Intent.Modules.Angular\Templates\Module\AngularModuleTemplate\AngularModuleTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GetProviders()));
            
            #line default
            #line hidden
            this.Write("],\r\n");
            
            #line 16 "C:\Dev\Intent.Modules.Web\Intent.Modules.Angular\Templates\Module\AngularModuleTemplate\AngularModuleTemplate.tt"
  } 
            
            #line default
            #line hidden
            this.Write("  imports: [\r\n    CommonModule,");
            
            #line 18 "C:\Dev\Intent.Modules.Web\Intent.Modules.Angular\Templates\Module\AngularModuleTemplate\AngularModuleTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GetAngularImports()));
            
            #line default
            #line hidden
            this.Write("\r\n  ]\r\n})\r\nexport class ");
            
            #line 21 "C:\Dev\Intent.Modules.Web\Intent.Modules.Angular\Templates\Module\AngularModuleTemplate\AngularModuleTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ClassName));
            
            #line default
            #line hidden
            this.Write(" { }\r\n");
            return this.GenerationEnvironment.ToString();
        }
    }
    
    #line default
    #line hidden
}
