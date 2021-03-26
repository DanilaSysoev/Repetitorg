using System;
using System.Collections.Generic;
using System.Text;

namespace Repetitorg.Core
{
    public class Payment
    {
        public DateTime Date { get; private set; }
        public long ValueInKopeks { get; private set; }
        public PaymentDocumentType DocumentType { get; private set; }
        public long DocumentNumber { get; private set; }

        private Payment(DateTime date, long valueInKopeks, PaymentDocumentType documentType, long documentNumber)
        {
            Date = date;
            ValueInKopeks = valueInKopeks;
            documentType = DocumentType;
            DocumentNumber = documentNumber;
        }

        public static Payment CreateNew(
            DateTime date, 
            long valueInKopeks,
            PaymentDocumentType documentType,
            long documentNumber
        )
        {
            return new Payment(date, valueInKopeks, documentType, documentNumber);
        }
    }
}
