using Repetitorg.Core;
using Repetitorg.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repetitorg.CoreTest
{
    class DummyLessonStorage : IStorage<Lesson>
    {
        private List<Lesson> lessons;
        public int UpdatesCount { get; private set; }

        public DummyLessonStorage()
        {
            lessons = new List<Lesson>();
            UpdatesCount = 0;
        }
        public void Add(Lesson lesson)
        {
            lessons.Add(lesson);
        }
        public IReadOnlyList<Lesson> GetAll()
        {
            return lessons;
        }

        public void Update(Lesson entity)
        {
            UpdatesCount += 1;
        }

        public void Remove(Lesson entity)
        {
            lessons.Remove(entity);
        }

        public IReadOnlyList<Lesson> Filter(Predicate<Lesson> predicate)
        {
            return (from lesson in lessons
                    where predicate(lesson)
                    select lesson).ToList();
        }
    }
}
