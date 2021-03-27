// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 16.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace Intent.Modules.Angular.Templates.Component.AngularComponentTsTemplate
{
    using System.Collections.Generic;
    using System.Linq;
    using Intent.Modules.Common;
    using Intent.Modules.Common.Templates;
    using Intent.Modules.Common.TypeScript.Templates;
    using Intent.Modules.Angular.Api;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "C:\Dev\Intent.Modules.Web\Intent.Modules.Angular\Templates\Component\AngularComponentTsTemplate\AngularComponentTsTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "16.0.0.0")]
    public partial class AngularComponentTsTemplate : TypeScriptTemplateBase<Intent.Angular.Api.ComponentModel>
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public override string TransformText()
        {
            this.Write("import { Component, OnInit } from \'@angular/core\';");
            
            #line 9 "C:\Dev\Intent.Modules.Web\Intent.Modules.Angular\Templates\Component\AngularComponentTsTemplate\AngularComponentTsTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GetImports()));
            
            #line default
            #line hidden
            this.Write("\r\n\r\n@Component({\r\n  selector: \'app-");
            
            #line 12 "C:\Dev\Intent.Modules.Web\Intent.Modules.Angular\Templates\Component\AngularComponentTsTemplate\AngularComponentTsTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ComponentName.ToKebabCase()));
            
            #line default
            #line hidden
            this.Write("\',\r\n  templateUrl: \'./");
            
            #line 13 "C:\Dev\Intent.Modules.Web\Intent.Modules.Angular\Templates\Component\AngularComponentTsTemplate\AngularComponentTsTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ComponentName.ToKebabCase()));
            
            #line default
            #line hidden
            this.Write(".component.html\',\r\n  styleUrls: [\'./");
            
            #line 14 "C:\Dev\Intent.Modules.Web\Intent.Modules.Angular\Templates\Component\AngularComponentTsTemplate\AngularComponentTsTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ComponentName.ToKebabCase()));
            
            #line default
            #line hidden
            this.Write(".component.css\']\r\n})\r\nexport class ");
            
            #line 16 "C:\Dev\Intent.Modules.Web\Intent.Modules.Angular\Templates\Component\AngularComponentTsTemplate\AngularComponentTsTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ClassName));
            
            #line default
            #line hidden
            this.Write(" implements OnInit {\r\n");
            
            #line 17 "C:\Dev\Intent.Modules.Web\Intent.Modules.Angular\Templates\Component\AngularComponentTsTemplate\AngularComponentTsTemplate.tt"
  foreach (var model in Model.Models) {
            
            #line default
            #line hidden
            this.Write("\r\n  ");
            
            #line 19 "C:\Dev\Intent.Modules.Web\Intent.Modules.Angular\Templates\Component\AngularComponentTsTemplate\AngularComponentTsTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(model.Name));
            
            #line default
            #line hidden
            this.Write(": ");
            
            #line 19 "C:\Dev\Intent.Modules.Web\Intent.Modules.Angular\Templates\Component\AngularComponentTsTemplate\AngularComponentTsTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GetTypeName(model.TypeReference)));
            
            #line default
            #line hidden
            this.Write(";\r\n");
            
            #line 20 "C:\Dev\Intent.Modules.Web\Intent.Modules.Angular\Templates\Component\AngularComponentTsTemplate\AngularComponentTsTemplate.tt"
  }
            
            #line default
            #line hidden
            this.Write("\r\n  //@IntentCanAdd()\r\n  constructor(");
            
            #line 23 "C:\Dev\Intent.Modules.Web\Intent.Modules.Angular\Templates\Component\AngularComponentTsTemplate\AngularComponentTsTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GetConstructorParams()));
            
            #line default
            #line hidden
            this.Write(") { }\r\n\r\n  ");
            
            #line 25 "C:\Dev\Intent.Modules.Web\Intent.Modules.Angular\Templates\Component\AngularComponentTsTemplate\AngularComponentTsTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(this.IntentIgnoreDecorator()));
            
            #line default
            #line hidden
            this.Write("\r\n  ngOnInit() {\r\n  }\r\n");
            
            #line 28 "C:\Dev\Intent.Modules.Web\Intent.Modules.Angular\Templates\Component\AngularComponentTsTemplate\AngularComponentTsTemplate.tt"
  foreach (var command in Model.Commands) {
            
            #line default
            #line hidden
            this.Write("\r\n  ");
            
            #line 30 "C:\Dev\Intent.Modules.Web\Intent.Modules.Angular\Templates\Component\AngularComponentTsTemplate\AngularComponentTsTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(this.IntentIgnoreBodyDecorator()));
            
            #line default
            #line hidden
            this.Write("\r\n  ");
            
            #line 31 "C:\Dev\Intent.Modules.Web\Intent.Modules.Angular\Templates\Component\AngularComponentTsTemplate\AngularComponentTsTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(command.Name.ToCamelCase()));
            
            #line default
            #line hidden
            this.Write("(");
            
            #line 31 "C:\Dev\Intent.Modules.Web\Intent.Modules.Angular\Templates\Component\AngularComponentTsTemplate\AngularComponentTsTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GetParameters(command)));
            
            #line default
            #line hidden
            this.Write("): ");
            
            #line 31 "C:\Dev\Intent.Modules.Web\Intent.Modules.Angular\Templates\Component\AngularComponentTsTemplate\AngularComponentTsTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GetReturnType(command)));
            
            #line default
            #line hidden
            this.Write(" {\r\n    // write your business logic here for this command\r\n  }\r\n");
            
            #line 34 "C:\Dev\Intent.Modules.Web\Intent.Modules.Angular\Templates\Component\AngularComponentTsTemplate\AngularComponentTsTemplate.tt"
  }
            
            #line default
            #line hidden
            this.Write("}\r\n");
            return this.GenerationEnvironment.ToString();
        }
    }
    
    #line default
    #line hidden
}
