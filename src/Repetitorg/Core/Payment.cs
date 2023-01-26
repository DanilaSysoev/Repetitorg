using Repetitorg.Core.Base;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Repetitorg.Core
{
    public class Payment : StorageWrapper<Payment>, IId
    {
        public long Id { get; private set; }
        public DateTime Date { get; private set; }
        public long SummInKopeks { get; private set; }
        public PaymentDocumentType DocumentType { get; private set; }
        public string DocumentId { get; private set; }
        public Client Client { get; internal set; }

        private Payment(DateTime date, long valueInKopeks, PaymentDocumentType documentType, string documentId)
        {
            Date = date;
            SummInKopeks = valueInKopeks;
            DocumentType = documentType;
            DocumentId = documentId;
        }

        public static Payment CreateNew(
            DateTime date, 
            long valueInKopeks,
            PaymentDocumentType documentType,
            string documentId
        )
        {            
            var payment = new Payment(date, valueInKopeks, documentType, documentId);
            CheckConditionsForCreateNew(payment);
            payment.Id = storage.Add(payment);
            return payment;
        }

        public static Payment CreateLoaded(
            long id,
            DateTime date,
            long valueInKopeks,
            PaymentDocumentType documentType,
            string documentId,
            Client client
        )
        {
            var payment = new Payment(date, valueInKopeks, documentType, documentId);
            payment.Id = id;
            payment.Client = client;
            return payment;
        }

        private static void CheckConditionsForCreateNew(Payment payment)
        {
            new Checker()
                .Add(valueInKopeks => valueInKopeks <= 0,
                     payment.SummInKopeks,
                     "Payment should has positive value. " +
                     "Can not create payment with non-positive value")
                .AddNull(
                     payment.DocumentId,
                     "Can not create payment with null document id")
                .Add(payment => Storage.Filter((p) => p.Equals(payment)).Count > 0,
                     payment,
                     "Payment already exist")
                .Check();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Payment) || obj == null)
                return false;
            Payment p = obj as Payment;
            return
                p.Date.Equals(Date) &&
                p.DocumentId.Equals(DocumentId) &&
                p.DocumentType.Equals(DocumentType) &&
                p.SummInKopeks.Equals(SummInKopeks);
        }
        public override int GetHashCode()
        {
            return System.HashCode.Combine(
                Date, 
                DocumentId,
                DocumentType,
                SummInKopeks
            );
        }
        public override string ToString()
        {
            return string.Format(
                NumberFormatInfo.InvariantInfo,
                "{0}: {1:0.00}", 
                Date.ToString("d"),
                SummInKopeks / 100.0
            );
        }
    }
}
