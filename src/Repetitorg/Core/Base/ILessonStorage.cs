using System;
using System.Collections.Generic;
using System.Text;

namespace Repetitorg.Core.Base
{
    public interface ILessonStorage
    {
        IReadOnlyList<Lesson> GetAll();
        void Add(Lesson lesson);
    }
}
