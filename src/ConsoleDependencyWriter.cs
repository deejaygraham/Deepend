using System;
using System.Collections.Generic;
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

			foreach (var node in graph.Nodes)
			{
				Console.WriteLine("{0} {1} {2}", ++count, node.Name, node.Version);

				int edgeCount = 0;

				foreach (var edge in graph.EdgesFor(node))
				{
					Console.WriteLine("\t{0} {1} {2}", ++edgeCount, edge.Name, edge.Version);
				}
			}
		}
	}
}
