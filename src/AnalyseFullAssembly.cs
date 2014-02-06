using Mono.Cecil;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Deepend
{
    public class AnalyseFullAssembly : ISupportedCommand
    {
        private string AssemblyName { get; set; }

        public void Initialise(IEnumerable<string> arguments)
        {
            this.AssemblyName = arguments.First();

            if (!File.Exists(this.AssemblyName))
            {
                throw new FileNotFoundException("File " + this.AssemblyName + " not found");
            }
        }

        public void Execute()
        {
            var assemblyContent = AssemblyDefinition.ReadAssembly(this.AssemblyName);

            var typeInventory = new TypeInventory();

            var factory = new IntrospectedTypeFactory();

            foreach (var type in assemblyContent.MainModule.Types.Where(t => t.FullName != "<Module>"))
            {
                factory.CreateFrom(type, typeInventory);
            }

            bool outputAsDot = false;

            IGraphDependencies dg = null;
            string graphExtension = string.Empty;

            if (outputAsDot)
            {
                dg = new DotGraphDependencies();
                graphExtension = ".dot";
            }
            else
            {
                dg = new DgmlDependencies();
                graphExtension = ".dgml";
            }

            const string assemblyExtension = ".dll";
            string outputFileName = this.AssemblyName.Replace(assemblyExtension, graphExtension);

            typeInventory.Generate(dg);

            using (StreamWriter writer = new StreamWriter(outputFileName))
            {
                dg.SaveTo(writer);
            }
        }

        public string Name { get { return "all"; } }

        public string Syntax
        {
            get
            {
                StringBuilder builder = new StringBuilder();

                builder.AppendLine("<assembly name>");

                return builder.ToString();
            }
        }
    }
}
