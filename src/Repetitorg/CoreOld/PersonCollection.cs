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
                return entities.GetAll().Count;
            }
        }
        public static IReadOnlyList<T> GetAll()
        {
            return entities.GetAll();
        }
        public static IReadOnlyList<T> FilterByName(string condition)
        {
            new Checker().
                AddNull(condition, "Filtering by null pattern is impossible").
                Check();

            return
                (from entity in entities.GetAll()
                 where entity.FullName.ToLower().Contains(condition.ToLower())
                 select entity).ToList();
        }


        internal PersonsCollection(string fullName, string phoneNumber)
            : base(fullName, phoneNumber)
        { }
        protected static IPersonStorage<T> entities;
    }
}
