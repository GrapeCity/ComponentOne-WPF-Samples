using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Word.Creator
{
    public partial class Advanced : UserControl
    {
        public Advanced()
        {
            InitializeComponent();

            foreach (Button btn in this.Buttons)
            {
                btn.Click += WordCodeSamples.HandleButtonClick;
            }
        }

        public IEnumerable<Button> Buttons
        {
            get 
            {
                yield return btnPaperSize;
                yield return btnTableOfContent;
                yield return btnTables;
                yield return btnTextFlow;
            }
        }
    }
}
