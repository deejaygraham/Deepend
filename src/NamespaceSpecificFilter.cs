using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deepend
{
	public class NamespaceSpecificFilter : IFilterTypes
	{
		private readonly string _namespace;

		public NamespaceSpecificFilter(string ns)
		{
			this._namespace = ns;
		}

		public IEnumerable<TypeDefinition> Filter(IEnumerable<TypeDefinition> typeList)
		{
			foreach (TypeDefinition type in typeList)
			{
				if (type.FullName.StartsWith(this._namespace))
					continue;

				yield return type;
			}
		}
	}
}
