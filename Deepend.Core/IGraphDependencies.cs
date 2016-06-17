using System;
using System.IO;

namespace Deepend
{
    public interface IGraphDependencies
    {
		void Node(Node n);
		void Edge(Edge e);

		void SaveTo(TextWriter writer);

		void Write(Graph<AssemblyInfo> graph, TextWriter writer);
		void Write(Graph<TypeInfo> graph, TextWriter writer);
		
    }
}
