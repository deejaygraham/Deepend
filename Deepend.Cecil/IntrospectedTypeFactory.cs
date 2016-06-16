using Mono.Cecil;

namespace Deepend
{
    public class IntrospectedTypeFactory
    {
		public IntrospectedType CreateFrom(TypeDefinition td, TypeNameInventory tni)
        {
			IntrospectedType it = new IntrospectedType
			{
				Name = td.Name,
				Namespace = td.Namespace,
				FullName = td.FullName
			};

            td.DiscoverInheritance(it, tni);
            td.DiscoverFields(it, tni);
            td.DiscoverProperties(it, tni);
            td.DiscoverMethods(it, tni);

			return it;
        }
    }
}
