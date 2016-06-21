using System;
using System.IO;

namespace Deepend
{
    public class DotGraphDependencies : IGraphDependencies
    {
		readonly string DotGraphHeader = "digraph G";
		readonly string OpeningBrace = "{";
		readonly string ClosingBrace = "}";

		public void Write(Graph<AssemblyInfo> graph, TextWriter writer)
		{
			writer.WriteLine(DotGraphHeader);
			writer.WriteLine(OpeningBrace);
			writer.WriteLine("rankdir = LR");

			foreach(var node in graph.Nodes)
			{
				writer.WriteLine(node.Name + "[shape = box]");

				foreach (var edge in graph.EdgesFor(node))
				{
					writer.WriteLine(String.Format("{0} -> {1};", node.Name, edge.Name));
				}
			}

			writer.WriteLine(ClosingBrace);
		}

		public void Write(Graph<TypeInfo> graph, TextWriter writer)
		{
			writer.WriteLine(DotGraphHeader);
			writer.WriteLine(OpeningBrace);
			writer.WriteLine("rankdir = LR");

			foreach (var node in graph.Nodes)
			{
				writer.WriteLine(node.Name + "[shape = box]");

				foreach (var edge in graph.EdgesFor(node))
				{
					writer.WriteLine(String.Format("{0} -> {1};", node.Name, edge.Name));
				}
			}

			writer.WriteLine(ClosingBrace);
		}
    }
}
