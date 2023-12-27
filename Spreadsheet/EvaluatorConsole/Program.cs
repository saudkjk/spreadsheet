using System;
using FormulaEvaluator;
namespace EvaluatorConsole
{
    class Program
    {
        static void Main(string[] args)
        {



            Console.WriteLine(" => " + Evaluator.Evaluate("1+2", LookupDelagate));

            Console.WriteLine(" => " + Evaluator.Evaluate("(2 + A6) * 5 + 2", LookupDelagate));

            Console.WriteLine(" => " + Evaluator.Evaluate("(2 + 3) / 2 + 2", LookupDelagate));



        }


        private static int LookupDelagate(string s)
        {
            if (s.Equals("A6"))
                return 7;
            else
                return 0;

        }


    }
}








