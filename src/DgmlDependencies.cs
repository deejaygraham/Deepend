using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Deepend
{
    public class DgmlDependencies : IGraphDependencies
    {
		private HashSet<string> _nodes = new HashSet<string>();
		private HashSet<string> _links = new HashSet<string>();

        public void SaveTo(TextWriter writer)
        {
            writer.WriteLine("<?xml version=\'1.0\' encoding=\'utf-8\'?>");
            writer.WriteLine("<DirectedGraph xmlns=\'http://schemas.microsoft.com/vs/2009/dgml\'>");

            writer.WriteLine("\t<Nodes>");

			foreach (string node in this._nodes)
			{
				writer.WriteLine("\t\t{0}", node);
			}

            writer.WriteLine("\t</Nodes>");

            writer.WriteLine("\t<Links>");

			foreach (string link in this._links)
			{
				writer.WriteLine("\t\t{0}", link);
			}

            writer.WriteLine("\t</Links>");

            writer.WriteLine("</DirectedGraph>");
        }

		public void Node(Node n)
		{
			var builder = new StringBuilder();

			builder.AppendFormat("<Node Id=\"{0}\" Label=\"{1}\" ", n.Id, n.Name);

			if (n.Group)
			{
				builder.Append("Group=\"Expanded\" ");
			}

			if (!String.IsNullOrEmpty(n.Colour))
			{
				builder.AppendFormat("Background=\"{0}\" ", n.Colour);
			}

			builder.AppendLine(" />");

			this._nodes.Add(builder.ToString());
		}

		public void Edge(Edge e)
		{
			var builder = new StringBuilder();

			builder.AppendFormat("<Link Source=\"{0}\" Target=\"{1}\" ", e.From, e.To);

			if (e.Group)
			{
				builder.Append("Category=\"Contains\" ");
			}

			if (!String.IsNullOrEmpty(e.Description))
			{
				builder.AppendFormat("Label=\"{0}\" ", e.Description);
			}

			if (e.DotLine)
			{
				builder.Append("StrokeDashArray=\"1,3\" ");
			}

			builder.AppendLine(" />");

			this._links.Add(builder.ToString());
		}
    }
}
