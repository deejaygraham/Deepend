using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deepend
{
	public interface IFilterTypes
	{
		IEnumerable<TypeDefinition> Filter(IEnumerable<TypeDefinition> typeList);
	}
}
