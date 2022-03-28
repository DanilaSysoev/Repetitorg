using System;
using System.Collections.Generic;
using System.Text;

namespace Repetitorg.Core.Base
{
    public interface IProjectStorage
    {
        void Add(Project project);
        IReadOnlyList<Project> GetAll();
    }
}
