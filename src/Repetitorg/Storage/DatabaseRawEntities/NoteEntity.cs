﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.SQLite.DatabaseRawEntities
{
    class NoteEntity : EntityWithId
    {
        public string Text { get; set; }
    }
}
