using System;
using System.Collections.Generic;
using System.Text;

namespace Repetitorg.Core
{
    public class Client
    {
        public static double ClientsCount 
        {
            get { return clients.Count; }
        }

        public static void CreateNew(string fullName)
        {
            clients.Add(new Client(fullName));
        }

        public static void Initialize()
        {
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
    }
}
