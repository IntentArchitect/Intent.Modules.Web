using Intent.IArchitect.Common.Publishing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intent.Modules.Angular.HttpClients.ImplementationStrategies.Infrastructure;

public class GenerateRequestResult(string RequestName, string RequestStatement)
{
    public string RequestName { get; } = RequestName;
    public string RequestStatement { get; } = RequestStatement;
}