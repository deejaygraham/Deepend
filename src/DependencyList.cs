using Mono.Cecil;
using System.Collections.Generic;

namespace Deepend
{
    [System.Obsolete("Not used")]
    public class DependencyList
    {
        private Dictionary<string, List<string>> _dependencies = new Dictionary<string, List<string>>();

        public void Add(TypeReference unit, TypeReference friend)
        {
            if (!unit.ShouldBeIncluded() || !friend.ShouldBeIncluded())
                return;

            if (unit.FullName == friend.FullName)
                return;

            string unitName = unit.Name.Replace("&", "");
            string friendName = friend.Name.Replace("&", "");

            if (unitName.Contains("AnonymousType") || unitName.StartsWith("<"))
                return;

            if (friendName.Contains("AnonymousType") || friendName.StartsWith("<"))
                return;

            if (!this._dependencies.ContainsKey(unitName))
                this._dependencies.Add(unitName, new List<string>());

            var list = this._dependencies[unitName];
            
            if (!list.Contains(friendName))
                list.Add(friendName);
        }

        public List<string> Units()
        {
            var list = new List<string>();

            list.AddRange(this._dependencies.Keys);

            return list;
        }

        public List<string> FriendsOf(string unit)
        {
            try
            {
                return this._dependencies[unit];
            }
            catch (KeyNotFoundException)
            {
                return new List<string>();
            }
        }
    }

}
