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
        public IList<Student> Students
        {
            get
            {
                return studentsCosts.Keys.ToList();
            }
        }

        public long GetCostPerHourFor(Student student)
        {
            CheckConditionsForGetCostPerHourFor(student);
            return studentsCosts[student];
        }
        private void CheckConditionsForGetCostPerHourFor(Student student)
        {
            new Checker()
                .AddNull(student, "Student can't be null.\n")
                .Add(s => s != null && !studentsCosts.ContainsKey(s),
                    student,
                    "Student not in order.\n")
                .Check();
        }

        public void AddStudent(Student student, long costPerHourInCopex)
        {
            CheckConditionsForAddStudent(student, costPerHourInCopex);

            studentsCosts.Add(student, costPerHourInCopex);
            storage.Update(this);
        }
        private void CheckConditionsForAddStudent(Student student, long costPerHourInCopex)
        {
            new Checker()
                .AddNull(student, "Can not add null student to order")
                .Add(arg => arg < 0,
                        costPerHourInCopex,
                        "Can not add student with negative cost to order")
                .Add(arg => Students.Contains(arg),
                        student,
                        "Student already added")
                .Check();
        }

        public void RemoveStudent(Student student)
        {
            CheckConditionsForRemoveStudent(student);

            studentsCosts.Remove(student);
            storage.Update(this);
        }
        private void CheckConditionsForRemoveStudent(Student student)
        {
            new Checker()
                .AddNull(student, "Student can not be null")
                .Add(order => !order.Students.Contains(student),
                     this,
                     "Student is not in order")
                .Check();
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
            Order order = new Order(name);
            CheckConditionsForCreateNew(name, order);

            storage.Add(order);
            return order;
        }
        private static void CheckConditionsForCreateNew(string name, Order order)
        {
            new Checker()
                .AddNull(name, "Can not create order with null name")
                .Check();
            new Checker()
                .Add(order => storage.GetAll().Contains(order),
                     order,
                     "Order with given name already exist")
                .Check(message => new InvalidOperationException(message));
        }

        private Dictionary<Student, long> studentsCosts;
        private string name;

        private Order(string name)
        {
            this.name = name;
            studentsCosts = new Dictionary<Student, long>();
        }
    }
}
