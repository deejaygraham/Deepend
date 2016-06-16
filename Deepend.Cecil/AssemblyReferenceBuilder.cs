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
			var assemblyContent = AssemblyDefinition.ReadAssembly(assembly);

			var graph = new Graph<AssemblyInfo>();

			var mainAssembly = new AssemblyInfo
			(
				assemblyContent.Name.Name,
				assemblyContent.Name.Version,
				assemblyContent.MainModule.Runtime.ToString().Replace("_", "."),
				AssemblyLocation.Local
			);

			graph.Add(mainAssembly);

			string folder = System.IO.Path.GetDirectoryName(assemblyContent.MainModule.FullyQualifiedName);

			var references = assemblyContent.MainModule.AssemblyReferences;

			foreach (var r in references)
			{
				graph.EdgeBetween(mainAssembly, new AssemblyInfo(r.Name, r.Version));
			}
			
			return new Graph<AssemblyInfo>();
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
