using System;
using System.Collections.Generic;
using System.Linq;

namespace Deepend
{
	public class TypeInfo : IGraphItem
	{
		private readonly string name;

		public TypeInfo(string fullName)
		{
			this.name = fullName;
			this.Id = fullName.DotsToUnderscores().ToSafeName();
			this.Metadata = new Dictionary<string, string>();
		}

		public string Id { get; private set; }

		public Dictionary<string, string> Metadata { get; private set; }

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
				return this.FullName.ToSafeName();
			}
		}

		public string NameWithoutNamespace
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

		public static bool operator ==(TypeInfo first, TypeInfo second)
		{
			if (object.ReferenceEquals(first, second))
			{
				return true;
			}

			if (((object)first == null) || ((object)second == null))
			{
				return false;
			}

			return first.FullName == second.FullName;
		}

		public static bool operator !=(TypeInfo first, TypeInfo second)
		{
			return !(first == second);
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

			return string.Compare(this.FullName, another.FullName, StringComparison.CurrentCultureIgnoreCase) == 0;
		}

		public int CompareTo(TypeInfo other)
		{
			if (other == null)
			{
				return 1;
			}

			if (this.Namespace != other.Namespace)
			{
				return string.Compare(this.Namespace, other.Namespace, StringComparison.CurrentCultureIgnoreCase);
			}

			return string.Compare(this.FullName, other.FullName, StringComparison.CurrentCultureIgnoreCase);
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
