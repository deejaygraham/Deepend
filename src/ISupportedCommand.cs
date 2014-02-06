using System.Collections.Generic;

namespace Deepend
{
    public interface ISupportedCommand
    {
        string Name { get; }

        string Syntax { get; }

        void Initialise(IEnumerable<string> arguments);

        void Execute();
    }
}
