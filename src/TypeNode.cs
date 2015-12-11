using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deepend
{
	public class TypeNode : Node
	{
		public TypeNode(IntrospectedType type)
			//: this((type.Namespace.StartsWith("System") || type.Namespace.StartsWith("Microsoft")) ? type.FullName : type.Name)
			: this(type.FullName)
		{
		}

		public TypeNode(TypeName tn)
			: this(tn.FullName)
		{
		}

		private TypeNode(string name)
		{
			this.Name = name.Replace("&", "").Replace("<", "&lt;").Replace(">", "&gt;");
			this.Id = Node.SuggestTypeId(this.Name);
		}
	}
}
