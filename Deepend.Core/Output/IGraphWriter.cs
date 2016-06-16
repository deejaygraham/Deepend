using System.IO;

namespace Deepend
{
	public interface IGraphWriter<T>
	{
		void Write(Graph<T> graph, TextWriter writer);
	}
}
