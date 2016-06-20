using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deepend
{
	public static class AssemblyReferenceBuilder
	{
		public static Graph<AssemblyInfo> Build(string assembly, ReferenceDepth depth)
		{
			var graph = new Graph<AssemblyInfo>();

			Build(graph, null, assembly, depth, 0, new List<string>());

			return graph;
		}

		private static void Build(Graph<AssemblyInfo> graph, AssemblyInfo dependent, string assemblyName, ReferenceDepth depth, int level, IList<string> visitedAssemblies)
		{
			var reflection = AssemblyDefinition.ReadAssembly(assemblyName);

			var assembly = new AssemblyInfo
			(
				reflection.Name.Name,
				reflection.Name.Version,
				reflection.MainModule.Runtime.ToString().Replace("_", "."),
				AssemblyLocation.Local
			);

			if (dependent == null)
				graph.Add(assembly);
			else
				graph.EdgeBetween(dependent, assembly);

			if (depth == ReferenceDepth.TopLevelOnly && level >= 1)
				return;

			// now look at references to this assembly...
			string folder = System.IO.Path.GetDirectoryName(reflection.MainModule.FullyQualifiedName);

			foreach (var reference in reflection.MainModule.AssemblyReferences)
			{
				string referenceAssembly = reference.Name + ".dll";
				string pathToReference = System.IO.Path.Combine(folder, referenceAssembly);

				if (!System.IO.File.Exists(pathToReference))
				{
					// is it in the GAC?
					pathToReference = NativeMethods.QueryPathInGlobalAssemblyCache(reference.Name);
				}

				if (!System.IO.File.Exists(pathToReference))
				{
					graph.EdgeBetween(assembly, new AssemblyInfo(reference.Name, reference.Version));
				}
				else
				{
					if (!visitedAssemblies.Contains(referenceAssembly))
					{
						visitedAssemblies.Add(referenceAssembly);
						Build(graph, assembly, pathToReference, depth, level + 1, visitedAssemblies);
					}
					else
					{
						// already seen it!
					}
				}
			}
		}
	}
}
