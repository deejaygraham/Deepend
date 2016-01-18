using NDesk.Options;
using System;
using System.IO;

namespace Deepend
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
				bool analyseReferences = false;
				bool recursive = false;
				string outputPath = string.Empty;
				bool outputAsDot = false;
				bool outputAsDgml = true;

				bool showHelp = false;

				var options = new OptionSet() {
					{ "a|assembly=", "Path to assembly", v => assemblyPath = v },
					{ "t|type=", "Analyse Specific Type", v => specificType = v },
					{ "n|namespace=", "Analyse Specific Namespace", v => specificNamespace = v },
					{ "r|refs", "Analyse References Only", v => analyseReferences = true },
					{ "rr|recursive", "Recursively Analyse References", v => recursive = true },
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

				// default command
				ISupportedCommand command = null;

				if (analyseReferences)
				{
					command = new AnalyseAssemblyReferences
					{
						AssemblyName = assemblyPath,
						Depth = recursive ? ReferenceDepth.Recursive : ReferenceDepth.TopLevelOnly
					};
				}
				else
				{
					AnalyseAssemblyTypes typeAnalysis = new AnalyseAssemblyTypes
					{
						AssemblyName = assemblyPath
					};
					
					if (!String.IsNullOrEmpty(specificType))
					{
						typeAnalysis.Filters.Add(new TypeSpecificFilter(specificType));
					}

					if (!String.IsNullOrEmpty(specificNamespace))
					{
						typeAnalysis.Filters.Add(new NamespaceSpecificFilter(specificNamespace));
					}

					command = typeAnalysis;
				}

				IGraphDependencies dg = null;

				if (outputAsDot)
				{
					dg = new DotGraphDependencies();
				}
				else if (outputAsDgml)
				{
					dg = new DgmlDependencies();
				}

				command.Execute(dg);
				
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
					dg.SaveTo(writer);
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
			Console.WriteLine("Deepend: .net dependency analysis");
			Console.WriteLine();
			Console.WriteLine("Options:");
			p.WriteOptionDescriptions(Console.Out);
		}
    }
}
