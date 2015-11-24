using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deepend
{
	/// <summary>
	/// Independent entity 
	/// </summary>
	public class Node : IGraphable
	{
		public Node()
		{
			this.Id = Guid.NewGuid().ToString();
		}

		public string Id { get; set; }

		public string Name { get; set; }

		public bool Group { get; set; }

		public string Colour { get; set; }

		public static string SuggestTypeId(string name)
		{
			return string.Format("CLS_{0}", name);
//			return string.Format("T_{0}", name.Replace('.', '_'));
		}

		public static string SuggestNamespaceId(string name)
		{
			return string.Format("NS_{0}", name);
//			return string.Format("N_{0}", name.Replace('.', '_'));
		}

		public static string SuggestAssemblyId(string name)
		{
			return string.Format("A_{0}", name.Replace(".", "_"));
		}

		public static string SuggestColour(string name)
		{
			string colour = string.Empty;

			if (name.Contains("WindowsAzure"))
			{
				// azure blue
				colour = "#71B1D1";
			}
			else if (name.StartsWith("Microsoft."))
			{
				// blue-ish
				colour = "#00BCF2";
			}
			else if (name.StartsWith("System") || name.StartsWith("mscor"))
			{
				// gray
				colour = "#D4D4D4";
			}

			return colour;
		}

		public void WriteTo(IGraphDependencies graph)
		{
			graph.Node(this);
		}

		public override string ToString()
		{
			return string.Format("{0} {1}", this.Id, this.Name);
		}
	}
}
