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

namespace Validation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // items that validate by throwing
            {
                #region ValidateThrow
                var list = new List<ProductThrow>();
                for (int i = 0; i < 20; i++)
                {
                    list.Add(new ProductThrow());
                }
                _flexThrow.ItemsSource = list;
                _msThrow.ItemsSource = _flexThrow.CollectionView;
                #endregion
            }
            // items that implement IDataErrorInfo
            {
                #region ImplementIDataErrorInfo
                var list = new List<ProductDataErrorInfo>();
                for (int i = 0; i < 20; i++)
                {
                    list.Add(new ProductDataErrorInfo());
                }
                list[0].Price = -10;
                _flexDei.ItemsSource = list;
                _msDei.ItemsSource = _flexDei.CollectionView; 
                #endregion
            }

            // items that implement INotifyDataErrorInfo
            {
                #region ImplementINotifyDataErrorInfo
                var list = new List<ProductNotifyDataErrorInfo>();
                for (int i = 0; i < 20; i++)
                {
                    list.Add(new ProductNotifyDataErrorInfo());
                }
                list[0].Price = -10;
                _flexNdei.ItemsSource = list;
                _msNdei.ItemsSource = _flexNdei.CollectionView; 
                #endregion
            }

            // items that use validation attributes (and IDataErrorInfo)
            {
                #region ValidationAttributes
                var list = new List<ProductValidationAttributes>();
                for (int i = 0; i < 20; i++)
                {
                    list.Add(new ProductValidationAttributes());
                }
                list[0].Price = -10;
                _flexAttributes.ItemsSource = list;
                _msAttributes.ItemsSource = _flexAttributes.CollectionView; 
                #endregion
            }
        }
    }
}
