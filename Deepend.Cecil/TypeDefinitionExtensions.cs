using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Collections.Generic;
using System.Linq;

namespace Deepend
{
    public static class TypeDefinitionExtensions
    {
		//public static bool HasSuperClass(this TypeDefinition discovered)
		//{
		//	const string BaseObjectName = "System.Object";

		//	return discovered.BaseType != null && discovered.BaseType.FullName != BaseObjectName;
		//}

		//public static TypeName FindInheritance(this TypeDefinition td)
		//{
		//	if (td.HasSuperClass())
		//	{
		//		return td.BaseType.ToTypeName();
		//	}

		//	return null;
		//}

		//public static IEnumerable<TypeName> ListImplementations(this TypeDefinition td)
		//{
		//	var list = new List<TypeName>();

		//	if (td.HasInterfaces)
		//	{
		//		foreach (var intf in td.Interfaces)
		//		{
		//			list.Add(intf.ToTypeName());
		//		}
		//	}

		//	return list;
		//}

		public static void DiscoverInheritance(this TypeDefinition td, IntrospectedType it, TypeNameInventory tni)
        {
            if (td.HasInterfaces)
            {
                foreach (var intf in td.Interfaces)
                {
					tni.Add(intf.ToTypeName());
                    it.Implementing(intf);
                }
            }

            if (td.HasSuperClass())
            {
				tni.Add(td.BaseType.ToTypeName());
                it.DerivingFrom(td.BaseType);
            }
        }

		//public IEnumerable<TypeName> ListProperties(this TypeDefinition td)
		//{
		//	var list = new List<TypeName>();

		//	if (td.HasProperties)
		//	{
		//		foreach (var prop in td.Properties)
		//		{
		//			list.Add(prop.PropertyType.ToTypeName());
		//		}
		//	}

		//	return list;
		//}

		public static void DiscoverProperties(this TypeDefinition td, IntrospectedType it, TypeNameInventory tni)
        {
            if (td.HasProperties)
            {
                foreach (var prop in td.Properties)
                {
					tni.Add(prop.PropertyType.ToTypeName());

                    it.TalkingTo(prop.PropertyType);
                }
            }
        }

		public static void DiscoverMethods(this TypeDefinition td, IntrospectedType it, TypeNameInventory tni)
        {
            if (td.HasMethods)
            {
                foreach (var meth in td.Methods)
                {
                    it.TalkingTo(meth.ReturnType);

                    if (meth.HasParameters)
                    {
                        foreach (var par in meth.Parameters)
                        {
							tni.Add(par.ParameterType.ToTypeName());

                            it.TalkingTo(par.ParameterType);
                        }
                    }

                    if (!meth.IsAbstract && meth.HasBody)
                    {
                        var body = meth.Body;

                        if (body == null)
                        {
                            continue;
                        }

                        foreach (Instruction instr in body.Instructions.Where(i => i.OpCode.Code == Code.Newobj))
                        {
                            MethodReference methodReference = instr.Operand as MethodReference;

                            if (methodReference != null)
                            {
								tni.Add(methodReference.DeclaringType.ToTypeName());
                                it.Creating(methodReference.DeclaringType);
                            }
                        }
                    }
                }
            }
        }

		public static void DiscoverFields(this TypeDefinition td, IntrospectedType it, TypeNameInventory tni)
		{
			if (td.HasFields)
			{
				foreach (var field in td.Fields.Where(f => !f.IsPrivate && f.FieldType.ShouldBeIncluded()))
				{
					tni.Add(field.FieldType.ToTypeName());

					it.TalkingTo(field.FieldType);
				}
			}
		}
    }
}
