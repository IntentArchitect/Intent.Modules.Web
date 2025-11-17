using Intent.Engine;
using Intent.Metadata.Models;
using Intent.Modelers.Services.Api;
using Intent.Modules.Common.TypeScript.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intent.Modules.Angular.HttpClients.ImplementationStrategies.Infrastructure;

public interface IImplementationStrategy
{
    bool IsMatch();
    GenerateRequestResult GenerateImplementation();
}

public interface IIsSourceStrategy { }

public interface IIsTargetStrategy { }
