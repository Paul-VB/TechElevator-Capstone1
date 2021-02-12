using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models.CustomExceptions
{
    public class SalesReportsUnlockedException : Exception
    {
        public SalesReportsUnlockedException() : base() { }
        public SalesReportsUnlockedException(string message) : base(message) { }
        public SalesReportsUnlockedException(string message, Exception inner) : base(message, inner) { }

    }
}
