using Mono.Cecil;

namespace Deepend
{
    public static class TypeReferenceExtensions
    {
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

    }

}
