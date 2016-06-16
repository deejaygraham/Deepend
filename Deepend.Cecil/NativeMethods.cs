using System;
using System.Runtime.InteropServices;

namespace Deepend
{
	internal static class NativeMethods
	{
		[DllImport("fusion.dll")]
		internal static extern IntPtr CreateAssemblyCache(out IAssemblyCache ppAsmCache, int reserved);

		public static String QueryPathInGlobalAssemblyCache(String assemblyName)
		{
			const int BufferLength = 512;

			ASSEMBLY_INFO assembyInfo = new ASSEMBLY_INFO
			{
				cchBuf = BufferLength,
				currentAssemblyPath = new String('\0', BufferLength)
			};

			IAssemblyCache assemblyCache = null;

			// Get IAssemblyCache pointer
			IntPtr hr = NativeMethods.CreateAssemblyCache(out assemblyCache, 0);
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
