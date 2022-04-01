using Repetitorg.Core.Base;
using Repetitorg.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repetitorg.Core
{
    public class Client : StorageWrapper<Client>
    {
        public long BalanceInKopeks 
        {
            get
            {
                return balanceInKopeks;
            }
        }
        public IList<Payment> Payments
        {
            get
            {
                return Payment.Storage.Filter(p => p.Client.Equals(this));
            }
        }
        public Person PersonData
        {
            get { return personData; }
        }

        public void MakePayment(Payment payment)
        {
            new Checker().AddNull(payment, "Payment can't be NULL").Check();

            balanceInKopeks += payment.ValueInKopeks;
            payment.Client = this;
            Payment.Storage.Add(payment);
            storage.Update(this);
        }
        public void RemovePayment(Payment payment)
        {
            new Checker()
                .AddNull(payment, "Payment can't be NULL")
                .Add(c => !c.Payments.Contains(payment), this, "Payment is not exist")
                .Check();

            balanceInKopeks -= payment.ValueInKopeks;
            Payment.Storage.Remove(payment);
            storage.Update(this);
        }
        public IList<Payment> GetPaymentsLater(DateTime dateExclude)
        {
            return
                (from payment in Payments
                 where payment.Date > dateExclude
                 select payment).ToList();
        }
        public IList<Payment> GetPaymentsBefore(DateTime dateExclude)
        {
            return
                (from payment in Payments
                 where payment.Date < dateExclude
                 select payment).ToList();
        }
        public IList<Payment> GetPaymentsBetween(DateTime beginInclude, DateTime endExclude)
        {
            return
                (from payment in Payments
                 where payment.Date >= beginInclude && payment.Date < endExclude
                 select payment).ToList();
        }
        public override bool Equals(object obj)
        {
            if (obj is Client)
                return personData.Equals(((Client)obj).PersonData);
            return false;
        }
        public override int GetHashCode()
        {
            return personData.GetHashCode();
        }
        public override string ToString()
        {
            return personData.ToString();
        }


        public static Client CreateNew(string fullName, string phoneNumber = "")
        {
            new Checker().
                AddNull(fullName, string.Format("Can not create client with NULL name")).
                AddNull(phoneNumber, string.Format("Can not create client with NULL phone number")).
                Check();

            var client = new Client(fullName, phoneNumber);

            if (storage.GetAll().Contains(client))
                throw new InvalidOperationException(
                     "Creation client with same names and phone numbers is impossible"
                );

            storage.Add(client);
            return client;
        }
        public static IReadOnlyList<Client> FilterByName(string condition)
        {
            new Checker().
                AddNull(condition, "Filtering by null pattern is impossible").
                Check();

            return
                (from entity in storage.GetAll()
                 where entity.personData.FullName.ToLower().Contains(condition.ToLower())
                 select entity).ToList();
        }        

        private Client(string fullName, string phoneNumber)
        {
            balanceInKopeks = 0;
            personData = new Person(fullName, phoneNumber);
        }

        private long balanceInKopeks;
        private Person personData;
    }
}
