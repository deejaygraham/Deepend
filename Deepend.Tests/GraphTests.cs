using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Deepend;
using System.Linq;

namespace Deepend.Tests
{
	[TestClass]
	public class GraphTests
	{
		[TestMethod]
		public void Graph_Independent_Items_Are_Not_Linked()
		{
			Graph<int> g = new Graph<int>();

			g.Add(1);
			g.Add(2);

			Assert.AreEqual(0, g.EdgesFor(1).Count());
			Assert.AreEqual(0, g.EdgesFor(2).Count());
		}

		[TestMethod]
		public void Graph_Dependent_Items_Are_Linked()
		{
			Graph<int> g = new Graph<int>();

			int node = 1;
			int dependency = 2;

			g.EdgeBetween(node, dependency);

			Assert.AreEqual(1, g.EdgesFor(node).Count());
		}

		[TestMethod]
		public void Graph_Dependency_Is_Not_Reciprocated()
		{
			Graph<int> g = new Graph<int>();

			int node = 1;
			int dependency = 2;

			g.EdgeBetween(node, dependency);

			Assert.AreEqual(0, g.EdgesFor(dependency).Count());
		}

		[TestMethod]
		public void Graph_Multi_Dependencies_Are_Unidirectional()
		{
			Graph<int> g = new Graph<int>();

			int startNode = 1;
			int middleNode = 2;
			int endNode = 3;

			g.EdgeBetween(startNode, middleNode);
			g.EdgeBetween(middleNode, endNode);

			Assert.AreEqual(1, g.EdgesFor(startNode).Count());
			Assert.AreEqual(1, g.EdgesFor(middleNode).Count());
			Assert.AreEqual(0, g.EdgesFor(endNode).Count());
		}

	}
}

