using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using StackCalculator;

namespace UnitTests
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void TestNegationConversion()
        {
            string input = "3 * -2";
            string expected = "3*~2";

            Assert.AreEqual(expected, InfixToPostfix.ConvertNegations(input));
        }

        [TestMethod]
        public void TestParenthesesMultiplication()
        {
            string input = "4 + 5(20)";
            string expected = "4+5*(20)";
            
            string inputNegationsConverted = InfixToPostfix.ConvertNegations(input);

            Assert.AreEqual(expected, InfixToPostfix.ConvertParenthesisMultiplication(inputNegationsConverted));
        }

        [TestMethod]
        public void TestInfixToPostfixSimple()
        {
            string input = "3 * -2";
            string expected = "3 ~2 *";

            string InfixToPostfixConverted = InfixToPostfix.ConvertToPostfix(InfixToPostfix.ConvertParenthesisMultiplication(InfixToPostfix.ConvertNegations(input))).Trim();

            Assert.AreEqual(expected, InfixToPostfixConverted);
        }
    }
}
