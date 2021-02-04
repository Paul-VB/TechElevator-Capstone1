using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models.CustomExceptions
{
    public class InvalidTypeException : Exception
    {
        public InvalidTypeException() : base() { }
        public InvalidTypeException(string message) : base(message) { }
        public InvalidTypeException(string message, Exception inner) : base(message, inner) { }



    }
}
