using System;
using System.Collections.Generic;
using System.Text;

namespace JacobPitkin.Calculator.Strategies
{
    class SubtractStrategy : Strategy
    {
        public SubtractStrategy(string symbol) : base(symbol) { }

        public override double Evaluate(double left, double right)
        {
            return left - right;
        }
    }
}
