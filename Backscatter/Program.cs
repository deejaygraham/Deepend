using Deepend;
using NDesk.Options;
using System;
using System.IO;

namespace Backscatter
{
	class Program
	{
		static int Main(string[] args)
		{
			try
            {
				string assemblyPath = string.Empty;

				string specificType = string.Empty;
				string specificNamespace = string.Empty;
				string outputPath = string.Empty;
				bool outputAsDot = false;
				bool outputAsDgml = true;

				bool showHelp = false;

				var options = new OptionSet() {
					{ "a|assembly=", "Path to assembly", v => assemblyPath = v },
					{ "o|output=", "Output Path", v => outputPath = v },
					{ "dot", "Output as .dot file", v => outputAsDot = true },
					{ "dgml", "Output as .dgml file", v => outputAsDgml = true },
					{"?|help", "Show this help", v => showHelp = v != null }
				};
			
				options.Parse(args);

				if (args.Length == 0 || showHelp)
				{
					ShowHelp(options);
					return 0;
				}

				// look at assembly 
				string assemblyFolder = System.IO.Path.GetDirectoryName(assemblyPath);

				// find all assemblies that contain a reference to this assembly
				var graph = ReverseAssemblyReferenceBuilder.Build(assemblyPath, assemblyFolder);

				IGraphDependencies dg = new DgmlDependencies();

				if (String.IsNullOrEmpty(outputPath))
				{
					string graphExtension = ".dgml";

					if (outputAsDot)
					{
						graphExtension = ".dot";
					}
					else if (outputAsDgml)
					{
						graphExtension = ".dgml";
					}

					const string assemblyExtension = ".dll";
					const string exeExtension = ".exe";

					outputPath = assemblyPath.Replace(assemblyExtension, graphExtension).Replace(exeExtension, graphExtension);
				}

				using (StreamWriter writer = new StreamWriter(outputPath))
				{
					dg.Write(graph, writer);
				}
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }

            return 0;
		}

		static void ShowHelp(OptionSet p)
		{
			Console.WriteLine("Backscatter: .net reverse dependency analysis");
			Console.WriteLine();
			Console.WriteLine("Options:");
			p.WriteOptionDescriptions(Console.Out);
		}
	}
}

