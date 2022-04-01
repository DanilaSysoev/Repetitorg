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
