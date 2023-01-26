using NUnit.Framework;
using Repetitorg.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repetitorg.CoreTest
{
    [TestFixture]
    class PaymentDocumentTypeTests
    {
        private DummyPaymentDocumentTypeStorage storage;
        [SetUp]
        public void Initialize()
        {
            storage = new DummyPaymentDocumentTypeStorage();
            PaymentDocumentType.SetupStorage(storage);
        }

        [TestCase]
        public void CreateNew_newStorage_CountEqualsZero()
        {
            Assert.AreEqual(0, PaymentDocumentType.Count);
        }
        [TestCase]
        public void CreateNew_CreateWithSomeName_CountIncreasingByOne()
        {
            int oldCount = PaymentDocumentType.Count;
            PaymentDocumentType.CreateNew("test type");
            Assert.AreEqual(oldCount + 1, PaymentDocumentType.Count);
        }
        [TestCase]
        public void CreateNew_CreateWithSomeName_PropertySetCorrect()
        {
            var docType = PaymentDocumentType.CreateNew("test type");
            Assert.AreEqual("test type", docType.Name);
        }
        [TestCase]
        public void CreateNew_CreateWithEmptyName_ThrowsError()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => PaymentDocumentType.CreateNew("")
            );
            Assert.IsTrue(
                exception.Message.ToLower().Contains(
                    "document type name can't be empty"
                )
            );
        }
        [TestCase]
        public void CreateNew_CreateWithNullName_ThrowsError()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => PaymentDocumentType.CreateNew(null)
            );
            Assert.IsTrue(
                exception.Message.ToLower().Contains(
                    "document type name can't be null"
                )
            );
        }
        [TestCase]
        public void CreateNew_CreateWithSomeName_AddingCountIncrease()
        {
            int oldAddCount = storage.AddCount;
            PaymentDocumentType.CreateNew("test type");
            Assert.AreEqual(oldAddCount + 1, storage.AddCount);
        }
        [TestCase]
        public void CreateNew_CreateWithExistingName_ThrowsError()
        {            
            PaymentDocumentType.CreateNew("test type");
            var exception = Assert.Throws<ArgumentException>(
                () => PaymentDocumentType.CreateNew("test type")
            );
            Assert.IsTrue(
                exception.Message.ToLower().Contains(
                    "payment document type already exist"
                )
            );
        }
        [TestCase]
        public void CreateNew_CreateWithDifferentName_Ok()
        {
            PaymentDocumentType.CreateNew("test type");
            Assert.DoesNotThrow(
                () => PaymentDocumentType.CreateNew("test type 2")
            );
        }

        [TestCase]
        public void Equals_CreateWithSameNames_AreEquals()
        {
            var docType1 = PaymentDocumentType.CreateNew("test type");
            Initialize();
            var docType2 = PaymentDocumentType.CreateNew("test type");
            Assert.IsTrue(docType1.Equals(docType2));
            Assert.IsTrue(docType2.Equals(docType1));
        }
        [TestCase]
        public void Equals_CreateWithDifferentNames_NotEquals()
        {
            var docType1 = PaymentDocumentType.CreateNew("test type");
            var docType2 = PaymentDocumentType.CreateNew("test type 1");
            Assert.IsFalse(docType1.Equals(docType2));
            Assert.IsFalse(docType2.Equals(docType1));
        }

        [TestCase]
        public void ToString_CreateWithSomeName_ReturnName()
        {
            var docType = PaymentDocumentType.CreateNew("Test type");
            Assert.AreEqual("Test type", docType.ToString());
        }


    }
}
