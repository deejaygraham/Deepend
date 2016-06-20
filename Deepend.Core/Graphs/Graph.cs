using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Deepend
{
	public class Graph<T>
	{
		private List<T> nodes = new List<T>();
		private Dictionary<int, HashSet<int>> edges = new Dictionary<int, HashSet<int>>();

		readonly int InvalidIndex = -1;

		public int Add(T node)
		{
			if (node == null)
				return InvalidIndex;

			// find by name ?? 
			if (!this.nodes.Contains(node))
				this.nodes.Add(node);

			int index = this.nodes.IndexOf(node);

			if (!this.edges.ContainsKey(index))
				this.edges.Add(index, new HashSet<int>());

			return index;
		}

		public void EdgeBetween(T from, T to)
		{
			int fromIndex = Add(from);
			int toIndex = Add(to);

			if (fromIndex == InvalidIndex || toIndex == InvalidIndex)
				return;

//			throw new ArgumentException(string.Format("Edges cannot connect two instances of the same object {0} -> {1}", from.ToString(), to.ToString()));

			if (fromIndex != toIndex)
			{
				this.edges[fromIndex].Add(toIndex);
			}
		}

		public IEnumerable<T> Nodes
		{
			get
			{
				return new ReadOnlyCollection<T>(this.nodes);
			}
		}

		public T FindIf(Predicate<T> predicate)
		{
			foreach (T node in this.nodes)
			{
				if (predicate(node))
					return node;
			}

			return default(T);
		}

		public IEnumerable<T> EdgesFor(T from)
		{
			int index = this.nodes.IndexOf(from);

			var list = new List<T>();

			if (this.edges.ContainsKey(index))
			{
				foreach (int dependIndex in this.edges[index])
				{
					list.Add(this.nodes[dependIndex]);
				}
			}

			return list;
		}
	}

}
