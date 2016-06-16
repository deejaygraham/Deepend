using System.IO;

namespace Deepend
{
    public interface IGraphDependencies
    {
		void Node(Node n);
		void Edge(Edge e);

        void SaveTo(TextWriter writer);

		
    }
}
