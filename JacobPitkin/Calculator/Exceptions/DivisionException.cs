using System;
using System.Collections.Generic;
using System.Text;

namespace JacobPitkin.Exceptions
{
    class DivisionException : Exception
    {
        public DivisionException() { }

        public DivisionException(string message) : base(String.Format("There as an issue with a division operation: {0}", message))
        {

        }
    }
}
