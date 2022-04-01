using NUnit.Framework;
using Repetitorg.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repetitorg.CoreTest
{
    [TestFixture]
    class LessonTests
    {
        private DummyOrderStorage orders;
        private DummyLessonStorage lessons;
        [SetUp]
        public void Setup()
        {
            orders = new DummyOrderStorage();
            lessons = new DummyLessonStorage();
            Order.InitializeStorage(orders);
            Lesson.InitializeStorage(lessons);
        }

        [TestCase]
        public void CreateNew_CreateWithCorrectArgs_LessonCountIncrease()
        {
            Order order = Order.CreateNew("test order");
            
            Assert.AreEqual(0, Lesson.Count);
            Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 90, order);
            Assert.AreEqual(1, Lesson.Count);
            Lesson.CreateNew(new DateTime(2021, 10, 10, 16, 0, 0), 90, order);
            Assert.AreEqual(2, Lesson.Count);
        }
        [TestCase]
        public void CreateNew_CreateWithCorrectArgs_LessonIsNonActive()
        {
            Order order = Order.CreateNew("test order");

            Lesson lesson = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 90, order);
            Assert.AreEqual(LessonStatus.NonActive, lesson.Status);
        }
        [TestCase]
        public void CreateNew_CreateWithNegativeLength_ThrowsError()
        {
            Order order = Order.CreateNew("test order");

            var exc = Assert.Throws<ArgumentException>(
                () => Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), -90, order)
            );

            Assert.IsTrue(
                exc.Message.ToLower().Contains(
                    "lesson length can't be non positive"
                )
            );
        }
        [TestCase]
        public void CreateNew_CreateWithZeroLength_ThrowsError()
        {
            Order order = Order.CreateNew("test order");

            var exc = Assert.Throws<ArgumentException>(
                () => Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 0, order)
            );

            Assert.IsTrue(
                exc.Message.ToLower().Contains(
                    "lesson length can't be non positive"
                )
            );
        }
        [TestCase]
        public void CreateNew_CreateWithNullOrder_ThrowsError()
        {
            var exc = Assert.Throws<ArgumentException>(
                () => Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 0, null)
            );

            Assert.IsTrue(
                exc.Message.ToLower().Contains(
                    "order can't be null"
                )
            );
        }
        [TestCase]
        public void CreateNew_CreateWithCorrectArgs_PropertiesSettedCorrectly()
        {
            Order order = Order.CreateNew("test order");
            DateTime dateTime = new DateTime(2021, 10, 10, 12, 0, 0);
            Lesson lesson = Lesson.CreateNew(dateTime, 90, order);
            Assert.AreEqual(dateTime, lesson.DateTime);
            Assert.AreEqual(90, lesson.LengthInMinutes);
            Assert.AreEqual(order, lesson.Order);
        }


        [TestCase]
        public void GetAll_NewCollection_ReturnEmpty()
        {
            Assert.AreEqual(0, Lesson.GetAll().Count);
        }
        [TestCase]
        public void GetAll_AddThreeLessons_ReturnCorrectList()
        {
            Order order = Order.CreateNew("test order");

            Lesson l1 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 90, order);
            Lesson l2 = Lesson.CreateNew(new DateTime(2021, 10, 10, 14, 0, 0), 90, order);
            Lesson l3 = Lesson.CreateNew(new DateTime(2021, 10, 10, 16, 0, 0), 90, order);

            var all = Lesson.GetAll();

            Assert.IsTrue(all.Contains(l1));
            Assert.IsTrue(all.Contains(l2));
            Assert.IsTrue(all.Contains(l3));
            Assert.AreEqual(3, all.Count);
        }

        [TestCase]
        public void GetIntersectionWithAll_ExistOnlyOneLesson_ReturnEmptyList()
        {
            Order order = Order.CreateNew("test order");
            Lesson lesson = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 90, order);

            IList<Lesson> intersection = Lesson.GetIntersectionWithAll(lesson);
            Assert.AreEqual(0, intersection.Count);
        }
    }
}
