using Mono.Cecil;
using System.Collections.Generic;

namespace Deepend
{
    public class TypeInventory
    {
        private SortedDictionary<string, IntrospectedType> _types = new SortedDictionary<string, IntrospectedType>();

        public IntrospectedType this[TypeReference index]
        {
            get
            {
                if (!this._types.ContainsKey(index.FullName))
                {
                    IntrospectedType it = new IntrospectedType
                    {
                        Name = index.Name,
                        Namespace = index.Namespace
                    };

                    if (!index.ShouldBeIncluded())
                    {
                        return it;
                    }

                    this._types.Add(index.FullName, it);
                }

                return this._types[index.FullName];
            }
        }

        public IntrospectedType this[string fullName]
        {
            get
            {
                return this._types[fullName];
            }
        }

        public void Generate(IGraphDependencies dependencies)
        {
            foreach (var t in this._types.Keys)
            {
                IntrospectedType it = this._types[t];

                it.WriteSelf(dependencies);
            }

            foreach(var t in this._types.Keys)
            {
                IntrospectedType it = this._types[t];

                it.WriteRelationships(dependencies, this);
            }
        }
    }
}
