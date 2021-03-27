using Repetitorg.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repetitorg.Core
{
    public class Client
    {
        public string FullName
        {
            get
            {
                return fullName;
            }
        }
        public string PhoneNumber
        {
            get
            {
                return phoneNumber;
            }
            set
            {
                if (value == null)
                    throw new InvalidPhoneNumberException("PhoneNumber can't be null");
                phoneNumber = value;
            }
        }
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
        public static Client CreateNew(string fullName, string phoneNumber = "")
        {
            var client = new Client(fullName, phoneNumber);
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

        private Client(string fullName, string phoneNumber)
        {
            this.fullName = fullName;
            this.phoneNumber = phoneNumber;
            balanceInKopeks = 0;
            payments = new List<Payment>();
        }

        private string fullName;
        private string phoneNumber;
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
