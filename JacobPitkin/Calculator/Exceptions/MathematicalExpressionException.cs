using System;
using System.Collections.Generic;
using System.Text;

namespace JacobPitkin.Exceptions
{
    class MathematicalExpressionException : Exception
    {
        public MathematicalExpressionException() { }

        public MathematicalExpressionException(string expression) : base(String.Format("The given mathematical expression is invalid: {0}", expression))
        {

        }
    }
}
