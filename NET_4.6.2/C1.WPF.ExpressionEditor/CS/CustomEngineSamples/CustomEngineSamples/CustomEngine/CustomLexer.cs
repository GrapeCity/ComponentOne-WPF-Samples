using C1.ExpressionEditor.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomEngineSamples
{
    class CustomLexer : IExpressionLexer
    {
        public void AddFunction(List<ExpressionItem> items, Func<List<object>, object> function, int minArgsCount, int maxArgsCount)
        {
            
        }

        public List<ExpressionItem> GetExpressionItems()
        {
            return new List<ExpressionItem>();
        }
    }
}
