using C1.WPF.Theming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCustomTheme
{
    public class MyCustomTheme : C1Theme
    {
        public MyCustomTheme()
            : base(typeof(MyCustomTheme).Assembly, "MyCustomTheme.Base.Part_C1Overrides.xaml", string.Empty)
        {

        }
    }
}
