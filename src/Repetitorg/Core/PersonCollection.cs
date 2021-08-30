using Repetitorg.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repetitorg.Core
{
    public class PersonsCollection<T> : Person where T : Person
    {
        public static int Count
        {
            get
            {
                return entities.Count;
            }
        }
        public static IList<T> GetAll()
        {
            return new List<T>(entities);
        }
        public static IList<T> FilterByName(string condition)
        {
            new Checker().
                AddNull(condition, "Filtering by null pattern is impossible").
                Check();

            return
                (from entity in entities
                 where entity.FullName.ToLower().Contains(condition.ToLower())
                 select entity).ToList();
        }
        public static void Clear()
        {
            entities.Clear();
        }


        internal PersonsCollection(string fullName, string phoneNumber)
            : base(fullName, phoneNumber)
        { }
        protected static List<T> entities;
        static PersonsCollection()
        {
            entities = new List<T>();
        }
    }
}
