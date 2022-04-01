using Repetitorg.Core;
using Repetitorg.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repetitorg.CoreTest
{
    class DummyOrderStorage : IStorage<Order>
    {
        private List<Order> orders;
        private Dictionary<Order, List<Student>> studentsInOrder;

        public DummyOrderStorage()
        {
            studentsInOrder = new Dictionary<Order, List<Student>>();
            orders = new List<Order>();
        }

        public void Add(Order order)
        {
            orders.Add(order);
        }

        public void AttachStudent(Order order, Student student)
        {
            if(!studentsInOrder.ContainsKey(order))
                studentsInOrder.Add(order, new List<Student>());
            studentsInOrder[order].Add(student);
        }

        public void DetachStudent(Order order, Student student)
        {
            studentsInOrder[order].Remove(student);
        }

        public IReadOnlyList<Order> Filter(Predicate<Order> predicate)
        {
            return (from order in orders
                    where predicate(order)
                    select order).ToList();
        }

        public IReadOnlyList<Order> GetAll()
        {
            return orders;
        }

        public IReadOnlyList<Student> GetStudentsForOrder(Order order)
        {
            if (!studentsInOrder.ContainsKey(order))
                studentsInOrder.Add(order, new List<Student>());
            return studentsInOrder[order];
        }

        public void Remove(Order entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Order entity)
        {
        }
    }
}
