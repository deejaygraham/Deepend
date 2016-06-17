using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deepend
{
	public class AssemblyInfo : IComparable<AssemblyInfo>
	{
		public AssemblyInfo(string name, Version version)
			: this(name, version, string.Empty, AssemblyLocation.Unknown)
		{
		}

		public AssemblyInfo(string name, Version version, string runtimeVersion, AssemblyLocation location)
		{
			this.Name = name;
			this.Version = version.ToString();
			this.Runtime = runtimeVersion;
			this.Location = location;
		}

		public string Id { get { return this.Name.Replace(".", "_"); } }

		public string Name { get; private set; }

		public string Version { get; private set; }

		public string Runtime { get; private set; }

		public AssemblyLocation Location { get; private set; }

		public override string ToString()
		{
			return string.Format("{0} {1}", this.Name, this.Version);
		}

		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}

			AssemblyInfo another = obj as AssemblyInfo;

			if ((object)another == null)
			{
				return false;
			}

			const bool IgnoreCase = false;

			return string.Compare(this.ToString(), another.ToString(), IgnoreCase) == 0;
		}

		public int CompareTo(AssemblyInfo other)
		{
			if (other == null)
			{
				return 1;
			}

			return this.ToString().CompareTo(other.ToString());
		}


	}
}
