using System;
using System.Collections.Generic;
using System.Text;

namespace Repetitorg.Core.Base
{
    public interface IPaymentsStorage
    {
        IReadOnlyList<Payment> GetAll();
        IReadOnlyList<Payment> GetAllForClient(Client client);
        void Add(Payment payment, Client client);
        void Remove(Payment payment);
    }
}
