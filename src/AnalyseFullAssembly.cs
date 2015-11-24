using Mono.Cecil;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Deepend
{
    public class AnalyseFullAssembly : ISupportedCommand
    {
        public string AssemblyName { get; set; }

		public void Execute(IGraphDependencies graph)
        {
			if (!File.Exists(this.AssemblyName))
			{
				throw new FileNotFoundException("File " + this.AssemblyName + " not found");
			}

            var assemblyContent = AssemblyDefinition.ReadAssembly(this.AssemblyName);

            var typeInventory = new TypeInventory();

            var factory = new IntrospectedTypeFactory();

            foreach (var type in assemblyContent.MainModule.Types.Where(t => t.FullName != "<Module>"))
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
