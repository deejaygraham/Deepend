using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

			var assemblyContent = AssemblyDefinition.ReadAssembly(this.AssemblyName);

			var typeInventory = new TypeInventory();

			var factory = new IntrospectedTypeFactory();

			// restrict by namespace
			foreach (var type in assemblyContent.MainModule.Types.Where(t => t.FullName != "<Module>" && t.Name.Equals(this.TypeName, StringComparison.CurrentCultureIgnoreCase)))
			{
				factory.CreateFrom(type, typeInventory);
			}

			var list = typeInventory.Generate();

			foreach (var item in list)
			{
				item.WriteTo(graph);
			}

        }
    }
}
