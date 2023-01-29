using Repetitorg.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repetitorg.Core
{
    public class PaymentDocumentType : StorageWrapper<PaymentDocumentType>, IId
    {
        public long Id { get; private set; }
        public string Name { get; private set; }

        public static PaymentDocumentType CreateNew(string documentTypeName)
        {
            PaymentDocumentType documentType = 
                new PaymentDocumentType(documentTypeName);
            CheckConditionsForCreateNew(documentType);

            documentType.Id = storage.Add(documentType);
            return documentType;
        }
        private static void CheckConditionsForCreateNew(
            PaymentDocumentType documentType
        )
        {
            new Checker()
                .AddNull(documentType.Name,
                         "Document type name can't be null")
                .Add(name => name == "",
                     documentType.Name,
                     "Document type name can't be empty")
                .Add(type => storage.GetAll().Contains(type),
                     documentType,
                     "Payment document type already exist")
                .Check();
        }

        public static PaymentDocumentType CreateLoaded(
            long id, string documentTypeName
        )
        {
            PaymentDocumentType documentType = 
                new PaymentDocumentType(documentTypeName);
            documentType.Id = id;
            return documentType;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is PaymentDocumentType) || obj == null)
                return false;
            return Name.Equals(((PaymentDocumentType)obj).Name);
        }
        public override string ToString()
        {
            return Name;
        }

        public override int GetHashCode()
        {
            return System.HashCode.Combine(Id, Name);
        }

        private PaymentDocumentType(string documentTypeName)
        {
            Name = documentTypeName;

            NotesUpdated += () => storage.Update(this);
        }
    }
}
