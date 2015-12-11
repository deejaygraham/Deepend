using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deepend
{
	public class ReferenceNode : Node
	{
		public ReferenceNode(AssemblyReference ar)
		{
			this.Id = Node.SuggestAssemblyId(ar.Name);
			this.Name = string.Format("{0} {1}", ar.Name, ar.Version);
			this.Colour = Node.SuggestColour(ar.Name);
		}
	}
}
