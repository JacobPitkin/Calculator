using System;
using System.Collections.Generic;
using System.Text;

namespace JacobPitkin.Calculator.Strategies
{
    class AddStrategy : Strategy
    {
        public AddStrategy(string symbol) : base(symbol) { }

        public override double Evaluate(double left, double right)
        {
            return left + right;
        }
    }
}
