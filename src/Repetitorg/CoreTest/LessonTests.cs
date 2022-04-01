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
        [TestCase]
        public void GetIntersectionWithAll_ExistTwoNonIntersectionLesson_ReturnEmptyList()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 90, order);
            Lesson l2 = Lesson.CreateNew(new DateTime(2021, 10, 10, 14, 0, 0), 90, order);

            IList<Lesson> inter = Lesson.GetIntersectionWithAll(l1);
            Assert.AreEqual(0, inter.Count);
            inter = Lesson.GetIntersectionWithAll(l2);
            Assert.AreEqual(0, inter.Count);
        }
        [TestCase]
        public void GetIntersectionWithAll_FirstIntersectSecont_ReturnCorrectList()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 90, order);
            Lesson l2 = Lesson.CreateNew(new DateTime(2021, 10, 10, 13, 0, 0), 90, order);

            IList<Lesson> inter = Lesson.GetIntersectionWithAll(l1);
            Assert.AreEqual(1, inter.Count);
            Assert.AreEqual(l2, inter[0]);
            inter = Lesson.GetIntersectionWithAll(l2);
            Assert.AreEqual(1, inter.Count);
            Assert.AreEqual(l1, inter[0]);
        }
        [TestCase]
        public void GetIntersectionWithAll_FirstContainsSecont_ReturnCorrectList()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 180, order);
            Lesson l2 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 30, 0), 60, order);

            IList<Lesson> inter = Lesson.GetIntersectionWithAll(l1);
            Assert.AreEqual(1, inter.Count);
            Assert.AreEqual(l2, inter[0]);
            inter = Lesson.GetIntersectionWithAll(l2);
            Assert.AreEqual(1, inter.Count);
            Assert.AreEqual(l1, inter[0]);
        }
        [TestCase]
        public void GetIntersectionWithAll_IntersectMoreThanOne_ReturnCorrectList()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 90, order);
            Lesson l2 = Lesson.CreateNew(new DateTime(2021, 10, 10, 14, 0, 0), 90, order);
            Lesson l3 = Lesson.CreateNew(new DateTime(2021, 10, 10, 16, 0, 0), 90, order);
            Lesson li = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 30, 0), 120, order);

            IList<Lesson> inter = Lesson.GetIntersectionWithAll(li);
            Assert.AreEqual(2, inter.Count);
            Assert.IsTrue(inter.Contains(l1));
            Assert.IsTrue(inter.Contains(l2));
            inter = Lesson.GetIntersectionWithAll(l1);
            Assert.AreEqual(1, inter.Count);
            Assert.AreEqual(li, inter[0]);
            inter = Lesson.GetIntersectionWithAll(l2);
            Assert.AreEqual(1, inter.Count);
            Assert.AreEqual(li, inter[0]);
        }
        [TestCase]
        public void GetIntersectionWithAll_EndToEndLessons_NoIntersection()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 90, order);
            Lesson l2 = Lesson.CreateNew(new DateTime(2021, 10, 10, 13, 30, 0), 90, order);

            IList<Lesson> inter = Lesson.GetIntersectionWithAll(l1);
            Assert.AreEqual(0, inter.Count);
            inter = Lesson.GetIntersectionWithAll(l2);
            Assert.AreEqual(0, inter.Count);
        }

        [TestCase]
        public void GetIntersectionWithScheduled_ExistOnlyOneLesson_ReturnEmptyList()
        {
            Order order = Order.CreateNew("test order");
            Lesson lesson = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 90, order);            

            IList<Lesson> intersection = Lesson.GetIntersectionWithScheduled(lesson);
            Assert.AreEqual(0, intersection.Count);
        }
        [TestCase]
        public void GetIntersectionWithScheduled_ExistTwoNonIntersectionLesson_ReturnEmptyList()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 90, order);
            Lesson l2 = Lesson.CreateNew(new DateTime(2021, 10, 10, 14, 0, 0), 90, order);

            Lesson.AddToSchedule(l1);

            IList<Lesson> inter = Lesson.GetIntersectionWithScheduled(l2);
            Assert.AreEqual(0, inter.Count);
        }
        [TestCase]
        public void GetIntersectionWithScheduled_ScheduledIntersectNonScheduled_ReturnCorrectList()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 90, order);
            Lesson l2 = Lesson.CreateNew(new DateTime(2021, 10, 10, 13, 0, 0), 90, order);

            Lesson.AddToSchedule(l1);

            IList<Lesson> inter = Lesson.GetIntersectionWithScheduled(l1);
            Assert.AreEqual(0, inter.Count);
            inter = Lesson.GetIntersectionWithScheduled(l2);
            Assert.AreEqual(1, inter.Count);
            Assert.AreEqual(l1, inter[0]);
        }
        [TestCase]
        public void GetIntersectionWithScheduled_NonScheduledIntersectScheduled_ReturnCorrectList()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 90, order);
            Lesson l2 = Lesson.CreateNew(new DateTime(2021, 10, 10, 13, 0, 0), 90, order);

            Lesson.AddToSchedule(l2);

            IList<Lesson> inter = Lesson.GetIntersectionWithScheduled(l2);
            Assert.AreEqual(0, inter.Count);
            inter = Lesson.GetIntersectionWithScheduled(l1);
            Assert.AreEqual(1, inter.Count);
            Assert.AreEqual(l2, inter[0]);
        }
        [TestCase]
        public void GetIntersectionWithScheduled_FirstContainsSecont_ReturnCorrectList()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 180, order);
            Lesson l2 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 30, 0), 60, order);

            Lesson.AddToSchedule(l1);

            IList<Lesson> inter = Lesson.GetIntersectionWithScheduled(l1);
            Assert.AreEqual(0, inter.Count);
            inter = Lesson.GetIntersectionWithScheduled(l2);
            Assert.AreEqual(1, inter.Count);
            Assert.AreEqual(l1, inter[0]);
        }
        [TestCase]
        public void GetIntersectionWithScheduled_SecondContainsFirst_ReturnCorrectList()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 180, order);
            Lesson l2 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 30, 0), 60, order);

            Lesson.AddToSchedule(l2);

            IList<Lesson> inter = Lesson.GetIntersectionWithScheduled(l2);
            Assert.AreEqual(0, inter.Count);
            inter = Lesson.GetIntersectionWithScheduled(l1);
            Assert.AreEqual(1, inter.Count);
            Assert.AreEqual(l2, inter[0]);
        }
        [TestCase]
        public void GetIntersectionWithScheduled_IntersectMoreThanOne_ReturnCorrectList()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 90, order);
            Lesson l2 = Lesson.CreateNew(new DateTime(2021, 10, 10, 14, 0, 0), 90, order);
            Lesson l3 = Lesson.CreateNew(new DateTime(2021, 10, 10, 16, 0, 0), 90, order);
            Lesson li = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 30, 0), 120, order);

            Lesson.AddToSchedule(l1);
            Lesson.AddToSchedule(l2);

            IList<Lesson> inter = Lesson.GetIntersectionWithScheduled(li);
            Assert.AreEqual(2, inter.Count);
            Assert.IsTrue(inter.Contains(l1));
            Assert.IsTrue(inter.Contains(l2));
            inter = Lesson.GetIntersectionWithScheduled(l1);
            Assert.AreEqual(0, inter.Count);
            inter = Lesson.GetIntersectionWithScheduled(l2);
            Assert.AreEqual(0, inter.Count);
        }
        [TestCase]
        public void GetIntersectionWithScheduled_MoreThanOneIntersect_ReturnCorrectList()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 90, order);
            Lesson l2 = Lesson.CreateNew(new DateTime(2021, 10, 10, 14, 0, 0), 90, order);
            Lesson l3 = Lesson.CreateNew(new DateTime(2021, 10, 10, 16, 0, 0), 90, order);
            Lesson li = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 30, 0), 120, order);

            Lesson.AddToSchedule(li);

            IList<Lesson> inter = Lesson.GetIntersectionWithScheduled(li);
            Assert.AreEqual(0, inter.Count);
            inter = Lesson.GetIntersectionWithScheduled(l1);
            Assert.AreEqual(1, inter.Count);
            Assert.IsTrue(inter.Contains(li));
            inter = Lesson.GetIntersectionWithScheduled(l2);
            Assert.AreEqual(1, inter.Count);
            Assert.IsTrue(inter.Contains(li));
        }
        [TestCase]
        public void GetIntersectionWithScheduled_EndToEndLessons_NoIntersection()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 90, order);
            Lesson l2 = Lesson.CreateNew(new DateTime(2021, 10, 10, 13, 30, 0), 90, order);

            Lesson.AddToSchedule(l1);

            IList<Lesson> inter = Lesson.GetIntersectionWithScheduled(l1);
            Assert.AreEqual(0, inter.Count);
            inter = Lesson.GetIntersectionWithScheduled(l2);
            Assert.AreEqual(0, inter.Count);
        }

        [TestCase]
        public void AddToSchedule_AddOneLesson_StatusChangeToActive()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 90, order);

            Lesson.AddToSchedule(l1);
            Assert.AreEqual(LessonStatus.Active, l1.Status);
        }
        [TestCase]
        public void AddToSchedule_AddNull_ThrowsError()
        {
            var exc = Assert.Throws<ArgumentException>(
                () => Lesson.AddToSchedule(null)
            );
            Assert.IsTrue(
                exc.Message.ToLower().Contains("lesson can't be null")
            );
        }
        [TestCase]
        public void AddToSchedule_AddOneLesson_LessonUpdating()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 90, order);

            Assert.AreEqual(0, lessons.UpdatesCount);
            Lesson.AddToSchedule(l1);
            Assert.AreEqual(1, lessons.UpdatesCount);
        }
        [TestCase]
        public void AddToSchedule_AddNonIntersectionsLesson_AdditionOk()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 90, order);
            Lesson l2 = Lesson.CreateNew(new DateTime(2021, 10, 10, 14, 0, 0), 90, order);

            Lesson.AddToSchedule(l1);
            Lesson.AddToSchedule(l2);

            Assert.AreEqual(LessonStatus.Active, l1.Status);
            Assert.AreEqual(LessonStatus.Active, l2.Status);
        }
        [TestCase]
        public void AddToSchedule_FirstIntersectSecondLesson_ThrowsError()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 180, order);
            Lesson l2 = Lesson.CreateNew(new DateTime(2021, 10, 10, 14, 0, 0), 90, order);

            Lesson.AddToSchedule(l1);
            var exc = Assert.Throws<ArgumentException>(
                () => Lesson.AddToSchedule(l2)
            );

            Assert.IsTrue(
                exc.Message.ToLower().Contains(
                    string.Format(
                        "{0} intersect other lessons in schedule", 
                        l2.ToString().ToLower()
                    )
                )
            );
            Assert.AreEqual(LessonStatus.Active, l1.Status);
            Assert.AreEqual(LessonStatus.NonActive, l2.Status);
        }

    }
}
