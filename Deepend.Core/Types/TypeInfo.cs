using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deepend
{
	public class TypeInfo
	{
		private readonly string name;

		public TypeInfo(string fullName)
		{
			this.name = fullName;
		}

		public string Id { get { return this.Name.Replace(".", "_"); } }

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

				return text.Replace("<", "&lt;").Replace(">", "&gt;");
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

		public static bool operator ==(TypeInfo a, TypeInfo b)
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

		public static bool operator !=(TypeInfo a, TypeInfo b)
		{
			return !(a == b);
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}

			TypeInfo another = obj as TypeInfo;

			if ((object)another == null)
			{
				return false;
			}

			const bool IgnoreCase = false;

			return string.Compare(this.FullName, another.FullName, IgnoreCase) == 0;
		}

		public int CompareTo(TypeInfo other)
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
	}
}
