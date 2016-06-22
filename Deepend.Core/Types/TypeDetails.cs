using System;

namespace Deepend
{
	[Flags]
	public enum TypeDetails
	{
		None = 0,
		Fields = 1,
		Methods = 2,
		Properties = 4,
		MethodCalls = 8,
		ObjectCreation = 16,
		Inheritance = 32,
		Interfaces = 64,
		Events = 128,
		All = 255
	}
}
