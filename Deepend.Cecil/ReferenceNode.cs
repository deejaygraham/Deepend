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

			if (string.IsNullOrEmpty(ar.Runtime))
				this.Name = string.Format("{0} {1}", ar.Name, ar.Version);
			else
				this.Name = string.Format("{0} {1} ({2})", ar.Name, ar.Version, ar.Runtime);
			this.Colour = Node.SuggestColour(ar.Name);
		}
	}
}
