using Mono.Cecil;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Deepend
{
    public class AnalyseAssemblyTypes //: ISupportedCommand
    {
        public string AssemblyName { get; set; }

		public List<IFilterTypes> Filters { get; private set; }

		public AnalyseAssemblyTypes()
		{
			this.Filters = new List<IFilterTypes>();
			this.Filters.Add(new ModuleFilter());
			this.Filters.Add(new GeneratedTypeFilter());
		}

		public void Execute(IGraphDependencies graph)
        {
			if (!File.Exists(this.AssemblyName))
			{
				throw new FileNotFoundException("File " + this.AssemblyName + " not found");
			}

            var assemblyContent = AssemblyDefinition.ReadAssembly(this.AssemblyName);

			//var typeInventory = new TypeInventory();

			//var factory = new IntrospectedTypeFactory();

			//IEnumerable<TypeDefinition> types = assemblyContent.MainModule.Types;

			//foreach (IFilterTypes filter in this.Filters)
			//{
			//	types = filter.Filter(types);;
			//}

			//var typeList = new TypeNameInventory();

			//foreach (var type in types)
			//{
			//	typeList.Add(new TypeName(type.FullName));
			//}

			//foreach (var type in types)
			//{
			//	typeInventory.Add(factory.CreateFrom(type, typeList));
			//}

			//var list = typeInventory.Generate(typeList);

			//list = list.Concat(typeList.Generate());

			//foreach (var item in list)
			//{
			//	item.WriteTo(graph);
			//}
        }
    }
}
