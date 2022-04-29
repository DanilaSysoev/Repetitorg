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

            l1.AddToSchedule();

            IList<Lesson> inter = Lesson.GetIntersectionWithScheduled(l2);
            Assert.AreEqual(0, inter.Count);
        }
        [TestCase]
        public void GetIntersectionWithScheduled_ScheduledIntersectNonScheduled_ReturnCorrectList()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 90, order);
            Lesson l2 = Lesson.CreateNew(new DateTime(2021, 10, 10, 13, 0, 0), 90, order);

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
            Lesson l1 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 90, order);
            Lesson l2 = Lesson.CreateNew(new DateTime(2021, 10, 10, 13, 0, 0), 90, order);

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
            Lesson l1 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 180, order);
            Lesson l2 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 30, 0), 60, order);

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
            Lesson l1 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 180, order);
            Lesson l2 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 30, 0), 60, order);

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
            Lesson l1 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 90, order);
            Lesson l2 = Lesson.CreateNew(new DateTime(2021, 10, 10, 14, 0, 0), 90, order);
            Lesson l3 = Lesson.CreateNew(new DateTime(2021, 10, 10, 16, 0, 0), 90, order);
            Lesson li = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 30, 0), 120, order);

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
            Lesson l1 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 90, order);
            Lesson l2 = Lesson.CreateNew(new DateTime(2021, 10, 10, 14, 0, 0), 90, order);
            Lesson l3 = Lesson.CreateNew(new DateTime(2021, 10, 10, 16, 0, 0), 90, order);
            Lesson li = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 30, 0), 120, order);

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
            Lesson l1 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 90, order);
            Lesson l2 = Lesson.CreateNew(new DateTime(2021, 10, 10, 13, 30, 0), 90, order);

            l1.AddToSchedule();

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

            l1.AddToSchedule();
            Assert.AreEqual(LessonStatus.Active, l1.Status);
        }
        [TestCase]
        public void AddToSchedule_AddOneLesson_LessonUpdating()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 90, order);

            Assert.AreEqual(0, lessons.UpdatesCount);
            l1.AddToSchedule();
            Assert.AreEqual(1, lessons.UpdatesCount);
        }
        [TestCase]
        public void AddToSchedule_AddNonIntersectionsLesson_AdditionOk()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 90, order);
            Lesson l2 = Lesson.CreateNew(new DateTime(2021, 10, 10, 14, 0, 0), 90, order);

            l1.AddToSchedule();
            l2.AddToSchedule();

            Assert.AreEqual(LessonStatus.Active, l1.Status);
            Assert.AreEqual(LessonStatus.Active, l2.Status);
        }
        [TestCase]
        public void AddToSchedule_FirstIntersectSecondLesson_ThrowsError()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 180, order);
            Lesson l2 = Lesson.CreateNew(new DateTime(2021, 10, 10, 14, 0, 0), 90, order);

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
            Lesson l1 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 180, order);
            Lesson l2 = Lesson.CreateNew(new DateTime(2021, 10, 10, 14, 0, 0), 90, order);

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
            Lesson l1 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 180, order);
            Lesson l2 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 30, 0), 90, order);

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
            Lesson l1 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 180, order);
            Lesson l2 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 30, 0), 90, order);

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
            Lesson l1 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 90, order);
            Lesson l2 = Lesson.CreateNew(new DateTime(2021, 10, 10, 14, 0, 0), 90, order);
            Lesson li = Lesson.CreateNew(new DateTime(2021, 10, 10, 13, 0, 0), 120, order);

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
            Lesson l1 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 90, order);

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
            Lesson l1 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 90, order);

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
            Lesson l1 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 90, order);

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
        public void Complete_completingActiveLesson_statusChangeToCompleted()
        {
            Order o1 = Order.CreateNew("test order 1");
            Lesson l1 = Lesson.CreateNew(new DateTime(2022, 1, 15, 12, 0, 0), 90, o1);
            l1.AddToSchedule();
            l1.Complete();
            Assert.AreEqual(LessonStatus.Completed, l1.Status);
        }
        [TestCase]
        public void Complete_completingActiveLesson_balancesIncludedOfClientsDecrease()
        {
            Order o1 = Order.CreateNew("test order 1");
            Order o2 = Order.CreateNew("test order 2");
            Client c1 = Client.CreateNew("tc1");
            Client c2 = Client.CreateNew("tc2");
            Client c3 = Client.CreateNew("tc3");
            Student s1 = Student.CreateNew("ts1", c1);
            Student s2 = Student.CreateNew("ts2", c2);
            Student s3 = Student.CreateNew("ts3", c3);
            o1.AddStudent(s1, 300000);
            o1.AddStudent(s3, 400000);
            o2.AddStudent(s2, 350000);

            Lesson l1 = Lesson.CreateNew(new DateTime(2022, 1, 15, 12, 0, 0), 90, o1);
            Lesson l2 = Lesson.CreateNew(new DateTime(2021, 10, 10, 14, 0, 0), 90, o2);
            Lesson l3 = Lesson.CreateNew(new DateTime(2021, 10, 10, 16, 0, 0), 120, o1);
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
            Client c1 = Client.CreateNew("tc1");
            Client c2 = Client.CreateNew("tc2");
            Client c3 = Client.CreateNew("tc3");
            Student s1 = Student.CreateNew("ts1", c1);
            Student s2 = Student.CreateNew("ts2", c2);
            Student s3 = Student.CreateNew("ts3", c3);
            o1.AddStudent(s1, 300000);
            o1.AddStudent(s3, 400000);
            o2.AddStudent(s2, 350000);

            Lesson l1 = Lesson.CreateNew(new DateTime(2022, 1, 15, 12, 0, 0), 90, o1);
            Lesson l2 = Lesson.CreateNew(new DateTime(2021, 10, 10, 14, 0, 0), 90, o2);
            Lesson l3 = Lesson.CreateNew(new DateTime(2021, 10, 10, 16, 0, 0), 120, o1);
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
            Client c1 = Client.CreateNew("tc1");
            Client c2 = Client.CreateNew("tc2");
            Client c3 = Client.CreateNew("tc3");
            Student s1 = Student.CreateNew("ts1", c1);
            Student s2 = Student.CreateNew("ts2", c2);
            Student s3 = Student.CreateNew("ts3", c3);
            o1.AddStudent(s1, 100);
            o1.AddStudent(s3, 200);
            o2.AddStudent(s2, 350000);

            Lesson l1 = Lesson.CreateNew(new DateTime(2022, 1, 15, 12, 0, 0), 50, o1);
            Lesson l2 = Lesson.CreateNew(new DateTime(2021, 10, 10, 14, 0, 0), 90, o2);
            Lesson l3 = Lesson.CreateNew(new DateTime(2021, 10, 10, 16, 0, 0), 100, o1);
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
            Lesson l1 = Lesson.CreateNew(new DateTime(2022, 1, 15, 12, 0, 0), 90, o1);

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
            Lesson l1 = Lesson.CreateNew(new DateTime(2022, 1, 15, 12, 0, 0), 90, o1);
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
            Lesson l1 = Lesson.CreateNew(new DateTime(2022, 1, 15, 12, 0, 0), 90, o1);
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
            Lesson l1 = Lesson.CreateNew(new DateTime(2022, 1, 15, 12, 0, 0), 90, o1);
            l1.AddToSchedule();

            var oldUpdCnt = lessons.UpdatesCount;
            l1.Complete();

            Assert.AreEqual(oldUpdCnt + 1, lessons.UpdatesCount);
        }

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
            Lesson l1 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 90, o1);
            Lesson l2 = Lesson.CreateNew(new DateTime(2021, 10, 10, 14, 0, 0), 90, o2);
            Lesson l3 = Lesson.CreateNew(new DateTime(2021, 10, 10, 16, 0, 0), 120, o1);

            var lessons = Lesson.GetScheduledOnDate(new DateTime(2022, 1, 15));
            Assert.AreEqual(0, lessons.Count);
        }
        [TestCase]
        public void GetScheduledOnDate_notExistScheduledOnGivenDate_returnEmptyList()
        {
            Order o1 = Order.CreateNew("test order 1");
            Order o2 = Order.CreateNew("test order 2");
            Lesson l1 = Lesson.CreateNew(new DateTime(2022, 1, 15, 12, 0, 0), 90, o1);
            Lesson l2 = Lesson.CreateNew(new DateTime(2021, 10, 10, 14, 0, 0), 90, o2);
            Lesson l3 = Lesson.CreateNew(new DateTime(2021, 10, 10, 16, 0, 0), 120, o1);
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
            Lesson l1 = Lesson.CreateNew(new DateTime(2022, 1, 15, 12, 0, 0), 90, o1);
            Lesson l2 = Lesson.CreateNew(new DateTime(2021, 10, 10, 14, 0, 0), 90, o2);
            Lesson l3 = Lesson.CreateNew(new DateTime(2022, 1, 15, 16, 0, 0), 120, o1);
            l1.AddToSchedule();
            l2.AddToSchedule();
            l3.AddToSchedule();

            var lessons = Lesson.GetScheduledOnDate(new DateTime(2022, 1, 15));
            Assert.AreEqual(2, lessons.Count);
            Assert.IsTrue(lessons.Contains(l1));
            Assert.IsTrue(lessons.Contains(l3));
        }


        [TestCase]
        public void RemoveFromSchedule_removeNonScheduledLesson_throwsException()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 90, order);

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
            Lesson l1 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 90, order);
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
            Lesson l1 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 90, order);
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
            Lesson l1 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 90, order);
            l1.AddToSchedule();
            l1.RemoveFromSchedule();

            Assert.AreEqual(LessonStatus.NonActive, l1.Status);
        }
        [TestCase]
        public void RemoveFromSchedule_removeActiveLesson_lessonUpdated()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 90, order);
            l1.AddToSchedule();
            var oldUpdCnt = lessons.UpdatesCount;
            l1.RemoveFromSchedule();
            Assert.AreEqual(oldUpdCnt + 1, lessons.UpdatesCount);
        }

        [TestCase]
        public void Cancel_cancelNonActiveLesson_changeStatusToCanceled()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 90, order);            
            l1.Cancel();
            Assert.AreEqual(LessonStatus.Canceled, l1.Status);
        }
        [TestCase]
        public void Cancel_cancelActiveLesson_changeStatusToCanceled()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 90, order);
            l1.AddToSchedule();
            l1.Cancel();
            Assert.AreEqual(LessonStatus.Canceled, l1.Status);
        }
        [TestCase]
        public void Cancel_cancelCompletedLesson_throwsException()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 90, order);
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
            Lesson l1 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 90, order);            
            var oldUpdCnt = lessons.UpdatesCount;
            l1.Cancel();
            Assert.AreEqual(oldUpdCnt + 1, lessons.UpdatesCount);
        }
        [TestCase]
        public void Cancel_CancelActiveLesson_lessonUpdated()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 90, order);
            l1.AddToSchedule();
            var oldUpdCnt = lessons.UpdatesCount;
            l1.Cancel();
            Assert.AreEqual(oldUpdCnt + 1, lessons.UpdatesCount);
        }

        [TestCase]
        public void Renew_renewNonActiveLesson_throwsError()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 90, order);
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
            Lesson l1 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 90, order);
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
            Lesson l1 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 90, order);
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
            Lesson l1 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 90, order);
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
            Lesson l1 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 90, order);
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
            Client c1 = Client.CreateNew("tc1");
            Client c2 = Client.CreateNew("tc2");
            Client c3 = Client.CreateNew("tc3");
            Student s1 = Student.CreateNew("ts1", c1);
            Student s2 = Student.CreateNew("ts2", c2);
            Student s3 = Student.CreateNew("ts3", c3);
            o1.AddStudent(s1, 300000);
            o1.AddStudent(s3, 400000);
            o2.AddStudent(s2, 350000);

            Lesson l1 = Lesson.CreateNew(new DateTime(2022, 1, 15, 12, 0, 0), 90, o1);
            Lesson l2 = Lesson.CreateNew(new DateTime(2021, 10, 10, 14, 0, 0), 90, o2);
            Lesson l3 = Lesson.CreateNew(new DateTime(2021, 10, 10, 16, 0, 0), 120, o1);
            l1.AddToSchedule();
            l2.AddToSchedule();
            l3.AddToSchedule();

            l1.Complete();
            l1.Renew();
            Assert.AreEqual(0, c1.BalanceInKopeks);
            Assert.AreEqual(0, c3.BalanceInKopeks);
        }

        [TestCase]
        public void Restore_restoreCanceledLesson_statusChangeToNonActive()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 90, order);
            l1.AddToSchedule();
            l1.Cancel();
            l1.Restore();
            Assert.AreEqual(LessonStatus.NonActive, l1.Status);
        }
        [TestCase]
        public void Restore_restoreCanceledLesson_lessonUpdated()
        {
            Order order = Order.CreateNew("test order");
            Lesson l1 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 90, order);
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
            Lesson l1 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 90, order);
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
            Lesson l1 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 90, order);
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
            Lesson l1 = Lesson.CreateNew(new DateTime(2021, 10, 10, 12, 0, 0), 90, order);
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
    }
}
