using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Deepend
{
    public class DgmlDependencies : IGraphDependencies
    {
		private SortedSet<string> _nodes = new SortedSet<string>();
		private SortedSet<string> _links = new SortedSet<string>();

        public void SaveTo(TextWriter writer)
        {
            writer.WriteLine("<?xml version=\'1.0\' encoding=\'utf-8\'?>");
			writer.WriteLine("<DirectedGraph xmlns=\'http://schemas.microsoft.com/vs/2009/dgml\' GraphDirection=\"LeftToRight\">");

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

			writer.WriteLine("\t<Styles>");
			writer.WriteLine("\t\t<Style TargetType=\"Node\">");
			writer.WriteLine("\t\t\t<Setter Property=\"FontFamily\" Value=\"Consolas\" />");
			writer.WriteLine("\t\t\t<Setter Property=\"FontSize\" Value=\"12\" />");
			writer.WriteLine("\t\t\t<Setter Property=\"Background\" Value=\"White\" />");
			writer.WriteLine("\t\t\t<Setter Property=\"NodeRadius\" Value=\"3\" />");
			writer.WriteLine("\t\t</Style>");
			writer.WriteLine("\t\t<Style TargetType=\"Link\">");
			writer.WriteLine("\t\t\t<Setter Property=\"FontFamily\" Value=\"Consolas\" />");
			writer.WriteLine("\t\t\t<Setter Property=\"FontSize\" Value=\"12\" />");
			writer.WriteLine("\t\t</Style>");
			writer.WriteLine("\t</Styles>");

            writer.WriteLine("</DirectedGraph>");
        }

		public void Write(Graph<AssemblyInfo> graph, TextWriter writer)
		{
			writer.WriteLine("<?xml version=\'1.0\' encoding=\'utf-8\'?>");
			writer.WriteLine("<DirectedGraph xmlns=\'http://schemas.microsoft.com/vs/2009/dgml\' GraphDirection=\"LeftToRight\">");

			writer.WriteLine("\t<Nodes>");

			foreach (var node in graph.Nodes)
			{
				writer.WriteLine("\t\t<Node Id=\"{0}\" Label=\"{1}\" />", node.Id, node.Name);
			}

			writer.WriteLine("\t</Nodes>");

			writer.WriteLine("\t<Links>");

			foreach (var node in graph.Nodes)
			{
				foreach(var edge in graph.EdgesFor(node))
				{
					writer.WriteLine("\t\t<Link Source=\"{0}\" Target=\"{1}\" />", node.Id, edge.Id);
				}
			}
			writer.WriteLine("\t</Links>");

			writer.WriteLine("</DirectedGraph>");
		}

		public void Write(Graph<TypeInfo> graph, TextWriter writer)
		{
			writer.WriteLine("<?xml version=\'1.0\' encoding=\'utf-8\'?>");
			writer.WriteLine("<DirectedGraph xmlns=\'http://schemas.microsoft.com/vs/2009/dgml\' GraphDirection=\"LeftToRight\">");

			writer.WriteLine("\t<Nodes>");

			foreach (var node in graph.Nodes)
			{
				writer.WriteLine("\t\t<Node Id=\"{0}\" Label=\"{1}\" />", node.Id, node.Name.Replace("<", "&lt;").Replace(">", "&gt;"));
			}

			writer.WriteLine("\t</Nodes>");

			writer.WriteLine("\t<Links>");

			foreach (var node in graph.Nodes)
			{
				foreach (var edge in graph.EdgesFor(node))
				{
					writer.WriteLine("\t\t<Link Source=\"{0}\" Target=\"{1}\" />", node.Id, edge.Id);
				}
			}
			writer.WriteLine("\t</Links>");

			writer.WriteLine("</DirectedGraph>");
		}

		public void Node(Node n)
		{
			var builder = new StringBuilder();

			if (n.Name.StartsWith("N_") || String.IsNullOrEmpty(n.Name))
			{

			
			}

			builder.AppendFormat("<Node Id=\"{0}\" Label=\"{1}\" ", n.Id, n.Name);

			if (n.Group)
			{
				string state = n.Expand ? "Expanded" : "Collapsed";

				builder.AppendFormat("Group=\"{0}\" ", state);
			}

			if (!String.IsNullOrEmpty(n.Colour))
			{
				builder.AppendFormat("Background=\"{0}\" ", n.Colour);
			}

			builder.Append(" />");

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

			if (!String.IsNullOrEmpty(e.Colour))
			{
				builder.AppendFormat("Stroke=\"{0}\" ", e.Colour);
			}

			if (e.DotLine)
			{
				builder.Append("StrokeDashArray=\"1,3\" ");
			}

			builder.Append(" />");

			this._links.Add(builder.ToString());
		}
    }
}
