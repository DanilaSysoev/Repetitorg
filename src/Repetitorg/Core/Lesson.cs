using Repetitorg.Core.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repetitorg.Core
{
    public class Lesson : StorageWrapper<Lesson>
    {
        private const int MinutesInHour = 60;

        public DateTime DateTime { get; private set; }
        public int LengthInMinutes { get; private set; }
        public Order Order { get; private set; } 
        public LessonStatus Status { get; private set; }

        public override bool Equals(object obj)
        {
            if (!(obj is Lesson))
                return false;
            Lesson other = obj as Lesson;
            return
                DateTime == other.DateTime &&
                LengthInMinutes == other.LengthInMinutes &&
                Order.Equals(other.Order) &&
                Status == other.Status;
        }
        public override int GetHashCode()
        {
            return (((DateTime.GetHashCode()
                + LengthInMinutes.GetHashCode()) * 31
                + Order.GetHashCode()) * 31
                + Status.GetHashCode()) * 31;
        }
        public override string ToString()
        {
            return string.Format("{0}, {1}", Order.ToString(), DateTime);
        }

        private Lesson(DateTime dateTime, int lengthInMinutes, Order order)
        {
            DateTime = dateTime;
            LengthInMinutes = lengthInMinutes;
            Order = order;
            Status = LessonStatus.NonActive;
        }

        public static Lesson CreateNew(
            DateTime dateTime, int lengthInMinutes, Order order
        )
        {
            CheckConditionsForCreateNew(lengthInMinutes, order);

            Lesson lesson = new Lesson(dateTime, lengthInMinutes, order);
            storage.Add(lesson);
            return lesson;
        }
        private static void CheckConditionsForCreateNew(
            int lengthInMinutes, Order order
        )
        {
            new Checker()
                .Add(v => v <= 0,
                     lengthInMinutes,
                     "Lesson length can't be non positive\n")
                .AddNull(order,
                         "Order can't be null.\n")
                .Check();
        }

        public static IList<Lesson> GetIntersectionWithAll(Lesson lesson)
        {
            return storage.Filter(les =>
               LessonsIntersecting(les, lesson)
            );
        }
        public static IList<Lesson> GetIntersectionWithScheduled(Lesson lesson)
        {
            return storage.Filter(les =>
               LessonsIntersecting(les, lesson) &&
               les.Status == LessonStatus.Active
            );
        }

        private static bool LessonsIntersecting(Lesson l1, Lesson l2)
        {
            return (l1.DateTime >= l2.DateTime &&
                    l1.DateTime < l2.DateTime.AddMinutes(l2.LengthInMinutes) ||
                    l2.DateTime >= l1.DateTime &&
                    l2.DateTime < l1.DateTime.AddMinutes(l1.LengthInMinutes)) &&
                    !l1.Equals(l2);
        }

        public void AddToSchedule()
        {
            var inters = GetIntersectionWithScheduled(this);
            CheckConditionsForAddToSchedule(inters);

            this.Status = LessonStatus.Active;
            storage.Update(this);
        }
        private void CheckConditionsForAddToSchedule(IList<Lesson> inters)
        {
            new Checker()
                .Add(les =>
                    inters.Count > 0,
                    this,
                    this + " intersect other lessons in schedule.\n")
                .Add(les => les.Status == LessonStatus.Active,
                     this,
                     "Lesson already added to schedule.")
                .Add(les => les.Status == LessonStatus.Canceled,
                     this,
                     "Can't add to schedule canceled lesson.")
                .Add(les => les.Status == LessonStatus.Completed,
                     this,
                     "Can't add to schedule completed lesson.")
                .Check(message => new InvalidOperationException(message));
        }

        public static IList<Lesson> GetScheduledOnDate(DateTime date)
        {
            return storage.Filter(
                lesson => lesson.DateTime.Date == date
                       && (lesson.Status == LessonStatus.Active
                       || lesson.Status == LessonStatus.Completed)
            );
        }

        public void Complete()
        {
            CheckConditionsForComplete();

            Status = LessonStatus.Completed;
            foreach (var student in Order.Students)
            {
                student.Client.DecreaseBalance(
                    Order.GetCostPerHourFor(student) * LengthInMinutes / MinutesInHour
                );
            }
            storage.Update(this);
        }
        private void CheckConditionsForComplete()
        {
            new Checker()
               .Add(les => les.Status == LessonStatus.Completed,
                    this,
                    "Can't complete completed lesson")
               .Add(les => les.Status == LessonStatus.NonActive,
                    this,
                    "Can't complete non active lesson")
               .Add(les => les.Status == LessonStatus.Canceled,
                    this,
                    "Can't complete cancelled lesson")
               .Check(s => new InvalidOperationException(s));
        }

        public void RemoveFromSchedule()
        {
            CheckConditionsForRemoveFromSchedule();

            Status = LessonStatus.NonActive;
            storage.Update(this);
        }
        private void CheckConditionsForRemoveFromSchedule()
        {
            new Checker().
                Add(les => les.Status == LessonStatus.Completed,
                    this,
                    "Can't remove from schedule completed lesson.").
                Add(les => les.Status == LessonStatus.NonActive,
                    this,
                    "Can't remove from schedule non-scheduled lesson.").
                Add(les => les.Status == LessonStatus.Canceled,
                    this,
                    "Can't remove from schedule canceled lesson.").
                Check((message) => new InvalidOperationException(message));
        }

        public void Cancel()
        {
            CheckConditionsForCancel();

            Status = LessonStatus.Canceled;
            storage.Update(this);
        }
        private void CheckConditionsForCancel()
        {
            new Checker()
                .Add(les => les.Status == LessonStatus.Completed,
                     this,
                     "Can't cancel completed lesson.")
                .Check((message) => new InvalidOperationException(message));
        }

        public void Renew()
        {
            CheckConditionsForRenew();

            Status = LessonStatus.Active;
            foreach (var student in Order.Students)
            {
                student.Client.IncreaseBalance(
                    Order.GetCostPerHourFor(student) * LengthInMinutes / MinutesInHour
                );
            }
            storage.Update(this);
        }
        private void CheckConditionsForRenew()
        {
            new Checker()
                .Add(les => les.Status == LessonStatus.Canceled,
                     this, 
                     "Can't renew canceled lesson.")
                .Add(les => les.Status == LessonStatus.Active,
                     this,
                     "Can't renew active lesson.")
                .Add(les => les.Status == LessonStatus.NonActive,
                     this,
                     "Can't renew non-active lesson.")
                .Check((message) => new InvalidOperationException(message));
        }
    }

    public enum LessonStatus
    {
        NonActive,
        Active,
        Canceled,
        Moved,
        Completed
    }
}
