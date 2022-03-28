using System;
using System.Collections.Generic;
using System.Text;

namespace Repetitorg.Core.Base
{
    public interface ITaskStorage
    {
        void Add(Task task);
        IReadOnlyList<Task> GetByDate(DateTime date);
        IReadOnlyList<Task> GetByProject(Project project);
        IReadOnlyList<Task> GetAll();
        void Remove(Task task);
        void Update(Task task);
    }
}
