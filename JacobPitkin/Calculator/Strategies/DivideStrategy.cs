using System;
using System.Collections.Generic;
using System.Text;
using JacobPitkin.Exceptions;

namespace JacobPitkin.Calculator.Strategies
{
    class DivideStrategy : Strategy
    {
        public DivideStrategy(string symbol) : base(symbol) { }

        public override double Evaluate(double left, double right)
        {
            if (right == 0)
            {
                throw new DivisionException("Cannot divide by 0");
            }

            return left / right;
        }
    }
}
