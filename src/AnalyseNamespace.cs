using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Deepend
{
    public class AnalyseNamespace : ISupportedCommand
    {
        public string AssemblyName { get; set; }

        public string Namespace { get; set; }

		public void Execute(IGraphDependencies graph)
        {
			if (!File.Exists(this.AssemblyName))
			{
				throw new FileNotFoundException("File " + this.AssemblyName + " not found");
			}

			if (String.IsNullOrEmpty(this.Namespace))
			{
				throw new ArgumentException("Namespace not specified");
			}

			var assemblyContent = AssemblyDefinition.ReadAssembly(this.AssemblyName);

            var typeInventory = new TypeInventory();

            var factory = new IntrospectedTypeFactory();

            // restrict by namespace
            foreach (var type in assemblyContent.MainModule.Types.Where(t => t.FullName != "<Module>" && t.Namespace.Contains(this.Namespace)))
            {
                factory.CreateFrom(type, typeInventory);
            }

            typeInventory.Generate(graph);
        }
    }
}
