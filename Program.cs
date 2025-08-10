using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MathematicalExpression
{
    internal class Program
    {
        private readonly char[] Operators = new char[] { '+', '-', '*', '/' };

        static void Main(string[] args)
        {
            Program program = new Program();

            string strExpression1 = "3 + (2 * 4) - 5 / (1 + 1)";
            float fOutput1 = program.CalculateExpression(strExpression1);
            PrintResult(strExpression1, fOutput1);

            string strExpression2 = "6 + (3 * 5) - 8 / (2 + 2)";
            float fOutput2 = program.CalculateExpression(strExpression2);
            PrintResult(strExpression2, fOutput2);

            string strExpression3 = "5 + (6 * 2) - 9 / (3 + 0)";
            float fOutput3 = program.CalculateExpression(strExpression3);
            PrintResult(strExpression3, fOutput3);

            string strExpression4 = "6 - (3 * 5) - 8 / (2 - 2)";
            float fOutput4 = program.CalculateExpression(strExpression4);
            PrintResult(strExpression4, fOutput4);

            Console.ReadKey();
        }

        private float CalculateExpression(string strExpression)
        {
            if(strExpression.Contains("(") || strExpression.Contains(")"))
            {
                strExpression = ReplaceParenthesesWithCalculatedValue(strExpression);
            }

            if(strExpression.Contains("/"))
            {
                strExpression = ReplaceDivideWithCalculatedValue(strExpression);
            }

            return Calculate(strExpression);
        }

        private string ReplaceParenthesesWithCalculatedValue(string strExpression)
        {
            string strPattern = @"\([^\)]*\)";

            MatchCollection regexMatches = Regex.Matches(strExpression, strPattern);

            foreach(Match regexMatch in regexMatches)
            {
                string strMatch = regexMatch.Value.Replace("(", "").Replace(")", "");

                float fValue = Calculate(strMatch);

                strExpression = strExpression.Replace(regexMatch.Value, fValue.ToString());
            }

            return strExpression;
        }

        private string ReplaceDivideWithCalculatedValue(string strExpression)
        {
            string strPattern = @"\b\d{1,3}\s*/\s*\d{1,3}\b";

            MatchCollection regexMatches = Regex.Matches(strExpression, strPattern);

            foreach(Match regexMatch in regexMatches)
            {
                float fValue = Calculate(regexMatch.Value);

                strExpression = strExpression.Replace(regexMatch.Value, fValue.ToString());
            }

            return strExpression;
        }

        private float Calculate(string strInput)
        {
            List<string> liMathCharacters = new List<string>(strInput.Split(' '));

            float fValue = 0;
            char cOperator = char.MaxValue;

            for(int i = 0; i < liMathCharacters.Count; i++)
            {
                if(liMathCharacters[i].IndexOfAny(Operators) >= 0) cOperator = Convert.ToChar(liMathCharacters[i]);

                if(float.TryParse(liMathCharacters[i], out float nNumber))
                {
                    if (i == 0) fValue = nNumber;
                    else
                    {
                        switch (cOperator)
                        {
                            case '+':
                                fValue += nNumber;
                                break;
                            case '-':
                                fValue -= nNumber;
                                break;
                            case '*':
                                fValue *= nNumber;
                                break;
                            case '/':
                                fValue /= nNumber;
                                break;
                        }
                    }
                }
            }

            return fValue;
        }

        private static void PrintResult(string strExpression, float fOutput)
        {
            Console.WriteLine($"Expression: {strExpression} - Result: {fOutput}");
        }
    }
}
