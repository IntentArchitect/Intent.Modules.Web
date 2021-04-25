using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Metadata.Models;
using Intent.Utils;

namespace Intent.Modules.Angular.Templates.Component.Controls
{
    public interface IControl
    {
        string TransformText();
    }

    public class ControlWriter
    {
        private readonly IDictionary<string, Func<IElement, IControl>> _controlsRegistry = new Dictionary<string, Func<IElement, IControl>>();

        public ControlWriter()
        {
        }

        public void RegisterControl(string elementTypeId, Func<IElement, IControl> controlActivator)
        {
            _controlsRegistry.Add(elementTypeId, controlActivator);
        }

        public string WriteControls(IEnumerable<IElement> elements, string currentIndent)
        {
            return string.Join($"{System.Environment.NewLine}{currentIndent}", elements.Select(element =>
            {
                if (!_controlsRegistry.ContainsKey(element.SpecializationTypeId))
                {
                    Logging.Log.Warning("No control template has been registered for type: " + element.ToString());
                    return null;
                }

                try
                {
                    return _controlsRegistry[element.SpecializationTypeId](element).TransformText().Trim();
                }
                catch (Exception e)
                {
                    Logging.Log.Warning("Failed to write control for type: " + element.ToString());
                    Logging.Log.Warning(e.ToString());
                    return null;
                }
            }).Where(x => x != null));
        }
    }
}