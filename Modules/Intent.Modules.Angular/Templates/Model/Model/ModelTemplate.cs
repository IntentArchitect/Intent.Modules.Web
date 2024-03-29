// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 17.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace Intent.Modules.Angular.Templates.Model.Model
{
    using System.Collections.Generic;
    using System.Linq;
    using Intent.Modules.Common;
    using Intent.Modules.Common.Templates;
    using Intent.Modules.Common.TypeScript.Templates;
    using Intent.Templates;
    using Intent.Metadata.Models;
    using Intent.Modules.Angular.Api;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "C:\Dev\Intent.Modules.Web\Intent.Modules.Angular\Templates\Model\Model\ModelTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "17.0.0.0")]
    public partial class ModelTemplate : TypeScriptTemplateBase<Intent.Modelers.WebClient.Angular.Api.ModelDefinitionModel>
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public override string TransformText()
        {
            this.Write("\r\nexport class ");
            
            #line 12 "C:\Dev\Intent.Modules.Web\Intent.Modules.Angular\Templates\Model\Model\ModelTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ClassName));
            
            #line default
            #line hidden
            
            #line 12 "C:\Dev\Intent.Modules.Web\Intent.Modules.Angular\Templates\Model\Model\ModelTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GetGenericParameters()));
            
            #line default
            #line hidden
            this.Write(" { \r\n");
            
            #line 13 "C:\Dev\Intent.Modules.Web\Intent.Modules.Angular\Templates\Model\Model\ModelTemplate.tt"
  if (Model.Mapping != null) { 
            
            #line default
            #line hidden
            this.Write("  public static create");
            
            #line 14 "C:\Dev\Intent.Modules.Web\Intent.Modules.Angular\Templates\Model\Model\ModelTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GetGenericParameters()));
            
            #line default
            #line hidden
            this.Write("(dto: ");
            
            #line 14 "C:\Dev\Intent.Modules.Web\Intent.Modules.Angular\Templates\Model\Model\ModelTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GetTypeName(Model.Mapping)));
            
            #line default
            #line hidden
            
            #line 14 "C:\Dev\Intent.Modules.Web\Intent.Modules.Angular\Templates\Model\Model\ModelTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GetGenericParameters()));
            
            #line default
            #line hidden
            this.Write("): ");
            
            #line 14 "C:\Dev\Intent.Modules.Web\Intent.Modules.Angular\Templates\Model\Model\ModelTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ClassName));
            
            #line default
            #line hidden
            
            #line 14 "C:\Dev\Intent.Modules.Web\Intent.Modules.Angular\Templates\Model\Model\ModelTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GetGenericParameters()));
            
            #line default
            #line hidden
            this.Write(" {\r\n    var model = new ");
            
            #line 15 "C:\Dev\Intent.Modules.Web\Intent.Modules.Angular\Templates\Model\Model\ModelTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ClassName));
            
            #line default
            #line hidden
            
            #line 15 "C:\Dev\Intent.Modules.Web\Intent.Modules.Angular\Templates\Model\Model\ModelTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GetGenericParameters()));
            
            #line default
            #line hidden
            this.Write("();\r\n");
            
            #line 16 "C:\Dev\Intent.Modules.Web\Intent.Modules.Angular\Templates\Model\Model\ModelTemplate.tt"
 foreach (var field in Model.Fields.Where(x => x.InternalElement.IsMapped)) {
            
            #line default
            #line hidden
            this.Write("    model.");
            
            #line 17 "C:\Dev\Intent.Modules.Web\Intent.Modules.Angular\Templates\Model\Model\ModelTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(field.Name.ToCamelCase()));
            
            #line default
            #line hidden
            this.Write(" = dto.");
            
            #line 17 "C:\Dev\Intent.Modules.Web\Intent.Modules.Angular\Templates\Model\Model\ModelTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GetPath(field.InternalElement.MappedElement.Path)));
            
            #line default
            #line hidden
            this.Write(";\r\n");
            
            #line 18 "C:\Dev\Intent.Modules.Web\Intent.Modules.Angular\Templates\Model\Model\ModelTemplate.tt"
}
            
            #line default
            #line hidden
            this.Write("    return model;\r\n  }\r\n\r\n");
            
            #line 22 "C:\Dev\Intent.Modules.Web\Intent.Modules.Angular\Templates\Model\Model\ModelTemplate.tt"
  } 
            
            #line default
            #line hidden
            
            #line 23 "C:\Dev\Intent.Modules.Web\Intent.Modules.Angular\Templates\Model\Model\ModelTemplate.tt"
 foreach (var field in Model.Fields) {
            
            #line default
            #line hidden
            this.Write("  ");
            
            #line 24 "C:\Dev\Intent.Modules.Web\Intent.Modules.Angular\Templates\Model\Model\ModelTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(field.Name.ToCamelCase()));
            
            #line default
            #line hidden
            this.Write(": ");
            
            #line 24 "C:\Dev\Intent.Modules.Web\Intent.Modules.Angular\Templates\Model\Model\ModelTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GetTypeName(field.TypeReference)));
            
            #line default
            #line hidden
            this.Write(";\r\n");
            
            #line 25 "C:\Dev\Intent.Modules.Web\Intent.Modules.Angular\Templates\Model\Model\ModelTemplate.tt"
}
            
            #line default
            #line hidden
            this.Write("}");
            return this.GenerationEnvironment.ToString();
        }
    }
    
    #line default
    #line hidden
}
