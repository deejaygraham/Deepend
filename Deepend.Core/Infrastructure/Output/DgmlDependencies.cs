using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Deepend
{
    public class DgmlDependencies : IGraphDependencies
    {
		public DgmlDependencies()
		{
			this.FontFamily = "Consolas";
			this.FontSize = 12;
		}

		public string FontFamily { get; set; }

		public int FontSize { get; set; }

		public void Write(Graph<IGraphItem> graph, TextWriter writer)
		{
			writer.WriteLine("<?xml version=\'1.0\' encoding=\'utf-8\'?>");
			writer.WriteLine("<DirectedGraph xmlns=\'http://schemas.microsoft.com/vs/2009/dgml\' GraphDirection=\"LeftToRight\">");

			writer.WriteLine("\t<Nodes>");

			foreach (var node in graph.Nodes)
			{
				writer.Write("\t\t<Node Id=\"{0}\" Label=\"{1}\" ", node.Id, node.Name);

				foreach (var pair in node.Metadata)
				{
					writer.Write("{0}=\"{1}\" ", pair.Key, pair.Value);
				}

				writer.WriteLine("/>");
			}

			writer.WriteLine("\t</Nodes>");

			writer.WriteLine("\t<Links>");

			foreach (var node in graph.Nodes)
			{
				foreach(var edge in graph.EdgesFor(node))
				{
					writer.Write("\t\t<Link Source=\"{0}\" Target=\"{1}\" ", node.Id, edge.Id);

					//foreach (var pair in edge.Metadata)
					//{
					//	writer.Write("{0}=\"{1}\" ", pair.Key, pair.Value);
					//}

					writer.WriteLine("/>");

				}
			}
			writer.WriteLine("\t</Links>");

			writer.WriteLine("\t<Styles>");
			writer.WriteLine("\t\t<Style TargetType=\"Node\">");
			writer.WriteLine("\t\t\t<Setter Property=\"FontFamily\" Value=\"{0}\" />", this.FontFamily);
			writer.WriteLine("\t\t\t<Setter Property=\"FontSize\" Value=\"{0}\" />", this.FontSize);
			writer.WriteLine("\t\t\t<Setter Property=\"NodeRadius\" Value=\"3\" />");
			writer.WriteLine("\t\t</Style>");
			writer.WriteLine("\t\t<Style TargetType=\"Link\">");
			writer.WriteLine("\t\t\t<Setter Property=\"FontFamily\" Value=\"{0}\" />", this.FontFamily);
			writer.WriteLine("\t\t\t<Setter Property=\"FontSize\" Value=\"{0}\" />", this.FontSize);
			writer.WriteLine("\t\t</Style>");
			writer.WriteLine("\t</Styles>");

			writer.WriteLine("</DirectedGraph>");
		}

		//public void Write(Graph<TypeInfo> graph, TextWriter writer)
		//{
		//	// need handling for namespaces ???

		//	writer.WriteLine("<?xml version=\'1.0\' encoding=\'utf-8\'?>");
		//	writer.WriteLine("<DirectedGraph xmlns=\'http://schemas.microsoft.com/vs/2009/dgml\' GraphDirection=\"LeftToRight\">");

		//	writer.WriteLine("\t<Nodes>");

		//	foreach (var node in graph.Nodes)
		//	{
		//		writer.Write("\t\t<Node Id=\"{0}\" Label=\"{1}\" ", node.Id, node.FullName.ToSafeName());

		//		foreach(var pair in node.Metadata)
		//		{
		//			writer.Write("{0}=\"{1}\" ", pair.Key, pair.Value);
		//		}

		//		writer.WriteLine("/>");
		//	}

		//	writer.WriteLine("\t</Nodes>");

		//	writer.WriteLine("\t<Links>");

		//	foreach (var node in graph.Nodes)
		//	{
		//		foreach (var edge in graph.EdgesFor(node))
		//		{
		//			writer.Write("\t\t<Link Source=\"{0}\" Target=\"{1}\" ", node.Id, edge.Id);

		//			//foreach (var pair in edge.Metadata)
		//			//{
		//			//	writer.Write("{0}=\"{1}\" ", pair.Key, pair.Value);
		//			//}

		//			writer.WriteLine(" ./>");
		//		}
		//	}
		//	writer.WriteLine("\t</Links>");

		//	writer.WriteLine("\t<Styles>");
		//	writer.WriteLine("\t\t<Style TargetType=\"Node\">");
		//	writer.WriteLine("\t\t\t<Setter Property=\"FontFamily\" Value=\"{0}\" />", this.FontFamily);
		//	writer.WriteLine("\t\t\t<Setter Property=\"FontSize\" Value=\"{0}\" />", this.FontSize);
		//	writer.WriteLine("\t\t\t<Setter Property=\"NodeRadius\" Value=\"3\" />");
		//	writer.WriteLine("\t\t</Style>");
		//	writer.WriteLine("\t\t<Style TargetType=\"Link\">");
		//	writer.WriteLine("\t\t\t<Setter Property=\"FontFamily\" Value=\"{0}\" />", this.FontFamily);
		//	writer.WriteLine("\t\t\t<Setter Property=\"FontSize\" Value=\"{0}\" />", this.FontSize);
		//	writer.WriteLine("\t\t</Style>");
		//	writer.WriteLine("\t</Styles>");

		//	writer.WriteLine("</DirectedGraph>");
		//}

		// node
		//	if (n.Group)
		//	{
		//		string state = n.Expand ? "Expanded" : "Collapsed";

		//		builder.AppendFormat("Group=\"{0}\" ", state);
		//	}

		// edge
		//	if (e.Group)
		//	{
		//		builder.Append("Category=\"Contains\" ");
		//	}

		//	if (!String.IsNullOrEmpty(e.Description))
		//	{
		//		builder.AppendFormat("Label=\"{0}\" ", e.Description);
		//	}

		//	if (!String.IsNullOrEmpty(e.Colour))
		//	{
		//		builder.AppendFormat("Stroke=\"{0}\" ", e.Colour);
		//	}

		//	if (e.DotLine)
		//	{
		//		builder.Append("StrokeDashArray=\"1,3\" ");
		//	}

    }
}
