using System;
using System.IO;

namespace Deepend
{
	public interface IGraphDependencies
	{
		void Write(Graph<IGraphItem> graph, TextWriter writer);
		//void Write(Graph<TypeInfo> graph, TextWriter writer);
	}
}
