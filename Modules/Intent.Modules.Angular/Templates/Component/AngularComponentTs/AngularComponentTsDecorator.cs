using System.Collections.Generic;
using System.Linq;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Ignore)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateDecoratorContract", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.Component.AngularComponentTs
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public abstract class AngularComponentTsDecorator : ITemplateDecorator
    {
        public int Priority { get; protected set; } = 0;

        public virtual string OnInit() => null;

        public virtual IEnumerable<string> GetFields() => Enumerable.Empty<string>();
    }
}