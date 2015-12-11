using Mono.Cecil;
using System.Collections.Generic;
using System.Text;

namespace Deepend
{
    public static class TypeReferenceExtensions
    {
		public static TypeName ToTypeName(this TypeReference type)
		{
			return new TypeName(type.FriendlyName());
		}

        public static bool ShouldBeIncluded(this TypeReference type)
        {
            return !type.IsPrimitive
                && !type.IsPrimitive()
                //&& !type.InSystemNamespace()
                //&& !type.IsSimpleType()
                && !type.IsGenericInstance;
        }

        public static bool IsSimpleType(this TypeReference type)
        {
            return type.IsPrimitive();
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

        public static bool IsPrimitive(this TypeReference type)
        {
            return type.Name == "Void"
                || type.Name == "Byte"
                || type.Name == "Byte[]"
                || type.Name == "Object"
                || type.Name == "Type"
                || type.Name == "TimeSpan" // may be useful to see these.
                || type.Name == "DateTime"
                || type.Name == "EventArgs"
                || type.Name.EndsWith("Exception")
                || type.Name.EndsWith("EventArgs")
                || type.Name.EndsWith("EventHandler")
                || type.Name == "Regex"
                || type.Name == "IntPtr"
                || type.Name == "Point"
                || type.Name == "Size"
                || type.Name == "SizeF"
                || type.Name == "Font"
                    || type.Name == "Boolean"
                    || type.Name == "String"
                    || type.Name == "String[]"
                    || type.Name == "String&"
                    || type.Name == "Int16"
                    || type.Name == "Int32"
                    || type.Name == "Int64"
                    || type.Name == "Double"
                    || type.Name == "Float"
                    || type.Name == "Guid"
                    || type.Name.Contains("AnonymousType")
                    ;
        }

		public static string FriendlyName(this TypeReference td)
		{
			if (td.IsGenericInstance || td.HasGenericParameters)
			{
				return MakeGenericFriendlyName(td);
			}

			return td.FullName;
		}

		public static bool IsPublicInterface(this TypeReference candidateInterace)
		{
			TypeDefinition interfaceDefinition = candidateInterace as TypeDefinition;

			if (interfaceDefinition != null)
			{
				return interfaceDefinition.IsPublic;
			}

			return false;
		}

		private static string MakeGenericFriendlyName(TypeReference td)
		{
			var builder = new StringBuilder();

			if (!string.IsNullOrEmpty(td.Namespace))
				builder.AppendFormat("{0}.", td.Namespace);

			string className = StripGenericNameSuffix(td);

			builder.Append(className);

			const string OpenTag = "<";
			const string CloseTag = ">";

			builder.Append(OpenTag);

			if (td.HasGenericParameters)
			{
				builder.Append(CreateGenericParameterList(td.GenericParameters));
			}
			else if (td.IsGenericInstance)
			{
				GenericInstanceType git = td as GenericInstanceType;

				if (git != null)
				{
					builder.Append(CreateParameterList(git.GenericArguments));
				}
			}


			builder.Append(CloseTag);

			return builder.ToString();
		}

		private static string StripGenericNameSuffix(TypeReference td)
		{
			const char Tilde = '`';

			int tildePosition = td.Name.IndexOf(Tilde);

			if (tildePosition > 0)
			{
				return td.Name.Substring(0, tildePosition);
			}

			return td.Name;
		}

		private static string CreateGenericParameterList(IEnumerable<GenericParameter> parameters)
		{
			var list = new List<string>();

			foreach (var p in parameters)
			{
				list.Add(p.Name);
			}

			const string CommaDelimiter = ",";

			return string.Join(CommaDelimiter, list);
		}

		private static string CreateParameterList(IEnumerable<TypeReference> parameters)
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
