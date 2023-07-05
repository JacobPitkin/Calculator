using System;
using System.Collections.Generic;
using System.Text;

namespace JacobPitkin.Calculator.Strategies
{
    abstract class Strategy
    {
        private string _symbol;

        public Strategy(string symbol)
        {
            _symbol = symbol;
        }

        public abstract double Evaluate(double left, double right);

        public string GetSymbol()
        {
            return _symbol;
        }
    }
}
