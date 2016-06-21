using System;

namespace Deepend
{
	[Flags]
	public enum TypeDetail
	{
		None = 0,
		Fields = 1,
		Methods = 2,
		Properties = 4,
		MethodCalls = 8,
		ObjectCreation = 16,
		Inheritance = 32,
		Interfaces = 64,
		All = 255
	}
}
