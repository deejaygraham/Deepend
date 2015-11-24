using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deepend
{
	/// <summary>
	/// dependency between nodes
	/// </summary>
	public class Edge : IGraphable
	{
		public string From { get; set; }

		public string To { get; set; }

		public string Description { get; set; }

		public bool Group { get; set; }

		public bool DotLine { get; set; }

		public void WriteTo(IGraphDependencies graph)
		{
			graph.Edge(this);
		}

		public override string ToString()
		{
			return string.Format("{0} -> {1}", this.From, this.To);
		}
	}
}
