using Mono.Cecil;
using Mono.Cecil.Cil;
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

			return td.BaseType != null 
				&& !td.BaseType.FullName.Equals(BaseObjectName, StringComparison.CurrentCultureIgnoreCase);
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

		public static IEnumerable<TypeInfo> PropertyTypes(this TypeDefinition td, Predicate<TypeReference> predicate)
		{
			var list = new List<TypeInfo>();

			if (td.HasProperties)
			{
				foreach (var prop in td.Properties.Select(p => p.PropertyType))
				{
					if (predicate(prop))
						list.Add(prop.ToTypeInfo());
				}
			}

			return list;
		}

		public static IEnumerable<TypeInfo> FieldTypes(this TypeDefinition td, Predicate<TypeReference> predicate)
		{
			var list = new List<TypeInfo>();

			if (td.HasFields)
			{
				foreach (var field in td.Fields.Select(f => f.FieldType)) 
				{
					if (predicate(field))
						list.Add(field.ToTypeInfo());
				}
			}

			return list;
		}

		public static TypeInfo ToTypeInfo(this TypeReference tr)
		{
			return new TypeInfo(tr.FriendlyName());
		}

		public static string ParentNamespaceOf(this string namesp)
		{
			string parent = string.Empty;

			const char Dot = '.';

			if (namesp.Contains(Dot))
			{
				int index = namesp.LastIndexOf(Dot);
				parent = namesp.Substring(0, index);
			}

			return parent;
		}

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

		public static IEnumerable<MethodInfo> MethodTypes(this TypeDefinition td, Predicate<TypeReference> predicate)
		{
			var list = new List<MethodInfo>();

			if (!td.HasMethods)
				return list;

			foreach (var method in td.Methods)
			{
				MethodInfo mi = new MethodInfo();
				
				if (predicate(method.ReturnType))
					mi.Signature.ReturnType = method.ReturnType.ToTypeInfo();

				if (method.HasParameters)
				{
					foreach (var parameter in method.Parameters.Select(p => p.ParameterType))
					{
						if (predicate(parameter))
							mi.Signature.Parameters.Add(parameter.ToTypeInfo());
					}
				}

				if (!method.IsAbstract && method.HasBody)
				{
					var body = method.Body;

					if (body != null)
					{
						foreach (Instruction instr in body.Instructions.Where(i => i.OpCode.Code == Code.Newobj))
						{
							MethodReference methodReference = instr.Operand as MethodReference;

							if (methodReference != null)
							{
								if (predicate(method.DeclaringType))
									mi.CreatedObjects.Add(methodReference.DeclaringType.ToTypeInfo());
							}
						}
					}
				}

				if (mi.CreatedObjects.Any() || mi.Signature.ReturnType != null || mi.Signature.Parameters.Any())
					list.Add(mi);
			}

			return list;
		}

		public static bool InSystemNamespace(this TypeReference type)
		{
			return type.Namespace.StartsWith("System")
				|| type.Namespace.StartsWith("Microsoft")
				|| type.Namespace == "System.Resources"
				|| type.Namespace.StartsWith("System.Windows.Forms")
				|| type.Namespace.StartsWith("System.Drawing")
				|| type.Namespace.StartsWith("Microsoft.ManagementConsole");
		}
	}
}
