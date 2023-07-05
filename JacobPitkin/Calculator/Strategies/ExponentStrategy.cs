using System;
using System.Collections.Generic;
using System.Text;

namespace JacobPitkin.Calculator.Strategies
{
    class ExponentStrategy : Strategy
    {
        public ExponentStrategy(string symbol) : base(symbol) { }

        public override double Evaluate(double left, double right)
        {
            return Math.Pow(left, right);
        }
    }
}
