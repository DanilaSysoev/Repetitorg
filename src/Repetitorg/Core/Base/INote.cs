using System;
using System.Collections.Generic;
using System.Text;

namespace Repetitorg.Core.Base
{
    public interface INote
    {
        string Note { get; }
        void UpdateNote(string newNote);
    }
}
