using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deepend
{
	public class MethodSignature
	{
		public MethodSignature()
		{
			this.Parameters = new List<TypeInfo>();
		}

		public TypeInfo ReturnType { get; set; }

		public List<TypeInfo> Parameters { get; private set; }
	}
}
