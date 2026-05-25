using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DashboardWPF
{
    /// <summary>
    /// Interaction logic for TaskBar.xaml
    /// </summary>
    public partial class TaskBar : UserControl
    {
        public TaskBar()
        {
            InitializeComponent();
        }

        public SolidColorBrush BarColor
        {
            set
            {
                Bar.Fill = value;
            }
        }

        public double PrecentComplete
        {
            set
            {
                string precent = value.ToString("P0");
                Label.Text = precent;
                Bar.Height = Height;
                Bar.Width = Width * value;
                UpdataTextStyle(Bar.Width, Width, Label, precent);
            }
        }

        void UpdataTextStyle(double currentWidth, double maxWidth, TextBlock input,string target)
        {
            var textWidth = GetTextWidth(input, target);
            double textLeft = (maxWidth - textWidth) / 2;
            double textRight = textLeft + textWidth;
            input.TextEffects.Clear();
            if (currentWidth >= textRight)
                input.Foreground = new SolidColorBrush(Colors.White);
            else if (currentWidth <= textLeft)
                input.Foreground = new SolidColorBrush(Colors.Black);
            else
            {
                for (int i = 0; i < target.Length; i++)
                {
                    string letter = target.ElementAt(i).ToString();
                    double width = GetTextWidth(input, letter);
                    textLeft += width;
                    TextEffect effect = new TextEffect();
                    effect.Foreground = currentWidth > textLeft ? new SolidColorBrush(Colors.White) : new SolidColorBrush(Colors.Black);
                    effect.PositionStart = i;
                    effect.PositionCount = 1;
                    input.TextEffects.Add(effect);
                }
            }
        }

        double GetTextWidth(TextBlock input,string target)
        {
            return new FormattedText(target, CultureInfo.CurrentUICulture, input.FlowDirection, new Typeface(input.FontFamily, input.FontStyle, input.FontWeight, input.FontStretch), input.FontSize, input.Foreground).Width;
        }
    }
}
