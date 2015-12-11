using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deepend
{
	public class CreatesRelationship : Edge
	{
		public CreatesRelationship(TypeName createdType, Node creator)
		{
			this.From = creator.Id;
			this.To = Node.SuggestTypeId(createdType.FullName);
			this.Description = "&lt;&lt;new&gt;&gt;";
			this.Colour = Edge.SuggestColour(this.Description);
		}
	}
}
