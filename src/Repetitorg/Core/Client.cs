﻿using Repetitorg.Core.Base;
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

        public void DecreaseBalance(long summInCopex)
        {
            CheckConditionsForDecreaseBalance(summInCopex);

            balanceInKopeks -= summInCopex;
            storage.Update(this);
        }
        private static void CheckConditionsForDecreaseBalance(long summInCopex)
        {
            new Checker()
               .Add(summ => summ < 0,
                    summInCopex,
                    "Value of decreasing balance can't be negative.\n")
               .Check();
        }

        public void IncreaseBalance(long summInCopex)
        {
            CheckConditionsForIncreaseBalance(summInCopex);

            balanceInKopeks += summInCopex;
            storage.Update(this);
        }
        private static void CheckConditionsForIncreaseBalance(long summInCopex)
        {
            new Checker()
                  .Add(summ => summ < 0,
                       summInCopex,
                       "Value of increasing balance can't be negative.\n")
                  .Check();
        }

        public void MakePayment(Payment payment)
        {
            CheckConditionsForMakePayment(payment);

            balanceInKopeks += payment.SummInKopeks;
            payment.Client = this;
            Payment.Storage.Add(payment);
            storage.Update(this);
        }
        private static void CheckConditionsForMakePayment(Payment payment)
        {
            new Checker()
                .AddNull(payment, "Payment can't be NULL")
                .Check();
        }

        public void RemovePayment(Payment payment)
        {
            CheckConditionsForRemovePayment(payment);

            balanceInKopeks -= payment.SummInKopeks;
            Payment.Storage.Remove(payment);
            storage.Update(this);
        }
        private void CheckConditionsForRemovePayment(Payment payment)
        {
            new Checker()
                .AddNull(payment, "Payment can't be NULL")
                .Add(c => !c.Payments.Contains(payment), this, "Payment is not exist")
                .Check();
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
        public IList<Payment> GetPaymentsBetween(
            DateTime beginInclude, DateTime endExclude
        )
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
            var client = new Client(fullName, phoneNumber);
            CheckConditionsForCreateNew(fullName, phoneNumber, client);

            storage.Add(client);
            return client;
        }
        private static void CheckConditionsForCreateNew(
            string fullName, string phoneNumber, Client client
        )
        {
            new Checker()
                .AddNull(fullName,
                         string.Format("Can not create client with NULL name"))
                .AddNull(phoneNumber,
                         string.Format("Can not create client with NULL phone number"))
                .Check();
            new Checker()
                .Add(client => storage.GetAll().Contains(client),
                     client,
                     "Creation client with same names and phone numbers is impossible")
                .Check(message => new InvalidOperationException(message));
        }

        public static IReadOnlyList<Client> FilterByName(string condition)
        {
            CheckConditionsForFilterByName(condition);

            return
                (from entity in storage.GetAll()
                 where entity.personData.FullName.ToLower().Contains(condition.ToLower())
                 select entity).ToList();
        }
        private static void CheckConditionsForFilterByName(string condition)
        {
            new Checker().
                AddNull(condition, "Filtering by null pattern is impossible").
                Check();
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
