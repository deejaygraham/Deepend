using System.Collections.Generic;
using System.Linq;

namespace Deepend
{
	public class TypeInfo
	{
		private readonly string name;

		public TypeInfo(string fullName)
		{
			this.name = fullName;
			this.Id = fullName.DotsToUnderscores().ToSafeName();
			this.Metadata = new Dictionary<string, string>();
		}

		public string Id { get; private set; }

		public Dictionary<string, string> Metadata { get; set; }

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

	public static class StringExtensions
	{
		public static string ToSafeName(this string name)
		{
			string safe = name;

			if (name.EndsWith("&"))
				safe = name.TrimEnd(new char[] { '&' });
 
			return safe.Replace("<", "&lt;").Replace(">", "&gt;");
		}

		public static string DotsToUnderscores(this string name)
		{
			return name.Replace(".", "_").Replace("&", "");
		}
	}
}
