using System;
using System.Collections.Generic;
using System.Text;

namespace Repetitorg.Core
{
    public class Client
    {
        public static double ClientsCount 
        {
            get 
            {
                return clients.Count; 
            }
        }

        public static IEnumerable<Client> All
        {
            get
            {
                return clients;
            }
        }

        public static Client CreateNew(string fullName)
        {
            var client = new Client(fullName);
            clients.Add(client);
            return client;
        }

        public static void Clear()
        {
            clients.Clear();
        }

        static Client()
        {
            clients = new List<Client>();
        }

        private Client(string fullName)
        {
            this.fullName = fullName;
        }

        private string fullName;

        private static List<Client> clients;

        public static IReadOnlyList<Client> FilterByName(string condition)
        {
            throw new NotImplementedException();
        }
    }
}
