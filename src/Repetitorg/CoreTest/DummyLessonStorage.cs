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
        public int AddCount { get; private set; }

        public DummyLessonStorage()
        {
            lessons = new List<Lesson>();
            UpdatesCount = 0;
            AddCount = 0;
        }
        public long Add(Lesson lesson)
        {
            lessons.Add(lesson);
            AddCount += 1;
            return lessons.Count;
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

        public IList<Lesson> Filter(Predicate<Lesson> predicate)
        {
            return (from lesson in lessons
                    where predicate(lesson)
                    select lesson).ToList();
        }
    }
}
