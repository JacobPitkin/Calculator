using System;
using JacobPitkin.Calculator;

namespace JacobPitkin
{
    class Program
    {
        static void Main(string[] args)
        {
            Calculator.Calculator calc = new Calculator.Calculator();
            //Console.WriteLine(calc.Evaluate("1+1"));
            //Console.WriteLine(calc.Evaluate("3.5 /4.2"));
            //Console.WriteLine(calc.Evaluate("(2 + 1) - (-4.9 * 2.2)"));
            //Console.WriteLine(calc.Evaluate("-10 + 4"));
            //Console.WriteLine(calc.Evaluate("3*(-6.6 - (2 + 1))"));
            //Console.WriteLine(calc.Evaluate("3*S"));
            //Console.WriteLine(calc.Evaluate("1.1+1.1.1"));
            //Console.WriteLine(calc.Evaluate("(1+1"));
            //Console.WriteLine(calc.Evaluate("1+1)"));
            //Console.WriteLine(calc.Evaluate("(1+(2)"));

            while (true)
            {
                Console.Write("Please enter an expression to evaluate: ");
                string userInput = Console.ReadLine();

                if (userInput.ToUpper().Equals("EXIT")) return;

                Console.WriteLine(string.Format("Result: {0}", calc.Evaluate(userInput)));
            }
        }
    }
}
