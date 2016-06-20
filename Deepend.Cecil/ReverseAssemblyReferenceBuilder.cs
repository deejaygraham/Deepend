using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Deepend
{
	public static class ReverseAssemblyReferenceBuilder
	{
		public static Graph<AssemblyInfo> Build(string assembly, string folder)
		{
			var graph = new Graph<AssemblyInfo>();

			var allAssemblies = new List<string>(Directory.EnumerateFiles(folder, "*.dll"));

			Build(graph, new Graph<AssemblyInfo>(), assembly, folder, allAssemblies, new List<string>());

			return graph;
		}

		private static void Build(Graph<AssemblyInfo> graph, Graph<AssemblyInfo> reverseGraph, string assemblyName, string folder, List<string> allAssemblies, List<string> alreadySeen)
		{
			var reflection = AssemblyDefinition.ReadAssembly(assemblyName);

			var assembly = new AssemblyInfo
			(
				reflection.Name.Name,
				reflection.Name.Version,
				reflection.MainModule.Runtime.ToString().Replace("_", "."),
				AssemblyLocation.Local
			);

			graph.Add(assembly);

			alreadySeen.Add(assemblyName);

			// now do inventory of this folder...

			foreach(var candidateAssembly in allAssemblies.Where(a => !alreadySeen.Contains(a)))
			{
				try
				{
					var ca = AssemblyDefinition.ReadAssembly(candidateAssembly);

					foreach (var r in ca.MainModule.AssemblyReferences)
					{
						if (r.FullName == reflection.FullName)
						{
							var dependency = new AssemblyInfo(ca.Name.Name, ca.Name.Version);

							graph.EdgeBetween(dependency, assembly);
							reverseGraph.EdgeBetween(assembly, dependency);
						}
					}
				}
				catch(BadImageFormatException)
				{
				}
			}

			// now look at all edges for this assembly
			foreach (var edge in reverseGraph.EdgesFor(assembly))
			{
				// load each one ???
				Build(graph, reverseGraph, Path.Combine(folder, edge.Name + ".dll"), folder, allAssemblies, alreadySeen);
			}
		}

	}
}
