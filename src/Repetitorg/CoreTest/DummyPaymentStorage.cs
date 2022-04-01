using Repetitorg.Core;
using Repetitorg.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repetitorg.CoreTest
{
    public class DummyPaymentStorage : IStorage<Payment>
    {
        private List<Payment> payments;

        public DummyPaymentStorage()
        {
            payments = new List<Payment>();
        }

        public void Add(Payment entity)
        {
            payments.Add(entity);
        }

        public IReadOnlyList<Payment> Filter(Predicate<Payment> predicate)
        {
            return (from payment in GetAll()
                    where predicate(payment)
                    select payment).ToList();
        }

        public IReadOnlyList<Payment> GetAll()
        {
            return payments;
        }

        public void Remove(Payment payment)
        {
            payments.Remove(payment);
        }

        public void Update(Payment entity)
        {
        }
    }
}
