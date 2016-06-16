using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deepend
{
	public class ImplementationRelationship : Edge
	{
		public ImplementationRelationship(TypeName interfaceType, Node implementation)
		{
			this.To = Node.SuggestTypeId(interfaceType.FullName);
			this.From = implementation.Id;
			this.Description = "Implements";
			this.Colour = Edge.SuggestColour(this.Description);
		}
	}
}
