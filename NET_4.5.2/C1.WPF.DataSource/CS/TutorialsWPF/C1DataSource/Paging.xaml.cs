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

using C1.Data.DataSource;

namespace TutorialsWPF
{
    public partial class Paging : Window
    {
        ClientCollectionView _view;

        public Paging()
        {
            InitializeComponent();

            // Update the displayed page index and count 
            // when they change in C1DataSource.

            _view = c1DataSource1["Orders"];

            RefreshPageInfo();
            _view.PropertyChanged += delegate { RefreshPageInfo(); };
        }

        private void RefreshPageInfo()
        {
            pageInfo.Text = string.Format("{0} / {1}", _view.PageIndex + 1, _view.PageCount);
        }

        private void MoveToPrevPage(object sender, RoutedEventArgs e)
        {
            _view.MoveToPreviousPage();
        }

        private void MoveToNextPage(object sender, RoutedEventArgs e)
        {
            _view.MoveToNextPage();
        }
    }
}
