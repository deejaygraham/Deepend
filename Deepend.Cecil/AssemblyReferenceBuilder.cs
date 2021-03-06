﻿using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deepend
{
	public static class AssemblyReferenceBuilder
	{
		#region Colours

		// dgml specific !
		private static readonly Dictionary<string, Dictionary<string, string>> assemblyMetadata = new Dictionary<string, Dictionary<string, string>>
		{
			//{ 
			//	"Microsoft.WindowsAzure", 
			//	new Dictionary<string, string> 
			//	{ 
			//		{ "Background", "#1a1a1a" }, 
			//		{ "Foreground", "#2793c5" }
			//	}
			//},
			{ 
				"System", 
				new Dictionary<string, string> 
				{ 
					{ "Background", "#68217a" }, 
					{ "Stroke", "#68217a" },
					{ "Foreground", "#fff" }
				}
			},
			{ 
				"mscorlib", 
				new Dictionary<string, string> 
				{ 
					{ "Background", "#68217a" }, 
					{ "Stroke", "#68217a" }, 
					{ "Foreground", "#fff" }
				}
			},
			{ 
				"Microsoft", 
				new Dictionary<string, string>
				{ 
					{ "Background", "#91ce00" }, 
					{ "Stroke", "#91ce00" }, 
					{ "Foreground", "#fff" }
				}
			},
			{
				"PresentationCore",
				new Dictionary<string, string>
				{
					{ "Background", "#91ce00" }, 
					{ "Stroke", "#91ce00" }, 
					{ "Foreground", "#fff" }
				}
			},
			{
				"PresentationFramework",
				new Dictionary<string, string>
				{
					{ "Background", "#91ce00" }, 
					{ "Stroke", "#91ce00" }, 
					{ "Foreground", "#fff" }
				}
			},
			{
				"WindowsBase",
				new Dictionary<string, string>
				{
					{ "Background", "#91ce00" }, 
					{ "Stroke", "#91ce00" }, 
					{ "Foreground", "#fff" }
				}
			},
			{
				"WindowsForms",
				new Dictionary<string, string>
				{
					{ "Background", "#91ce00" }, 
					{ "Stroke", "#91ce00" }, 
					{ "Foreground", "#fff" }
				}
			},
			{
				"UIAutomation",
				new Dictionary<string, string>
				{
					{ "Background", "#91ce00" }, 
					{ "Stroke", "#91ce00" }, 
					{ "Foreground", "#fff" }
				}
			}
		};

		#endregion Colours

		public static Graph<IGraphItem> Build(string assembly, ReferenceDepth depth)
		{
			var graph = new Graph<IGraphItem>();

			Build(graph, null, assembly, depth, 0, new List<string>());

			return graph;
		}

		private static void Build(Graph<IGraphItem> graph, AssemblyInfo dependent, string assemblyName, ReferenceDepth depth, int level, IList<string> visitedAssemblies)
		{
			var reflection = AssemblyDefinition.ReadAssembly(assemblyName);

			var assembly = new AssemblyInfo
			(
				reflection.Name.Name,
				reflection.Name.Version,
				reflection.MainModule.Runtime.ToString().Replace("_", "."),
				AssemblyLocation.Local
			);

			if (level == 0)
			{
				// starting assembly !
				assembly.Metadata.Add("Background", "#ffb900");
			}
			else
			{
				foreach (var metadata in assemblyMetadata)
				{
					if (assembly.Name.StartsWith(metadata.Key))
					{
						foreach (var specific in metadata.Value)
						{
							assembly.Metadata.Add(specific.Key, specific.Value);
						}
					}
				}
			}

			if (dependent == null)
				graph.Add(assembly);
			else
				graph.EdgeBetween(dependent, assembly);

			if (depth == ReferenceDepth.TopLevelOnly && level >= 1)
				return;

			// now look at references to this assembly...
			string folder = System.IO.Path.GetDirectoryName(reflection.MainModule.FullyQualifiedName);

			foreach (var reference in reflection.MainModule.AssemblyReferences)
			{
				string referenceAssembly = reference.Name + ".dll";
				string pathToReference = System.IO.Path.Combine(folder, referenceAssembly);

				if (!System.IO.File.Exists(pathToReference))
				{
					// is it in the GAC?
					try
					{
						pathToReference = NativeMethods.QueryPathInGlobalAssemblyCache(reference.Name);
					}
					catch
					{
					}
				}

				if (!System.IO.File.Exists(pathToReference))
				{
					Console.WriteLine("Reference {0} not found", pathToReference);
					graph.EdgeBetween(assembly, new AssemblyInfo(reference.Name, reference.Version));
				}
				else
				{
					if (!visitedAssemblies.Contains(referenceAssembly))
					{
						visitedAssemblies.Add(referenceAssembly);
						Build(graph, assembly, pathToReference, depth, level + 1, visitedAssemblies);
					}
					else
					{
						// already seen it!
					}
				}
			}
		}
	}
}
