using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deepend
{
	public class ModuleFilter : IFilterTypes
	{
		public IEnumerable<TypeDefinition> Filter(IEnumerable<TypeDefinition> typeList)
		{
			foreach (TypeDefinition type in typeList)
			{
				if (type.FullName == "<Module>")
					continue;

				yield return type;
			}
		}
	}
}
