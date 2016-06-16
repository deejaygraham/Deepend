using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deepend
{
	public class AssemblyInfo // needs to be IEquatable etc.
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

		public string Name { get; private set; }

		public string Version { get; private set; }

		public string Runtime { get; private set; }

		public AssemblyLocation Location { get; private set; }
	}
}
