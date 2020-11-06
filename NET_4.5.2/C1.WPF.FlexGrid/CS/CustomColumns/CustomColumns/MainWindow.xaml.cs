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

namespace CustomColumns
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        System.ComponentModel.ICollectionView _view = Product.GetProducts(200);
        public MainWindow()
        {
            InitializeComponent();
            _flex.ItemsSource = _view;
        }

        private void ShowDetail_Click(object sender, RoutedEventArgs e)
        {
            var product = GetProduct(sender);
            if (product != null)
            {
                var detail = string.Format("Product {0}, Color {1}, Rating {2}",
                    product.Name, product.Color, product.Rating);
                MessageBox.Show(detail, "Detail", MessageBoxButton.OK);
            }
        }

        private void MoveUp_Click(object sender, RoutedEventArgs e)
        {
            var product = GetProduct(sender);
            var list = _view.SourceCollection as IList<Product>;
            var index = list.IndexOf(product);
            if (index > 0)
            {
                list.RemoveAt(index);
                list.Insert(index - 1, product);
                _view.MoveCurrentToPosition(index);
                _flex.Invalidate();
            }
        }

        private void MoveDown_Click(object sender, RoutedEventArgs e)
        {
            var product = GetProduct(sender);
            var list = _view.SourceCollection as IList<Product>;
            var index = list.IndexOf(product);
            if (index < list.Count - 1)
            {
                list.RemoveAt(index);
                list.Insert(index + 1, product);
                _flex.Invalidate();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var product = GetProduct(sender);
            if (product != null)
            {
                var result = MessageBox.Show("Are you sure you want to delete this item?", "Confirm", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    var ev = _view as System.ComponentModel.IEditableCollectionView;
                    if (ev.IsEditingItem)
                    {
                        ev.CommitEdit();
                    }    
                    ev.Remove(product);
                }
            }
        }
        // get the product that is represented by a control on the grid
        Product GetProduct(object control)
        {
            FrameworkElement e = control as FrameworkElement;
            return e.DataContext as Product;
        }
    }
}
