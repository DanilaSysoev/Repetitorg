using Repetitorg.Core;
using Repetitorg.Core.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repetitorg.CoreTest
{
    class DummyLessonStorage : ILessonStorage
    {
        private List<Lesson> lessons;

        public DummyLessonStorage()
        {
            lessons = new List<Lesson>();
        }
        public void Add(Lesson lesson)
        {
            lessons.Add(lesson);
        }
        public IReadOnlyList<Lesson> GetAll()
        {
            return lessons;
        }
    }
}
