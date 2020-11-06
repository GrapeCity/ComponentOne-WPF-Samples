using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockAnalysis.Partial.CustomControls
{
    public class MyBasicColorPickerPart : C1.WPF.Extended.C1BasicColorPickerPart
    {
        public MyBasicColorPickerPart()
        {
            this.ColorPicked += (o, e) =>
            {
                Command.CloseDropdownCommand cmd = new Command.CloseDropdownCommand();
                cmd.Execute(this);
            };
        }
    }
}
