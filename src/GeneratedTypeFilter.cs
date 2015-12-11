using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deepend
{
	public class GeneratedTypeFilter : IFilterTypes
	{
		public IEnumerable<TypeDefinition> Filter(IEnumerable<TypeDefinition> typeList)
		{
			foreach (TypeDefinition type in typeList)
			{
				if (type.FullName.StartsWith("<"))
					continue;

				yield return type;
			}
		}
	}
}
