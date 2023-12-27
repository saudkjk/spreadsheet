using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using System;
using System.Collections.Generic;

// testing the Formula class by Saoud Aldowaish

namespace FormulaTests
{
    [TestClass]
    public class FormulaTests
    {

        /// <summary>
        /// creating a basic formula 
        ///</summary>
        [TestMethod()]
        public void SimpleTest()
        {
            Formula f = new Formula("x1+22+0");
        }

        /// <summary>
        /// should throw exception because there is zero tokens
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestingExceptions1()
        {
            Formula f = new Formula("");
        }

        /// <summary>
        /// should throw exception because The first token is not a number, a variable, or an opening parenthesis
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestingExceptions2()
        {
            Formula f = new Formula("*1");
        }

        /// <summary>
        /// should throw exception because The last token is not a number, a variable, or an closing parenthesis
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestingExceptions3()
        {
            Formula f = new Formula("1*");
        }

        /// <summary>
        /// should throw exception because an invalid token was passed
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestingExceptions4()
        {
            Formula f = new Formula("(1)&)");
        }

        /// <summary>
        /// should throw exception because after opening parenthesis or operator next token is not a number, a variable, or an opening parenthesis
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestingExceptions5()
        {
            Formula f = new Formula("()((6");
        }

        /// <summary>
        /// should throw exception because after a number, a variable, or a closing parenthesis next token is not an operator or a closing parenthesis
        /// 
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestingExceptions6()
        {
            Formula f = new Formula("1(");
        }

        /// <summary>
        /// should throw exception because the number of closing parentheses exceeded the number of opening parentheses
        /// 
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestingExceptions7()
        {
            Formula f = new Formula("(1*2))");
        }

        /// <summary>
        /// should throw exception because the number of closing parentheses isn't equal the number of opening parentheses
        /// 
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestingExceptions8()
        {
            Formula f = new Formula("((6)");
        }

        /// <summary>
        /// should throw exception because the variable after normalizing is not a variable
        /// 
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestingExceptions9()
        {
            Formula f = new Formula("a10 + b20", x => "1", x => true);
        }

        /// <summary>
        /// should throw exception because the variable after normalizing is not a valid
        /// 
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestingExceptions10()
        {
            Formula f = new Formula("a10 + b20", x => "x1", x => false);
        }

        /// <summary>
        /// Testing if two formulas with the same details are equal
        /// 
        [TestMethod()]
        public void TestingGetHashCode()
        {
            Formula f = new Formula("a10 + b20", x => "x1", x => true);
            int hashCode1 = f.GetHashCode();
            Formula f2 = new Formula("a10 + b20", x => "x1", x => true);
            int hashCode2 = f2.GetHashCode();
            Assert.AreEqual(hashCode1, hashCode2);
        }

        /// <summary>
        /// Testing if two formulas with the same details are equal using ==
        /// 
        [TestMethod()]
        public void TestingOverloadedOperators1()
        {
            Formula f = new Formula("a10 + b20", x => "x1", x => true);
            Formula f2 = new Formula("a10 + b20", x => "x1", x => true);
            Assert.IsTrue(f == f2);
        }

        /// <summary>
        /// Testing if two null formulas are equal using == 
        /// 
        [TestMethod()]
        public void TestingOverloadedOperators2()
        {
            Formula f = null;
            Formula f2 = null;
            Assert.IsTrue(f == f2);
        }
        /// <summary>
        /// Testing if two formulas the first formula is normal and second formula is null are equal using == 
        /// 
        [TestMethod()]
        public void TestingOverloadedOperators3()
        {
            Formula f = new Formula("a10 + b20", x => "x1", x => true);
            Formula f2 = null;
            Assert.IsFalse(f == f2);
        }

        /// <summary>
        /// Testing if two formulas the first formula is null and second formula is normal are equal using == 
        /// 
        [TestMethod()]
        public void TestingOverloadedOperators4()
        {
            Formula f = null;
            Formula f2 = new Formula("a10 + b20", x => "x1", x => true);
            Assert.IsFalse(f == f2);
        }

        /// <summary>
        /// Testing if two formulas with the different details are not equals using !=
        /// 
        [TestMethod()]
        public void TestingOverloadedOperators5()
        {
            Formula f = new Formula("a10 + b20", x => "x1", x => true);
            Formula f2 = new Formula("g10 + t20", x => "q1", x => true);
            Assert.IsTrue(f != f2);
        }

        /// <summary>
        /// Testing if a normal formula and a null formula are equal using the Equals method
        /// 
        [TestMethod()]
        public void TestingEquals()
        {
            Formula f = new Formula("a10 + b20", x => "x1", x => true);
            Formula f2 = null;
            Assert.IsFalse(f.Equals(f2));
        }

        /// <summary>
        /// Testing if two normal formulas with different numbers are equal using the Equals method
        /// 
        [TestMethod()]
        public void TestingEquals2()
        {
            Formula f = new Formula("10 + 20");
            Formula f2 = new Formula("15 + 20");
            Assert.IsFalse(f.Equals(f2));
        }

        /// <summary>
        /// Testing GetVaribles if it returns the correct variables
        /// 
        [TestMethod()]
        public void TestingGetVaribles()
        {
            Formula f = new Formula("a10 + b20", x => "x1", x => true);
            HashSet<string> ExpectedVaribles = new HashSet<string>();
            ExpectedVaribles.Add("x1");
            Assert.AreEqual(ExpectedVaribles.ToString(), f.GetVariables().ToString());
        }

        /// <summary>
        /// testing evaluate using a valid formula with one number
        /// 
        [TestMethod()]
        public void TestingEvaluate1()
        {
            Formula f = new Formula("10");
            Assert.AreEqual(10.0, f.Evaluate(x => 5));
        }

        /// <summary>
        /// testing evaluate using a valid formula with all the operators test
        /// 
        [TestMethod()]
        public void TestingEvaluate2()
        {
            Formula f = new Formula("(a10 * b20)+(a20 / b30) - 10 + 10 - 10", x => "x1", x => true);
            Assert.AreEqual(16.0, f.Evaluate(x => 5));
        }

        /// <summary>
        /// testing evaluate using a valid formula with all the operators but more comlicated
        /// 
        [TestMethod()]
        public void TestingEvaluate3()
        {
            Formula f = new Formula("x1*2-1/1+1*(2-1*1)/1*x1", x => "x1", x => true);
            Assert.AreEqual(2.0, f.Evaluate(x => 1));
        }

        /// <summary>
        /// testing divding by zero 
        /// 
        [TestMethod()]
        public void TestDivideByZero()
        {
            Formula f = new Formula("5/0");
            Assert.IsTrue(f.Evaluate(x => 5) is FormulaError);
        }

        /// <summary>
        /// testing divding by zero but the varible is zero
        /// 
        [TestMethod()]
        public void TestDivideByZero2()
        {
            Formula f = new Formula("5/x1");
            Assert.IsTrue(f.Evaluate(x => 0) is FormulaError);
        }

        /// <summary>
        /// testing the failed lookup case
        /// 
        [TestMethod()]
        public void TestLookupFailing()
        {
            Formula f = new Formula("5/x1");
            Assert.IsTrue(f.Evaluate(s => { throw new ArgumentException("Unknown variable"); }) is FormulaError);
        }
    }
}
