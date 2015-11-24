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

		public IEnumerable<IGraphable> Generate(TypeInventory ti)
		{
			var list = new List<IGraphable>();

			if (this.Name.StartsWith("<"))
				return list;

			string t1Name = this.Name;

			if (this.Namespace.StartsWith("System") || this.Namespace.StartsWith("Microsoft"))
				t1Name = this.Namespace + "." + this.Name;

			t1Name = t1Name.Replace("&", "");

			var thisType = new Node
			{
				Id = Node.SuggestTypeId(t1Name),
				Name = t1Name
			};

			var thisNamespace = new Node
			{
				Id = Node.SuggestNamespaceId(this.Namespace),
				Name = this.Namespace,
				Group = true
			};

			var namespaceContainsType = new Edge
			{
				From = thisNamespace.Id,
				To = thisType.Id,
				Group = true
			};

			list.Add(thisType);
			list.Add(thisNamespace);
			list.Add(namespaceContainsType);

			// make sure names are valid...
			// namespaces exist
			// 
			// look at links...
			if (!String.IsNullOrEmpty(this.DerivesFrom))
			{
				var t = ti[this.DerivesFrom];

				var derivesFromType = new Edge
				{
					From = Node.SuggestTypeId(t.Name),
					To = thisType.Id,
					Description = "Inherits from"
				};

				list.Add(derivesFromType);
			}

			foreach (var implements in this.Implements)
			{
				var t = ti[implements];

				var implementsInterface = new Edge
				{
					From = Node.SuggestTypeId(t.Name),
					To = thisType.Id,
					Description = "Implements"
				};

				list.Add(implementsInterface);
			}

			foreach (var talksTo in this.TalksTo)
			{
				var t = ti[talksTo];

				var talksToType = new Edge
				{
					From = Node.SuggestTypeId(t.Name),
					To = thisType.Id,
					Description = "Talks To"
				};

				list.Add(talksToType);
			}

			foreach (var creates in this.Creates)
			{
				var t = ti[creates];

				var createsType = new Edge
				{
					From = Node.SuggestTypeId(t.Name),
					To = thisType.Id,
					Description = "&lt;&lt;new&gt;&gt;"
				};

				list.Add(createsType);
			}

			return list;
		}
    }
}
