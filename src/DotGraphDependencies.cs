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

        public void Declare(IntrospectedType t)
        {
            this._graphContent.Add(t.Name + "[shape = box]"); 
        }

        public void Link(IntrospectedType t1, IntrospectedType t2, LinkRelationship relationship)
        {
            this._graphContent.Add(String.Format("{0} -> {1};", t1.Name, t2.Name));
        }
    }
}
