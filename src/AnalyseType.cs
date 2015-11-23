using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Deepend
{
    public class AnalyseType : ISupportedCommand
    {
		public string AssemblyName { get; set; }

        public string TypeName { get; set; }

        public void Execute(IGraphDependencies graph)
        {
			if (!File.Exists(this.AssemblyName))
			{
				throw new FileNotFoundException("File " + this.AssemblyName + " not found");
			}

			if (String.IsNullOrEmpty(this.TypeName))
			{
				throw new ArgumentException("Type not specified");
			}

        }
    }
}
