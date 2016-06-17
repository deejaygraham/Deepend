﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deepend
{
	public class ConsoleDependencyWriter
	{
		public void Write(Graph<AssemblyInfo> graph)
		{
			int count = 0;

			foreach(var node in graph.Nodes)
			{
				Console.WriteLine("{0} {1} {2}", ++count, node.Name, node.Version);

				int edgeCount = 0;

				foreach(var edge in graph.EdgesFor(node))
				{
					Console.WriteLine("\t{0} {1} {2}", ++edgeCount, edge.Name, edge.Version);
				}
			}
		}
	}

	public class AnalyseAssemblyReferences : ISupportedCommand
	{
		public string AssemblyName { get; set; }

		public ReferenceDepth Depth { get; set; }

		public void Execute(IGraphDependencies graph)
		{
			if (!File.Exists(this.AssemblyName))
			{
				throw new FileNotFoundException("File " + this.AssemblyName + " not found");
			}

			var assemblyList = new List<string>();

			var graph2 = AssemblyReferenceBuilder.Build(this.AssemblyName, this.Depth);

			var cdw = new ConsoleDependencyWriter();
			cdw.Write(graph2);

			var ar = AssemblyReferenceBuilder.Load(this.AssemblyName, this.Depth, 0, assemblyList);

			var list = new HashSet<IGraphable>();

			var globalAssemblyCache = new GroupNode("GAC", "Global Assembly Cache");

			list.Add(globalAssemblyCache);

			foreach (var item in ar.Generate(globalAssemblyCache))
			{
				list.Add(item);
			}

			foreach (var item in list)
			{
				item.WriteTo(graph);
			}
		}
	}
}
