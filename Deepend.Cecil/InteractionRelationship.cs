using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deepend
{
	public class InteractionRelationship : Edge
	{
		public InteractionRelationship(TypeName interactsWith, Node callFrom)
		{
			this.From = callFrom.Id;
			this.To = Node.SuggestTypeId(interactsWith.FullName);
			this.Description = "Talks To";
			this.Colour = Edge.SuggestColour(this.Description);
		}
	}
}
