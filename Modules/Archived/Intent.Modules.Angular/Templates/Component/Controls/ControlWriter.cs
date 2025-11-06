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

        public string CurrentIndent = "";

        public void RegisterControl(string elementTypeId, Func<IElement, IControl> controlActivator)
        {
            _controlsRegistry.Add(elementTypeId, controlActivator);
        }

        public string WriteControls(IEnumerable<IElement> elements, string currentIndent)
        {
            var previousIndent = CurrentIndent;
            CurrentIndent = currentIndent;
            var result = string.Join($"{System.Environment.NewLine}", elements.Select(element =>
            {
                return WriteControl(element, currentIndent);
                //if (!_controlsRegistry.ContainsKey(element.SpecializationTypeId))
                //{
                //    Logging.Log.Warning("No control template has been registered for type: " + element.ToString());
                //    return null;
                //}

                //try
                //{
                //    var lines = _controlsRegistry[element.SpecializationTypeId](element).TransformText()
                //        .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                //    return currentIndent + string.Join($"{System.Environment.NewLine}{currentIndent}", lines);
                //}
                //catch (Exception e)
                //{
                //    Logging.Log.Warning("Failed to write control for type: " + element.ToString());
                //    Logging.Log.Warning(e.ToString());
                //    return null;
                //}
            }).Where(x => x != null));
            CurrentIndent = previousIndent;
            return result;
        }

        public string WriteControl(IElement element, string currentIndent)
        {
            var previousIndent = CurrentIndent;
            CurrentIndent = currentIndent;
            if (!_controlsRegistry.ContainsKey(element.SpecializationTypeId))
            {
                Logging.Log.Warning("No control template has been registered for type: " + element.ToString());
                CurrentIndent = previousIndent;
                return null;
            }

            try
            {
                var lines = _controlsRegistry[element.SpecializationTypeId](element).TransformText()
                    .Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);
                return $"{System.Environment.NewLine}{currentIndent + string.Join($"{System.Environment.NewLine}{currentIndent}", lines)}";
            }
            catch (Exception e)
            {
                Logging.Log.Warning("Failed to write control for type: " + element.ToString());
                Logging.Log.Warning(e.ToString());
                return null;
            }
            finally
            {
                CurrentIndent = previousIndent;
            }
        }
    }
}