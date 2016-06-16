using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deepend
{
	public class GroupNode : Node
	{
		public GroupNode(string id, string name)
		{
			this.Id = id;
			this.Name = name;
			this.Group = true;
			this.Expand = true;
		}
	}
}
