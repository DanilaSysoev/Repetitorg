using Repetitorg.Core.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repetitorg.Core
{
    public class Lesson
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


        public static int Count
        {
            get
            {
                return lessons.GetAll().Count;
            }
        }
        public static Lesson CreateNew(DateTime dateTime, int lengthInMinutes, Order order)
        {
            new Checker()
                .Add(v => v <= 0, lengthInMinutes, "Lesson length can't be non positive\n")
                .AddNull(order, "Order can't be null.\n")
                .Check();

            Lesson lesson = new Lesson(dateTime, lengthInMinutes, order);
            lessons.Add(lesson);
            return lesson;
        }
        public static void InitializeStorage(ILessonStorage storage)
        {
            lessons = storage;
        }
        private static ILessonStorage lessons;
    }

    public enum LessonStatus
    {
        NonActive
    }
}
