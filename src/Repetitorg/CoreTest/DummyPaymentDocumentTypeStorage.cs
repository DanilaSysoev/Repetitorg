using Repetitorg.Core;
using Repetitorg.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repetitorg.CoreTest
{
    class DummyPaymentDocumentTypeStorage : IStorage<PaymentDocumentType>
    {
        private List<PaymentDocumentType> payments;
        public int AddCount { get; private set; }

        public DummyPaymentDocumentTypeStorage()
        {
            payments = new List<PaymentDocumentType>();
            AddCount = 0;
        }

        public long Add(PaymentDocumentType entity)
        {
            payments.Add(entity);
            AddCount += 1;
            return payments.Count;
        }

        public IList<PaymentDocumentType> Filter(Predicate<PaymentDocumentType> predicate)
        {
            return (from payment in GetAll()
                    where predicate(payment)
                    select payment).ToList();
        }

        public IReadOnlyList<PaymentDocumentType> GetAll()
        {
            return payments;
        }

        public void Remove(PaymentDocumentType payment)
        {
            payments.Remove(payment);
        }

        public void Update(PaymentDocumentType entity)
        {
        }
    }
}
