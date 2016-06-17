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

		public static AssemblyReference Load(string name, ReferenceDepth depth, int level, IList<string> assemblies)
		{
			var assemblyContent = AssemblyDefinition.ReadAssembly(name);

			string folder = System.IO.Path.GetDirectoryName(assemblyContent.MainModule.FullyQualifiedName);

			AssemblyReference ar = new AssemblyReference
			(
				assemblyContent.Name.Name,
				assemblyContent.Name.Version,
				assemblyContent.MainModule.Runtime.ToString().Replace("_", "."),
				AssemblyLocation.Local
			);

			if (depth == ReferenceDepth.TopLevelOnly && level >= 1)
				return ar;

			var refList = assemblyContent.MainModule.AssemblyReferences;

			foreach (var r in refList)
			{
				string candidateAssembly = System.IO.Path.Combine(folder, r.Name + ".dll");

				if (System.IO.File.Exists(candidateAssembly))
				{
					if (!assemblies.Contains(candidateAssembly))
					{
						assemblies.Add(candidateAssembly);
						ar.References.Add(AssemblyReferenceBuilder.Load(candidateAssembly, depth, level + 1, assemblies));
					}
				}
				else
				{
					try
					{
						string ai = NativeMethods.QueryPathInGlobalAssemblyCache(r.Name);
						ar.References.Add(new AssemblyReference(r.Name, r.Version, string.Empty, AssemblyLocation.GlobalAssemblyCache));

						if (!assemblies.Contains(candidateAssembly))
						{
							assemblies.Add(candidateAssembly);
						}
					}
					catch (Exception)
					{
						ar.References.Add(new AssemblyReference(r.Name, r.Version));
					}
					// ignore
				}
			}

			return ar;
		}
	}
}
