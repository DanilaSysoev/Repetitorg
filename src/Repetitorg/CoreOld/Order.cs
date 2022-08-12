using Repetitorg.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
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
                return orders.GetStudentsForOrder(this);
            }
        }

        public void AddStudent(Student student, long costPerHourInCopex)
        {
            new Checker().
                AddNull(student, "Can not add null student to order").
                Add(arg => (long)arg < 0, costPerHourInCopex, "Can not add student with negative cost to order").
                Add(arg => Students.Contains((Student)arg), student, "Student already added").
                Check();

            orders.AttachStudent(this, student);
        }
        public void RemoveStudent(Student student)
        {
            new Checker().AddNull(student, "Student can not be null").
                Check();

            if (!Students.Contains(student))
                throw new ArgumentException("Student is not in order");
            orders.DetachStudent(this, student);
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
                return orders.GetAll().Count;
            }
        }

        public static Order CreateNew(string name)
        {
            new Checker().
                AddNull(name, "Can not create order with null name").
                Check();

            Order order = new Order(name);
            if(orders.GetAll().Contains(order))
                throw new InvalidOperationException(
                    "Order with given name already exist"
                );

            orders.Add(order);
            return order;
        }


        private string name;

        private Order(string name)
        {
            this.name = name;
        }

        private static IOrderStorage orders;
        public static void InitializeStorage(IOrderStorage storage)
        {
            orders = storage;
        }
    }
}
