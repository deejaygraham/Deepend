using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deepend
{
	[DebuggerDisplay("{Name}")]
	public class TypeName : IComparable<TypeName>
	{
		private readonly string name;

		public TypeName(string fullName)
		{
			this.name = fullName;
		}

		public string FullName
		{
			get
			{
				return this.name;
			}
		}

		public string Name
		{
			get
			{
				string text = this.name;

				const char Dot = '.';

				if (this.name.Contains(Dot))
				{
					int index = this.name.LastIndexOf(Dot);
					text = this.name.Substring(index + 1);
				}

				return text;
			}
		}

		public string Namespace
		{
			get
			{
				string text = string.Empty;

				const char Dot = '.';

				if (name.Contains(Dot))
				{
					int index = this.name.LastIndexOf(Dot);
					text = this.name.Substring(0, index);
				}

				return text;
			}
		}

		public override string ToString()
		{
			return this.FullName;
		}

		public override int GetHashCode()
		{
			return this.FullName.GetHashCode();
		}

		public static bool operator ==(TypeName a, TypeName b)
		{
			if (object.ReferenceEquals(a, b))
			{
				return true;
			}

			if (((object)a == null) || ((object)b == null))
			{
				return false;
			}

			return a.FullName == b.FullName;
		}

		public static bool operator !=(TypeName a, TypeName b)
		{
			return !(a == b);
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}

			TypeName another = obj as TypeName;

			if ((object)another == null)
			{
				return false;
			}

			const bool IgnoreCase = false;

			return string.Compare(this.FullName, another.FullName, IgnoreCase) == 0;
		}

		public int CompareTo(TypeName other)
		{
			if (other == null)
			{
				return 1;
			}

			if (this.Namespace != other.Namespace)
			{
				return this.Namespace.CompareTo(other.Namespace);
			}

			return this.FullName.CompareTo(other.FullName);
		}

		public IEnumerable<IGraphable> Generate()
		{
			var list = new HashSet<IGraphable>();

			if (this.FullName.StartsWith("<"))
				return list;

			var thisType = new TypeNode(this);

			list.Add(thisType);

			if (!String.IsNullOrEmpty(this.Namespace))
			{
				// most specific namespace
				var thisNamespace = new GroupNode(Node.SuggestNamespaceId(this.Namespace), this.Namespace);
				// I am in this namespace...
				list.Add(thisNamespace);
				list.Add(new GroupingRelationship(thisNamespace, thisType));

				string childNamespace = this.Namespace;
				string parentNamespace = childNamespace.ParentNamespaceOf();

				while (!String.IsNullOrEmpty(parentNamespace))
				{
					var childGroup = new GroupNode(Node.SuggestNamespaceId(childNamespace), childNamespace);
					list.Add(childGroup);

					var parentGroup = new GroupNode(Node.SuggestNamespaceId(parentNamespace), parentNamespace);
					list.Add(parentGroup);

					list.Add(new GroupingRelationship(parentGroup, childGroup));

					childNamespace = parentNamespace;
					parentNamespace = childNamespace.ParentNamespaceOf();
				}
			}

			return list;
		}
	}

	public class TypeNameInventory
	{
		private HashSet<TypeName> _typeNames = new HashSet<TypeName>();

		public void Add(TypeName t)
		{
			this._typeNames.Add(t);
		}

		public TypeName this[string fullName]
		{
			get
			{
				var found = this._typeNames.FirstOrDefault(x => x.ToString() == fullName);

				if (found == null)
				{
					found = new TypeName(fullName);

					this._typeNames.Add(found);
				}

				return found;
			}
		}

		public IEnumerable<IGraphable> Generate()
		{
			var list = new List<IGraphable>();

			foreach (var t in this._typeNames)
			{
				list.AddRange(t.Generate());
			}

			return list;
		}

	}
}
