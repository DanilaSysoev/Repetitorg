using NUnit.Framework;
using Repetitorg.Core;
using System;
using System.Collections.Generic;
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

    }
}
