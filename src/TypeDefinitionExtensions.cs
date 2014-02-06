using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Linq;

namespace Deepend
{
    public static class TypeDefinitionExtensions
    {
        public static void DiscoverInheritance(this TypeDefinition td, IntrospectedType it, TypeInventory ti)
        {
            if (td.HasInterfaces)
            {
                foreach (var intf in td.Interfaces)
                {
                    IntrospectedType iImpl = ti[intf];
                    it.Implementing(intf);
                }
            }

            if (td.BaseType != null)
            {
                IntrospectedType iBase = ti[td.BaseType];
                it.DerivingFrom(td.BaseType);
            }
        }

        public static void DiscoverProperties(this TypeDefinition td, IntrospectedType it, TypeInventory ti)
        {
            if (td.HasProperties)
            {
                foreach (var prop in td.Properties)
                {
                    IntrospectedType iProp = ti[prop.PropertyType];
                    it.TalkingTo(prop.PropertyType);
                }
            }
        }

        public static void DiscoverMethods(this TypeDefinition td, IntrospectedType it, TypeInventory ti)
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
                            IntrospectedType iParam = ti[par.ParameterType];
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
                                IntrospectedType iNewObj = ti[methodReference.DeclaringType];
                                it.Creating(methodReference.DeclaringType);
                            }
                        }
                    }
                }
            }

        }

        public static void DiscoverFields(this TypeDefinition td, IntrospectedType it, TypeInventory ti)
        {
            if (td.HasFields)
            {
                foreach (var field in td.Fields.Where(f => !f.IsPrivate && f.FieldType.ShouldBeIncluded()))
                {
                    IntrospectedType ifield = ti[field.FieldType];

                    it.TalkingTo(field.FieldType);
                }
            }
        }
    }
}
