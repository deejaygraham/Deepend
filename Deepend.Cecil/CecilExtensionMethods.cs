using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deepend
{
	public static class CecilExtensionMethods
	{
		public static bool HasSuperClass(this TypeDefinition td)
		{
			const string BaseObjectName = "System.Object";

			return td.BaseType != null && td.BaseType.FullName != BaseObjectName;
		}

		public static TypeInfo FindInheritance(this TypeDefinition td)
		{
			if (td.HasSuperClass())
			{
				return td.BaseType.ToTypeInfo();
			}

			return null;
		}

		public static IEnumerable<TypeInfo> InterfaceTypes(this TypeDefinition td)
		{
			var list = new List<TypeInfo>();

			if (td.HasInterfaces)
			{
				foreach (var intf in td.Interfaces)
				{
					list.Add(intf.ToTypeInfo());
				}
			}

			return list;
		}

		public static IEnumerable<TypeInfo> PropertyTypes(this TypeDefinition td)
		{
			var list = new List<TypeInfo>();

			if (td.HasProperties)
			{
				foreach (var prop in td.Properties.Where(p => !p.PropertyType.IsPrimitive && !p.PropertyType.IsPrimitive()))
				{
					list.Add(prop.PropertyType.ToTypeInfo());
				}
			}

			return list;
		}

		public static IEnumerable<TypeInfo> FieldTypes(this TypeDefinition td)
		{
			var list = new List<TypeInfo>();

			if (td.HasFields)
			{
				foreach (var field in td.Fields.Where(f => !f.IsPrivate && !f.FieldType.IsPrimitive && !f.FieldType.IsPrimitive())) 
					//.Where(f => !f.IsPrivate && f.FieldType.ShouldBeIncluded()))
				{
					list.Add(field.FieldType.ToTypeInfo());
				}
			}

			return list;
		}

		public static TypeName ToTypeName(this TypeReference tr)
		{
			return new TypeName(tr.FriendlyName());
		}

		public static TypeInfo ToTypeInfo(this TypeReference tr)
		{
			return new TypeInfo(tr.FriendlyName());
		}

		// move into another filter ????

		private static List<string> primitiveTypes = new List<string>
		{
			"Void",
			"Byte",
			"Byte[]",
			"Object",
			"Type",
			"TimeSpan", // may be useful to see these.
			"DateTime",
			"EventArgs",
			"Regex",
			"IntPtr",
			"Point",
			"Size",
			"SizeF",
			"Font",
			"Boolean",
			"String",
			"String[]",
			"String&",
			"Int16",
			"Int32",
			"Int64",
			"Double",
			"Float",
			"Guid",
			"Enum"
		};

		public static bool IsPrimitive(this TypeReference type)
		{
			return primitiveTypes.Contains(type.Name)
				|| type.Name.EndsWith("Exception")
				|| type.Name.EndsWith("EventArgs")
				|| type.Name.EndsWith("EventHandler")
				|| type.Name.Contains("AnonymousType");
		}

		public static string FriendlyName(this TypeReference tr)
		{
			if (tr.IsGenericInstance || tr.HasGenericParameters)
			{
				return tr.MakeGenericFriendlyName();
			}

			return tr.FullName;
		}

		public static bool IsPublicInterface(this TypeReference tr)
		{
			TypeDefinition interfaceDefinition = tr as TypeDefinition;

			if (interfaceDefinition != null)
			{
				return interfaceDefinition.IsPublic;
			}

			return false;
		}

		public static string MakeGenericFriendlyName(this TypeReference tr)
		{
			var builder = new StringBuilder();

			if (!string.IsNullOrEmpty(tr.Namespace))
				builder.AppendFormat("{0}.", tr.Namespace);

			string className = tr.StripGenericNameSuffix();

			builder.Append(className);

			const string OpenTag = "<";
			const string CloseTag = ">";

			builder.Append(OpenTag);

			if (tr.HasGenericParameters)
			{
				builder.Append(tr.GenericParameters.ToParameterList());
			}
			else if (tr.IsGenericInstance)
			{
				GenericInstanceType git = tr as GenericInstanceType;

				if (git != null)
				{
					builder.Append(git.GenericArguments.ToParameterList());
				}
			}
			
			builder.Append(CloseTag);

			return builder.ToString();
		}

		public static string StripGenericNameSuffix(this TypeReference tr)
		{
			const char Backtick = '`';

			int backtickPosition = tr.Name.IndexOf(Backtick);

			if (backtickPosition > 0)
			{
				return tr.Name.Substring(0, backtickPosition);
			}

			return tr.Name;
		}

		public static string ToParameterList(this IEnumerable<TypeReference> parameters)
		{
			var list = new List<string>();

			foreach (var p in parameters)
			{
				list.Add(p.Name);
			}

			const string CommaDelimiter = ",";

			return string.Join(CommaDelimiter, list);
		}
	}
}
