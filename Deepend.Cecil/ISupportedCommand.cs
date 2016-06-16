using System.Collections.Generic;

namespace Deepend
{
    public interface ISupportedCommand
    {
        void Execute(IGraphDependencies graph);
    }
}
