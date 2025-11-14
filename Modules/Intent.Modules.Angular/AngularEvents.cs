using Intent.Modelers.UI.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Intent.Modules.Angular;

public class ComponentCreatedEvent
{
    public ComponentCreatedEvent(string componentName, string selector, string route, ComponentModel model)
    {
        ComponentName = componentName;
        Selector = selector;
        Route = route;
        Model = model;
    }
    
    public string ComponentName { get; }

    public string Selector { get; }

    public string Route { get; }

    public ComponentModel Model { get; }
}
