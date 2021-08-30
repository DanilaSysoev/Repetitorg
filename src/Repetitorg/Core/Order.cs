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
        public IReadOnlyList<Student> Students
        {
            get
            {
                return students;
            }
        }

        public void AddStudent(Student student, long costPerHourInCopex)
        {
            new Checker().
                AddNull(student, "Can not add null student to order").
                Add(arg => (long)arg < 0, costPerHourInCopex, "Can not add student with negative cost to order").
                Check();

            students.Add(student);
        }
        public void RemoveStudent(Student student)
        {
            new Checker().AddNull(student, "Student can not be null").
                Check();

            if (!students.Remove(student))
                throw new ArgumentException("Student is not in order");
        }
        public override bool Equals(object obj)
        {
            if(obj is Order)
            {
                var order = obj as Order;
                return
                    order.name.Equals(name);
            }
            return false;
        }
        public override int GetHashCode()
        {
            return name.GetHashCode();
        }
        public override string ToString()
        {
            return string.Format("{0}", name);
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
        public static Order CreateNew(string name)
        {
            new Checker().
                AddNull(name, "Can not create order with null name").
                Check();

            Order order = new Order(name);
            if(orders.Contains(order))
                throw new InvalidOperationException(
                    "Order with given name already exist"
                );

            orders.Add(order);
            return order;
        }


        private string name;
        private List<Student> students;

        private Order(string name)
        {
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
