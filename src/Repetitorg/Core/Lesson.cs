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
               (les.DateTime >= lesson.DateTime &&
                les.DateTime < lesson.DateTime.AddMinutes(lesson.LengthInMinutes) ||
                lesson.DateTime >= les.DateTime &&
                lesson.DateTime < les.DateTime.AddMinutes(les.LengthInMinutes)) &&
               !les.Equals(lesson)
            );
        }
        public static void AddToSchedule(Lesson lesson)
        {
            lesson.Status = LessonStatus.Active;
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
