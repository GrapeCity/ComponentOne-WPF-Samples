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

using C1.LiveLinq;
using C1.Data;
using C1.Data.Transactions;

namespace Orders
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // The default ClientCache for the NORTHWNDEntities object context type
        ClientCacheBase _cache;
        // All changes in this application are made in the scope of this client transaction.
        ClientTransaction _transaction;
        // The global transaction scope.
        IDisposable _transactionScope;

        public MainWindow()
        {
            InitializeComponent();

            // Initialize the default client cache and open the global transaction scope,
            // so all changes in the application are made in a transaction.
            _cache = C1.Data.Entities.EntityClientCache.GetDefault(typeof(NORTHWNDEntities));
            _transaction = _cache.CreateTransaction();
            _transactionScope = _transaction.Scope();
        }

        void OpenWorkspace(object workspace, string title)
        {
            // Create a tab page for the selected workspace control.
            var header = new ClosableTabItemHeader { Title = title };
            var tab = new TabItem
            {
                Header = header,
                Content = workspace
            };
            tabs.SelectedIndex = tabs.Items.Add(tab);
            // Remove the created tab page when the Close button is pressed.
            header.CloseRequested += delegate
            {
                tabs.Items.Remove(tab);
            };
        }

        void EditOrders(object sender, RoutedEventArgs e)
        {
            OpenWorkspace(new EditOrders(), "Edit Orders");
        }

        void EditCountryOrders(object sender, RoutedEventArgs e)
        {
            OpenWorkspace(new EditCountryOrders(), "Orders by Country");
        }

        void AllOrders(object sender, RoutedEventArgs e)
        {
            OpenWorkspace(new AllOrders(), "All Orders");
        }

        void EditProducts(object sender, RoutedEventArgs e)
        {
            OpenWorkspace(new EditProducts(), "Products");
        }

        void SaveChanges(object sender, RoutedEventArgs e)
        {
            _cache.SaveChanges();
        }

        void CancelAllChanges(object sender, RoutedEventArgs e)
        {
            // Cancel all changes made in this application by rolling back the global transaction.
            if (_transaction.State == TransactionState.Open)
            {
                _transaction.Rollback();
                _transactionScope.Dispose();
            }
            // Open a new transaction.
            _transaction = _cache.CreateTransaction();
            _transactionScope = _transaction.Scope();
        }
    }
}
