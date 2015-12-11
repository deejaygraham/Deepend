using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deepend
{
    public class IntrospectedType
    {
        public IntrospectedType()
        {
            this.TalksTo = new List<string>();
            this.Creates = new List<string>();
            this.Implements = new List<string>();
        }

		public string FullName { get; set; }

        public string Name { get; set; }

        public string Namespace { get; set; }

        public void TalkingTo(TypeReference tr)
        {
            if (tr.ShouldBeIncluded())
            {
                this.TalksTo.Add(tr.FullName);
            }
        }

        private List<string> TalksTo { get; set; }

		/// <summary>
		/// Type creates other types in methods
		/// </summary>
		/// <param name="tr"></param>
        public void Creating(TypeReference tr)
        {
            if (tr.ShouldBeIncluded())
            {
                this.Creates.Add(tr.FullName);
            }
        }

		private List<string> Creates { get; set; }

		/// <summary>
		/// Type inherits from another type
		/// </summary>
		/// <param name="tr"></param>
        public void DerivingFrom(TypeReference tr)
        {
            if (tr.ShouldBeIncluded())
            {
                this.DerivesFrom = tr.FullName;
            }
        }

		private string DerivesFrom { get; set; }

		/// <summary>
		/// Type implements an interface
		/// </summary>
		/// <param name="tr"></param>
        public void Implementing(TypeReference tr)
        {
            if (tr.ShouldBeIncluded())
            {
                this.Implements.Add(tr.FullName);
            }
        }

		private List<string> Implements { get; set; }

		public IEnumerable<IGraphable> Generate(TypeNameInventory tni)
		{
			var list = new HashSet<IGraphable>();

			if (this.Name.StartsWith("<"))
				return list;

			TypeName thisTypeName = tni[this.FullName];

			thisTypeName.Generate().ToList().ForEach(g => list.Add(g));

			var thisType = new TypeNode(this);

//			list.Add(thisType);
			
			//if (!String.IsNullOrEmpty(this.Namespace))
			//{
			//	// most specific namespace
			//	var thisNamespace = new GroupNode(Node.SuggestNamespaceId(this.Namespace), this.Namespace);
			//	// I am in this namespace...
			//	list.Add(new GroupingRelationship(thisNamespace, thisType));

			//	string childNamespace = this.Namespace;
			//	string parentNamespace = childNamespace.ParentNamespaceOf();

			//	while (!String.IsNullOrEmpty(parentNamespace))
			//	{
			//		var childGroup = new GroupNode(Node.SuggestNamespaceId(childNamespace), childNamespace);
			//		list.Add(childGroup);

			//		var parentGroup = new GroupNode(Node.SuggestNamespaceId(parentNamespace), parentNamespace);
			//		list.Add(parentGroup);

			//		list.Add(new GroupingRelationship(parentGroup, childGroup));

			//		childNamespace = parentNamespace;
			//		parentNamespace = childNamespace.ParentNamespaceOf();
			//	}
			//}

			if (!String.IsNullOrEmpty(this.DerivesFrom))
			{
				list.Add(new InheritanceRelationship(tni[this.DerivesFrom], thisType));
			}

			this.Implements.ForEach(i => list.Add(new ImplementationRelationship(tni[i], thisType)));

			//foreach (var implements in this.Implements)
			//{
			//	var t = tni[implements];

			//	list.Add(new ImplementationRelationship(t, thisType));
			//}

			this.TalksTo.ForEach(t => list.Add(new InteractionRelationship(tni[t], thisType)));
			//foreach (var talksTo in this.TalksTo)
			//{
			//	var t = tni[talksTo];

			//	list.Add(new InteractionRelationship(t, thisType));
			//}

			this.Creates.ForEach(c => list.Add(new CreatesRelationship(tni[c], thisType)));
			//foreach (var creates in this.Creates)
			//{
			//	var t = tni[creates];

			//	list.Add(new CreatesRelationship(t, thisType));
			//}

			return list;
		}
    }

	public static class StringExtensions
	{
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
	}
}
