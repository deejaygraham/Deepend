using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

		public List<TypeInfo> CreatedObjects { get; set; }
	}
}
