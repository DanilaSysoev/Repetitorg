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
        public static T CreateNew(string fullName, string phoneNumber = "")
        {
            var nc = new NullChecker();

            new NullChecker().
                Add(fullName, string.Format("Can not create {0} with NULL name", typeof(T).Name)).
                Add(phoneNumber, string.Format("Can not create {0} with NULL phone number", typeof(T).Name)).
                Check();

            object[] argsForConstructor = { fullName, phoneNumber };
            var entity = typeof(T).GetConstructors(
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance
            )[0].Invoke(argsForConstructor) as T;

            if (entities.Contains(entity))
                throw new InvalidOperationException(
                    string.Format(
                        "Creation {0} with same names and phone numbers is impossible",
                        typeof(T).Name
                    )
                );

            entities.Add(entity);
            return entity;
        }
        public static IList<T> GetAll()
        {
            return new List<T>(entities);
        }
        public static IList<T> FilterByName(string condition)
        {
            new NullChecker().
                Add(condition, "Filtering by null pattern is impossible").
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
