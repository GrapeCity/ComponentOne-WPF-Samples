using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C1ExpressionEditorSample
{
    public static class Expressions
    {
        static List<string> _expressions = new List<string>();
        static Random _rnd = new Random();

        static public string GetExpression()
        {
            if (_expressions.Count == 0)
            {
                _expressions.Add("GetYear([Introduced])");
                _expressions.Add("Len([Name])");
                _expressions.Add("Iif([Price]>500, \"Price > 500\", \"Price <= 500\")");
                _expressions.Add("[Color]");
                _expressions.Add("Concat(Concat(GetYear([Introduced]), \" / \"), GetMonth([Introduced]))");
            }

            var index = _rnd.Next(0, _expressions.Count);
            return _expressions[index];
        }
    }
}
