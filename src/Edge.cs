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

		public string Colour { get; set; }

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

		public static string SuggestColour(string name)
		{
			string colour = string.Empty;

			if (name.Contains("Inherits"))
			{
				// red
				colour = "#FFcccc";
			}
			else if (name.Contains("Implements"))
			{
				// gray
				colour = "#D4D4D4";
			}
			else if (name.Contains("Talks"))
			{
				// blue-ish
				colour = "#00BCF2";
			}
			else if (name.Contains("new"))
			{
				colour = "#009900";
			}

			return colour;
		}

	}
}
