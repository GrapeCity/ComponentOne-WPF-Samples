using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C1.WPF.Theming.ExpressionDark;

namespace MyCustomTheme
{
    public class MyCustomExpressionDark : C1ThemeExpressionDark
    {
        public MyCustomExpressionDark()
            : base(typeof(MyCustomExpressionDark).Assembly, "MyCustomTheme.Extending.Part_ExpressionDarkOverrides.xaml")
        {

        }
    }
}
