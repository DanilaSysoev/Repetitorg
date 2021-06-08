using System;
using System.Collections.Generic;
using System.Text;

namespace Repetitorg.Core
{
    class EntityCollection<T>
    {
        private static List<T> entities;

        public static int Count
        {
            get
            {
                return entities.Count;
            }
        }
        public static Client CreateNew(string fullName, string phoneNumber = "")
        {
            new NullChecker().
                Add(fullName, "Can not create client with NULL name").
                Add(phoneNumber, "Can not create client with NULL phone number").
                Check();

            var entity = new T(fullName, phoneNumber);

            if (entities.Contains(client))
                throw new InvalidOperationException(
                    "Creation clients with same names and phone numbers is impossible"
                );

            entities.Add(client);
            return client;
        }
        public static List<Client> GetAll()
        {
            return new List<Client>(entities);
        }
        public static IList<Client> FilterByName(string condition)
        {
            return
                (from client in entities
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
