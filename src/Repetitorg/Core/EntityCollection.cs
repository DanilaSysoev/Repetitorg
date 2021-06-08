using Repetitorg.Core.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repetitorg.Core
{
    class EntityCollection<T> where T : Setupable, new()
    {
        private static List<T> entities;

        public static int Count
        {
            get
            {
                return entities.Count;
            }
        }
        public static T CreateNew(string fullName, string phoneNumber = "")
        {
            new NullChecker().
                Add(fullName, "Can not create client with NULL name").
                Add(phoneNumber, "Can not create client with NULL phone number").
                Check();

            var entity = new T();
            entity.Setup(fullName, phoneNumber);

            if (entities.Contains(entity))
                throw new InvalidOperationException(
                    "Creation clients with same names and phone numbers is impossible"
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
            return
                (from entity in entities
                 where client.fullName.ToLower().Contains(condition.ToLower())
                 select client).ToList();
        }
        public static void Clear()
        {
            entities.Clear();
        }
        static EntityCollection()
        {
            entities = new List<T>();
        }
    }
}
