using Repetitorg.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repetitorg.Core
{
    public class Order : StorageWrapper<Order>
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
                Add(arg => Students.Contains((Student)arg), student, "Student already added").
                Check();

            students.Add(student);
            storage.Update(this);
        }
        public void RemoveStudent(Student student)
        {
            new Checker().AddNull(student, "Student can not be null").
                Check();

            if (!Students.Contains(student))
                throw new ArgumentException("Student is not in order");
            students.Remove(student);
            storage.Update(this);
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

        public static Order CreateNew(string name)
        {
            new Checker().
                AddNull(name, "Can not create order with null name").
                Check();

            Order order = new Order(name);
            if(storage.GetAll().Contains(order))
                throw new InvalidOperationException(
                    "Order with given name already exist"
                );

            storage.Add(order);
            return order;
        }

        private List<Student> students;
        private string name;

        private Order(string name)
        {
            this.name = name;
            students = new List<Student>();
        }
    }
}
