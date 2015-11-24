using System;
using System.Collections.Generic;
using System.IO;

namespace Deepend
{
    public class DotGraphDependencies : IGraphDependencies
    {
        private List<string> _graphContent = new List<string>();

        public void SaveTo(TextWriter writer)
        {
            writer.WriteLine("digraph G");
            writer.WriteLine("{");
            writer.WriteLine("rankdir = LR");

            foreach (var line in this._graphContent)
            {
                writer.WriteLine(line);
            }

            // close list...
            writer.WriteLine("}");
        }

		public void Node(Node n)
		{
			this._graphContent.Add(n.Name + "[shape = box]");
		}

		public void Edge(Edge e)
		{
			this._graphContent.Add(String.Format("{0} -> {1};", e.From, e.To));
		}
    }
}
