using System.Collections.Generic;

namespace Deepend
{
	public class MethodInfo
	{
		public MethodInfo ()
		{
			this.Signature = new MethodSignature();
			this.CreatedObjects = new List<TypeInfo>();
		}

		public MethodSignature Signature { get; set; }

		public List<TypeInfo> CreatedObjects { get; private set; }
	}
}
