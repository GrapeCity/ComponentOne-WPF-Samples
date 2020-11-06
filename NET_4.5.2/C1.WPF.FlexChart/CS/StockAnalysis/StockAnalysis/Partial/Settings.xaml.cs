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
using System.Windows.Shapes;

namespace StockAnalysis.Partial
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings(string key)
        {
            this.Key = key;
            InitializeComponent();
        }

        public void ShowDialog(Window owner)
        {
            this.Owner = owner;

            Binding forgroundBinding = new Binding();
            forgroundBinding.Path = new PropertyPath(Window.ForegroundProperty);
            forgroundBinding.Source = owner;
            BindingOperations.SetBinding(this, Window.ForegroundProperty, forgroundBinding);
            
            this.ShowDialog();
        }

        public string Key
        {
            get;
            private set;
        }

        private IEnumerable<object> itemSource;
        public IEnumerable<object> ItemSource
        {
            get
            {
                if (itemSource == null)
                {
                    IEnumerable<Data.SettingParam> settings;
                    if (ViewModel.ViewModel.Instance.Settings.TryGetValue(Key, out settings))
                    {
                        itemSource = settings;
                    }
                }
                return itemSource;

            }
        }

        private void SettingList_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.Height = e.NewSize.Height + 100;
        }
    }
}
