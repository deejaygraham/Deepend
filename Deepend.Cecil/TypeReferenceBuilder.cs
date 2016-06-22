using Mono.Cecil;
using System.Collections.Generic;
using System.IO;

namespace Deepend
{
	public static class TypeReferenceBuilder
	{
		public static Graph<TypeInfo> Build(string assemblyName, TypeDetails detail, IEnumerable<IFilterTypes> filters)
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

		private static void Build(Graph<TypeInfo> graph, TypeDefinition type, TypeDetails detailLevel)
		{
			var thisType = new TypeInfo(type.FullName);

			graph.Add(thisType);

			if (detailLevel == TypeDetails.None)
				return;

			// filter simple types out ...
			if (detailLevel.HasFlag(TypeDetails.Properties))
			{
				BuildGraphFromProperties(graph, type, thisType);
			}

			if (detailLevel.HasFlag(TypeDetails.Fields))
			{
				BuildGraphFromFields(graph, type, thisType);
			}

			if (detailLevel.HasFlag(TypeDetails.Interfaces))
			{
				BuildGraphFromInterfaces(graph, type, thisType);
			}

			if (detailLevel.HasFlag(TypeDetails.Inheritance))
			{
				BuildGraphFromInheritance(graph, type, thisType);
			}

			if (detailLevel.HasFlag(TypeDetails.MethodCalls) || detailLevel.HasFlag(TypeDetails.ObjectCreation))
			{
				BuildGraphFromMethods(graph, type, detailLevel, thisType);
			}
		}

		private static void BuildGraphFromProperties(Graph<TypeInfo> graph, TypeDefinition type, TypeInfo thisType)
		{
			foreach (var property in type.PropertyTypes(r => !r.IsPrimitive && !r.IsPrimitive()))
			{
				try
				{
					graph.EdgeBetween(thisType, property);
				}
				catch { }
			}
		}

		private static void BuildGraphFromFields(Graph<TypeInfo> graph, TypeDefinition type, TypeInfo thisType)
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

		private static void BuildGraphFromInterfaces(Graph<TypeInfo> graph, TypeDefinition type, TypeInfo thisType)
		{
			foreach (var intf in type.InterfaceTypes())
			{
				try
				{
					graph.EdgeBetween(thisType, intf);
				}
				catch { }
			}
		}

		private static void BuildGraphFromInheritance(Graph<TypeInfo> graph, TypeDefinition type, TypeInfo thisType)
		{
			var inheritance = type.FindInheritance();

			if (inheritance != null)
			{
				graph.EdgeBetween(thisType, inheritance);
			}
		}

		private static void BuildGraphFromMethods(Graph<TypeInfo> graph, TypeDefinition type, TypeDetails detailLevel, TypeInfo thisType)
		{
			var mis = type.MethodTypes(m => !m.IsPrimitive && !m.IsPrimitive());

			foreach (var mi in mis)
			{
				if (detailLevel.HasFlag(TypeDetails.MethodCalls))
				{
					if (mi.Signature.ReturnType != null)
						graph.EdgeBetween(thisType, mi.Signature.ReturnType);

					foreach (var p in mi.Signature.Parameters)
					{
						graph.EdgeBetween(thisType, p);
					}
				}

				if (detailLevel.HasFlag(TypeDetails.ObjectCreation))
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
