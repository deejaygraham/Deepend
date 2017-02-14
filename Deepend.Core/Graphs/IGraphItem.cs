using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deepend
{
	/// <summary>
	/// An item appearing in a graph
	/// </summary>
	public interface IGraphItem
	{
		/// <summary>
		/// Unique identifier for this item used to distiguish
		/// objects in a graph
		/// </summary>
		string Id { get; }

		/// <summary>
		/// Friendly name of this item
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Optional free-format data as key-value pairs
		/// </summary>
		Dictionary<string, string> Metadata { get; }
	}
}
