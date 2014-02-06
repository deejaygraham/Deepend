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

        public void WriteSelf(IGraphDependencies dependencies)
        {
            if (this.Name.StartsWith("<"))
                return;

            dependencies.Declare(this);
        }

        public void WriteRelationships(IGraphDependencies dependencies, TypeInventory ti)
        {
            if (this.Name.StartsWith("<"))
                return;

            // don't link to self.

            // derives from
            if (!String.IsNullOrEmpty(this.DerivesFrom))
            {
				Link(this.DerivesFrom, dependencies, ti, LinkRelationship.Inheritance);
            }

            Link(this.Implements, dependencies, ti, LinkRelationship.Interface);
            Link(this.TalksTo, dependencies, ti, LinkRelationship.Dependency);
			Link(this.Creates, dependencies, ti, LinkRelationship.Dependency);
        }

        private void Link(IEnumerable<string> types, IGraphDependencies dependencies, TypeInventory ti, LinkRelationship relationship)
        {
            foreach (var typeName in types)
            {
				Link(typeName, dependencies, ti, relationship);
            }
        }

		private void Link(string typeName, IGraphDependencies dependencies, TypeInventory ti, LinkRelationship relationship)
        {
            try
            {
				dependencies.Link(this, ti[typeName], relationship);
            }
            catch (KeyNotFoundException)
            {
            }
        }
    }
}
