using System;
using System.Collections.Generic;
using System.Text;

namespace Repetitorg.Core.Base
{
    public interface IOrderStorage
    {
        void Add(Order order);
        void AttachStudent(Order order, Student student);
        void DetachStudent(Order order, Student student);
        IReadOnlyList<Student> GetStudentsForOrder(Order order);
        IReadOnlyList<Order> GetAll();
    }
}
