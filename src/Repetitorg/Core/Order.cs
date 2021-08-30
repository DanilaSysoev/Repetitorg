using System;
using System.Collections.Generic;
using System.Text;

namespace Repetitorg.Core
{
    public class Order
    {
        public static int Count 
        { 
            get
            {
                return orders.Count;
            }
        }

        public static void Clear()
        { }

        public static Order CreateNew(Client client)
        {
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
