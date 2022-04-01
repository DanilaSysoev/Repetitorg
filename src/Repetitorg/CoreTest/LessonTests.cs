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
        public void Create_CreateWithCorrectArgs_LessonCountIncrease()
        {
            Order order = Order.CreateNew("test order");
            Assert.AreEqual(0, Lesson.Count);
            Lesson lesson = Lesson.Create(new DateTime(2021, 10, 10, 12, 0, 0), 90, order);
            Assert.AreEqual(1, Lesson.Count);
        }
    }
}
