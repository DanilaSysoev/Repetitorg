using NUnit.Framework;
using Repetitorg.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repetitorg.CoreTest
{
    [TestFixture]
    class PaymentTests
    {
        DummyPaymentStorage payments;
        DummyPaymentDocumentTypeStorage paymentDocuments;

        PaymentDocumentType paymentOrder;

        [SetUp]
        public void Initialize()
        {
            payments = new DummyPaymentStorage();
            paymentDocuments = new DummyPaymentDocumentTypeStorage();
            Payment.SetupStorage(payments);
            PaymentDocumentType.SetupStorage(paymentDocuments);

            paymentOrder = PaymentDocumentType.CreateNew("PaymentOrder");
        }

        [TestCase]
        public void CreateNew_CreateTwoPayments_IncrementCount()
        {
            Assert.AreEqual(0, Payment.Count);
            Payment.CreateNew(
                new DateTime(2022, 02, 01),
                3920,
                paymentOrder,
                "123"
            );
            Assert.AreEqual(1, Payment.Count);
            Payment.CreateNew(
                new DateTime(2022, 02, 01),
                3920,
                paymentOrder,
                "111"
            );
            Assert.AreEqual(2, Payment.Count);
        }
        [TestCase]
        public void CreateNew_CreateWithZeroValue_ThrowsError()
        {
            var exception =
                Assert.Throws<ArgumentException>(
                    () => Payment.CreateNew(
                             new DateTime(2022, 02, 01),
                             0,
                             paymentOrder,
                             "123"
                         )
                );
            Assert.IsTrue(exception.Message.ToLower().Contains(
                "can not create payment with non-positive value"
            ));
        }
        [TestCase]
        public void CreateNew_CreateWithNullDocumentId_ThrowsError()
        {
            var exception =
                Assert.Throws<ArgumentException>(
                    () => Payment.CreateNew(
                             new DateTime(2022, 02, 01),
                             1000,
                             paymentOrder,
                             null
                         )
                );
            Assert.IsTrue(exception.Message.ToLower().Contains(
                "can not create payment with null document id"
            ));
        }
        [TestCase]
        public void CreateNew_CreateOnePayment_CountOfAddCallsIncrease()
        {
            int addCnt = payments.AddCount;
            Payment.CreateNew(
                new DateTime(2022, 02, 01),
                1000,
                paymentOrder,
                "123"
            );
            Assert.AreEqual(addCnt + 1, payments.AddCount);
        }
        [TestCase]
        public void CreateNew_CreateOnePayment_SetupPropertiesCorrectly()
        {
            var payment = Payment.CreateNew(
                new DateTime(2022, 02, 01),
                1000,
                paymentOrder,
                "123"
            );
            Assert.AreEqual(new DateTime(2022, 02, 01), payment.Date);
            Assert.AreEqual(1000, payment.SummInKopeks);
            Assert.AreEqual(paymentOrder, payment.DocumentType);
            Assert.AreEqual("123", payment.DocumentId);
        }
        [TestCase]
        public void CreateNew_CreateExistingPayment_ThrowsError()
        {
            var payment = Payment.CreateNew(
                new DateTime(2022, 02, 01),
                1000,
                paymentOrder,
                "123"
            );
            var exception =
                Assert.Throws<ArgumentException>(
                    () => Payment.CreateNew(
                             new DateTime(2022, 02, 01),
                            1000,
                            paymentOrder,
                            "123"
                         )
                );
            Assert.IsTrue(exception.Message.ToLower().Contains(
                "payment already exist"
            ));
        }
        [TestCase]
        public void Equals_CreateEqualsPayments_IsOk()
        {
            var payment1 = Payment.CreateNew(
                new DateTime(2022, 02, 01),
                1000,
                paymentOrder,
                "123"
            );
            Payment.Remove(payment1);
            var payment2 = Payment.CreateNew(
                new DateTime(2022, 02, 01),
                1000,
                paymentOrder,
                "123"
            );

            Assert.AreEqual(payment1, payment2);
        }
        [TestCase]
        public void Equals_CreateDifferentDateEqualsPayments_NotEqual()
        {
            var payment1 = Payment.CreateNew(
                new DateTime(2022, 02, 01),
                1000,
                paymentOrder,
                "123"
            );
            var payment2 = Payment.CreateNew(
                new DateTime(2022, 03, 01),
                1000,
                paymentOrder,
                "123"
            );

            Assert.AreNotEqual(payment1, payment2);
        }
        [TestCase]
        public void Equals_CreateDifferentSumEqualsPayments_NotEqual()
        {
            var payment1 = Payment.CreateNew(
                new DateTime(2022, 02, 01),
                1000,
                paymentOrder,
                "123"
            );
            var payment2 = Payment.CreateNew(
                new DateTime(2022, 02, 01),
                10000,
                paymentOrder,
                "123"
            );

            Assert.AreNotEqual(payment1, payment2);
        }
        [TestCase]
        public void Equals_CreateDifferentDocIdEqualsPayments_NotEqual()
        {
            var payment1 = Payment.CreateNew(
                new DateTime(2022, 02, 01),
                1000,
                paymentOrder,
                "123"
            );
            var payment2 = Payment.CreateNew(
                new DateTime(2022, 02, 01),
                1000,
                paymentOrder,
                "111"
            );

            Assert.AreNotEqual(payment1, payment2);
        }

        [TestCase]
        public void ToString_CreatePayment_ReturnCorrectRepresentation()
        {
            var payment = Payment.CreateNew(
                new DateTime(2022, 02, 01),
                1000,
                paymentOrder,
                "123"
            );

            Assert.AreEqual("01.02.2022: 10.00", payment.ToString());
        }
    }
}
