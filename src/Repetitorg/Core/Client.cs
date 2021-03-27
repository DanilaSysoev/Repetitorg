using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repetitorg.Core
{
    public class Client
    {
        public long BalanceInKopeks 
        {
            get
            {
                return balanceInKopeks;
            }
        }
        public IReadOnlyList<Payment> Payments
        {
            get
            {
                return payments;
            }
        }

        public void MakePayment(Payment payment)
        {
            balanceInKopeks += payment.ValueInKopeks;
            payments.Add(payment);
        }
        public IList<Payment> GetPaymentsLater(DateTime date)
        {
            return
                (from payment in payments
                 where payment.Date > date
                 select payment).ToList();
        }
        public IList<Payment> GetPaymentsBefore(DateTime date)
        {
            return
                (from payment in payments
                 where payment.Date < date
                 select payment).ToList();
        }

        public static double ClientsCount 
        {
            get 
            {
                return clients.Count; 
            }
        }
        public static IEnumerable<Client> All
        {
            get
            {
                return clients;
            }
        }
        public static Client CreateNew(string fullName)
        {
            var client = new Client(fullName);
            clients.Add(client);
            return client;
        }
        public static void Clear()
        {
            clients.Clear();
        }

        static Client()
        {
            clients = new List<Client>();
        }

        private Client(string fullName)
        {
            this.fullName = fullName;
            balanceInKopeks = 0;
            payments = new List<Payment>();
        }

        private string fullName;
        private long balanceInKopeks;
        private List<Payment> payments;

        private static List<Client> clients;

        public static IList<Client> FilterByName(string condition)
        {
            return 
                (from client in clients
                 where client.fullName.ToLower().Contains(condition.ToLower())
                 select client).ToList();
        }

        public override string ToString()
        {
            return fullName;
        }
    }
}
