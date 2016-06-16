using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deepend
{
	public class TypeSpecificFilter : IFilterTypes
	{
		private readonly string _typename;

		public TypeSpecificFilter(string name)
		{
			this._typename = name;
		}

		public IEnumerable<TypeDefinition> Filter(IEnumerable<TypeDefinition> typeList)
		{
			foreach (TypeDefinition type in typeList)
			{
				if (!type.FullName.Equals(this._typename, StringComparison.CurrentCultureIgnoreCase))
					continue;

				yield return type;
			}
		}
	}
}
