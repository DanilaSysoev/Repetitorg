﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Repetitorg.Core
{
    public class Payment
    {
        public DateTime Date { get; private set; }
        public long ValueInKopeks { get; private set; }

        public Payment(DateTime date, long valueInKopeks)
        {
            Date = date;
            ValueInKopeks = valueInKopeks;
        }
    }
}
