using JacobPitkin.Calculator.Strategies;
using JacobPitkin.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace JacobPitkin.Calculator
{
    public class Calculator
    {
        Strategy[] strategies;

        public Calculator() {
            // Build out strategies array.
            // Reason for array is it gives us a priority order to loop through.
            strategies = new Strategy[] {
                new ExponentStrategy("^"),
                new MultiplyStrategy("*"),
                new DivideStrategy("/"),
                new AddStrategy("+"),
                new SubtractStrategy("-")
            };
        }

        public string Evaluate(string expression)
        {
            string filteredExpression = Regex.Replace(expression, @"\s", string.Empty);

            try
            {
                if (!ContainsValidSymbols(filteredExpression))
                {
                    Console.Write("Invalid symbols\t");
                    throw new MathematicalExpressionException(filteredExpression);
                }
                if (!HasEvenParentheses(filteredExpression))
                {
                    Console.Write("Invalid parens\t");
                    throw new MathematicalExpressionException(filteredExpression);
                }

                return ParseExpression(filteredExpression);
            }
            catch (MathematicalExpressionException exception)
            {
                return exception.Message;
            }
        }

        private Boolean ContainsValidSymbols(string expression)
        {
            return Regex.IsMatch(expression, "^([()^*/+-]|\\d(\\.\\d+)?)+$");
        }

        private Boolean HasEvenParentheses(string expression)
        {
            if (!expression.Contains("(") && !expression.Contains(")")) return true;

            int parenLayers = 0;

            foreach (char c in expression) {
                if (c.Equals('('))
                    parenLayers++;
                if (c.Equals(')'))
                    parenLayers--;
            }

            return parenLayers == 0;
        }

        private string ParseExpression(string expression)
        {
            while (expression.Contains("("))
            {
                int startIndex = expression.IndexOf("(");
                int layer = 1;

                // Find the matching closing parenthesis.
                for (int i = startIndex + 1; i < expression.Length; i++)
                {
                    if (expression.Substring(i, 1).Equals("("))
                        layer++;
                    else if (expression.Substring(i, 1).Equals(")"))
                        layer--;

                    // We've found the matching "layer" parenthesis.
                    if (layer == 0)
                    {
                        // Replace the parenthetical expression with the result.
                        // This is recursive, so we're digging down to each layer of parenthetical expression, evaluating, and bubbling back up.
                        expression = expression.Replace(expression.Substring(startIndex, i - startIndex + 1), ParseExpression(expression.Substring(startIndex + 1, i - startIndex - 1)));
                        break;
                    }
                }
            }

            // Build out list of operators for use later.
            string operators = "";
            foreach (Strategy s in strategies)
            {
                operators += s.GetSymbol();
            }

            char[] operatorCharacters = operators.ToCharArray();

            string[] expandedExpression = new string[expression.Length];
            char[] expandedCharacters = expression.ToCharArray();

            // Splitting the char array into a string array.
            for (int i = 0; i < expandedCharacters.Length; i++)
            {
                expandedExpression[i] = expandedCharacters[i].ToString();
            }

            // Concatenizing all the numbers so we can just combine near indicies when performing mathematical operations.
            for (int i = 0; i < expandedExpression.Length; i++)
            {
                if (i + 1 >= expandedExpression.Length) break;

                string left = expandedExpression[i];
                if (!Regex.IsMatch(left, "^(\\d(\\.\\d*)?)+$")) continue;

                string right = expandedExpression[i + 1];
                if (Regex.IsMatch(right, "^[0-9.]+$"))
                {
                    string[] temp = new string[expandedExpression.Length - 1];
                    Array.Copy(expandedExpression, 0, temp, 0, i + 1);
                    temp[i] = temp[i] + expandedExpression[i + 1];
                    Array.Copy(expandedExpression, i + 2, temp, i + 1, expandedExpression.Length - (i + 2));
                    expandedExpression = temp;
                    i--;
                }
            }

            if (expandedExpression[0].Equals("-")) {
                string[] temp = new string[expandedExpression.Length - 1];
                temp[0] = expandedExpression[0] + expandedExpression[1];
                Array.Copy(expandedExpression, 2, temp, 1, temp.Length - 1);
                expandedExpression = temp;
            }

            // Now we can start doing the math.
            foreach (Strategy strategy in strategies)
            {
                while (expandedExpression.Contains(strategy.GetSymbol()))
                {
                    int operatorIndex = Array.IndexOf(expandedExpression, strategy.GetSymbol());
                    int leftIndex = operatorIndex - 1;
                    int rightIndex = operatorIndex + 1;
                    string left = expandedExpression[leftIndex];

                    // Looking to see if the value to the left should actually be negative.
                    if ((leftIndex - 1 >= 0) && expandedExpression[leftIndex - 1].Equals("-") && ((leftIndex - 1 == 0) || ((leftIndex - 2 >= 0)
                        && (operatorCharacters.Any(c => expandedExpression[leftIndex - 2].Equals(c)) || expandedExpression[leftIndex - 2].Equals("(")))))
                    {
                        leftIndex--;
                        rightIndex--;
                        left = expandedExpression[leftIndex] + left;
                        string[] temporary = new string[expandedExpression.Length - 1];
                        Array.Copy(expandedExpression, 0, temporary, 0, leftIndex);
                        temporary[leftIndex] = left;
                        Array.Copy(expandedExpression, operatorIndex, temporary, leftIndex + 1, expandedExpression.Length - operatorIndex);
                        operatorIndex--;
                        expandedExpression = temporary;
                    }

                    string right = expandedExpression[rightIndex];

                    // Looking to see if the value to the right should actually be negative.
                    if (right.Equals("-") && (rightIndex + 1) < expandedExpression.Length)
                    {
                        rightIndex++;
                        right += expandedExpression[rightIndex];
                        string[] temporary = new string[expandedExpression.Length - 1];
                        Array.Copy(expandedExpression, 0, temporary, 0, operatorIndex + 1);
                        temporary[rightIndex - 1] = right;
                        Array.Copy(expandedExpression, rightIndex, temporary, operatorIndex + 1, expandedExpression.Length - rightIndex);
                        expandedExpression = temporary;
                    }

                    double value = strategy.Evaluate(Double.Parse(left), Double.Parse(right));

                    // The expression has finished and has a final value, so we're returning a single value array.
                    if (expandedExpression.Length <= 3)
                    {
                        expandedExpression = new string[] { value.ToString() };
                        break;
                    }

                    string[] temp = new string[expandedExpression.Length - 3];
                    Array.Copy(expandedExpression, 0, temp, 0, operatorIndex - 1);
                    temp[operatorIndex - 1] = value.ToString();
                    Array.Copy(expandedExpression, operatorIndex + 1, temp, operatorIndex + 1, expandedExpression.Length - 3);
                    expandedExpression = temp;
                }
            }

            return string.Join("", expandedExpression);
        }
    }
}
