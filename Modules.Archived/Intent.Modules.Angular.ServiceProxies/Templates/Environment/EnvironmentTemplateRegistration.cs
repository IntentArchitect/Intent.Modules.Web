using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Metadata.Models;
using Intent.Modules.Common;
using Intent.Modules.Common.Registrations;
using Intent.Registrations;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.TemplateRegistration.SingleFileNoModel", Version = "1.0")]

namespace Intent.Modules.Angular.ServiceProxies.Templates.Environment
{
    [IntentManaged(Mode.Merge, Body = Mode.Merge, Signature = Mode.Fully)]
    public class EnvironmentTemplateRegistration : SingleFileTemplateRegistration
    {
        public override string TemplateId => EnvironmentTemplate.TemplateId;

        [IntentManaged(Mode.Fully)]
        public override ITemplate CreateTemplateInstance(IOutputTarget outputTarget)
        {
            return new EnvironmentTemplate(outputTarget);
        }

        protected override void Register(ITemplateInstanceRegistry registry, IApplication application)
        {
            // This template instance should only be used when the "Intent.Angular.ServiceProxies" module is being used "stand-alone"
            if (application.InstalledModules.Any(x => x.ModuleId == "Intent.Angular"))
            {
                return;
            }

            base.Register(registry, application);
        }
    }
}