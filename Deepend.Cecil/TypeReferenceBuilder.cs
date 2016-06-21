using Mono.Cecil;
using System.Collections.Generic;
using System.IO;

namespace Deepend
{
	public static class TypeReferenceBuilder
	{
		public static Graph<TypeInfo> Build(string assemblyName, TypeDetail detail, IEnumerable<IFilterTypes> filters)
		{
			if (!File.Exists(assemblyName))
			{
				throw new FileNotFoundException(assemblyName);
			}

			var graph = new Graph<TypeInfo>();

			var assemblyContent = AssemblyDefinition.ReadAssembly(assemblyName);

			IEnumerable<TypeDefinition> types = assemblyContent.MainModule.Types;
			
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
				foreach(var property in type.PropertyTypes(r => !r.IsPrimitive && !r.IsPrimitive()))
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
				foreach (var field in type.FieldTypes(f => !f.IsPrimitive && !f.IsPrimitive()))
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

			if (detailLevel.HasFlag(TypeDetail.MethodCalls) || detailLevel.HasFlag(TypeDetail.ObjectCreation))
			{
				var mis = type.MethodTypes(m => !m.IsPrimitive && !m.IsPrimitive());

				foreach(var mi in mis)
				{
					if (detailLevel.HasFlag(TypeDetail.MethodCalls))
					{
						if (mi.Signature.ReturnType != null)
							graph.EdgeBetween(thisType, mi.Signature.ReturnType);

						foreach (var p in mi.Signature.Parameters)
						{
							graph.EdgeBetween(thisType, p);
						}
					}

					if (detailLevel.HasFlag(TypeDetail.ObjectCreation))
					{
						foreach (var newObject in mi.CreatedObjects)
						{
							graph.EdgeBetween(thisType, newObject);
						}
					}
				}
			}
		}
	}
}
