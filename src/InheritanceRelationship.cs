using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deepend
{
	public class InheritanceRelationship : Edge
	{
		public InheritanceRelationship(TypeName superClass, Node subClass)
		{
			this.To = Node.SuggestTypeId(superClass.FullName);
			this.From = subClass.Id;
			this.Description = "Inherits from";
			this.Colour = Edge.SuggestColour(this.Description);

		}
	}
}
