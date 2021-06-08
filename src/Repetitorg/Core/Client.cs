using Repetitorg.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repetitorg.Core
{
    public class Client : Person
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
            new NullChecker().Add(payment, "Payment can't be NULL").Check();

            balanceInKopeks += payment.ValueInKopeks;
            payments.Add(payment);
        }
        public IList<Payment> GetPaymentsLater(DateTime dateExclude)
        {
            return
                (from payment in payments
                 where payment.Date > dateExclude
                 select payment).ToList();
        }
        public IList<Payment> GetPaymentsBefore(DateTime dateExclude)
        {
            return
                (from payment in payments
                 where payment.Date < dateExclude
                 select payment).ToList();
        }
        public IList<Payment> GetPaymentsBetween(DateTime beginInclude, DateTime endExclude)
        {
            return
                (from payment in payments
                 where payment.Date >= beginInclude && payment.Date < endExclude
                 select payment).ToList();
        }

        public static int ClientsCount 
        {
            get 
            {
                return clients.Count; 
            }
        }
        public static Client CreateNew(string fullName, string phoneNumber = "")
        {
            new NullChecker().
                Add(fullName, "Can not create client with NULL name").
                Add(phoneNumber, "Can not create client with NULL phone number").
                Check();

            var client = new Client(fullName, phoneNumber);

            if (clients.Contains(client))
                throw new InvalidOperationException(
                    "Creation clients with same names and phone numbers is impossible"
                );

            clients.Add(client);
            return client;
        }
        public static List<Client> GetAll()
        {
            return new List<Client>(clients);
        }
        public static IList<Client> FilterByName(string condition)
        {
            return
                (from client in clients
                 where client.FullName.ToLower().Contains(condition.ToLower())
                 select client).ToList();
        }
        public static void Clear()
        {
            clients.Clear();
        }

        static Client()
        {
            clients = new List<Client>();
        }

        private Client(string fullName, string phoneNumber)
            : base(fullName, phoneNumber)
        {
            balanceInKopeks = 0;
            payments = new List<Payment>();
        }

        private long balanceInKopeks;
        private List<Payment> payments;

        private static List<Client> clients;
    }
}
