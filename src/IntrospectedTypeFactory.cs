using Mono.Cecil;

namespace Deepend
{
    public class IntrospectedTypeFactory
    {
        public void CreateFrom(TypeDefinition td, TypeInventory ti)
        {
            IntrospectedType it = ti[td];

            td.DiscoverInheritance(it, ti);
            td.DiscoverFields(it, ti);
            td.DiscoverProperties(it, ti);
            td.DiscoverMethods(it, ti);
        }
    }
}
