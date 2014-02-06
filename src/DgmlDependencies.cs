using System;
using System.Collections.Generic;
using System.IO;

namespace Deepend
{
    public class DgmlDependencies : IGraphDependencies
    {
        private List<string> _nodes = new List<string>();
        private List<KeyValuePair<string, string>> _links = new List<KeyValuePair<string,string>>();
		private List<KeyValuePair<string, string>> _inheritances = new List<KeyValuePair<string, string>>();
		private List<KeyValuePair<string, string>> _implementations = new List<KeyValuePair<string, string>>();
        
        private Dictionary<string, List<string>> _namespacesToTypes = new Dictionary<string, List<string>>();
        // groups for ms, system...

        public void SaveTo(TextWriter writer)
        {
            writer.WriteLine("<?xml version=\'1.0\' encoding=\'utf-8\'?>");
            writer.WriteLine("<DirectedGraph xmlns=\'http://schemas.microsoft.com/vs/2009/dgml\'>");

            writer.WriteLine("\t<Nodes>");

            //int i = 0;

            this._nodes.Sort();

            foreach (string node in this._nodes)
            {
                writer.WriteLine("\t\t<Node Id=\"CLS_{0}\" Label=\"{1}\" />", node, node);
            }

            foreach (string node in this._namespacesToTypes.Keys)
            {
                writer.WriteLine("\t\t<Node Id=\"NS_{0}\" Label=\"{0}\" Group=\"Expanded\" />", node);
            }

            writer.WriteLine("\t</Nodes>");

            writer.WriteLine("\t<Links>");

            foreach (string node in this._namespacesToTypes.Keys)
            {
                var list = this._namespacesToTypes[node];

                foreach (string c in list)
                {
                    writer.WriteLine("\t\t<Link Source=\"NS_{0}\" Target=\"CLS_{1}\" Category=\"Contains\" />", node, c);
                }
            }

            foreach (KeyValuePair<string, string> pair in this._links)
            {
				writer.WriteLine("\t\t<Link Source=\"CLS_{0}\" Target=\"CLS_{1}\" />", pair.Key, pair.Value);
            }

			foreach (KeyValuePair<string, string> pair in this._inheritances)
			{
				writer.WriteLine("\t\t<Link Source=\"CLS_{0}\" Target=\"CLS_{1}\" Label=\"Derived From\" />", pair.Key, pair.Value);
			}

			foreach (KeyValuePair<string, string> pair in this._implementations)
			{
				writer.WriteLine("\t\t<Link Source=\"CLS_{0}\" Target=\"CLS_{1}\" Label=\"Implements\" StrokeDashArray=\"1,3\" />", pair.Key, pair.Value);
			}

            writer.WriteLine("\t</Links>");

            writer.WriteLine("</DirectedGraph>");
        }

        public void Declare(IntrospectedType t)
        {
            if (t.Name.StartsWith("<"))
                return;

            string t1Name = t.Name;
            
            if (t.Namespace.StartsWith("System") || t.Namespace.StartsWith("Microsoft"))
                t1Name = t.Namespace + "." + t.Name;

            if (!this._namespacesToTypes.ContainsKey(t.Namespace))
                this._namespacesToTypes.Add(t.Namespace, new List<string>());

			t1Name = t1Name.Replace("&", "");

            if (!this._nodes.Contains(t1Name))
            {
                this._nodes.Add(t1Name);
                this._namespacesToTypes[t.Namespace].Add(t1Name);
            }
        }

        public void Link(IntrospectedType t1, IntrospectedType t2, LinkRelationship relationship)
        {
            if (t1.Name.StartsWith("<") || t2.Name.StartsWith("<"))
                return;

            string t1Name = t1.Name;
            string t2Name = t2.Name;
			
            if (String.Compare(t1Name, t2Name, true) == 0)
                return;

            if (t1.Namespace.StartsWith("System") || t1.Namespace.StartsWith("Microsoft"))
                t1Name = t1.Namespace + "." + t1.Name;

            if (t2.Namespace.StartsWith("System") || t2.Namespace.StartsWith("Microsoft"))
                t2Name = t2.Namespace + "." + t2.Name;

            if (!this._namespacesToTypes.ContainsKey(t1.Namespace))
                this._namespacesToTypes.Add(t1.Namespace, new List<string>());

            if (!this._namespacesToTypes.ContainsKey(t2.Namespace))
                this._namespacesToTypes.Add(t2.Namespace, new List<string>());

			t1Name = t1Name.Replace("&", "");
			t2Name = t2Name.Replace("&", "");

            this._namespacesToTypes[t1.Namespace].Add(t1Name);
            this._namespacesToTypes[t2.Namespace].Add(t2Name);

			KeyValuePair<string, string> pair = new KeyValuePair<string, string>(t1Name, t2Name);

			if (relationship == LinkRelationship.Dependency)
				this._links.Add(pair);
			else if (relationship == LinkRelationship.Inheritance)
				this._inheritances.Add(pair);
			else if (relationship == LinkRelationship.Interface)
				this._implementations.Add(pair);
        }
    }
}
