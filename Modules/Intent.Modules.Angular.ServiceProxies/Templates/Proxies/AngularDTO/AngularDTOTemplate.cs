// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 17.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace Intent.Modules.Angular.ServiceProxies.Templates.Proxies.AngularDTO
{
    using System.Collections.Generic;
    using System.Linq;
    using Intent.Modules.Common;
    using Intent.Modules.Common.Templates;
    using Intent.Modules.Common.TypeScript.Templates;
    using Intent.Metadata.Models;
    using Intent.Modelers.Types.ServiceProxies.Api;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "C:\Dev\Intent.Modules.Web\Modules\Intent.Modules.Angular.ServiceProxies\Templates\Proxies\AngularDTO\AngularDTOTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "17.0.0.0")]
    public partial class AngularDTOTemplate : TypeScriptTemplateBase<Intent.Modelers.Services.Api.DTOModel>
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public override string TransformText()
        {
            this.Write("\r\nexport interface ");
            
            #line 11 "C:\Dev\Intent.Modules.Web\Modules\Intent.Modules.Angular.ServiceProxies\Templates\Proxies\AngularDTO\AngularDTOTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ClassName));
            
            #line default
            #line hidden
            
            #line 11 "C:\Dev\Intent.Modules.Web\Modules\Intent.Modules.Angular.ServiceProxies\Templates\Proxies\AngularDTO\AngularDTOTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GenericTypes));
            
            #line default
            #line hidden
            this.Write(" {\r\n");
            
            #line 12 "C:\Dev\Intent.Modules.Web\Modules\Intent.Modules.Angular.ServiceProxies\Templates\Proxies\AngularDTO\AngularDTOTemplate.tt"
  foreach (var field in Model.Fields) { 
            
            #line default
            #line hidden
            this.Write("    ");
            
            #line 13 "C:\Dev\Intent.Modules.Web\Modules\Intent.Modules.Angular.ServiceProxies\Templates\Proxies\AngularDTO\AngularDTOTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(field.Name.ToCamelCase(true)));
            
            #line default
            #line hidden
            
            #line 13 "C:\Dev\Intent.Modules.Web\Modules\Intent.Modules.Angular.ServiceProxies\Templates\Proxies\AngularDTO\AngularDTOTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(field.TypeReference.IsNullable ? "?" : ""));
            
            #line default
            #line hidden
            this.Write(": ");
            
            #line 13 "C:\Dev\Intent.Modules.Web\Modules\Intent.Modules.Angular.ServiceProxies\Templates\Proxies\AngularDTO\AngularDTOTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(this.GetTypeName(field.TypeReference)));
            
            #line default
            #line hidden
            this.Write(";\r\n");
            
            #line 14 "C:\Dev\Intent.Modules.Web\Modules\Intent.Modules.Angular.ServiceProxies\Templates\Proxies\AngularDTO\AngularDTOTemplate.tt"
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
