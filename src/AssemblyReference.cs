using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Deepend
{
	public class AssemblyReference
	{
		public AssemblyReference()
		{
			this.References = new List<AssemblyReference>();
		}

		public string Name { get; set; }

		public string Version { get; set; }

		public string Runtime { get; set; }

		public AssemblyLocation Location { get; set; }

		public List<AssemblyReference> References { get; set; }

		public static AssemblyReference Load(string name, ReferenceDepth depth, int level, IList<string> assemblies)
		{
			var assemblyContent = AssemblyDefinition.ReadAssembly(name);

			string folder = System.IO.Path.GetDirectoryName(assemblyContent.MainModule.FullyQualifiedName);

			AssemblyReference ar = new AssemblyReference 
			{ 
				Name = assemblyContent.Name.Name, 
				Version = assemblyContent.Name.Version.ToString(),
 				Location = AssemblyLocation.Local,
				Runtime = assemblyContent.MainModule.Runtime.ToString()
			};

			if (depth == ReferenceDepth.TopLevelOnly && level >= 1)
				return ar;


			var refList = assemblyContent.MainModule.AssemblyReferences;

			foreach (var r in refList)
			{
				string candidateAssembly = System.IO.Path.Combine(folder, r.Name + ".dll");

				if (System.IO.File.Exists(candidateAssembly))
				{
					if (!assemblies.Contains(candidateAssembly))
					{
						assemblies.Add(candidateAssembly);
						ar.References.Add(AssemblyReference.Load(candidateAssembly, depth, level + 1, assemblies));
					}
				}
				else
				{
					try
					{
						// handle gac assemblies
						string ai = QueryAssemblyInfo(r.Name);
						ar.References.Add(new AssemblyReference 
						{ 
							Name = r.Name, 
							Version = r.Version.ToString(),
							Location = AssemblyLocation.GlobalAssemblyCache
						});

						if (!assemblies.Contains(candidateAssembly))
						{
							assemblies.Add(candidateAssembly);
						}
					}
					catch(Exception)
					{
						ar.References.Add(new AssemblyReference 
						{ 
							Name = r.Name, 
							Version = r.Version.ToString(),
 							Location = AssemblyLocation.Unknown
						});
					}
					// ignore
				}
			}

			return ar;
		}

		public IEnumerable<IGraphable> Generate(Node gac)
		{
			var list = new HashSet<IGraphable>();

			var thisAssembly = new ReferenceNode(this);

			list.Add(thisAssembly);

			if (this.Location == AssemblyLocation.GlobalAssemblyCache)
				list.Add(new Edge { From = gac.Id, To = thisAssembly.Id, Group = true });

			foreach (var r in this.References)
			{
				var refAssembly = new ReferenceNode(r);

				list.Add(refAssembly);

				var assemblyReferences = new Edge
				{
					From = thisAssembly.Id,
					To = refAssembly.Id
				};

				list.Add(assemblyReferences);

				if (r.Location == AssemblyLocation.GlobalAssemblyCache)
					list.Add(new Edge { From = gac.Id, To = refAssembly.Id, Group = true });

				foreach(var item in r.Generate(gac))
				{
					list.Add(item);
				}
			}

			return list;
		}

		// If assemblyName is not fully qualified, a random matching may be 
		public static String QueryAssemblyInfo(String assemblyName)
		{
			ASSEMBLY_INFO assembyInfo = new ASSEMBLY_INFO();
			assembyInfo.cchBuf = 512;
			assembyInfo.currentAssemblyPath = new String('\0',
				assembyInfo.cchBuf);

			IAssemblyCache assemblyCache = null;

			// Get IAssemblyCache pointer
			IntPtr hr = GacApi.CreateAssemblyCache(out assemblyCache, 0);
			if (hr == IntPtr.Zero)
			{
				hr = assemblyCache.QueryAssemblyInfo(1, assemblyName, ref assembyInfo);
				if (hr != IntPtr.Zero)
				{
					Marshal.ThrowExceptionForHR(hr.ToInt32());
				}
			}
			else
			{
				Marshal.ThrowExceptionForHR(hr.ToInt32());
			}

			return assembyInfo.currentAssemblyPath;
		}
	}

	public enum AssemblyLocation
	{
		Unknown,
		Local,
		GlobalAssemblyCache
	}
	
	public enum ReferenceDepth
	{
		TopLevelOnly,
		Recursive
	}

	internal class GacApi
	{
		[DllImport("fusion.dll")]
		internal static extern IntPtr CreateAssemblyCache(out IAssemblyCache ppAsmCache, int reserved);
	}

	// GAC Interfaces - IAssemblyCache. As a sample, non used vtable entries     
	[ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("e707dcde-d1cd-11d2-bab9-00c04f8eceae")]
	internal interface IAssemblyCache
	{
		int Dummy1();
		[PreserveSig()]
		IntPtr QueryAssemblyInfo(
			int flags,
			[MarshalAs(UnmanagedType.LPWStr)]
			String assemblyName,
			ref ASSEMBLY_INFO assemblyInfo);

		int Dummy2();
		int Dummy3();
		int Dummy4();
	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct ASSEMBLY_INFO
	{
		public int cbAssemblyInfo;
		public int assemblyFlags;
		public long assemblySizeInKB;

		[MarshalAs(UnmanagedType.LPWStr)]
		public String currentAssemblyPath;

		public int cchBuf;
	}
}
