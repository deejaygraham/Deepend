using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deepend
{
	public class GroupingRelationship : Edge
	{
		public GroupingRelationship(Node container, Node contained)
		{
			this.From = container.Id;
			this.To = contained.Id;
			this.Group = true;
		}
	}
}
