using Intent.AI;
using Intent.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intent.Modules.Angular.AITask;

public class TemplateAITaskProvider(Func<IChange[], IOutputFile[], IAITask?> createTask) : IAITaskProvider
{

    public IAITask[] GetTasks(IChange[] changes, IOutputFile[] outputFiles)
    {
        var task = createTask(changes, outputFiles);

        return task != null ? [task] : [];
    }
}
