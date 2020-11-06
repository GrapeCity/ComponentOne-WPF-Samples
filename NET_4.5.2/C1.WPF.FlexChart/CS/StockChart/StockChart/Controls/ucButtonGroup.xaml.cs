using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StockChart
{
    /// <summary>
    /// Interaction logic for ucButtonGroup.xaml
    /// </summary>
    public partial class ucButtonGroup : UserControl
    {
        public ucButtonGroup()
        {
            InitializeComponent();
        }
        
        #region DependencyProperty
        public int SelectedIndex
        {
            get { return (int)this.GetValue(SelectedIndexProperty); }
            set { this.SetValue(SelectedIndexProperty, value); }
        }

        public static DependencyProperty SelectedIndexProperty = DependencyProperty.Register(
            "SelectedIndex", typeof(int), typeof(ucButtonGroup), new PropertyMetadata(0
                , new PropertyChangedCallback((o, e)=>
                {
                    (o as ucButtonGroup).SelectIndex();
                })
                )

            );

        #endregion

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            int index = -1;
            foreach (var item in this.stackPanel.Children)
            {
                index++;
                if (item == sender)
                {
                    this.SelectedIndex = index;
                }
            }
        }

        protected void SelectIndex()
        {
            if (this.stackPanel.Children.Count > SelectedIndex && SelectedIndex >= 0)
            {
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    var rb = (this.stackPanel.Children[SelectedIndex] as RadioButton);
                    if (rb != null)
                    {
                        rb.IsChecked = true;
                    }
                }));
            }
        }
    }


    public class RangeIndexEventArgs: EventArgs
    {
        public int RangeIndex { get; set; }
    }
}
