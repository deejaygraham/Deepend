using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deepend
{
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

			var ar = AssemblyReference.Load(this.AssemblyName, this.Depth, 0, assemblyList);

			var list = new HashSet<IGraphable>();

			var globalAssemblyCache = new Node
			{
				Id = "GAC",
				Name = "Global Assembly Cache",
				Group = true
			};

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
