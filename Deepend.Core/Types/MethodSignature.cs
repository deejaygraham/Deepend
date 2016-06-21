using System.Collections.Generic;

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
