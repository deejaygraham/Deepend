using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Deepend
{
	public class AssemblyReference
	{
		public AssemblyReference(string name, Version version)
			: this(name, version, string.Empty, AssemblyLocation.Unknown)
		{
		}

		public AssemblyReference(string name, Version version, string runtimeVersion, AssemblyLocation location)
		{
			this.Name = name;
			this.Version = version.ToString();
			this.Runtime = runtimeVersion;
			this.Location = location;
			this.References = new List<AssemblyReference>();
		}

		public string Name { get; private set; }

		public string Version { get; private set; }

		public string Runtime { get; private set; }

		public AssemblyLocation Location { get; private set; }

		public List<AssemblyReference> References { get; private set; }

		//public static AssemblyReference Load(string name, ReferenceDepth depth, int level, IList<string> assemblies)
		//{
		//	var assemblyContent = AssemblyDefinition.ReadAssembly(name);

		//	string folder = System.IO.Path.GetDirectoryName(assemblyContent.MainModule.FullyQualifiedName);

		//	AssemblyReference ar = new AssemblyReference 
		//	{ 
		//		Name = assemblyContent.Name.Name, 
		//		Version = assemblyContent.Name.Version.ToString(),
		//		Location = AssemblyLocation.Local,
		//		Runtime = assemblyContent.MainModule.Runtime.ToString()
		//	};

		//	if (depth == ReferenceDepth.TopLevelOnly && level >= 1)
		//		return ar;


		//	var refList = assemblyContent.MainModule.AssemblyReferences;

		//	foreach (var r in refList)
		//	{
		//		string candidateAssembly = System.IO.Path.Combine(folder, r.Name + ".dll");

		//		if (System.IO.File.Exists(candidateAssembly))
		//		{
		//			if (!assemblies.Contains(candidateAssembly))
		//			{
		//				assemblies.Add(candidateAssembly);
		//				ar.References.Add(AssemblyReference.Load(candidateAssembly, depth, level + 1, assemblies));
		//			}
		//		}
		//		else
		//		{
		//			try
		//			{
		//				// handle gac assemblies
		//				string ai = QueryAssemblyInfo(r.Name);
		//				ar.References.Add(new AssemblyReference 
		//				{ 
		//					Name = r.Name, 
		//					Version = r.Version.ToString(),
		//					Location = AssemblyLocation.GlobalAssemblyCache
		//				});

		//				if (!assemblies.Contains(candidateAssembly))
		//				{
		//					assemblies.Add(candidateAssembly);
		//				}
		//			}
		//			catch(Exception)
		//			{
		//				ar.References.Add(new AssemblyReference 
		//				{ 
		//					Name = r.Name, 
		//					Version = r.Version.ToString(),
		//					Location = AssemblyLocation.Unknown
		//				});
		//			}
		//			// ignore
		//		}
		//	}

		//	return ar;
		//}

		public IEnumerable<IGraphable> Generate(Node gac)
		{
			var list = new HashSet<IGraphable>();

			var thisAssembly = new ReferenceNode(this);

			list.Add(thisAssembly);

			if (this.Location == AssemblyLocation.GlobalAssemblyCache)
				list.Add(new Edge { From = gac.Id, To = thisAssembly.Id, Group = true });

			foreach (var r in this.References)
			{
				var refAssembly = new ReferenceNode(r);

				list.Add(refAssembly);

				var assemblyReferences = new Edge
				{
					From = thisAssembly.Id,
					To = refAssembly.Id
				};

				list.Add(assemblyReferences);

				if (r.Location == AssemblyLocation.GlobalAssemblyCache)
					list.Add(new Edge { From = gac.Id, To = refAssembly.Id, Group = true });

				foreach(var item in r.Generate(gac))
				{
					list.Add(item);
				}
			}

			return list;
		}
	}
}
