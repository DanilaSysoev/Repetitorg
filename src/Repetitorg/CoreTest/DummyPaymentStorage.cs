using Repetitorg.Core;
using Repetitorg.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repetitorg.CoreTest
{
    public class DummyPaymentStorage : IPaymentStorage
    {
        private Dictionary<Client, List<Payment>> payments;

        public DummyPaymentStorage()
        {
            payments = new Dictionary<Client, List<Payment>>();
        }

        public void Add(Payment payment, Client client)
        {
            if (!payments.ContainsKey(client))
                payments.Add(client, new List<Payment>());
            payments[client].Add(payment);
        }

        public IReadOnlyList<Payment> GetAll()
        {
            return null;
        }

        public IReadOnlyList<Payment> GetAllForClient(Client client)
        {
            if (!payments.ContainsKey(client))
                payments.Add(client, new List<Payment>());
            return payments[client];
        }

        public void Remove(Payment payment)
        {
            foreach(var client in payments.Keys)
                if(payments[client].Contains(payment))
                    payments[client].Remove(payment);
        }
    }
}
