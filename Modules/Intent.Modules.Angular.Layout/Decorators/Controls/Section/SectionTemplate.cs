// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 16.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace Intent.Modules.Angular.Layout.Decorators.Controls.Section
{
    using System.Collections.Generic;
    using System.Linq;
    using Intent.Modules.Common;
    using Intent.Modules.Common.Templates;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "C:\Dev\Intent.Modules.Web\Intent.Modules.Angular.Layout\Decorators\Controls\Section\SectionTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "16.0.0.0")]
    public partial class SectionTemplate : T4TemplateBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public override string TransformText()
        {
            this.Write("<div class=\"row\" intent-id=\"");
            
            #line 7 "C:\Dev\Intent.Modules.Web\Intent.Modules.Angular.Layout\Decorators\Controls\Section\SectionTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.Name));
            
            #line default
            #line hidden
            this.Write("\">\r\n  <div class=\"col\" intent-manage>\r\n");
            
            #line 9 "C:\Dev\Intent.Modules.Web\Intent.Modules.Angular.Layout\Decorators\Controls\Section\SectionTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ControlWriter.WriteControls(Model.InternalElement.ChildElements, "    ")));
            
            #line default
            #line hidden
            this.Write("\r\n  </div>\r\n</div>\r\n");
            return this.GenerationEnvironment.ToString();
        }
    }
    
    #line default
    #line hidden
}