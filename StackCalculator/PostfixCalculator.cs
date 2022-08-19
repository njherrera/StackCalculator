using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackCalculator
{
    public class PostfixCalculator
    {
        public static double CalculateFromPostfix(string postfixExpr)
        {
            string[] inputTokenized = postfixExpr.Split(' ');
            Stack<double> numberStack = new Stack<double>();

            for (int i = 0; i < inputTokenized.Length; i++)
            {
                if (inputTokenized[i][0] == '~')
                {
                    numberStack.Push(ProcessTilde(Double.Parse(inputTokenized[i].Substring(1))));
                } else if (Char.IsDigit(inputTokenized[i][0]))
                {
                    numberStack.Push(Double.Parse(inputTokenized[i]));
                } else
                {
                    double value2 = numberStack.Pop();
                    double value1 = numberStack.Pop();

                    numberStack.Push(ProcessOperator(inputTokenized[i][0], value1, value2));
                }
            }
            return numberStack.Pop();
        }
        
        public static double ProcessOperator(char op, double value1, double value2)
        {
            switch (op)
            {
                case '+':
                    return value1 + value2;

                case '-':
                    return value1 - value2;

                case '*':
                    return value1 * value2;

                case '/':
                    return value1 / value2;

                case '^':
                    return Math.Pow(value1, value2);

                default:
                    return 0;
            }
        }

        public static double ProcessTilde(double value)
        {
            return value * -1;
        }
    }
}
