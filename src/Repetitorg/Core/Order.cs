using System;
using System.Collections.Generic;
using System.Text;

namespace Repetitorg.Core
{
    public class Order
    {
        public Client Client
        {
            get
            {
                return client;
            }
        }


        public static int Count 
        { 
            get
            {
                return orders.Count;
            }
        }
        public static void Clear()
        {
            orders.Clear();
        }
        public static Order CreateNew(string name, Client client)
        {
            new NullChecker().
                Add(client, "Can not create order with null client").
                Add(name, "Can not create order with null name").
                Check();

            Order order = new Order(client);
            orders.Add(order);
            return order;
        }


        private Client client;

        private Order(Client client)
        {
            this.client = client;
        }

        static Order()
        {
            orders = new List<Order>();
        }
        private static List<Order> orders;
    }
}
