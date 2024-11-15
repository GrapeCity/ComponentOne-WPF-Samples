﻿using System;
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

namespace PdfCreator
{
    public partial class Basic : UserControl
    {
        public Basic()
        {
            InitializeComponent();

            foreach (Button btn in this.Buttons)
            {
                btn.Click += PdfCodeSamples.HandleButtonClick;
            }
        }

        public IEnumerable<Button> Buttons
        {
            get
            {
                yield return btnGraphics;
                yield return btnImages;
                yield return btnQuotes;
                yield return btnText;
            }
        }
    }
}
