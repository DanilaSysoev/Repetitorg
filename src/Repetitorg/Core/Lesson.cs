using Repetitorg.Core.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repetitorg.Core
{
    public class Lesson : StorageWrapper<Lesson>
    {
        public LessonStatus Status
        {
            get
            {
                return LessonStatus.NonActive;
            }
        }

        private Lesson(DateTime dateTime, int lengthInMinutes, Order order)
        {

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
