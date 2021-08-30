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
                return payments;
            }
        }

        public void MakePayment(Payment payment)
        {
            new Checker().AddNull(payment, "Payment can't be NULL").Check();

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

        internal Client(string fullName, string phoneNumber)
            : base(fullName, phoneNumber)
        {
            balanceInKopeks = 0;
            payments = new List<Payment>();
        }

        private long balanceInKopeks;
        private List<Payment> payments;
    }
}
