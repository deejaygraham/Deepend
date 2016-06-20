using System;
using System.IO;

namespace Deepend
{
    public class DotGraphDependencies : IGraphDependencies
    {
		public void Write(Graph<AssemblyInfo> graph, TextWriter writer)
		{
			writer.WriteLine("digraph G");
			writer.WriteLine("{");
			writer.WriteLine("rankdir = LR");

			foreach(var node in graph.Nodes)
			{
				writer.WriteLine(node.Name + "[shape = box]");

				foreach (var edge in graph.EdgesFor(node))
				{
					writer.WriteLine(String.Format("{0} -> {1};", node.Name, edge.Name));
				}
			}

			writer.WriteLine("}");
		}

		public void Write(Graph<TypeInfo> graph, TextWriter writer)
		{
			writer.WriteLine("digraph G");
			writer.WriteLine("{");
			writer.WriteLine("rankdir = LR");

			foreach (var node in graph.Nodes)
			{
				writer.WriteLine(node.Name + "[shape = box]");

				foreach (var edge in graph.EdgesFor(node))
				{
					writer.WriteLine(String.Format("{0} -> {1};", node.Name, edge.Name));
				}
			}

			writer.WriteLine("}");
		}


    }
}
