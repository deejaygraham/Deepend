using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deepend
{
	public static class TypeReferenceBuilder
	{
		public static Graph<TypeInfo> Build(string assemblyName, TypeDetail detail)
		{
			if (!File.Exists(assemblyName))
			{
				throw new FileNotFoundException(assemblyName);
			}

			var graph = new Graph<TypeInfo>();

			var assemblyContent = AssemblyDefinition.ReadAssembly(assemblyName);

			IEnumerable<TypeDefinition> types = assemblyContent.MainModule.Types;

			var filters = new List<IFilterTypes> { new ModuleFilter(), new GeneratedTypeFilter() };

			foreach (IFilterTypes filter in filters)
			{
				types = filter.Filter(types);
			}

			foreach(var type in types)
			{
				Build(graph, type, detail);
			}

			return graph;
		}

		private static void Build(Graph<TypeInfo> graph, TypeDefinition type, TypeDetail detailLevel)
		{
			var thisType = new TypeInfo(type.FullName);

			graph.Add(thisType);

			if (detailLevel == TypeDetail.None)
				return;

			// filter simple types out ...
			if (detailLevel.HasFlag(TypeDetail.Properties))
			{
				foreach(var property in type.PropertyTypes())
				{
					try
					{
						graph.EdgeBetween(thisType, property);
					}
					catch { }
				}
			}

			if (detailLevel.HasFlag(TypeDetail.Fields))
			{
				foreach(var field in type.FieldTypes())
				{
					try
					{
						graph.EdgeBetween(thisType, field);
					}
					catch { }
				}
			}

			if (detailLevel.HasFlag(TypeDetail.Interfaces))
			{
				foreach(var intf in type.InterfaceTypes())
				{
					try
					{
						graph.EdgeBetween(thisType, intf);
					}
					catch { }
				}
			}

			if (detailLevel.HasFlag(TypeDetail.Inheritance))
			{
				var inheritance = type.FindInheritance();

				if (inheritance != null)
				{
					graph.EdgeBetween(thisType, inheritance);
				}
			}
		}
	}
}
