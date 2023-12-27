
// Skeleton written by Joe Zachary for CS 3500, September 2013
// Read the entire skeleton carefully and completely before you
// do anything else!

// Version 1.1 (9/22/13 11:45 a.m.)

// Change log:
//  (Version 1.1) Repaired mistake in GetTokens
//  (Version 1.1) Changed specification of second constructor to
//                clarify description of how validation works

// (Daniel Kopta) 
// Version 1.2 (9/10/17) 

// Change log:
//  (Version 1.2) Changed the definition of equality with regards
//                to numeric tokens

// Implementation by Saoud Aldowaish

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace SpreadsheetUtilities
{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  The allowed symbols are non-negative numbers written using double-precision 
    /// floating-point syntax (without unary preceeding '-' or '+'); 
    /// variables that consist of a letter or underscore followed by 
    /// zero or more letters, underscores, or digits; parentheses; and the four operator 
    /// symbols +, -, *, and /.  
    /// 
    /// Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
    /// a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable; 
    /// and "x 23" consists of a variable "x" and a number "23".
    /// 
    /// Associated with every formula are two delegates:  a normalizer and a validator.  The
    /// normalizer is used to convert variables into a canonical form, and the validator is used
    /// to add extra restrictions on the validity of a variable (beyond the standard requirement 
    /// that it consist of a letter or underscore followed by zero or more letters, underscores,
    /// or digits.)  Their use is described in detail in the constructor and method comments.
    /// </summary>
    public class Formula
    {
        string[] formulaTokens;            // string array were each token of the formula is stored and used
        HashSet<string> noramlVariables;  // a hashset where varibles that got normalized are stored and it used for the getVaribles() method because it should return normalized varibles


        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically invalid,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer is the identity function, and the associated validator
        /// maps every string to true.  
        /// </summary>
        public Formula(String formula) :
            this(formula, s => s, s => true)
        {
        }

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically incorrect,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer and validator are the second and third parameters,
        /// respectively.  
        /// 
        /// If the formula contains a variable v such that normalize(v) is not a legal variable, 
        /// throws a FormulaFormatException with an explanatory message. 
        /// 
        /// If the formula contains a variable v such that isValid(normalize(v)) is false,
        /// throws a FormulaFormatException with an explanatory message.
        /// 
        /// Suppose that N is a method that converts all the letters in a string to upper case, and
        /// that V is a method that returns true only if a string consists of one letter followed
        /// by one digit.  Then:
        /// 
        /// new Formula("x2+y3", N, V) should succeed
        /// new Formula("x+y3", N, V) should throw an exception, since V(N("x")) is false
        /// new Formula("2x+y3", N, V) should throw an exception, since "2x+y3" is syntactically incorrect.
        /// </summary>
        public Formula(String formula, Func<string, string> normalize, Func<string, bool> isValid)
        {
            formulaTokens = GetTokens(formula).ToArray();
            if (formulaTokens.Length < 1)
                throw new FormulaFormatException("There must be at least one token");
            noramlVariables = new HashSet<string>();
            List<string> variables = new List<string>();
            int parenthesesCounter= 0;

            string firstToken = formulaTokens[0];
            string lastToken = formulaTokens[formulaTokens.Length - 1];

            if (!firstToken.Equals("(") && !IsVariable(firstToken) && !Double.TryParse(firstToken, out double d))
                throw new FormulaFormatException("The first token of an expression must be a number, a variable, or an opening parenthesis");

            if (!lastToken.Equals(")") && !IsVariable(lastToken) && !Double.TryParse(lastToken, out d))
                throw new FormulaFormatException("The last token of an expression must be a number, a variable, or a closing parenthesis");

            for (int i = 0; i < formulaTokens.Length; i++)
            {
                string t = formulaTokens[i];
                if (t.Equals("(")) parenthesesCounter++; // counting the number of opening parenthesis
                if (t.Equals(")")) parenthesesCounter--; // counting the number of closing parenthesis

                if (i + 1 < formulaTokens.Length) // final token was reached there is no next token to check
                {
                    string nextToken = formulaTokens[i + 1];
                    if (t.Equals("(") || t.Equals("+") || t.Equals("-") || t.Equals("*") || t.Equals("/"))
                        if (!nextToken.Equals("(") && !IsVariable(nextToken) && !Double.TryParse(nextToken, out d))
                            throw new FormulaFormatException("After opening parenthesis or operator next token must be a number, a variable, or an opening parenthesis");

                    if (t.Equals(")") || IsVariable(t) || Double.TryParse(t, out d))
                        if (!nextToken.Equals(")") && !nextToken.Equals("+") && !nextToken.Equals("-") && !nextToken.Equals("*") && !nextToken.Equals("/"))
                            throw new FormulaFormatException("After a number, a variable, or a closing parenthesis next token must be an operator or a closing parenthesis");
                }

                if (parenthesesCounter < 0)
                    throw new FormulaFormatException("The number of closing parentheses shouldn't exceed the number of opening parentheses");

                if (IsVariable(t))  // if the token is a varible we add it to the varibles list to normalize and validate later
                    variables.Add(t);
            }
            if (parenthesesCounter != 0)
                throw new FormulaFormatException("The number of closing parentheses isn't equal the number of opening parentheses");

            foreach (string varible in variables)
            {
                string normalVarible = normalize(varible);
                if (!IsVariable(normalVarible))
                    throw new FormulaFormatException("the normalized varible must be a varibale");
                if (!isValid(normalVarible))
                    throw new FormulaFormatException("the normalized varible must be valid");

                noramlVariables.Add(normalVarible);
            }
        }

        /// <summary>
        /// Evaluates this Formula, using the lookup delegate to determine the values of
        /// variables.  When a variable symbol v needs to be determined, it should be looked up
        /// via lookup(normalize(v)). (Here, normalize is the normalizer that was passed to 
        /// the constructor.)
        /// 
        /// For example, if L("x") is 2, L("X") is 4, and N is a method that converts all the letters 
        /// in a string to upper case:
        /// 
        /// new Formula("x+7", N, s => true).Evaluate(L) is 11
        /// new Formula("x+7").Evaluate(L) is 9
        /// 
        /// Given a variable symbol as its parameter, lookup returns the variable's value 
        /// (if it has one) or throws an ArgumentException (otherwise).
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, the value is returned.  Otherwise, a FormulaError is returned.  
        /// The Reason property of the FormulaError should have a meaningful explanation.
        ///
        /// This method should never throw an exception.
        /// </summary>
        public object Evaluate(Func<string, double> lookup)
        {
            string[] substrings = Regex.Split(this.ToString(), "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");
            double results, val1, val2;
            String topOperator = ""; ;
            Stack<double> valueStack = new Stack<double>();
            Stack<String> operatorStack = new Stack<String>();

            // removing white space
            for (int i = 0; i < substrings.Length; i++)
            {
                String trimmedArray = substrings[i].Trim();
                substrings[i] = trimmedArray;
            }

            foreach (string token in substrings)
            {
                if (token.Equals(""))
                    continue;
                if (token.Equals("+") || token.Equals("-"))
                {
                    topOperator = GetTopOperator(operatorStack);
                    if (topOperator.Equals("-") || topOperator.Equals("+"))
                    {
                        results = SubOrAddVariables(operatorStack, valueStack, topOperator);
                        operatorStack.Pop();
                        valueStack.Push(results);
                    }
                    operatorStack.Push(token); // pushing the operation to the stack if not used
                }

                if (token.Equals("*") || token.Equals("/") || token.Equals("("))
                    operatorStack.Push(token);

                if (token.Equals(")"))
                {
                    topOperator = GetTopOperator(operatorStack);
                    if (topOperator.Equals("-") || topOperator.Equals("+"))
                    {
                        results = SubOrAddVariables(operatorStack, valueStack, topOperator);
                        operatorStack.Pop();
                        valueStack.Push(results);
                    }
                    operatorStack.Pop();
                    topOperator = GetTopOperator(operatorStack);
                    if (topOperator.Equals("*") || topOperator.Equals("/"))
                    {
                        val1 = valueStack.Pop();
                        val2 = valueStack.Pop();
                        String operation = operatorStack.Pop();
                        if (val1 == 0)
                            return new FormulaError("Can't divide by zero");
                        results = multiplyOrDivideVariables(val1, val2, operation);
                        valueStack.Push(results);
                    }
                }

                if (double.TryParse(token, out double num)) // if the token was a number
                {
                    double t = num;
                    topOperator = GetTopOperator(operatorStack);
                    if (topOperator.Equals("/") || topOperator.Equals("*"))
                    {
                        val1 = valueStack.Pop();
                        String operation = operatorStack.Pop();
                        if (t == 0)
                            return new FormulaError("Can't divide by zero");
                        results = multiplyOrDivideVariables(t, val1, operation);
                        valueStack.Push(results);
                    }
                    else
                        valueStack.Push(t);
                }
                else // if the token was a variable
                {
                    if (IsVariable(token)) //using Regular expression to make sure that the varible is structured correctly
                    {
                            double t;
                        try
                        {
                            t = lookup(token);
                        }
                        catch (ArgumentException)
                        {
                            return new FormulaError("lookup failed to find the varible");
                        }
                        topOperator = GetTopOperator(operatorStack);
                        if (topOperator.Equals("/") || topOperator.Equals("*"))
                        {
                            val1 = valueStack.Pop();
                            String operation = operatorStack.Pop();
                            if (t == 0)
                                return new FormulaError("Can't divide by zero");
                            results = multiplyOrDivideVariables(t, val1, operation);
                            valueStack.Push(results);
                        }
                        else
                            valueStack.Push(t);
                    }
                }
            }
            // Return the last value in the stack if there are no operations left and only one value in the stack
            if (operatorStack.Count == 0)
                return valueStack.Pop();
            else // Use the last operation then return the last value in the stack
            {
                String operation = operatorStack.Pop();
                results = SubOrAddVariables(operatorStack, valueStack, operation);
                return results;
            }
        }

        /// <summary>
        /// Enumerates the normalized versions of all of the variables that occur in this 
        /// formula.  No normalization may appear more than once in the enumeration, even 
        /// if it appears more than once in this Formula.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x+y*z", N, s => true).GetVariables() should enumerate "X", "Y", and "Z"
        /// new Formula("x+X*z", N, s => true).GetVariables() should enumerate "X" and "Z".
        /// new Formula("x+X*z").GetVariables() should enumerate "x", "X", and "z".
        /// </summary>
        public IEnumerable<String> GetVariables()
        {
            return noramlVariables;
        }

        /// <summary>
        /// Returns a string containing no spaces which, if passed to the Formula
        /// constructor, will produce a Formula f such that this.Equals(f).  All of the
        /// variables in the string should be normalized.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x + y", N, s => true).ToString() should return "X+Y"
        /// new Formula("x + Y").ToString() should return "x+Y"
        /// </summary>
        public override string ToString()
        {
            return string.Join("",formulaTokens);
        }

        /// <summary>
        /// If obj is null or obj is not a Formula, returns false.  Otherwise, reports
        /// whether or not this Formula and obj are equal.
        /// 
        /// Two Formulae are considered equal if they consist of the same tokens in the
        /// same order.  To determine token equality, all tokens are compared as strings 
        /// except for numeric tokens and variable tokens.
        /// Numeric tokens are considered equal if they are equal after being "normalized" 
        /// by C#'s standard conversion from string to double, then back to string. This 
        /// eliminates any inconsistencies due to limited floating point precision.
        /// Variable tokens are considered equal if their normalized forms are equal, as 
        /// defined by the provided normalizer.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        ///  
        /// new Formula("x1+y2", N, s => true).Equals(new Formula("X1  +  Y2")) is true
        /// new Formula("x1+y2").Equals(new Formula("X1+Y2")) is false
        /// new Formula("x1+y2").Equals(new Formula("y2+x1")) is false
        /// new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")) is true
        /// </summary>
        public override bool Equals(object obj)
        {
            if (Object.ReferenceEquals(obj, null) || !(obj is Formula))
                return false;

            string[] objTokens = (GetTokens(obj.ToString())).ToArray();
            for (int i = 0; i < objTokens.Length; i++)
            {
                if (double.TryParse(objTokens[i].ToLower(), out double objDouble) && double.TryParse(formulaTokens[i].ToLower(), out double formulaDouble))
                {
                    if (objDouble != formulaDouble)
                        return false;
                }
                else if (objTokens[i].ToLower() != formulaTokens[i].ToLower())
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Reports whether f1 == f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return true.  If one is
        /// null and one is not, this method should return false.
        /// </summary>
        public static bool operator ==(Formula f1, Formula f2)
        {
            if (ReferenceEquals(f1, null) && ReferenceEquals(f2, null))
                return true;
            if ((!ReferenceEquals(f1, null) && ReferenceEquals(f2, null)) || (ReferenceEquals(f1, null) && !ReferenceEquals(f2, null)))
                return false;
            else return f1.Equals(f2);
        }

        /// <summary>
        /// Reports whether f1 != f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return false.  If one is
        /// null and one is not, this method should return true.
        /// </summary>
        public static bool operator !=(Formula f1, Formula f2)
        {
            return !(f1 == f2);
        }

        /// <summary>
        /// Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
        /// case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two 
        /// randomly-generated unequal Formulae have the same hash code should be extremely small.
        /// </summary>
        public override int GetHashCode()
        {
            string newString = "";

            for (int i = 0; i < formulaTokens.Length; i++)
            {
                string formulaToken = formulaTokens[i];
                if (double.TryParse(formulaToken, out double formulaDouble))
                    newString = newString + formulaDouble;
                else
                    newString = newString + formulaToken;
            }
            return newString.GetHashCode();
        }

        /// <summary>
        /// Given an expression, enumerates the tokens that compose it.  Tokens are left paren;
        /// right paren; one of the four operator symbols; a string consisting of a letter or underscore
        /// followed by zero or more letters, digits, or underscores; a double literal; and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }
        }

        /// <summary>
        /// returns true if s is a varible using a regular expression
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static bool IsVariable(string s)
        {
            return Regex.IsMatch(s, @"[a-zA-Z_](?:[a-zA-Z_]|\d)*");
        }

        /// <summary>
        /// Checks if the operation stack is empty if not it returns the top operator in the stack else it returns an empty string.
        /// </summary>
        /// <param name="operatorStack"> the operator stack </param>
        /// <returns> the top operator in the stack or an empty string </returns>
        /// 
        private static String GetTopOperator(Stack<String> operatorStack)
        {
            if (operatorStack.Count > 0)
                return operatorStack.Peek();
            else
                return "";
        }

        /// <summary>
        ///  Pop the value stack twice and subtract or add the numebrs depending on the operation and returns the result.
        /// </summary>
        /// <param name="operatorStack"> the operator stack </param>
        /// <param name="valueStack"> the value stack </param>
        /// <returns> the subtraction results </returns>
        private static double SubOrAddVariables(Stack<String> operatorStack, Stack<double> valueStack, string operation)
        {
            double val1 = valueStack.Pop();
            double val2 = valueStack.Pop();
            double results = 0;
            if (operation.Equals("-"))
                results = val2 - val1;
            else
                results = val2 + val1;
            return results;
        }

        /// <summary>
        ///  multiply or divide depending on the operation and returns the result.
        /// </summary>
        /// <param name="operatorStack"> the operator stack </param>
        /// <param name="valueStack"> the value stack </param>
        /// <returns> the subtraction results </returns>
        private static double multiplyOrDivideVariables(double num1, double num2, string operation)
        {
            double results = 0;
            if (operation.Equals("*"))
                results = num2 * num1;
            else
                results = num2 / num1;
            return results;
        }
    }

    /// <summary>
    /// Used to report syntactic errors in the argument to the Formula constructor.
    /// </summary>
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Used as a possible return value of the Formula.Evaluate method.
    /// </summary>
    public struct FormulaError
    {
        /// <summary>
        /// Constructs a FormulaError containing the explanatory reason.
        /// </summary>
        /// <param name="reason"></param>
        public FormulaError(String reason)
            : this()
        {
            Reason = reason;
        }

        /// <summary>
        ///  The reason why this FormulaError was created.
        /// </summary>
        public string Reason { get; private set; }
    }
}


