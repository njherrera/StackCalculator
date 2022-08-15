using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackCalculator
{
    public class InfixToPostfix
    {

        private static readonly string operators = "-+/*^";
        private static readonly string operands = "0123456789";

        public static string ConvertNegations(string infixExpression) // converts all negative operators to "~" so we can properly evaluate them
        {
            string exprNoWhiteSpaces = infixExpression.Replace(" ", "");
            StringBuilder output = new StringBuilder();

            for (int i = 0; i < exprNoWhiteSpaces.Length; i++)
            {
                if (exprNoWhiteSpaces[i] == '-')
                {
                    if (i == 0)
                    {
                        output.Append('~'); // if the very first character of the expression is a negative sign, it must be negation
                    }
                    else if (IsOperator(exprNoWhiteSpaces[i - 1]))
                    {
                        output.Append('~'); // if there's an operator preceding the negative sign, it must be a negation
                    }
                    else if (exprNoWhiteSpaces[i - 1] == '(')
                    {
                        output.Append('~'); // if the preceding character is a (, it must be negation
                    }
                    else output.Append(exprNoWhiteSpaces[i]); // if none of these conditions are met, it must be subtractioin
                }
                else output.Append(exprNoWhiteSpaces[i]); // if current character isn't a negative sign, append and proceed
            }
            return output.ToString();
        }

        public static string ConvertParenthesisMultiplication(string negationsConvertedExpression) // converts all instances of "x(y)" to "x*(y)"
        {
            // loop through input negations converted expression
            // if current char is ( and char before is an operator, append * to stringbuilder and then append (
            // else append current char
            StringBuilder multiplicationConverted = new StringBuilder();
            for (int i = 0; i < negationsConvertedExpression.Length; i++)
            {
                if (negationsConvertedExpression[i] == '(')
                {
                    if (IsOperand(negationsConvertedExpression[i - 1]))
                    {
                        multiplicationConverted.Append('*');
                        multiplicationConverted.Append(negationsConvertedExpression[i]);
                    }
                    else multiplicationConverted.Append(negationsConvertedExpression[i]);
                }
                else multiplicationConverted.Append(negationsConvertedExpression[i]);
            }
            return multiplicationConverted.ToString();
        }

        public static string ConvertToPostfix(string expression)
        {
            string convertedExpression = ConvertParenthesisMultiplication(ConvertNegations(expression));
            Stack<char> charStack = new Stack<char>();
            StringBuilder output = new StringBuilder();

            for (int i = 0; i < convertedExpression.Length; i++)
            {
                if (convertedExpression[i] == '~')
                {
                    output.Append('~');
                }
                else if (IsOperator(convertedExpression[i]))
                {
                    while (charStack.Count > 0 && charStack.Peek() != '(')
                    {
                        if (OperatorGreaterOrEqual(charStack.Peek(), convertedExpression[i])) // if precedence of operator on top of stack is greater than or equal to precedence of incoming operator
                        {                                                                     // append operator on top of stack to the output expression  
                            output.Append(charStack.Pop());
                            output.Append(' ');
                        }
                        else break; // if incoming operator has greater or equal precedence to operator on top of stack, break from loop and push incoming operator onto stack
                    }
                    charStack.Push(convertedExpression[i]); // 
                }
                else if (convertedExpression[i] == '(')
                {
                    charStack.Push(convertedExpression[i]);
                }
                else if (convertedExpression[i] == ')')
                { // if we encounter a closing parenthesis, keep going until an opening parenthesis is reached
                    while (charStack.Count > 0 && charStack.Peek() != '(')
                    {
                        output.Append(charStack.Pop());
                        output.Append(' ');
                    }
                    if (charStack.Count != 0) // if there's only one char left on the stack here it must be (, so pop it and move on
                    {
                        charStack.Pop();
                    }
                } 
                else if (Char.IsDigit(convertedExpression[i]))
                {
                    output.Append(convertedExpression[i]);

                    // line below is how we handle multiple digit numbers - if there isn't a digit immediately following a digit, it must be a single-digit number
                    // the first condition is necessary to avoid an IndexOutOfRangeException, but also means that we'll have to trim the output of this method 
                    if (i+1 >= convertedExpression.Length || !Char.IsDigit(convertedExpression[i + 1]))
                    {
                        output.Append(' ');
                    }
                }
            }
            while (charStack.Count > 0)
            {
                output.Append(charStack.Pop());
                output.Append(' ');
            }
            return output.ToString();
        }

        private static bool IsOperator(char value)
        {
            return operators.IndexOf(value) >= 0;
        }

        private static bool IsOperand(char value)
        {
            return operands.IndexOf(value) >= 0;
        }

        private static int GetPrecedence(char op)
        {
            switch (op)
            {
                case '-':
                case '+':
                    return 1;

                case '*':
                case '/':
                    return 2;

                case '^':
                    return 3;

                default:
                    return 0; // if the char isn't an operator, return 0
            }
        }

        private static bool OperatorGreaterOrEqual(char op1, char op2)
        {
            return GetPrecedence(op1) >= GetPrecedence(op2);
        }
    }
}


