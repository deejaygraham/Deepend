using System.IO;

namespace Deepend
{
    public interface IGraphDependencies
    {
        void Declare(IntrospectedType t);
        void Link(IntrospectedType t1, IntrospectedType t2, LinkRelationship relationship);
        void SaveTo(TextWriter writer);
    }

	public enum LinkRelationship
	{
		Unknown,
		Dependency,
		Interface,
		Inheritance
	}
}
