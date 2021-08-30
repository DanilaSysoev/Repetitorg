using System;
using System.Collections.Generic;
using System.Text;

namespace Repetitorg.Core
{
    public class Order
    {
        public string Name
        {
            get
            {
                return name;
            }
        }
        public Client Client
        {
            get
            {
                return client;
            }
        }
        public IReadOnlyList<Student> Students
        {
            get
            {
                return students;
            }
        }

        public void AddStudent(Student student)
        {
            new Checker().Add(student, "Can not add null student to order").
                Check();

            students.Add(student);
        }
        public override bool Equals(object obj)
        {
            if(obj is Order)
            {
                var order = obj as Order;
                return
                    order.client.Equals(client) &&
                    order.name.Equals(name);
            }
            return false;
        }
        public override int GetHashCode()
        {
            return (name.GetHashCode() + client.GetHashCode()) * 31;
        }
        public override string ToString()
        {
            return string.Format("{0}: {1}", client.FullName, name);
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
            new Checker().
                Add(client, "Can not create order with null client").
                Add(name, "Can not create order with null name").
                Check();

            Order order = new Order(name, client);
            if(orders.Contains(order))
                throw new InvalidOperationException(
                    "Order with given name and client already exist"
                );

            orders.Add(order);
            return order;
        }


        private Client client;
        private string name;
        private List<Student> students;

        private Order(string name, Client client)
        {
            this.client = client;
            this.name = name;
            this.students = new List<Student>();
        }

        static Order()
        {
            orders = new List<Order>();
        }
        private static List<Order> orders;
    }
}
