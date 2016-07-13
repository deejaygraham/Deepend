using System;
using System.Collections.Generic;
using System.IO;

namespace Deepend
{
    public class DotGraphDependencies : IGraphDependencies
    {
		readonly string DotGraphHeader = "digraph G";
		readonly string OpeningBrace = "{";
		readonly string ClosingBrace = "}";
		readonly string GraphLayout = "graph[layout = dot];";
		readonly string NodeStyle = "node[style = filled, shape = box];";
		readonly string Direction = "rankdir = LR";

		public void Write(Graph<AssemblyInfo> graph, TextWriter writer)
		{
			writer.WriteLine(DotGraphHeader);
			writer.WriteLine(OpeningBrace);
			writer.WriteLine(GraphLayout);
			writer.WriteLine(NodeStyle);
			writer.WriteLine(Direction);

			foreach (var node in graph.Nodes)
			{
				foreach (var edge in graph.EdgesFor(node))
				{
					WriteDepends(node.Name, edge.Name, writer);
				}

				WriteMetadata(node.Name, node.Metadata, writer);
			}

			writer.WriteLine(ClosingBrace);
		}

		public void Write(Graph<TypeInfo> graph, TextWriter writer)
		{
			writer.WriteLine(DotGraphHeader);
			writer.WriteLine(OpeningBrace);
			writer.WriteLine(GraphLayout);
			writer.WriteLine(NodeStyle);
			writer.WriteLine(Direction);

			foreach (var node in graph.Nodes)
			{
				foreach (var edge in graph.EdgesFor(node))
				{
					WriteDepends(node.Name, edge.Name, writer);
				}

				WriteMetadata(node.Name, node.Metadata, writer);
			}

			writer.WriteLine(ClosingBrace);
		}

		private void WriteDepends(string from, string to, TextWriter writer)
		{
			writer.WriteLine("\"{0}\" -> \"{1}\";", from, to);
		}

		private void WriteMetadata(string objectName, Dictionary<string, string> metadata, TextWriter writer)
		{
			foreach (var pair in metadata)
			{
				//writer.WriteLine("\"{0}\" [{1}=\"{2}\"];", objectName, pair.Key, pair.Value);
			}
		}
	}
}
