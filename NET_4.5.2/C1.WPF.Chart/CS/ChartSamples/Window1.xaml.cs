using System;
using System.ComponentModel;
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

namespace ChartSamples
{
  /// <summary>
  /// Interaction logic for Window1.xaml
  /// </summary>
  public partial class Window1 : Window
  {
    public Window1()
    {
      InitializeComponent();

      Loaded += new RoutedEventHandler(Window1_Loaded);
    }

    void Window1_Loaded(object sender, RoutedEventArgs e)
    {
      ((TreeViewItem)tv.Items[0]).IsSelected = true;
    }

    private void tv_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
      TreeViewItem tvi = tv.SelectedItem as TreeViewItem;
      if (tvi != null)
      {
        var sample = tvi.Tag as UIElement;
        if (sample == null && tvi.Items.Count > 0)
          sample = ((TreeViewItem)tvi.Items[0]).Tag as UIElement;

        sampleContainer.Child = sample;
        DescriptionAttribute da = null;
        if (sample != null)
        {
          da = sample.GetType().GetCustomAttributes(typeof(DescriptionAttribute), false).
            FirstOrDefault(o => o is DescriptionAttribute) as DescriptionAttribute;
        }

        if (da != null)
          ControlDescription.Text = da.Description;
        else
          ControlDescription.Text = "";

      }
    }
  }
}
