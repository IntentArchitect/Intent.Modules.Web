using Intent.AI;
using Intent.Engine;
using Intent.Modules.Common.Templates;
using Intent.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Intent.Modules.Common;
using Intent.Modules.Common.Templates;

namespace Intent.Modules.Angular.AITask;

public class TemplateAITask : IAITask
{
    private readonly IIntentTemplate _template;
    public TemplateAITask(IIntentTemplate template, ITemplate[] filesToInclude)
    {
        Id = ((IntentTemplateBase)template).GetCorrelationId() ?? throw new ArgumentException("CorrelationId could not be found for template", nameof(template));
        _template = template;
        FilesToInclude = [.. filesToInclude.Select(x => x.GetMetadata().GetFilePath())];

        RelatedTemplates = _template.GetAllTemplateDependencies()
            .Select(x => _template.ExecutionContext.FindTemplateInstance(x))
            .Concat(filesToInclude)
            .Distinct()
            .ToList();
    }

    public string Id { get; }

    public ITemplate Template => _template;

    public string Type { get; init; }

    public string Title { get; init; }

    public string Instructions { get; init; }

    public string Context { get; init; }

    public IList<string> FilesToInclude { get; }

    public IList<ITemplate> RelatedTemplates { get; }

    public virtual bool IsApplicableToChanges(IChange[] changes)
    {
        if (changes.Any(change => change.Template == _template)
            || changes.Any(change => RelatedTemplates.Contains(change.Template)))
        {
            return true;
        }

        return false;
    }
}
