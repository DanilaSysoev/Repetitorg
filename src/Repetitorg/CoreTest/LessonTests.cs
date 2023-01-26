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
        private DummyPersonStorage<Client> clients;
        private DummyPersonStorage<Student> students;

        FullName testStudent1;
        FullName testStudent2;
        FullName testStudent3;
        FullName testClient1;
        FullName testClient2;
        FullName testClient3;

        [SetUp]
        public void Setup()
        {
            orders = new DummyOrderStorage();
            lessons = new DummyLessonStorage();
            clients = new DummyPersonStorage<Client>();
            students = new DummyPersonStorage<Student>();
            Order.SetupStorage(orders);
            Lesson.SetupStorage(lessons);
            Client.SetupStorage(clients);
            Student.SetupStorage(students);

            testStudent1 = new FullName
            {
                FirstName = "Student",
                LastName = "Test",
                Patronymic = ""
            };
            testStudent2 = new FullName
            {
                FirstName = "Student",
                LastName = "Test",
                Patronymic = "1"
            };
            testStudent3 = new FullName
            {
                FirstName = "Student",
                LastName = "Test",
                Patronymic = "2"
            };
            testClient1 = new FullName
            {
                FirstName = "Student",
                LastName = "Test",
                Patronymic = "3"
            };
            testClient2 = new FullName
            {
                FirstName = "Student",
                LastName = "Test",
                Patronymic = "4"
            };
            testClient3 = new FullName
            {
                FirstName = "",
                LastName = "c1",
                Patronymic = ""
            };
        }
        
        #region CreateNew Tests

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
        public void CreateNew_CreateWithCorrectArgs_LessonAddToStorage()
        {
            Order order = Order.CreateNew("test order");

            int pastAC = lessons.AddCount;
            Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 90, order);
            Assert.AreEqual(pastAC + 1, lessons.AddCount);
        }
        [TestCase]
        public void CreateNew_CreateWithCorrectArgs_LessonIsNonActive()
        {
            Order order = Order.CreateNew("test order");

            Lesson lesson = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );
            Assert.AreEqual(LessonStatus.NonActive, lesson.Status);
        }
        [TestCase]
        public void CreateNew_CreateWithNegativeLength_ThrowsError()
        {
            Order order = Order.CreateNew("test order");

            var exc = Assert.Throws<ArgumentException>(
                () => Lesson.CreateNew(
                    new DateTime(2021, 10, 10, 12, 0, 0), -90, order
                )
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
                () => Lesson.CreateNew(
                    new DateTime(2021, 10, 10, 12, 0, 0), 0, order
                )
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
                () => Lesson.CreateNew(
                    new DateTime(2021, 10, 10, 12, 0, 0), 0, null
                )
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
        public void CreateNew_createLesson_MovedFromPropertyIsNull()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );
            Assert.IsNull(l1.MovedFrom);
        }
        [TestCase]
        public void CreateNew_createLesson_MovedOnPropertyIsNull()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );
            Assert.IsNull(l1.MovedOn);
        }

        #endregion

        #region GetAll Tests

        [TestCase]
        public void GetAll_NewCollection_ReturnEmpty()
        {
            Assert.AreEqual(0, Lesson.GetAll().Count);
        }
        [TestCase]
        public void GetAll_AddThreeLessons_ReturnCorrectList()
        {
            Order order = Order.CreateNew("test order");

            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );
            Lesson l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 14, 0, 0), 90, order
            );
            Lesson l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 16, 0, 0), 90, order
            );

            var all = Lesson.GetAll();

            Assert.IsTrue(all.Contains(l1));
            Assert.IsTrue(all.Contains(l2));
            Assert.IsTrue(all.Contains(l3));
            Assert.AreEqual(3, all.Count);
        }

        #endregion

        #region GetIntersectionWithAll Tests

        [TestCase]
        public void GetIntersectionWithAll_ExistOnlyOneLesson_ReturnEmptyList()
        {
            Order order = Order.CreateNew("test order");
            Lesson lesson = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );

            IList<Lesson> intersection = Lesson.GetIntersectionWithAll(lesson);
            Assert.AreEqual(0, intersection.Count);
        }
        [TestCase]
        public void GetIntersectionWithAll_ExistTwoNonIntersectionLesson_ReturnEmptyList()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );
            Lesson l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 14, 0, 0), 90, order
            );

            IList<Lesson> inter = Lesson.GetIntersectionWithAll(l1);
            Assert.AreEqual(0, inter.Count);
            inter = Lesson.GetIntersectionWithAll(l2);
            Assert.AreEqual(0, inter.Count);
        }
        [TestCase]
        public void GetIntersectionWithAll_FirstIntersectSecont_ReturnCorrectList()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );
            Lesson l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 13, 0, 0), 90, order
            );

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
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 180, order
            );
            Lesson l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 30, 0), 60, order
            );

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
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );
            Lesson l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 14, 0, 0), 90, order
            );
            Lesson l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 16, 0, 0), 90, order
            );
            Lesson li = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 30, 0), 120, order
            );

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
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );
            Lesson l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 13, 30, 0), 90, order
            );

            IList<Lesson> inter = Lesson.GetIntersectionWithAll(l1);
            Assert.AreEqual(0, inter.Count);
            inter = Lesson.GetIntersectionWithAll(l2);
            Assert.AreEqual(0, inter.Count);
        }

        #endregion

        #region GetIntersectionWithScheduled Tests

        [TestCase]
        public void GetIntersectionWithScheduled_ExistOnlyOneLesson_ReturnEmptyList()
        {
            Order order = Order.CreateNew("test order");
            Lesson lesson = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order);


            IList<Lesson> intersection = Lesson.GetIntersectionWithScheduled(lesson);
            Assert.AreEqual(0, intersection.Count);
        }
        [TestCase]
        public void GetIntersectionWithScheduled_ExistTwoNonIntersectionLesson_ReturnEmptyList()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );
            Lesson l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 14, 0, 0), 90, order
            );

            l1.AddToSchedule();

            IList<Lesson> inter = Lesson.GetIntersectionWithScheduled(l2);
            Assert.AreEqual(0, inter.Count);
        }
        [TestCase]
        public void GetIntersectionWithScheduled_ScheduledIntersectNonScheduled_ReturnCorrectList()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );
            Lesson l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 13, 0, 0), 90, order
            );

            l1.AddToSchedule();

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
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );
            Lesson l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 13, 0, 0), 90, order
            );

            l2.AddToSchedule();

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
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 180, order
            );
            Lesson l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 30, 0), 60, order
            );

            l1.AddToSchedule();

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
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 180, order
            );
            Lesson l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 30, 0), 60, order
            );

            l2.AddToSchedule();

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
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );
            Lesson l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 14, 0, 0), 90, order
            );
            Lesson l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 16, 0, 0), 90, order
            );
            Lesson li = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 30, 0), 120, order
            );

            l1.AddToSchedule();
            l2.AddToSchedule();

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
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );
            Lesson l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 14, 0, 0), 90, order
            );
            Lesson l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 16, 0, 0), 90, order
            );
            Lesson li = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 30, 0), 120, order
            );

            li.AddToSchedule();

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
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );
            Lesson l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 13, 30, 0), 90, order
            );

            l1.AddToSchedule();

            IList<Lesson> inter = Lesson.GetIntersectionWithScheduled(l1);
            Assert.AreEqual(0, inter.Count);
            inter = Lesson.GetIntersectionWithScheduled(l2);
            Assert.AreEqual(0, inter.Count);
        }

        #endregion

        #region AddToSchedule Tests

        [TestCase]
        public void AddToSchedule_AddOneLesson_StatusChangeToActive()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );

            l1.AddToSchedule();
            Assert.AreEqual(LessonStatus.Active, l1.Status);
        }
        [TestCase]
        public void AddToSchedule_AddOneLesson_LessonUpdating()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );

            Assert.AreEqual(0, lessons.UpdatesCount);
            l1.AddToSchedule();
            Assert.AreEqual(1, lessons.UpdatesCount);
        }
        [TestCase]
        public void AddToSchedule_AddNonIntersectionsLesson_AdditionOk()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );
            Lesson l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 14, 0, 0), 90, order
            );

            l1.AddToSchedule();
            l2.AddToSchedule();

            Assert.AreEqual(LessonStatus.Active, l1.Status);
            Assert.AreEqual(LessonStatus.Active, l2.Status);
        }
        [TestCase]
        public void AddToSchedule_FirstIntersectSecondLesson_ThrowsError()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 180, order
            );
            Lesson l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 14, 0, 0), 90, order
            );

            l1.AddToSchedule();
            var exc = Assert.Throws<InvalidOperationException>(
                () => l2.AddToSchedule()
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
        [TestCase]
        public void AddToSchedule_SecondIntersectFirstLesson_ThrowsError()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 180, order
            );
            Lesson l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 14, 0, 0), 90, order
            );

            l2.AddToSchedule();
            var exc = Assert.Throws<InvalidOperationException>(
                () => l1.AddToSchedule()
            );

            Assert.IsTrue(
                exc.Message.ToLower().Contains(
                    string.Format(
                        "{0} intersect other lessons in schedule",
                        l1.ToString().ToLower()
                    )
                )
            );
            Assert.AreEqual(LessonStatus.Active, l2.Status);
            Assert.AreEqual(LessonStatus.NonActive, l1.Status);
        }
        [TestCase]
        public void AddToSchedule_FirstContainsSecond_ThrowsError()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 180, order
            );
            Lesson l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 30, 0), 90, order
            );

            l1.AddToSchedule();
            var exc = Assert.Throws<InvalidOperationException>(
                () => l2.AddToSchedule()
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
        [TestCase]
        public void AddToSchedule_SecondContainsFirst_ThrowsError()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 180, order
            );
            Lesson l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 30, 0), 90, order
            );

            l2.AddToSchedule();
            var exc = Assert.Throws<InvalidOperationException>(
                () => l1.AddToSchedule()
            );

            Assert.IsTrue(
                exc.Message.ToLower().Contains(
                    string.Format(
                        "{0} intersect other lessons in schedule",
                        l1.ToString().ToLower()
                    )
                )
            );
            Assert.AreEqual(LessonStatus.Active, l2.Status);
            Assert.AreEqual(LessonStatus.NonActive, l1.Status);
        }
        [TestCase]
        public void AddToSchedule_ActivetedIntersectMany_ThrowsError()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );
            Lesson l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 14, 0, 0), 90, order
            );
            Lesson li = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 13, 0, 0), 120, order
            );

            l1.AddToSchedule();
            l2.AddToSchedule();
            var exc = Assert.Throws<InvalidOperationException>(
                () => li.AddToSchedule()
            );

            Assert.IsTrue(
                exc.Message.ToLower().Contains(
                    string.Format(
                        "{0} intersect other lessons in schedule",
                        li.ToString().ToLower()
                    )
                )
            );
            Assert.AreEqual(LessonStatus.Active, l1.Status);
            Assert.AreEqual(LessonStatus.Active, l2.Status);
            Assert.AreEqual(LessonStatus.NonActive, li.Status);
        }
        [TestCase]
        public void AddToSchedule_AddActiveLesson_ThrowsError()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );

            l1.AddToSchedule();
            var exc = Assert.Throws<InvalidOperationException>(
                () => l1.AddToSchedule()
            );

            Assert.IsTrue(
                exc.Message.ToLower().Contains(
                    "lesson already added to schedule"
                )
            );
        }
        [TestCase]
        public void AddToSchedule_AddCanceledLesson_ThrowsError()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );

            l1.AddToSchedule();
            l1.Cancel();
            var exc = Assert.Throws<InvalidOperationException>(
                () => l1.AddToSchedule()
            );

            Assert.IsTrue(
                exc.Message.ToLower().Contains(
                    "can't add to schedule canceled lesson"
                )
            );
        }
        [TestCase]
        public void AddToSchedule_AddCompletedLesson_ThrowsError()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );

            l1.AddToSchedule();
            l1.Complete();
            var exc = Assert.Throws<InvalidOperationException>(
                () => l1.AddToSchedule()
            );

            Assert.IsTrue(
                exc.Message.ToLower().Contains(
                    "can't add to schedule completed lesson"
                )
            );
        }
        [TestCase]
        public void AddToSchedule_AddMovedLesson_ThrowsError()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );

            l1.AddToSchedule();
            var l2 = l1.MoveTo(new DateTime(2021, 10, 12, 12, 0, 0));
            var exc = Assert.Throws<InvalidOperationException>(
                () => l1.AddToSchedule()
            );

            Assert.IsTrue(
                exc.Message.ToLower().Contains(
                    "can't add to schedule moved lesson"
                )
            );
        }
        [TestCase]
        public void AddToSchedule_addNonActiveLesson_MovedOnPropertyIsNull()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );
            l1.AddToSchedule();
            Assert.IsNull(l1.MovedOn);
        }
        [TestCase]
        public void AddToSchedule_addNonActiveLesson_MovedFromPropertyIsNull()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );
            l1.AddToSchedule();
            Assert.IsNull(l1.MovedFrom);
        }

        #endregion

        #region Complete Tests

        [TestCase]
        public void Complete_completingActiveLesson_statusChangeToCompleted()
        {
            Order o1 = Order.CreateNew("test order 1");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2022, 1, 15, 12, 0, 0), 90, o1
            );
            l1.AddToSchedule();
            l1.Complete();
            Assert.AreEqual(LessonStatus.Completed, l1.Status);
        }
        [TestCase]
        public void Complete_completingActiveLesson_balancesIncludedOfClientsDecrease()
        {
            Order o1 = Order.CreateNew("test order 1");
            Order o2 = Order.CreateNew("test order 2");
            Client c1 = Client.CreateNew(testClient1);
            Client c2 = Client.CreateNew(testClient2);
            Client c3 = Client.CreateNew(testClient3);
            Student s1 = Student.CreateNew(testStudent1, c1);
            Student s2 = Student.CreateNew(testStudent2, c2);
            Student s3 = Student.CreateNew(testStudent3, c3);
            o1.AddStudent(s1, 300000);
            o1.AddStudent(s3, 400000);
            o2.AddStudent(s2, 350000);

            Lesson l1 = Lesson.CreateNew(
                new DateTime(2022, 1, 15, 12, 0, 0), 90, o1
            );
            Lesson l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 14, 0, 0), 90, o2
            );
            Lesson l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 16, 0, 0), 120, o1
            );
            l1.AddToSchedule();
            l2.AddToSchedule();
            l3.AddToSchedule();

            l1.Complete();
            Assert.AreEqual(-450000, c1.BalanceInKopeks);
            Assert.AreEqual(-600000, c3.BalanceInKopeks);
        }
        [TestCase]
        public void Complete_completingActiveLesson_balancesExludedOfClientsNotChanged()
        {
            Order o1 = Order.CreateNew("test order 1");
            Order o2 = Order.CreateNew("test order 2");
            Client c1 = Client.CreateNew(testClient1);
            Client c2 = Client.CreateNew(testClient2);
            Client c3 = Client.CreateNew(testClient3);
            Student s1 = Student.CreateNew(testStudent1, c1);
            Student s2 = Student.CreateNew(testStudent2, c2);
            Student s3 = Student.CreateNew(testStudent3, c3);
            o1.AddStudent(s1, 300000);
            o1.AddStudent(s3, 400000);
            o2.AddStudent(s2, 350000);

            Lesson l1 = Lesson.CreateNew(
                new DateTime(2022, 1, 15, 12, 0, 0), 90, o1
            );
            Lesson l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 14, 0, 0), 90, o2
            );
            Lesson l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 16, 0, 0), 120, o1
            );
            l1.AddToSchedule();
            l2.AddToSchedule();
            l3.AddToSchedule();

            l1.Complete();
            Assert.AreEqual(0, c2.BalanceInKopeks);
        }
        [TestCase]
        public void Complete_lessonCostNotInteger_balanceDecreaseOnRoundedDownValue()
        {
            Order o1 = Order.CreateNew("test order 1");
            Order o2 = Order.CreateNew("test order 2");
            Client c1 = Client.CreateNew(testClient1);
            Client c2 = Client.CreateNew(testClient2);
            Client c3 = Client.CreateNew(testClient3);
            Student s1 = Student.CreateNew(testStudent1, c1);
            Student s2 = Student.CreateNew(testStudent2, c2);
            Student s3 = Student.CreateNew(testStudent3, c3);
            o1.AddStudent(s1, 100);
            o1.AddStudent(s3, 200);
            o2.AddStudent(s2, 350000);

            Lesson l1 = Lesson.CreateNew(
                new DateTime(2022, 1, 15, 12, 0, 0), 50, o1
            );
            Lesson l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 14, 0, 0), 90, o2
            );
            Lesson l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 16, 0, 0), 100, o1
            );
            l1.AddToSchedule();
            l2.AddToSchedule();
            l3.AddToSchedule();

            l1.Complete();
            Assert.AreEqual(-83, c1.BalanceInKopeks);
            Assert.AreEqual(-166, c3.BalanceInKopeks);
        }
        [TestCase]
        public void Complete_completingNonActiveLesson_throwsException()
        {
            Order o1 = Order.CreateNew("test order 1");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2022, 1, 15, 12, 0, 0), 90, o1
            );

            var exception = Assert.Throws<InvalidOperationException>(
                () => l1.Complete()
            );
            Assert.IsTrue(
                exception.Message.ToLower().Contains(
                    "can't complete non active lesson"
                )
            );
        }
        [TestCase]
        public void Complete_completingCompletedLesson_throwsException()
        {
            Order o1 = Order.CreateNew("test order 1");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2022, 1, 15, 12, 0, 0), 90, o1
            );
            l1.AddToSchedule();

            l1.Complete();
            var exception = Assert.Throws<InvalidOperationException>(
                () => l1.Complete()
            );
            Assert.IsTrue(
                exception.Message.ToLower().Contains(
                    "can't complete completed lesson"
                )
            );
        }
        [TestCase]
        public void Complete_completingCanceledLesson_throwsException()
        {
            Order o1 = Order.CreateNew("test order 1");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2022, 1, 15, 12, 0, 0), 90, o1
            );
            l1.AddToSchedule();

            l1.Cancel();
            var exception = Assert.Throws<InvalidOperationException>(
                () => l1.Complete()
            );
            Assert.IsTrue(
                exception.Message.ToLower().Contains(
                    "can't complete cancelled lesson"
                )
            );
        }
        [TestCase]
        public void Complete_completingActiveLesson_storageUpdeted()
        {
            Order o1 = Order.CreateNew("test order 1");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2022, 1, 15, 12, 0, 0), 90, o1
            );
            l1.AddToSchedule();

            var oldUpdCnt = lessons.UpdatesCount;
            l1.Complete();

            Assert.AreEqual(oldUpdCnt + 1, lessons.UpdatesCount);
        }
        [TestCase]
        public void Complete_completingMoveddLesson_throwsException()
        {
            Order o1 = Order.CreateNew("test order 1");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2022, 1, 15, 12, 0, 0), 90, o1
            );
            l1.AddToSchedule();

            l1.MoveTo(new DateTime(2022, 1, 17, 12, 0, 0));
            var exception = Assert.Throws<InvalidOperationException>(
                () => l1.Complete()
            );
            Assert.IsTrue(
                exception.Message.ToLower().Contains(
                    "can't complete moved lesson"
                )
            );
        }
        [TestCase]
        public void Complete_completeActiveLesson_MovedOnPropertyIsNull()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );
            l1.AddToSchedule();
            l1.Complete();
            Assert.IsNull(l1.MovedOn);
        }
        [TestCase]
        public void Complete_completeActiveLesson_MovedFromPropertyIsNull()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );
            l1.AddToSchedule();
            l1.Complete();
            Assert.IsNull(l1.MovedFrom);
        }

        #endregion

        #region GetScheduledOnDate Tests

        [TestCase]
        public void GetScheduledOnDate_emptyCollection_returnEmptyList()
        {
            var lessons = Lesson.GetScheduledOnDate(new DateTime(2022, 1, 15));
            Assert.AreEqual(0, lessons.Count);
        }
        [TestCase]
        public void GetScheduledOnDate_notExistScheduledLesson_returnEmptyList()
        {
            Order o1 = Order.CreateNew("test order 1");
            Order o2 = Order.CreateNew("test order 2");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, o1
            );
            Lesson l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 14, 0, 0), 90, o2
            );
            Lesson l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 16, 0, 0), 120, o1
            );

            var lessons = Lesson.GetScheduledOnDate(new DateTime(2022, 1, 15));
            Assert.AreEqual(0, lessons.Count);
        }
        [TestCase]
        public void GetScheduledOnDate_notExistScheduledOnGivenDate_returnEmptyList()
        {
            Order o1 = Order.CreateNew("test order 1");
            Order o2 = Order.CreateNew("test order 2");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2022, 1, 15, 12, 0, 0), 90, o1
            );
            Lesson l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 14, 0, 0), 90, o2
            );
            Lesson l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 16, 0, 0), 120, o1
            );
            l2.AddToSchedule();
            l3.AddToSchedule();

            var lessons = Lesson.GetScheduledOnDate(new DateTime(2022, 1, 15));
            Assert.AreEqual(0, lessons.Count);
        }
        [TestCase]
        public void GetScheduledOnDate_existTwoActiveOnGivenDate_returnCorrectList()
        {
            Order o1 = Order.CreateNew("test order 1");
            Order o2 = Order.CreateNew("test order 2");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2022, 1, 15, 12, 0, 0), 90, o1
            );
            Lesson l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 14, 0, 0), 90, o2
            );
            Lesson l3 = Lesson.CreateNew(
                new DateTime(2022, 1, 15, 16, 0, 0), 120, o1
            );
            l1.AddToSchedule();
            l2.AddToSchedule();
            l3.AddToSchedule();

            var lessons = Lesson.GetScheduledOnDate(new DateTime(2022, 1, 15));
            Assert.AreEqual(2, lessons.Count);
            Assert.IsTrue(lessons.Contains(l1));
            Assert.IsTrue(lessons.Contains(l3));
        }
        [TestCase]
        public void GetScheduledOnDate_existTwoNonActiveLessonsOnDate_returnEmpty()
        {
            Order o = Order.CreateNew("o1");
            var l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 10, 0, 0), 90, o
            );
            var l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, o
            );
            var l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 10, 0, 0), 90, o
            );
            var lessons = Lesson.GetScheduledOnDate(new DateTime(2021, 10, 10));
            Assert.AreEqual(0, lessons.Count);
        }
        [TestCase]
        public void GetScheduledOnDate_existTwoActiveLessonsOnDate_returnBoth()
        {
            Order o = Order.CreateNew("o1");
            var l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 10, 0, 0), 90, o
            );
            var l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, o
            );
            var l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 10, 0, 0), 90, o
            );
            l1.AddToSchedule();
            l2.AddToSchedule();
            l3.AddToSchedule();
            var lessons = Lesson.GetScheduledOnDate(new DateTime(2021, 10, 10));
            Assert.AreEqual(2, lessons.Count);
            Assert.IsTrue(lessons.Contains(l1));
            Assert.IsTrue(lessons.Contains(l2));
        }
        [TestCase]
        public void GetScheduledOnDate_existTwoCancelledLessonsOnDate_returnEmpty()
        {
            Order o = Order.CreateNew("o1");
            var l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 10, 0, 0), 90, o
            );
            var l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, o
            );
            var l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 10, 0, 0), 90, o
            );
            l1.AddToSchedule();
            l2.AddToSchedule();
            l1.Cancel();
            l2.Cancel();
            l3.AddToSchedule();
            l3.Cancel();
            var lessons = Lesson.GetScheduledOnDate(new DateTime(2021, 10, 10));
            Assert.AreEqual(0, lessons.Count);
        }
        [TestCase]
        public void GetScheduledOnDate_existTwoCompletedLessonsOnDate_returnBoth()
        {
            Order o = Order.CreateNew("o1");
            var l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 10, 0, 0), 90, o
            );
            var l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, o
            );
            var l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 10, 0, 0), 90, o
            );
            l1.AddToSchedule();
            l2.AddToSchedule();
            l1.Complete();
            l2.Complete();
            l3.AddToSchedule();
            l3.Complete();
            var lessons = Lesson.GetScheduledOnDate(new DateTime(2021, 10, 10));
            Assert.AreEqual(2, lessons.Count);
            Assert.IsTrue(lessons.Contains(l1));
            Assert.IsTrue(lessons.Contains(l2));
        }
        [TestCase]
        public void GetScheduledOnDate_existTwoMovedLessonsOnDate_returnEmpty()
        {
            Order o = Order.CreateNew("o1");
            var l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 10, 0, 0), 90, o
            );
            var l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, o
            );
            var l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 10, 0, 0), 90, o
            );
            l1.AddToSchedule();
            l2.AddToSchedule();
            l3.AddToSchedule();
            l1.MoveTo(new DateTime(2021, 10, 13, 10, 0, 0));
            l2.MoveTo(new DateTime(2021, 10, 13, 12, 0, 0));
            l3.MoveTo(new DateTime(2021, 10, 14, 12, 0, 0));
            var lessons = Lesson.GetScheduledOnDate(new DateTime(2021, 10, 10));
            Assert.AreEqual(0, lessons.Count);
        }

        [TestCase]
        public void GetScheduledOnDate_existTwoMovedAndOneTargetLessonsOnDate_returnOnlyActiveTarget()
        {
            Order o = Order.CreateNew("o1");
            var l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 10, 0, 0), 90, o
            );
            var l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, o
            );
            var l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 10, 0, 0), 90, o
            );
            l1.AddToSchedule();
            l2.AddToSchedule();
            l3.AddToSchedule();
            var lt = l1.MoveTo(new DateTime(2021, 10, 10, 16, 0, 0));
            l2.MoveTo(new DateTime(2021, 10, 13, 12, 0, 0));
            l3.MoveTo(new DateTime(2021, 10, 14, 12, 0, 0));

            var lessons = Lesson.GetScheduledOnDate(new DateTime(2021, 10, 10));
            Assert.AreEqual(1, lessons.Count);
            Assert.IsTrue(lessons.Contains(lt));
        }

        #endregion

        #region RemoveFromSchedule Tests

        [TestCase]
        public void RemoveFromSchedule_removeNonScheduledLesson_throwsException()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );

            var exception = Assert.Throws<InvalidOperationException>(
                () => l1.RemoveFromSchedule()
            );
            Assert.IsTrue(
                exception.Message.ToLower().Contains(
                    "can't remove from schedule non-scheduled lesson"
                )
            );
        }
        [TestCase]
        public void RemoveFromSchedule_removeCanceledLesson_throwsException()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );
            l1.AddToSchedule();
            l1.Cancel();

            var exception = Assert.Throws<InvalidOperationException>(
                () => l1.RemoveFromSchedule()
            );
            Assert.IsTrue(
                exception.Message.ToLower().Contains(
                    "can't remove from schedule canceled lesson"
                )
            );
        }
        [TestCase]
        public void RemoveFromSchedule_removeCompletedLesson_throwsException()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );
            l1.AddToSchedule();
            l1.Complete();

            var exception = Assert.Throws<InvalidOperationException>(
                () => l1.RemoveFromSchedule()
            );

            Assert.IsTrue(
                exception.Message.ToLower().Contains(
                    "can't remove from schedule completed lesson"
                )
            );
        }
        [TestCase]
        public void RemoveFromSchedule_removeActiveLesson_lessonNowIsNonActive()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );
            l1.AddToSchedule();
            l1.RemoveFromSchedule();

            Assert.AreEqual(LessonStatus.NonActive, l1.Status);
        }
        [TestCase]
        public void RemoveFromSchedule_removeActiveLesson_lessonUpdated()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );
            l1.AddToSchedule();
            var oldUpdCnt = lessons.UpdatesCount;
            l1.RemoveFromSchedule();
            Assert.AreEqual(oldUpdCnt + 1, lessons.UpdatesCount);
        }
        [TestCase]
        public void RemoveFromSchedule_removeMovedLesson_throwsException()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );
            l1.AddToSchedule();
            l1.MoveTo(new DateTime(2021, 10, 15, 12, 0, 0));

            var exception = Assert.Throws<InvalidOperationException>(
                () => l1.RemoveFromSchedule()
            );

            Assert.IsTrue(
                exception.Message.ToLower().Contains(
                    "can't remove from schedule moved lesson"
                )
            );
        }

        #endregion

        #region Cancel Tests

        [TestCase]
        public void Cancel_cancelNonActiveLesson_changeStatusToCanceled()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order);

            l1.Cancel();
            Assert.AreEqual(LessonStatus.Canceled, l1.Status);
        }
        [TestCase]
        public void Cancel_cancelActiveLesson_changeStatusToCanceled()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );
            l1.AddToSchedule();
            l1.Cancel();
            Assert.AreEqual(LessonStatus.Canceled, l1.Status);
        }
        [TestCase]
        public void Cancel_cancelCompletedLesson_throwsException()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );
            l1.AddToSchedule();
            l1.Complete();

            var exception = Assert.Throws<InvalidOperationException>(
                () => l1.Cancel()
            );

            Assert.IsTrue(
                exception.Message.ToLower().Contains(
                    "can't cancel completed lesson"
                )
            );
        }
        [TestCase]
        public void Cancel_CancelNonActiveLesson_lessonUpdated()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order);

            var oldUpdCnt = lessons.UpdatesCount;
            l1.Cancel();
            Assert.AreEqual(oldUpdCnt + 1, lessons.UpdatesCount);
        }
        [TestCase]
        public void Cancel_CancelActiveLesson_lessonUpdated()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );
            l1.AddToSchedule();
            var oldUpdCnt = lessons.UpdatesCount;
            l1.Cancel();
            Assert.AreEqual(oldUpdCnt + 1, lessons.UpdatesCount);
        }
        [TestCase]
        public void Cancel_cancelMovedLesson_throwsException()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );
            l1.AddToSchedule();
            l1.MoveTo(new DateTime(2021, 10, 12, 12, 0, 0));

            var exception = Assert.Throws<InvalidOperationException>(
                () => l1.Cancel()
            );

            Assert.IsTrue(
                exception.Message.ToLower().Contains(
                    "can't cancel moved lesson"
                )
            );
        }
        [TestCase]
        public void Cancel_cancelActiveLesson_MovedOnPropertyIsNull()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );
            l1.AddToSchedule();
            l1.Cancel();
            Assert.IsNull(l1.MovedOn);
        }
        [TestCase]
        public void Cancel_cancelActiveLesson_MovedFromPropertyIsNull()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );
            l1.AddToSchedule();
            l1.Cancel();
            Assert.IsNull(l1.MovedFrom);
        }

        #endregion

        #region Renew Tests

        [TestCase]
        public void Renew_renewNonActiveLesson_throwsError()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );
            var exception = Assert.Throws<InvalidOperationException>(
                () => l1.Renew()
            );
            Assert.IsTrue(
                exception.Message.ToLower().Contains(
                    "can't renew non-active lesson"
                )
            );
        }
        [TestCase]
        public void Renew_renewActiveLesson_throwsError()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );
            l1.AddToSchedule();
            var exception = Assert.Throws<InvalidOperationException>(
                () => l1.Renew()
            );
            Assert.IsTrue(
                exception.Message.ToLower().Contains(
                    "can't renew active lesson"
                )
            );
        }
        [TestCase]
        public void Renew_renewCanceledLesson_throwsError()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );
            l1.Cancel();
            var exception = Assert.Throws<InvalidOperationException>(
                () => l1.Renew()
            );
            Assert.IsTrue(
                exception.Message.ToLower().Contains(
                    "can't renew canceled lesson"
                )
            );
        }
        [TestCase]
        public void Renew_renewCompletedLesson_storageUpdates()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );
            l1.AddToSchedule();
            l1.Complete();
            var oldUpdCnt = lessons.UpdatesCount;
            l1.Renew();
            Assert.AreEqual(oldUpdCnt + 1, lessons.UpdatesCount);
        }
        [TestCase]
        public void Renew_renewCompletedLesson_statusChangeToActive()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );
            l1.AddToSchedule();
            l1.Complete();
            l1.Renew();
            Assert.AreEqual(LessonStatus.Active, l1.Status);
        }
        [TestCase]
        public void Renew_renewCompletedLesson_balancesOfIncludedClientsIncrease()
        {
            Order o1 = Order.CreateNew("test order 1");
            Order o2 = Order.CreateNew("test order 2");
            Client c1 = Client.CreateNew(testClient1);
            Client c2 = Client.CreateNew(testClient2);
            Client c3 = Client.CreateNew(testClient3);
            Student s1 = Student.CreateNew(testStudent1, c1);
            Student s2 = Student.CreateNew(testStudent2, c2);
            Student s3 = Student.CreateNew(testStudent3, c3);
            o1.AddStudent(s1, 300000);
            o1.AddStudent(s3, 400000);
            o2.AddStudent(s2, 350000);

            Lesson l1 = Lesson.CreateNew(
                new DateTime(2022, 1, 15, 12, 0, 0), 90, o1
            );
            Lesson l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 14, 0, 0), 90, o2
            );
            Lesson l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 16, 0, 0), 120, o1
            );
            l1.AddToSchedule();
            l2.AddToSchedule();
            l3.AddToSchedule();

            l1.Complete();
            l1.Renew();
            Assert.AreEqual(0, c1.BalanceInKopeks);
            Assert.AreEqual(0, c3.BalanceInKopeks);
        }
        [TestCase]
        public void Renew_renewMovedLesson_throwsError()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );
            l1.AddToSchedule();
            l1.MoveTo(new DateTime(2021, 10, 12, 12, 0, 0));
            var exception = Assert.Throws<InvalidOperationException>(
                () => l1.Renew()
            );
            Assert.IsTrue(
                exception.Message.ToLower().Contains(
                    "can't renew moved lesson"
                )
            );
        }

        #endregion

        #region Restore Tests

        [TestCase]
        public void Restore_restoreCanceledLesson_statusChangeToNonActive()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );
            l1.AddToSchedule();
            l1.Cancel();
            l1.Restore();
            Assert.AreEqual(LessonStatus.NonActive, l1.Status);
        }
        [TestCase]
        public void Restore_restoreCanceledLesson_lessonUpdated()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );
            l1.AddToSchedule();
            l1.Cancel();
            var oldUpdCnt = lessons.UpdatesCount;
            l1.Restore();
            Assert.AreEqual(oldUpdCnt + 1, lessons.UpdatesCount);
        }
        [TestCase]
        public void Restore_restoreNonActiveLesson_throwsException()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );
            var exception = Assert.Throws<InvalidOperationException>(
                () => l1.Restore()
            );
            Assert.IsTrue(
                exception.Message.ToLower().Contains(
                    "can't restore non-active lesson"
                )
            );
        }
        [TestCase]
        public void Restore_restoreActiveLesson_throwsException()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );
            l1.AddToSchedule();
            var exception = Assert.Throws<InvalidOperationException>(
                () => l1.Restore()
            );
            Assert.IsTrue(
                exception.Message.ToLower().Contains(
                    "can't restore active lesson"
                )
            );
        }
        [TestCase]
        public void Restore_restoreCompletedLesson_throwsException()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );
            l1.AddToSchedule();
            l1.Complete();
            var exception = Assert.Throws<InvalidOperationException>(
                () => l1.Restore()
            );
            Assert.IsTrue(
                exception.Message.ToLower().Contains(
                    "can't restore completed lesson"
                )
            );
        }
        [TestCase]
        public void Restore_restoreMovedLesson_throwsException()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );
            l1.AddToSchedule();
            l1.MoveTo(new DateTime(2021, 10, 12, 12, 0, 0));
            var exception = Assert.Throws<InvalidOperationException>(
                () => l1.Restore()
            );
            Assert.IsTrue(
                exception.Message.ToLower().Contains(
                    "can't restore moved lesson"
                )
            );
        }

        #endregion

        #region MoveTo Tests

        [TestCase]
        public void MoveTo_moveActiveLesson_statusChangeToMoved()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );
            l1.AddToSchedule();
            l1.MoveTo(new DateTime(2021, 10, 12, 14, 0, 0));
            Assert.AreEqual(LessonStatus.Moved, l1.Status);
        }
        [TestCase]
        public void MoveTo_moveActiveLesson_newLessonStatusIsActive()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );
            l1.AddToSchedule();
            Lesson l2 = l1.MoveTo(new DateTime(2021, 10, 12, 14, 0, 0));
            Assert.AreEqual(LessonStatus.Active, l2.Status);
        }
        [TestCase]
        public void MoveTo_moveActiveLesson_MovedOnPropertySetCorrect()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );
            l1.AddToSchedule();
            Lesson l2 = l1.MoveTo(new DateTime(2021, 10, 12, 14, 0, 0));
            Assert.AreEqual(l2, l1.MovedOn);
        }
        [TestCase]
        public void MoveTo_moveActiveLesson_MovedFromPropertySetCorrect()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );
            l1.AddToSchedule();
            Lesson l2 = l1.MoveTo(new DateTime(2021, 10, 12, 14, 0, 0));
            Assert.AreEqual(l1, l2.MovedFrom);
        }
        [TestCase]
        public void MoveTo_moveActiveLesson_lessonUpdated()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );
            l1.AddToSchedule();
            var oldUpdCnt = lessons.UpdatesCount;
            l1.MoveTo(new DateTime(2021, 10, 12, 14, 0, 0));
            Assert.AreEqual(oldUpdCnt + 2, lessons.UpdatesCount);
        }
        [TestCase]
        public void MoveTo_doubleMoving_MovedOnPropertiecIsCorrect()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );
            l1.AddToSchedule();
            Lesson l2 = l1.MoveTo(new DateTime(2021, 10, 12, 14, 0, 0));
            Lesson l3 = l2.MoveTo(new DateTime(2021, 10, 14, 12, 0, 0));
            Assert.AreEqual(l2, l1.MovedOn);
            Assert.AreEqual(l3, l2.MovedOn);
        }
        [TestCase]
        public void MoveTo_doubleMoving_MovedFromPropertiecIsCorrect()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );
            l1.AddToSchedule();
            Lesson l2 = l1.MoveTo(new DateTime(2021, 10, 12, 14, 0, 0));
            Lesson l3 = l2.MoveTo(new DateTime(2021, 10, 14, 12, 0, 0));
            Assert.AreEqual(l1, l2.MovedFrom);
            Assert.AreEqual(l2, l3.MovedFrom);
        }
        [TestCase]
        public void MoveTo_movingNonActiveLesson_throwsException()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );
            var exception = Assert.Throws<InvalidOperationException>(
                () => l1.MoveTo(new DateTime(2021, 10, 12, 14, 0, 0))
            );
            Assert.IsTrue(
                exception.Message.ToLower().Contains(
                    "can't move non-active lesson"
                )
            );
        }
        [TestCase]
        public void MoveTo_movingCanceledLesson_throwsException()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );
            l1.AddToSchedule();
            l1.Cancel();
            var exception = Assert.Throws<InvalidOperationException>(
                () => l1.MoveTo(new DateTime(2021, 10, 12, 14, 0, 0))
            );
            Assert.IsTrue(
                exception.Message.ToLower().Contains(
                    "can't move canceled lesson"
                )
            );
        }
        [TestCase]
        public void MoveTo_movingCompletedLesson_throwsException()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );
            l1.AddToSchedule();
            l1.Complete();
            var exception = Assert.Throws<InvalidOperationException>(
                () => l1.MoveTo(new DateTime(2021, 10, 12, 14, 0, 0))
            );
            Assert.IsTrue(
                exception.Message.ToLower().Contains(
                    "can't move completed lesson"
                )
            );
        }
        [TestCase]
        public void MoveTo_movingMovedLesson_throwsException()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, order
            );
            l1.AddToSchedule();
            l1.MoveTo(new DateTime(2021, 10, 12, 14, 0, 0));
            var exception = Assert.Throws<InvalidOperationException>(
                () => l1.MoveTo(new DateTime(2021, 10, 14, 12, 0, 0))
            );
            Assert.IsTrue(
                exception.Message.ToLower().Contains(
                    "can't move moved lesson"
                )
            );
        }
        [TestCase]
        public void MoveTo_movedIntersectLessonLeft_throwsError()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 180, order
            );
            Lesson l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 14, 0, 0), 90, order
            );

            l1.AddToSchedule();
            l2.AddToSchedule();
            var exc = Assert.Throws<InvalidOperationException>(
                () => l1.MoveTo(new DateTime(2021, 10, 12, 12, 0, 0))
            );

            Assert.IsTrue(
                exc.Message.ToLower().Contains(
                    "intersect other lessons in schedule"
                )
            );
            Assert.AreEqual(LessonStatus.Active, l1.Status);
            Assert.IsNull(l1.MovedOn);
            Assert.AreEqual(LessonStatus.Active, l2.Status);
        }
        [TestCase]
        public void MoveTo_movedIntersectLessonRight_ThrowsError()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 180, order
            );
            Lesson l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 14, 0, 0), 90, order
            );

            l1.AddToSchedule();
            l2.AddToSchedule();
            var exc = Assert.Throws<InvalidOperationException>(
                () => l1.MoveTo(new DateTime(2021, 10, 12, 15, 0, 0))
            );

            Assert.IsTrue(
                exc.Message.ToLower().Contains(
                    "intersect other lessons in schedule"
                )
            );
            Assert.AreEqual(LessonStatus.Active, l2.Status);
            Assert.IsNull(l1.MovedOn);
            Assert.AreEqual(LessonStatus.Active, l1.Status);
        }
        [TestCase]
        public void MoveTo_movedContainsLesson_ThrowsError()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 180, order
            );
            Lesson l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 14, 0, 0), 90, order
            );

            l1.AddToSchedule();
            l2.AddToSchedule();
            var exc = Assert.Throws<InvalidOperationException>(
                () => l1.MoveTo(new DateTime(2021, 10, 12, 13, 30, 0))
            );

            Assert.IsTrue(
                exc.Message.ToLower().Contains(
                    "intersect other lessons in schedule"
                )
            );
            Assert.AreEqual(LessonStatus.Active, l2.Status);
            Assert.IsNull(l1.MovedOn);
            Assert.AreEqual(LessonStatus.Active, l1.Status);
        }
        [TestCase]
        public void MoveTo_lessonContainsMoved_ThrowsError()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 180, order
            );
            Lesson l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 14, 0, 0), 90, order
            );

            l1.AddToSchedule();
            l2.AddToSchedule();
            var exc = Assert.Throws<InvalidOperationException>(
                () => l2.MoveTo(new DateTime(2021, 10, 10, 12, 30, 0))
            );

            Assert.IsTrue(
                exc.Message.ToLower().Contains(
                    "intersect other lessons in schedule"
                )
            );
            Assert.AreEqual(LessonStatus.Active, l2.Status);
            Assert.IsNull(l2.MovedOn);
            Assert.AreEqual(LessonStatus.Active, l1.Status);
        }
        [TestCase]
        public void MoveTo_movedIntersectMany_ThrowsError()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 12, 0, 0), 90, order
            );
            Lesson l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 14, 0, 0), 90, order
            );
            Lesson li = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 13, 0, 0), 120, order
            );

            l1.AddToSchedule();
            l2.AddToSchedule();
            li.AddToSchedule();
            var exc = Assert.Throws<InvalidOperationException>(
                () => li.MoveTo(new DateTime(2021, 10, 12, 13, 0, 0))
            );

            Assert.IsTrue(
                exc.Message.ToLower().Contains(
                    "intersect other lessons in schedule"
                )
            );
            Assert.AreEqual(LessonStatus.Active, l1.Status);
            Assert.AreEqual(LessonStatus.Active, l2.Status);
            Assert.AreEqual(LessonStatus.Active, li.Status);
            Assert.IsNull(li.MovedOn);
        }
        [TestCase]
        public void MoveTo_moveLesson_orderNotChanged()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 12, 0, 0), 90, order
            );

            l1.AddToSchedule();
            var l1n = l1.MoveTo(new DateTime(2021, 10, 13, 13, 0, 0));

            Assert.AreEqual(l1.Order, l1n.Order);
        }
        [TestCase]
        public void MoveTo_moveLesson_lengthNotChanged()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 12, 0, 0), 90, order
            );

            l1.AddToSchedule();
            var l1n = l1.MoveTo(new DateTime(2021, 10, 13, 13, 0, 0));

            Assert.AreEqual(l1.LengthInMinutes, l1n.LengthInMinutes);
        }

        #endregion

        #region CancelMove Tests

        [TestCase]
        public void CancelMove_cancelMoveForNonActiveLesson_ThrowsError()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 12, 0, 0), 90, order
            );
            var exception = Assert.Throws<InvalidOperationException>(
                () => l1.CancelMove()
            );
            Assert.True(
                exception.Message.ToLower().Contains(
                    "can't cancel move for non-active lesson"
                )
            );
        }
        [TestCase]
        public void CancelMove_cancelMoveForActiveLesson_ThrowsError()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 12, 0, 0), 90, order
            );
            l1.AddToSchedule();
            var exception = Assert.Throws<InvalidOperationException>(
                () => l1.CancelMove()
            );
            Assert.True(
                exception.Message.ToLower().Contains(
                    "can't cancel move for active lesson"
                )
            );
        }
        [TestCase]
        public void CancelMove_cancelMoveForCanceledLesson_ThrowsError()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 12, 0, 0), 90, order
            );
            l1.AddToSchedule();
            l1.Cancel();
            var exception = Assert.Throws<InvalidOperationException>(
                () => l1.CancelMove()
            );
            Assert.True(
                exception.Message.ToLower().Contains(
                    "can't cancel move for canceled lesson"
                )
            );
        }
        [TestCase]
        public void CancelMove_cancelMoveForCompletedLesson_ThrowsError()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 12, 0, 0), 90, order
            );
            l1.AddToSchedule();
            l1.Complete();
            var exception = Assert.Throws<InvalidOperationException>(
                () => l1.CancelMove()
            );
            Assert.True(
                exception.Message.ToLower().Contains(
                    "can't cancel move for completed lesson"
                )
            );
        }
        [TestCase]
        public void CancelMove_oneMoveFinalLessonComplete_ThrowsError()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 12, 0, 0), 90, order
            );
            l1.AddToSchedule();
            Lesson l2 = l1.MoveTo(new DateTime(2021, 10, 14, 12, 0, 0));
            l2.Complete();
            var exception = Assert.Throws<InvalidOperationException>(
                () => l1.CancelMove()
            );
            Assert.True(
                exception.Message.ToLower().Contains(
                    "can't cancel move for completed final lesson"
                )
            );
        }
        [TestCase]
        public void CancelMove_twoMoveFinalLessonComplete_ThrowsError()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 12, 0, 0), 90, order
            );
            l1.AddToSchedule();
            Lesson l2 = l1.MoveTo(new DateTime(2021, 10, 14, 12, 0, 0));
            Lesson l3 = l2.MoveTo(new DateTime(2021, 10, 16, 12, 0, 0));
            l3.Complete();
            var exception = Assert.Throws<InvalidOperationException>(
                () => l1.CancelMove()
            );
            Assert.True(
                exception.Message.ToLower().Contains(
                    "can't cancel move for completed final lesson"
                )
            );
        }
        [TestCase]
        public void CancelMove_oneMoveCancelMovedLesson_removeAllNextLessons()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 12, 0, 0), 90, order
            );
            l1.AddToSchedule();
            Lesson l2 = l1.MoveTo(new DateTime(2021, 10, 14, 12, 0, 0));
            l1.CancelMove();

            Assert.IsFalse(lessons.GetAll().Contains(l2));
        }
        [TestCase]
        public void CancelMove_twoMoveCancelMovedLesson_removeAllNextLessons()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 12, 0, 0), 90, order
            );
            l1.AddToSchedule();
            Lesson l2 = l1.MoveTo(new DateTime(2021, 10, 14, 12, 0, 0));
            Lesson l3 = l2.MoveTo(new DateTime(2021, 10, 16, 12, 0, 0));            
            l1.CancelMove();

            Assert.IsFalse(lessons.GetAll().Contains(l2));
            Assert.IsFalse(lessons.GetAll().Contains(l3));
        }
        [TestCase]
        public void CancelMove_cancelMovedLesson_statusChangedToNonActive()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 12, 0, 0), 90, order
            );
            l1.AddToSchedule();
            Lesson l2 = l1.MoveTo(new DateTime(2021, 10, 14, 12, 0, 0));
            l1.CancelMove();

            Assert.AreEqual(LessonStatus.NonActive, l1.Status);
        }
        [TestCase]
        public void CancelMove_oneMoveCancelMovedLesson_makeTwoUpdates()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 12, 0, 0), 90, order
            );
            l1.AddToSchedule();
            Lesson l2 = l1.MoveTo(new DateTime(2021, 10, 14, 12, 0, 0));
            int updCntBefore = lessons.UpdatesCount;
            l1.CancelMove();
            Assert.AreEqual(updCntBefore + 1, lessons.UpdatesCount);
        }
        [TestCase]
        public void CancelMove_oneMoveCancelMovedLesson_MovedOnIsNull()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 12, 0, 0), 90, order
            );
            l1.AddToSchedule();
            l1.MoveTo(new DateTime(2021, 10, 14, 12, 0, 0));            
            l1.CancelMove();
            Assert.IsNull(l1.MovedOn);
        }
        [TestCase]
        public void CancelMove_twoMoveCancelMovedLesson_MovedOnIsNull()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 12, 0, 0), 90, order
            );
            l1.AddToSchedule();
            Lesson l2 = l1.MoveTo(new DateTime(2021, 10, 14, 12, 0, 0));
            l2.MoveTo(new DateTime(2021, 10, 15, 12, 0, 0));
            l1.CancelMove();
            Assert.IsNull(l1.MovedOn);
        }
        #endregion

        #region GetAllOnDate Tests

        [TestCase]
        public void GetAllOnDate_emptyCollection_returnEmptyList()
        {
            var lessons = Lesson.GetAllOnDate(new DateTime(2021, 1, 2));
            Assert.AreEqual(0, lessons.Count);
        }
        [TestCase]
        public void GetAllOnDate_existTwoNonActiveLessonsOnDate_returnBoth()
        {
            Order o = Order.CreateNew("o1");
            var l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 10, 0, 0), 90, o
            );
            var l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, o
            );
            var l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 10, 0, 0), 90, o
            );
            var lessons = Lesson.GetAllOnDate(new DateTime(2021, 10, 10));
            Assert.AreEqual(2, lessons.Count);
            Assert.IsTrue(lessons.Contains(l1));
            Assert.IsTrue(lessons.Contains(l2));
        }
        [TestCase]
        public void GetAllOnDate_existTwoActiveLessonsOnDate_returnBoth()
        {
            Order o = Order.CreateNew("o1");
            var l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 10, 0, 0), 90, o
            );
            var l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, o
            );
            var l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 10, 0, 0), 90, o
            );
            l1.AddToSchedule();
            l2.AddToSchedule();
            l3.AddToSchedule();
            var lessons = Lesson.GetAllOnDate(new DateTime(2021, 10, 10));
            Assert.AreEqual(2, lessons.Count);
            Assert.IsTrue(lessons.Contains(l1));
            Assert.IsTrue(lessons.Contains(l2));
        }
        [TestCase]
        public void GetAllOnDate_existTwoCancelledLessonsOnDate_returnBoth()
        {
            Order o = Order.CreateNew("o1");
            var l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 10, 0, 0), 90, o
            );
            var l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, o
            );
            var l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 10, 0, 0), 90, o
            );
            l1.AddToSchedule();
            l2.AddToSchedule();
            l1.Cancel();
            l2.Cancel();
            l3.AddToSchedule();
            l3.Cancel();
            var lessons = Lesson.GetAllOnDate(new DateTime(2021, 10, 10));
            Assert.AreEqual(2, lessons.Count);
            Assert.IsTrue(lessons.Contains(l1));
            Assert.IsTrue(lessons.Contains(l2));
        }
        [TestCase]
        public void GetAllOnDate_existTwoCompletedLessonsOnDate_returnBoth()
        {
            Order o = Order.CreateNew("o1");
            var l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 10, 0, 0), 90, o
            );
            var l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, o
            );
            var l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 10, 0, 0), 90, o
            );
            l1.AddToSchedule();
            l2.AddToSchedule();
            l1.Complete();
            l2.Complete();
            l3.AddToSchedule();
            l3.Complete();
            var lessons = Lesson.GetAllOnDate(new DateTime(2021, 10, 10));
            Assert.AreEqual(2, lessons.Count);
            Assert.IsTrue(lessons.Contains(l1));
            Assert.IsTrue(lessons.Contains(l2));
        }
        [TestCase]
        public void GetAllOnDate_existTwoMovedLessonsOnDate_returnBoth()
        {
            Order o = Order.CreateNew("o1");
            var l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 10, 0, 0), 90, o
            );
            var l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, o
            );
            var l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 10, 0, 0), 90, o
            );
            l1.AddToSchedule();
            l2.AddToSchedule();
            l3.AddToSchedule();
            l1.MoveTo(new DateTime(2021, 10, 13, 10, 0, 0));
            l2.MoveTo(new DateTime(2021, 10, 13, 12, 0, 0));
            l3.MoveTo(new DateTime(2021, 10, 14, 12, 0, 0));
            var lessons = Lesson.GetAllOnDate(new DateTime(2021, 10, 10));
            Assert.AreEqual(2, lessons.Count);
            Assert.IsTrue(lessons.Contains(l1));
            Assert.IsTrue(lessons.Contains(l2));
        }
        [TestCase]
        public void GetAllOnDate_existTwoMovedAndOneTargetLessonsOnDate_returnAll()
        {
            Order o = Order.CreateNew("o1");
            var l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 10, 0, 0), 90, o
            );
            var l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, o
            );
            var l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 10, 0, 0), 90, o
            );
            l1.AddToSchedule();
            l2.AddToSchedule();
            l3.AddToSchedule();
            var lt = l1.MoveTo(new DateTime(2021, 10, 10, 16, 0, 0));
            l2.MoveTo(new DateTime(2021, 10, 13, 12, 0, 0));
            l3.MoveTo(new DateTime(2021, 10, 14, 12, 0, 0));

            var lessons = Lesson.GetAllOnDate(new DateTime(2021, 10, 10));
            Assert.AreEqual(3, lessons.Count);
            Assert.IsTrue(lessons.Contains(l1));
            Assert.IsTrue(lessons.Contains(l2));
            Assert.IsTrue(lessons.Contains(lt));
        }

        #endregion

        #region GetScheduledLaterThan Tests

        [TestCase]
        public void GetScheduledLaterThan_emptyCollection_returnEmptyList()
        {
            var lessons = Lesson.GetScheduledLaterThan(new DateTime(2021, 1, 2));
            Assert.AreEqual(0, lessons.Count);
        }
        [TestCase]
        public void GetScheduledLaterThan_existTwoNonActiveLessonsOnDate_returnEmpty()
        {
            Order o = Order.CreateNew("o1");
            var l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 10, 0, 0), 90, o
            );
            var l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, o
            );
            var l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 10, 0, 0), 90, o
            );
            var lessons = Lesson.GetScheduledLaterThan(
                new DateTime(2021, 10, 10, 11, 0, 0)
            );
            Assert.AreEqual(0, lessons.Count);
        }
        [TestCase]
        public void GetScheduledLaterThan_existTwoActiveLessonsOnDate_returnBoth()
        {
            Order o = Order.CreateNew("o1");
            var l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 10, 0, 0), 90, o
            );
            var l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, o
            );
            var l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 10, 0, 0), 90, o
            );
            l1.AddToSchedule();
            l2.AddToSchedule();
            l3.AddToSchedule();
            var lessons = Lesson.GetScheduledLaterThan(
                new DateTime(2021, 10, 10, 11, 0, 0)
            );
            Assert.AreEqual(2, lessons.Count);
            Assert.IsTrue(lessons.Contains(l2));
            Assert.IsTrue(lessons.Contains(l3));
        }
        [TestCase]
        public void GetScheduledLaterThan_existTwoCancelledLessonsOnDate_returnEmpty()
        {
            Order o = Order.CreateNew("o1");
            var l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 10, 0, 0), 90, o
            );
            var l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, o
            );
            var l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 10, 0, 0), 90, o
            );
            l1.AddToSchedule();
            l2.AddToSchedule();
            l1.Cancel();
            l2.Cancel();
            l3.AddToSchedule();
            l3.Cancel();
            var lessons = Lesson.GetScheduledLaterThan(
                new DateTime(2021, 10, 10, 11, 0, 0)
            );
            Assert.AreEqual(0, lessons.Count);
        }
        [TestCase]
        public void GetScheduledLaterThan_existTwoCompletedLessonsOnDate_returnBoth()
        {
            Order o = Order.CreateNew("o1");
            var l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 10, 0, 0), 90, o
            );
            var l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, o
            );
            var l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 10, 0, 0), 90, o
            );
            l1.AddToSchedule();
            l2.AddToSchedule();
            l1.Complete();
            l2.Complete();
            l3.AddToSchedule();
            l3.Complete();
            var lessons = Lesson.GetScheduledLaterThan(
                new DateTime(2021, 10, 10, 11, 0, 0)
            );
            Assert.AreEqual(2, lessons.Count);
            Assert.IsTrue(lessons.Contains(l2));
            Assert.IsTrue(lessons.Contains(l3));
        }
        [TestCase]
        public void GetScheduledLaterThan_existTwoMovedLessonsOnDate_returnEmpty()
        {
            Order o = Order.CreateNew("o1");
            var l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 10, 0, 0), 90, o
            );
            var l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, o
            );
            var l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 10, 0, 0), 90, o
            );
            l1.AddToSchedule();
            l2.AddToSchedule();
            l3.AddToSchedule();
            l1.MoveTo(new DateTime(2021, 10, 9, 10, 0, 0));
            l2.MoveTo(new DateTime(2021, 10, 9, 12, 0, 0));
            l3.MoveTo(new DateTime(2021, 10, 8, 12, 0, 0));
            var lessons = Lesson.GetScheduledLaterThan(
                new DateTime(2021, 10, 10, 11, 0, 0)
            );
            Assert.AreEqual(0, lessons.Count);
        }
        [TestCase]
        public void GetScheduledLaterThan_existTwoMovedAndOneTargetLessonsOnDate_returnOnlyActive()
        {
            Order o = Order.CreateNew("o1");
            var l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 10, 0, 0), 90, o
            );
            var l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, o
            );
            var l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 10, 0, 0), 90, o
            );
            l1.AddToSchedule();
            l2.AddToSchedule();
            l3.AddToSchedule();
            l1.MoveTo(new DateTime(2021, 10, 9, 16, 0, 0));
            l2.MoveTo(new DateTime(2021, 10, 9, 12, 0, 0));
            var lt = l3.MoveTo(new DateTime(2021, 10, 14, 12, 0, 0));

            var lessons = Lesson.GetScheduledLaterThan(
                new DateTime(2021, 10, 10, 11, 0, 0)
            );
            Assert.AreEqual(1, lessons.Count);
            Assert.IsTrue(lessons.Contains(lt));
        }

        #endregion

        #region GetAllLaterThan Tests

        [TestCase]
        public void GetAllLaterThan_emptyCollection_returnEmptyList()
        {
            var lessons = Lesson.GetAllLaterThan(new DateTime(2021, 1, 2));
            Assert.AreEqual(0, lessons.Count);
        }
        [TestCase]
        public void GetAllLaterThan_existTwoNonActiveLessonsOnDate_returnBoth()
        {
            Order o = Order.CreateNew("o1");
            var l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 10, 0, 0), 90, o
            );
            var l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, o
            );
            var l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 10, 0, 0), 90, o
            );
            var lessons = Lesson.GetAllLaterThan(
                new DateTime(2021, 10, 10, 11, 0, 0)
            );
            Assert.AreEqual(2, lessons.Count);
            Assert.IsTrue(lessons.Contains(l2));
            Assert.IsTrue(lessons.Contains(l3));
        }
        [TestCase]
        public void GetAllLaterThan_existTwoActiveLessonsOnDate_returnBoth()
        {
            Order o = Order.CreateNew("o1");
            var l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 10, 0, 0), 90, o
            );
            var l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, o
            );
            var l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 10, 0, 0), 90, o
            );
            l1.AddToSchedule();
            l2.AddToSchedule();
            l3.AddToSchedule();
            var lessons = Lesson.GetAllLaterThan(
                new DateTime(2021, 10, 10, 11, 0, 0)
            );
            Assert.AreEqual(2, lessons.Count);
            Assert.IsTrue(lessons.Contains(l2));
            Assert.IsTrue(lessons.Contains(l3));
        }
        [TestCase]
        public void GetAllLaterThan_existTwoCancelledLessonsOnDate_returnBoth()
        {
            Order o = Order.CreateNew("o1");
            var l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 10, 0, 0), 90, o
            );
            var l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, o
            );
            var l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 10, 0, 0), 90, o
            );
            l1.AddToSchedule();
            l2.AddToSchedule();
            l1.Cancel();
            l2.Cancel();
            l3.AddToSchedule();
            l3.Cancel();
            var lessons = Lesson.GetAllLaterThan(
                new DateTime(2021, 10, 10, 11, 0, 0)
            );
            Assert.AreEqual(2, lessons.Count);
            Assert.IsTrue(lessons.Contains(l2));
            Assert.IsTrue(lessons.Contains(l3));
        }
        [TestCase]
        public void GetAllLaterThan_existTwoCompletedLessonsOnDate_returnBoth()
        {
            Order o = Order.CreateNew("o1");
            var l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 10, 0, 0), 90, o
            );
            var l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, o
            );
            var l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 10, 0, 0), 90, o
            );
            l1.AddToSchedule();
            l2.AddToSchedule();
            l1.Complete();
            l2.Complete();
            l3.AddToSchedule();
            l3.Complete();
            var lessons = Lesson.GetAllLaterThan(
                new DateTime(2021, 10, 10, 11, 0, 0)
            );
            Assert.AreEqual(2, lessons.Count);
            Assert.IsTrue(lessons.Contains(l2));
            Assert.IsTrue(lessons.Contains(l3));
        }
        [TestCase]
        public void GetAllLaterThan_existTwoMovedLessonsOnDate_returnBoth()
        {
            Order o = Order.CreateNew("o1");
            var l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 10, 0, 0), 90, o
            );
            var l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, o
            );
            var l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 10, 0, 0), 90, o
            );
            l1.AddToSchedule();
            l2.AddToSchedule();
            l3.AddToSchedule();
            l1.MoveTo(new DateTime(2021, 10, 9, 10, 0, 0));
            l2.MoveTo(new DateTime(2021, 10, 9, 12, 0, 0));
            l3.MoveTo(new DateTime(2021, 10, 8, 12, 0, 0));
            var lessons = Lesson.GetAllLaterThan(
                new DateTime(2021, 10, 10, 11, 0, 0)
            );
            Assert.AreEqual(2, lessons.Count);
            Assert.IsTrue(lessons.Contains(l2));
            Assert.IsTrue(lessons.Contains(l3));
        }
        [TestCase]
        public void GetAllLaterThan_existTwoMovedAndOneTargetLessonsOnDate_returnAll()
        {
            Order o = Order.CreateNew("o1");
            var l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 10, 0, 0), 90, o
            );
            var l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, o
            );
            var l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 10, 0, 0), 90, o
            );
            l1.AddToSchedule();
            l2.AddToSchedule();
            l3.AddToSchedule();
            l1.MoveTo(new DateTime(2021, 10, 9, 16, 0, 0));
            l2.MoveTo(new DateTime(2021, 10, 9, 12, 0, 0));
            var lt = l3.MoveTo(new DateTime(2021, 10, 14, 12, 0, 0));

            var lessons = Lesson.GetAllLaterThan(
                new DateTime(2021, 10, 10, 11, 0, 0)
            );
            Assert.AreEqual(3, lessons.Count);
            Assert.IsTrue(lessons.Contains(l2));
            Assert.IsTrue(lessons.Contains(l3));
            Assert.IsTrue(lessons.Contains(lt));
        }

        #endregion

        #region GetScheduledEarlierThan Tests

        [TestCase]
        public void GetScheduledEarlierThan_emptyCollection_returnEmptyList()
        {
            var lessons = Lesson.GetScheduledEarlierThan(new DateTime(2021, 1, 2));
            Assert.AreEqual(0, lessons.Count);
        }
        [TestCase]
        public void GetScheduledEarlierThan_existTwoNonActiveLessonsOnDate_returnEmpty()
        {
            Order o = Order.CreateNew("o1");
            var l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 10, 0, 0), 90, o
            );
            var l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, o
            );
            var l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 10, 0, 0), 90, o
            );
            var lessons = Lesson.GetScheduledEarlierThan(
                new DateTime(2021, 10, 11, 11, 0, 0)
            );
            Assert.AreEqual(0, lessons.Count);
        }
        [TestCase]
        public void GetScheduledEarlierThan_existTwoActiveLessonsOnDate_returnBoth()
        {
            Order o = Order.CreateNew("o1");
            var l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 10, 0, 0), 90, o
            );
            var l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, o
            );
            var l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 10, 0, 0), 90, o
            );
            l1.AddToSchedule();
            l2.AddToSchedule();
            l3.AddToSchedule();
            var lessons = Lesson.GetScheduledEarlierThan(
                new DateTime(2021, 10, 11, 11, 0, 0)
            );
            Assert.AreEqual(2, lessons.Count);
            Assert.IsTrue(lessons.Contains(l1));
            Assert.IsTrue(lessons.Contains(l2));
        }
        [TestCase]
        public void GetScheduledEarlierThan_existTwoCancelledLessonsOnDate_returnEmpty()
        {
            Order o = Order.CreateNew("o1");
            var l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 10, 0, 0), 90, o
            );
            var l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, o
            );
            var l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 10, 0, 0), 90, o
            );
            l1.AddToSchedule();
            l2.AddToSchedule();
            l1.Cancel();
            l2.Cancel();
            l3.AddToSchedule();
            l3.Cancel();
            var lessons = Lesson.GetScheduledEarlierThan(
                new DateTime(2021, 10, 11, 11, 0, 0)
            );
            Assert.AreEqual(0, lessons.Count);
        }
        [TestCase]
        public void GetScheduledEarlierThan_existTwoCompletedLessonsOnDate_returnBoth()
        {
            Order o = Order.CreateNew("o1");
            var l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 10, 0, 0), 90, o
            );
            var l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, o
            );
            var l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 10, 0, 0), 90, o
            );
            l1.AddToSchedule();
            l2.AddToSchedule();
            l1.Complete();
            l2.Complete();
            l3.AddToSchedule();
            l3.Complete();
            var lessons = Lesson.GetScheduledEarlierThan(
                new DateTime(2021, 10, 11, 11, 0, 0)
            );
            Assert.AreEqual(2, lessons.Count);
            Assert.IsTrue(lessons.Contains(l1));
            Assert.IsTrue(lessons.Contains(l2));
        }
        [TestCase]
        public void GetScheduledEarlierThan_existTwoMovedLessonsOnDate_returnEmpty()
        {
            Order o = Order.CreateNew("o1");
            var l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 10, 0, 0), 90, o
            );
            var l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, o
            );
            var l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 10, 0, 0), 90, o
            );
            l1.AddToSchedule();
            l2.AddToSchedule();
            l3.AddToSchedule();
            l1.MoveTo(new DateTime(2021, 10, 13, 10, 0, 0));
            l2.MoveTo(new DateTime(2021, 10, 13, 12, 0, 0));
            l3.MoveTo(new DateTime(2021, 10, 14, 12, 0, 0));
            var lessons = Lesson.GetScheduledEarlierThan(
                new DateTime(2021, 10, 11, 11, 0, 0)
            );
            Assert.AreEqual(0, lessons.Count);
        }
        [TestCase]
        public void GetScheduledEarlierThan_existTwoMovedAndOneTargetLessonsOnDate_returnOnlyActive()
        {
            Order o = Order.CreateNew("o1");
            var l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 10, 0, 0), 90, o
            );
            var l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, o
            );
            var l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 10, 0, 0), 90, o
            );
            l1.AddToSchedule();
            l2.AddToSchedule();
            l3.AddToSchedule();
            var lt = l1.MoveTo(new DateTime(2021, 10, 9, 16, 0, 0));
            l2.MoveTo(new DateTime(2021, 10, 12, 12, 0, 0));
            l3.MoveTo(new DateTime(2021, 10, 14, 12, 0, 0));

            var lessons = Lesson.GetScheduledEarlierThan(
                new DateTime(2021, 10, 11, 11, 0, 0)
            );
            Assert.AreEqual(1, lessons.Count);
            Assert.IsTrue(lessons.Contains(lt));
        }

        #endregion

        #region GetAllEarlierThan Tests

        [TestCase]
        public void GetAllEarlierThan_emptyCollection_returnEmptyList()
        {
            var lessons = Lesson.GetAllEarlierThan(new DateTime(2021, 1, 2));
            Assert.AreEqual(0, lessons.Count);
        }
        [TestCase]
        public void GetAllEarlierThan_existTwoNonActiveLessonsOnDate_returnBoth()
        {
            Order o = Order.CreateNew("o1");
            var l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 10, 0, 0), 90, o
            );
            var l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, o
            );
            var l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 10, 0, 0), 90, o
            );
            var lessons = Lesson.GetAllEarlierThan(
                new DateTime(2021, 10, 11, 11, 0, 0)
            );
            Assert.AreEqual(2, lessons.Count);
            Assert.IsTrue(lessons.Contains(l1));
            Assert.IsTrue(lessons.Contains(l2));
        }
        [TestCase]
        public void GetAllEarlierThan_existTwoActiveLessonsOnDate_returnBoth()
        {
            Order o = Order.CreateNew("o1");
            var l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 10, 0, 0), 90, o
            );
            var l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, o
            );
            var l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 10, 0, 0), 90, o
            );
            l1.AddToSchedule();
            l2.AddToSchedule();
            l3.AddToSchedule();
            var lessons = Lesson.GetAllEarlierThan(
                new DateTime(2021, 10, 11, 11, 0, 0)
            );
            Assert.AreEqual(2, lessons.Count);
            Assert.IsTrue(lessons.Contains(l1));
            Assert.IsTrue(lessons.Contains(l2));
        }
        [TestCase]
        public void GetAllEarlierThan_existTwoCancelledLessonsOnDate_returnBoth()
        {
            Order o = Order.CreateNew("o1");
            var l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 10, 0, 0), 90, o
            );
            var l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, o
            );
            var l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 10, 0, 0), 90, o
            );
            l1.AddToSchedule();
            l2.AddToSchedule();
            l1.Cancel();
            l2.Cancel();
            l3.AddToSchedule();
            l3.Cancel();
            var lessons = Lesson.GetAllEarlierThan(
                new DateTime(2021, 10, 11, 11, 0, 0)
            );
            Assert.AreEqual(2, lessons.Count);
            Assert.IsTrue(lessons.Contains(l1));
            Assert.IsTrue(lessons.Contains(l2));
        }
        [TestCase]
        public void GetAllEarlierThan_existTwoCompletedLessonsOnDate_returnBoth()
        {
            Order o = Order.CreateNew("o1");
            var l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 10, 0, 0), 90, o
            );
            var l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, o
            );
            var l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 10, 0, 0), 90, o
            );
            l1.AddToSchedule();
            l2.AddToSchedule();
            l1.Complete();
            l2.Complete();
            l3.AddToSchedule();
            l3.Complete();
            var lessons = Lesson.GetAllEarlierThan(
                new DateTime(2021, 10, 11, 11, 0, 0)
            );
            Assert.AreEqual(2, lessons.Count);
            Assert.IsTrue(lessons.Contains(l1));
            Assert.IsTrue(lessons.Contains(l2));
        }
        [TestCase]
        public void GetAllEarlierThan_existTwoMovedLessonsOnDate_returnBoth()
        {
            Order o = Order.CreateNew("o1");
            var l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 10, 0, 0), 90, o
            );
            var l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, o
            );
            var l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 10, 0, 0), 90, o
            );
            l1.AddToSchedule();
            l2.AddToSchedule();
            l3.AddToSchedule();
            l1.MoveTo(new DateTime(2021, 10, 13, 10, 0, 0));
            l2.MoveTo(new DateTime(2021, 10, 13, 12, 0, 0));
            l3.MoveTo(new DateTime(2021, 10, 14, 12, 0, 0));
            var lessons = Lesson.GetAllEarlierThan(
                new DateTime(2021, 10, 11, 11, 0, 0)
            );
            Assert.AreEqual(2, lessons.Count);
            Assert.IsTrue(lessons.Contains(l1));
            Assert.IsTrue(lessons.Contains(l2));
        }
        [TestCase]
        public void GetAllEarlierThan_existTwoMovedAndOneTargetLessonsOnDate_returnAll()
        {
            Order o = Order.CreateNew("o1");
            var l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 10, 0, 0), 90, o
            );
            var l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, o
            );
            var l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 10, 0, 0), 90, o
            );
            l1.AddToSchedule();
            l2.AddToSchedule();
            l3.AddToSchedule();
            l1.MoveTo(new DateTime(2021, 10, 13, 16, 0, 0));
            l2.MoveTo(new DateTime(2021, 10, 13, 12, 0, 0));
            var lt = l3.MoveTo(new DateTime(2021, 10, 9, 12, 0, 0));

            var lessons = Lesson.GetAllEarlierThan(
                new DateTime(2021, 10, 11, 11, 0, 0)
            );
            Assert.AreEqual(3, lessons.Count);
            Assert.IsTrue(lessons.Contains(l1));
            Assert.IsTrue(lessons.Contains(l2));
            Assert.IsTrue(lessons.Contains(lt));
        }

        #endregion

        #region GetScheduledBetween Tests

        [TestCase]
        public void GetScheduledBetween_emptyCollection_returnEmptyList()
        {
            var lessons = Lesson.GetScheduledBetween(
                new DateTime(2021, 1, 2), new DateTime(2021, 1, 5)
            );
            Assert.AreEqual(0, lessons.Count);
        }
        [TestCase]
        public void GetScheduledBetween_existTwoNonActiveLessonsOnDate_returnEmpty()
        {
            Order o = Order.CreateNew("o1");
            var l0 = Lesson.CreateNew(
                new DateTime(2021, 10, 9, 12, 0, 0), 90, o
            );
            var l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 10, 0, 0), 90, o
            );
            var l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, o
            );
            var l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 10, 0, 0), 90, o
            );
            var lessons = Lesson.GetScheduledBetween(
                new DateTime(2021, 10, 9, 13, 0, 0),
                new DateTime(2021, 10, 11, 11, 0, 0)
            );
            Assert.AreEqual(0, lessons.Count);
        }
        [TestCase]
        public void GetScheduledBetween_existTwoActiveLessonsOnDate_returnBoth()
        {
            Order o = Order.CreateNew("o1");
            var l0 = Lesson.CreateNew(
                new DateTime(2021, 10, 9, 12, 0, 0), 90, o
            );
            var l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 10, 0, 0), 90, o
            );
            var l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, o
            );
            var l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 10, 0, 0), 90, o
            );
            l0.AddToSchedule();
            l1.AddToSchedule();
            l2.AddToSchedule();
            l3.AddToSchedule();
            var lessons = Lesson.GetScheduledBetween(
                new DateTime(2021, 10, 9, 13, 0, 0),
                new DateTime(2021, 10, 11, 11, 0, 0)
            );
            Assert.AreEqual(2, lessons.Count);
            Assert.IsTrue(lessons.Contains(l1));
            Assert.IsTrue(lessons.Contains(l2));
        }
        [TestCase]
        public void GetScheduledBetween_existTwoCancelledLessonsOnDate_returnEmpty()
        {
            Order o = Order.CreateNew("o1");
            var l0 = Lesson.CreateNew(
                new DateTime(2021, 10, 9, 12, 0, 0), 90, o
            );
            var l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 10, 0, 0), 90, o
            );
            var l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, o
            );
            var l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 10, 0, 0), 90, o
            );
            l0.AddToSchedule();
            l1.AddToSchedule();
            l2.AddToSchedule();
            l0.Cancel();
            l1.Cancel();
            l2.Cancel();
            l3.AddToSchedule();
            l3.Cancel();
            var lessons = Lesson.GetScheduledBetween(
                new DateTime(2021, 10, 9, 13, 0, 0),
                new DateTime(2021, 10, 11, 11, 0, 0)
            );
            Assert.AreEqual(0, lessons.Count);
        }
        [TestCase]
        public void GetScheduledBetween_existTwoCompletedLessonsOnDate_returnBoth()
        {
            Order o = Order.CreateNew("o1");
            var l0 = Lesson.CreateNew(
                new DateTime(2021, 10, 9, 12, 0, 0), 90, o
            );
            var l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 10, 0, 0), 90, o
            );
            var l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, o
            );
            var l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 10, 0, 0), 90, o
            );
            l0.AddToSchedule();
            l1.AddToSchedule();
            l2.AddToSchedule();
            l0.Complete();
            l1.Complete();
            l2.Complete();
            l3.AddToSchedule();
            l3.Complete();
            var lessons = Lesson.GetScheduledBetween(
                new DateTime(2021, 10, 9, 13, 0, 0),
                new DateTime(2021, 10, 11, 11, 0, 0)
            );
            Assert.AreEqual(2, lessons.Count);
            Assert.IsTrue(lessons.Contains(l1));
            Assert.IsTrue(lessons.Contains(l2));
        }
        [TestCase]
        public void GetScheduledBetween_existTwoMovedLessonsOnDate_returnEmpty()
        {
            Order o = Order.CreateNew("o1");
            var l0 = Lesson.CreateNew(
                new DateTime(2021, 10, 9, 12, 0, 0), 90, o
            );
            var l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 10, 0, 0), 90, o
            );
            var l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, o
            );
            var l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 10, 0, 0), 90, o
            );
            l0.AddToSchedule();
            l1.AddToSchedule();
            l2.AddToSchedule();
            l3.AddToSchedule();
            l0.MoveTo(new DateTime(2021, 10, 8, 10, 0, 0));
            l1.MoveTo(new DateTime(2021, 10, 13, 10, 0, 0));
            l2.MoveTo(new DateTime(2021, 10, 13, 12, 0, 0));
            l3.MoveTo(new DateTime(2021, 10, 14, 12, 0, 0));
            var lessons = Lesson.GetScheduledBetween(
                new DateTime(2021, 10, 9, 13, 0, 0),
                new DateTime(2021, 10, 11, 11, 0, 0)
            );
            Assert.AreEqual(0, lessons.Count);
        }
        [TestCase]
        public void GetScheduledBetween_existTwoMovedAndOneTargetLessonsOnDate_returnOnlyActive()
        {
            Order o = Order.CreateNew("o1");
            var l0 = Lesson.CreateNew(
                new DateTime(2021, 10, 9, 12, 0, 0), 90, o
            );
            var l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 10, 0, 0), 90, o
            );
            var l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, o
            );
            var l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 10, 0, 0), 90, o
            );
            l0.AddToSchedule();
            l1.AddToSchedule();
            l2.AddToSchedule();
            l3.AddToSchedule();
            l0.MoveTo(new DateTime(2021, 10, 8, 10, 0, 0));
            var lt = l1.MoveTo(new DateTime(2021, 10, 9, 16, 0, 0));
            l2.MoveTo(new DateTime(2021, 10, 12, 12, 0, 0));
            l3.MoveTo(new DateTime(2021, 10, 14, 12, 0, 0));

            var lessons = Lesson.GetScheduledBetween(
                new DateTime(2021, 10, 9, 13, 0, 0),
                new DateTime(2021, 10, 11, 11, 0, 0)
            );
            Assert.AreEqual(1, lessons.Count);
            Assert.IsTrue(lessons.Contains(lt));
        }

        #endregion

        #region GetAllBetween Tests

        [TestCase]
        public void GetAllBetween_emptyCollection_returnEmptyList()
        {
            var lessons = Lesson.GetAllBetween(
                new DateTime(2021, 10, 9, 13, 0, 0),
                new DateTime(2021, 10, 11, 11, 0, 0)
            );
            Assert.AreEqual(0, lessons.Count);
        }
        [TestCase]
        public void GetAllBetween_existTwoNonActiveLessonsOnDate_returnBoth()
        {
            Order o = Order.CreateNew("o1");
            var l0 = Lesson.CreateNew(
                new DateTime(2021, 10, 9, 12, 0, 0), 90, o
            );
            var l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 10, 0, 0), 90, o
            );
            var l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, o
            );
            var l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 10, 0, 0), 90, o
            );
            var lessons = Lesson.GetAllBetween(
                new DateTime(2021, 10, 9, 13, 0, 0),
                new DateTime(2021, 10, 11, 11, 0, 0)
            );
            Assert.AreEqual(2, lessons.Count);
            Assert.IsTrue(lessons.Contains(l1));
            Assert.IsTrue(lessons.Contains(l2));
        }
        [TestCase]
        public void GetAllBetween_existTwoActiveLessonsOnDate_returnBoth()
        {
            Order o = Order.CreateNew("o1");
            var l0 = Lesson.CreateNew(
                new DateTime(2021, 10, 9, 12, 0, 0), 90, o
            );
            var l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 10, 0, 0), 90, o
            );
            var l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, o
            );
            var l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 10, 0, 0), 90, o
            );
            l0.AddToSchedule();
            l1.AddToSchedule();
            l2.AddToSchedule();
            l3.AddToSchedule();
            var lessons = Lesson.GetAllBetween(
                new DateTime(2021, 10, 9, 13, 0, 0),
                new DateTime(2021, 10, 11, 11, 0, 0)
            );
            Assert.AreEqual(2, lessons.Count);
            Assert.IsTrue(lessons.Contains(l1));
            Assert.IsTrue(lessons.Contains(l2));
        }
        [TestCase]
        public void GetAllBetween_existTwoCancelledLessonsOnDate_returnBoth()
        {
            Order o = Order.CreateNew("o1");
            var l0 = Lesson.CreateNew(
                new DateTime(2021, 10, 9, 12, 0, 0), 90, o
            );
            var l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 10, 0, 0), 90, o
            );
            var l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, o
            );
            var l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 10, 0, 0), 90, o
            );
            l0.AddToSchedule();
            l1.AddToSchedule();
            l2.AddToSchedule();
            l0.Cancel();
            l1.Cancel();
            l2.Cancel();
            l3.AddToSchedule();
            l3.Cancel();
            var lessons = Lesson.GetAllBetween(
                new DateTime(2021, 10, 9, 13, 0, 0),
                new DateTime(2021, 10, 11, 11, 0, 0)
            );
            Assert.AreEqual(2, lessons.Count);
            Assert.IsTrue(lessons.Contains(l1));
            Assert.IsTrue(lessons.Contains(l2));
        }
        [TestCase]
        public void GetAllBetween_existTwoCompletedLessonsOnDate_returnBoth()
        {
            Order o = Order.CreateNew("o1");
            var l0 = Lesson.CreateNew(
                new DateTime(2021, 10, 9, 12, 0, 0), 90, o
            );
            var l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 10, 0, 0), 90, o
            );
            var l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, o
            );
            var l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 10, 0, 0), 90, o
            );
            l0.AddToSchedule();
            l1.AddToSchedule();
            l2.AddToSchedule();
            l0.Complete();
            l1.Complete();
            l2.Complete();
            l3.AddToSchedule();
            l3.Complete();
            var lessons = Lesson.GetAllBetween(
                new DateTime(2021, 10, 9, 13, 0, 0),
                new DateTime(2021, 10, 11, 11, 0, 0)
            );
            Assert.AreEqual(2, lessons.Count);
            Assert.IsTrue(lessons.Contains(l1));
            Assert.IsTrue(lessons.Contains(l2));
        }
        [TestCase]
        public void GetAllBetween_existTwoMovedLessonsOnDate_returnBoth()
        {
            Order o = Order.CreateNew("o1");
            var l0 = Lesson.CreateNew(
                new DateTime(2021, 10, 9, 12, 0, 0), 90, o
            );
            var l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 10, 0, 0), 90, o
            );
            var l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, o
            );
            var l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 10, 0, 0), 90, o
            );
            l0.AddToSchedule();
            l1.AddToSchedule();
            l2.AddToSchedule();
            l3.AddToSchedule();
            l0.MoveTo(new DateTime(2021, 10, 8, 10, 0, 0));
            l1.MoveTo(new DateTime(2021, 10, 13, 10, 0, 0));
            l2.MoveTo(new DateTime(2021, 10, 13, 12, 0, 0));
            l3.MoveTo(new DateTime(2021, 10, 14, 12, 0, 0));
            var lessons = Lesson.GetAllBetween(
                new DateTime(2021, 10, 9, 13, 0, 0),
                new DateTime(2021, 10, 11, 11, 0, 0)
            );
            Assert.AreEqual(2, lessons.Count);
            Assert.IsTrue(lessons.Contains(l1));
            Assert.IsTrue(lessons.Contains(l2));
        }
        [TestCase]
        public void GetAllBetween_existTwoMovedAndOneTargetLessonsOnDate_returnAll()
        {
            Order o = Order.CreateNew("o1");
            var l0 = Lesson.CreateNew(
                new DateTime(2021, 10, 9, 12, 0, 0), 90, o
            );
            var l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 10, 0, 0), 90, o
            );
            var l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, o
            );
            var l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 10, 0, 0), 90, o
            );
            l0.AddToSchedule();
            l1.AddToSchedule();
            l2.AddToSchedule();
            l3.AddToSchedule();
            l0.MoveTo(new DateTime(2021, 10, 8, 16, 0, 0));
            l1.MoveTo(new DateTime(2021, 10, 13, 16, 0, 0));
            l2.MoveTo(new DateTime(2021, 10, 13, 12, 0, 0));
            var lt = l3.MoveTo(new DateTime(2021, 10, 9, 18, 0, 0));

            var lessons = Lesson.GetAllBetween(
                new DateTime(2021, 10, 9, 13, 0, 0),
                new DateTime(2021, 10, 11, 11, 0, 0)
            );
            Assert.AreEqual(3, lessons.Count);
            Assert.IsTrue(lessons.Contains(l1));
            Assert.IsTrue(lessons.Contains(l2));
            Assert.IsTrue(lessons.Contains(lt));
        }

        #endregion

        #region GetScheduledByOrder Tests

        [TestCase]
        public void GetScheduledByOrder_emptyCollection_returnEmptyList()
        {
            var o1 = Order.CreateNew("o1");
            var lessons = Lesson.GetScheduledByOrder(o1);
            Assert.AreEqual(0, lessons.Count);
        }
        [TestCase]
        public void GetScheduledByOrder_existTwoNonActive_returnEmptyList()
        {
            var o1 = Order.CreateNew("o1");
            var o2 = Order.CreateNew("o2");
            var l0 = Lesson.CreateNew(
                new DateTime(2021, 10, 9, 12, 0, 0), 90, o1
            );
            var l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 10, 0, 0), 90, o2
            );
            var l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, o1
            );
            var l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 10, 0, 0), 90, o2
            );

            var lessons = Lesson.GetScheduledByOrder(o1);
            Assert.AreEqual(0, lessons.Count);
        }
        [TestCase]
        public void GetScheduledByOrder_existTwoActive_returnBoth()
        {
            var o1 = Order.CreateNew("o1");
            var o2 = Order.CreateNew("o2");
            var l0 = Lesson.CreateNew(
                new DateTime(2021, 10, 9, 12, 0, 0), 90, o1
            );
            var l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 10, 0, 0), 90, o2
            );
            var l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, o1
            );
            var l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 10, 0, 0), 90, o2
            );
            l0.AddToSchedule();
            l1.AddToSchedule();
            l2.AddToSchedule();
            l3.AddToSchedule();

            var lessons = Lesson.GetScheduledByOrder(o1);
            Assert.AreEqual(2, lessons.Count);
            Assert.IsTrue(lessons.Contains(l0));
            Assert.IsTrue(lessons.Contains(l2));
        }
        [TestCase]
        public void GetScheduledByOrder_existTwoCancelled_returnEmptyList()
        {
            var o1 = Order.CreateNew("o1");
            var o2 = Order.CreateNew("o2");
            var l0 = Lesson.CreateNew(
                new DateTime(2021, 10, 9, 12, 0, 0), 90, o1
            );
            var l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 10, 0, 0), 90, o2
            );
            var l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, o1
            );
            var l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 10, 0, 0), 90, o2
            );
            l0.AddToSchedule();
            l1.AddToSchedule();
            l2.AddToSchedule();
            l3.AddToSchedule();
            l0.Cancel();
            l1.Cancel();
            l2.Cancel();
            l3.Cancel();

            var lessons = Lesson.GetScheduledByOrder(o1);
            Assert.AreEqual(0, lessons.Count);
        }
        [TestCase]
        public void GetScheduledByOrder_existTwoCompleted_returnBoth()
        {
            var o1 = Order.CreateNew("o1");
            var o2 = Order.CreateNew("o2");
            var l0 = Lesson.CreateNew(
                new DateTime(2021, 10, 9, 12, 0, 0), 90, o1
            );
            var l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 10, 0, 0), 90, o2
            );
            var l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, o1
            );
            var l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 10, 0, 0), 90, o2
            );
            l0.AddToSchedule();
            l1.AddToSchedule();
            l2.AddToSchedule();
            l3.AddToSchedule();
            l0.Complete();
            l1.Complete();
            l2.Complete();
            l3.Complete();

            var lessons = Lesson.GetScheduledByOrder(o1);
            Assert.AreEqual(2, lessons.Count);
            Assert.IsTrue(lessons.Contains(l0));
            Assert.IsTrue(lessons.Contains(l2));
        }
        [TestCase]
        public void GetScheduledByOrder_existTwoMoved_returnBothTargets()
        {
            var o1 = Order.CreateNew("o1");
            var o2 = Order.CreateNew("o2");
            var l0 = Lesson.CreateNew(
                new DateTime(2021, 10, 9, 12, 0, 0), 90, o1
            );
            var l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 10, 0, 0), 90, o2
            );
            var l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, o1
            );
            var l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 10, 0, 0), 90, o2
            );
            l0.AddToSchedule();
            l1.AddToSchedule();
            l2.AddToSchedule();
            l3.AddToSchedule();
            var lt0 = l0.MoveTo(new DateTime(2021, 10, 14, 12, 0, 0));
            l1.MoveTo(new DateTime(2021, 10, 15, 10, 0, 0));
            var lt2 = l2.MoveTo(new DateTime(2021, 10, 15, 12, 0, 0));
            l3.MoveTo(new DateTime(2021, 10, 17, 10, 0, 0));

            var lessons = Lesson.GetScheduledByOrder(o1);
            Assert.AreEqual(2, lessons.Count);
            Assert.IsTrue(lessons.Contains(lt0));
            Assert.IsTrue(lessons.Contains(lt2));
        }

        #endregion

        #region GetAllByOrder Tests

        [TestCase]
        public void GetAllByOrder_emptyCollection_returnEmptyList()
        {
            var o1 = Order.CreateNew("o1");
            var lessons = Lesson.GetAllByOrder(o1);
            Assert.AreEqual(0, lessons.Count);
        }
        [TestCase]
        public void GetAllByOrder_existTwoNonActive_returnBoth()
        {
            var o1 = Order.CreateNew("o1");
            var o2 = Order.CreateNew("o2");
            var l0 = Lesson.CreateNew(
                new DateTime(2021, 10, 9, 12, 0, 0), 90, o1
            );
            var l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 10, 0, 0), 90, o2
            );
            var l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, o1
            );
            var l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 10, 0, 0), 90, o2
            );

            var lessons = Lesson.GetAllByOrder(o1);
            Assert.AreEqual(2, lessons.Count);
            Assert.IsTrue(lessons.Contains(l0));
            Assert.IsTrue(lessons.Contains(l2));
        }
        [TestCase]
        public void GetAllByOrder_existTwoActive_returnBoth()
        {
            var o1 = Order.CreateNew("o1");
            var o2 = Order.CreateNew("o2");
            var l0 = Lesson.CreateNew(
                new DateTime(2021, 10, 9, 12, 0, 0), 90, o1
            );
            var l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 10, 0, 0), 90, o2
            );
            var l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, o1
            );
            var l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 10, 0, 0), 90, o2
            );
            l0.AddToSchedule();
            l1.AddToSchedule();
            l2.AddToSchedule();
            l3.AddToSchedule();

            var lessons = Lesson.GetAllByOrder(o1);
            Assert.AreEqual(2, lessons.Count);
            Assert.IsTrue(lessons.Contains(l0));
            Assert.IsTrue(lessons.Contains(l2));
        }
        [TestCase]
        public void GetAllByOrder_existTwoCancelled_returnBoth()
        {
            var o1 = Order.CreateNew("o1");
            var o2 = Order.CreateNew("o2");
            var l0 = Lesson.CreateNew(
                new DateTime(2021, 10, 9, 12, 0, 0), 90, o1
            );
            var l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 10, 0, 0), 90, o2
            );
            var l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, o1
            );
            var l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 10, 0, 0), 90, o2
            );
            l0.AddToSchedule();
            l1.AddToSchedule();
            l2.AddToSchedule();
            l3.AddToSchedule();
            l0.Cancel();
            l1.Cancel();
            l2.Cancel();
            l3.Cancel();

            var lessons = Lesson.GetAllByOrder(o1);
            Assert.AreEqual(2, lessons.Count);
            Assert.IsTrue(lessons.Contains(l0));
            Assert.IsTrue(lessons.Contains(l2));
        }
        [TestCase]
        public void GetAllByOrder_existTwoCompleted_returnBoth()
        {
            var o1 = Order.CreateNew("o1");
            var o2 = Order.CreateNew("o2");
            var l0 = Lesson.CreateNew(
                new DateTime(2021, 10, 9, 12, 0, 0), 90, o1
            );
            var l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 10, 0, 0), 90, o2
            );
            var l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, o1
            );
            var l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 10, 0, 0), 90, o2
            );
            l0.AddToSchedule();
            l1.AddToSchedule();
            l2.AddToSchedule();
            l3.AddToSchedule();
            l0.Complete();
            l1.Complete();
            l2.Complete();
            l3.Complete();

            var lessons = Lesson.GetAllByOrder(o1);
            Assert.AreEqual(2, lessons.Count);
            Assert.IsTrue(lessons.Contains(l0));
            Assert.IsTrue(lessons.Contains(l2));
        }
        [TestCase]
        public void GetAllByOrder_existTwoMoved_returnAllFour()
        {
            var o1 = Order.CreateNew("o1");
            var o2 = Order.CreateNew("o2");
            var l0 = Lesson.CreateNew(
                new DateTime(2021, 10, 9, 12, 0, 0), 90, o1
            );
            var l1 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 10, 0, 0), 90, o2
            );
            var l2 = Lesson.CreateNew(
                new DateTime(2021, 10, 10, 12, 0, 0), 90, o1
            );
            var l3 = Lesson.CreateNew(
                new DateTime(2021, 10, 12, 10, 0, 0), 90, o2
            );
            l0.AddToSchedule();
            l1.AddToSchedule();
            l2.AddToSchedule();
            l3.AddToSchedule();
            var lt0 = l0.MoveTo(new DateTime(2021, 10, 14, 12, 0, 0));
            l1.MoveTo(new DateTime(2021, 10, 15, 10, 0, 0));
            var lt2 = l2.MoveTo(new DateTime(2021, 10, 15, 12, 0, 0));
            l3.MoveTo(new DateTime(2021, 10, 17, 10, 0, 0));

            var lessons = Lesson.GetAllByOrder(o1);
            Assert.AreEqual(4, lessons.Count);
            Assert.IsTrue(lessons.Contains(lt0));
            Assert.IsTrue(lessons.Contains(lt2));
            Assert.IsTrue(lessons.Contains(l0));
            Assert.IsTrue(lessons.Contains(l2));
        }

        #endregion

    }
}
