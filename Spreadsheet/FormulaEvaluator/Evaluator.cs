using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;


namespace FormulaEvaluator



{/// <summary>
/// public static class that cotaisn methods to evaluate integer arithmetic expressions.
/// </summary>
/// 
    public static class Evaluator
    {

        /// <summary>
        /// Takes a variable then returns its corresponding integer depanding on the method.
        /// </summary>
        /// <param name="s"> the variable </param>
        /// <returns> the corresponding integer </returns>
        public delegate int Lookup(String s);


        /// <summary>
        /// evaluates integer arithmetic expressions written using standard infix notation.
        /// </summary>
        /// <param name="exp"> the expression that willl be evaluated </param>
        /// <param name="variableEvaluator"></param>
        /// <returns> the corresponding evaluation </returns>
        public static int Evaluate(String exp, Lookup variableEvaluator)
        {
            string[] substrings = Regex.Split(exp, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");
            int results;
            int val1;
            int val2;
            String topOperator = ""; ;
            Stack<int> valueStack = new Stack<int>();
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

                    topOperator = GetTopOperator(operatorStack);
                    if (!(topOperator.Equals("(")))
                        throw new ArgumentException();
                    else operatorStack.Pop();

                    topOperator = GetTopOperator(operatorStack);
                    if (topOperator.Equals("*") || topOperator.Equals("/"))
                    {
                        if (valueStack.Count < 2)
                            throw new ArgumentException();

                        val1 = valueStack.Pop();
                        val2 = valueStack.Pop();
                        String operation = operatorStack.Pop();

                        results = multiplyOrDivideVariables(val2, val1, operation);
                        valueStack.Push(results);
                    }
                }

                if (int.TryParse(token, out int num)) // if the token was a number
                {
                    int t = num;
                    topOperator = GetTopOperator(operatorStack);

                    if (topOperator.Equals("/") || topOperator.Equals("*"))
                    {
                        val1 = valueStack.Pop();
                        String operation = operatorStack.Pop();

                        results = multiplyOrDivideVariables(t, val1, operation);
                        valueStack.Push(results);
                    }
                    else
                        valueStack.Push(t);
                }
                else // if the token was a variable
                {
                    Regex regex = new Regex("^[a-zA-Z]+[0-9]+$");
                    Match match = regex.Match(token);

                    if (match.Success) //using Regular expression to make sure that the varible is structured correctly
                    {
                        int t = variableEvaluator(token);                 
                        topOperator = GetTopOperator(operatorStack);

                        if (topOperator.Equals("/") || topOperator.Equals("*"))
                        {
                            val1 = valueStack.Pop();
                            String operation = operatorStack.Pop();

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
            {
                if (valueStack.Count != 1)
                    throw new ArgumentException();
                return valueStack.Pop();
            }
            else
            {
                // Use the last operation then return the last value in the stack
                if ((operatorStack.Count != 1) || (valueStack.Count != 2))
                    throw new ArgumentException();

                String operation = operatorStack.Pop();
                results = SubOrAddVariables(operatorStack, valueStack, operation);
                return results;
            }
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
        private static int SubOrAddVariables(Stack<String> operatorStack, Stack<int> valueStack, string operation)
        {
            if (valueStack.Count < 2)
                throw new ArgumentException("need at least two operands");

            int val1 = valueStack.Pop();
            int val2 = valueStack.Pop();
            int results = 0;

            if (operation.Equals("-"))
                results = val2 - val1;
            else
                results = val2 + val1;
            return results;
        }

        /// <summary>
        ///  Pop the value stack twice and subtract or add the numebrs depending on the operation and returns the result.
        /// </summary>
        /// <param name="operatorStack"> the operator stack </param>
        /// <param name="valueStack"> the value stack </param>
        /// <returns> the subtraction results </returns>
        private static int multiplyOrDivideVariables(int num1, int num2, string operation)
        {
            int results = 0;

            if (operation.Equals("*"))
                results = num2 * num1;
            else
            {
                if (num1 == 0)
                    throw new ArgumentException("divide by zero");
                results = num2 / num1;
            }
            return results;
        }
    }
}