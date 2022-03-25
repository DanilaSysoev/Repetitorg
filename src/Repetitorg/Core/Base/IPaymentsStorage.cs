using System;
using System.Collections.Generic;
using System.Text;

namespace Repetitorg.Core.Base
{
    public interface IPaymentsStorage
    {
        IList<Payment> GetAll();
        IList<Payment> GetAllForClient(Client client);
        void Add(Payment payment, Client client);
    }
}
