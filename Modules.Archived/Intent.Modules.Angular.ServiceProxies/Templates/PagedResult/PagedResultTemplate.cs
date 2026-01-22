using System;
using System.Collections.Generic;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.TypeScript.Templates.TypescriptTemplateStringInterpolation", Version = "1.0")]

namespace Intent.Modules.Angular.ServiceProxies.Templates.PagedResult
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public partial class PagedResultTemplate
    {
        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public override string TransformText()
        {
            return $$"""
                   export class PagedResult<T> {
                       constructor(totalCount: number, pageCount: number, pageSize: number, pageNumber: number, data: Array<T>) {
                           this.totalCount = totalCount;
                           this.pageCount = pageCount;
                           this.pageSize = pageSize;
                           this.pageNumber = pageNumber;
                           this.data = data;
                       }
                   
                       totalCount: number;
                       pageCount: number;
                       pageSize: number;
                       pageNumber: number;
                       data: Array<T>;
                   }
                   """;
        }
    }
}