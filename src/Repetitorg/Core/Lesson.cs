using Repetitorg.Core.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repetitorg.Core
{
    public class Lesson : StorageWrapper<Lesson>
    {
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

        private Lesson(DateTime dateTime, int lengthInMinutes, Order order)
        {
            DateTime = dateTime;
            LengthInMinutes = lengthInMinutes;
            Order = order;
            Status = LessonStatus.NonActive;
        }

        public static Lesson CreateNew(DateTime dateTime, int lengthInMinutes, Order order)
        {
            new Checker()
                .Add(v => v <= 0, lengthInMinutes, "Lesson length can't be non positive\n")
                .AddNull(order, "Order can't be null.\n")
                .Check();

            Lesson lesson = new Lesson(dateTime, lengthInMinutes, order);
            storage.Add(lesson);
            return lesson;
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

        public static void AddToSchedule(Lesson lesson)
        {
            new Checker()
                .AddNull(lesson, "Lesson can't be null.\n")
                .Check();

            lesson.Status = LessonStatus.Active;
            storage.Update(lesson);
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
