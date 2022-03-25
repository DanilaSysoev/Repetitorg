﻿using Repetitorg.Core.Base;
using Repetitorg.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repetitorg.Core
{
    public class Client : PersonsCollection<Client>
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
                return payments.GetAllForClient(this);
            }
        }

        public void MakePayment(Payment payment)
        {
            new Checker().AddNull(payment, "Payment can't be NULL").Check();

            balanceInKopeks += payment.ValueInKopeks;
            payments.Add(payment, this);
        }
        public IList<Payment> GetPaymentsLater(DateTime dateExclude)
        {
            return
                (from payment in payments.GetAllForClient(this)
                 where payment.Date > dateExclude
                 select payment).ToList();
        }
        public IList<Payment> GetPaymentsBefore(DateTime dateExclude)
        {
            return
                (from payment in payments.GetAllForClient(this)
                 where payment.Date < dateExclude
                 select payment).ToList();
        }
        public IList<Payment> GetPaymentsBetween(DateTime beginInclude, DateTime endExclude)
        {
            return
                (from payment in payments.GetAllForClient(this)
                 where payment.Date >= beginInclude && payment.Date < endExclude
                 select payment).ToList();
        }

        public static Client CreateNew(IPaymentsStorage payments, string fullName, string phoneNumber = "")
        {
            new Checker().
                AddNull(fullName, string.Format("Can not create client with NULL name")).
                AddNull(phoneNumber, string.Format("Can not create client with NULL phone number")).
                Check();

            var client = new Client(payments, fullName, phoneNumber);

            if (entities.GetAll().Contains(client))
                throw new InvalidOperationException(
                     "Creation client with same names and phone numbers is impossible"
                );

            entities.Add(client);
            return client;
        }
        public static void InitializeStorage(IPersonStorage<Client> clients)
        {
            entities = clients;
        }

        internal Client(IPaymentsStorage payments, string fullName, string phoneNumber)
            : base(fullName, phoneNumber)
        {
            balanceInKopeks = 0;
            this.payments = payments;
        }

        private long balanceInKopeks;
        private IPaymentsStorage payments;
    }
}
