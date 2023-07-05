using System;
using System.Collections.Generic;
using System.Text;

namespace JacobPitkin.Calculator.Strategies
{
    interface IMathStrategy
    {
        public double Evaluate(double left, double right);
    }
}
