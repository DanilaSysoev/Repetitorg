using System;
using System.Collections.Generic;
using System.Text;

namespace Repetitorg.Core.Base
{
    public interface ITasksStorage
    {
        void Save(List<Task> tasks);
        List<Task> Load();
    }
}
