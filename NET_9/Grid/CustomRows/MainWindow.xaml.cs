using C1.WPF.Grid;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace CustomRows
{
    public partial class MainWindow : Window
    {
        private Random _rand = new Random();
        private ObservableCollection<Customer> _customers;

        public MainWindow()
        {
            this.Language = XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag);
            InitializeComponent();

            _customers = Customer.GetCustomerList(10);
            grid.CellFactory = Resources["listViewCellFactoryStyle"] as GridCellFactory;
            grid.ItemsSource = _customers;

            SimulateChanges();
        }

        private async void SimulateChanges()
        {
            await Task.Delay(TimeSpan.FromMilliseconds(800));
            GenerateRandomChange();
            SimulateChanges();
        }

        private void GenerateRandomChange()
        {
            switch (_rand.Next(4))
            {
                case 0:
                    _customers.Insert(_rand.Next(_customers.Count + 1), new Customer(_rand.Next()));
                    break;
                case 1:
                    if (_customers.Count > 0)
                        _customers.RemoveAt(_rand.Next(_customers.Count));
                    break;
                case 2:
                    if (_customers.Count > 0)
                        _customers.Move(_rand.Next(_customers.Count), _rand.Next(_customers.Count));
                    break;
                case 3:
                    if (_customers.Count > 0)
                        _customers[_rand.Next(_customers.Count)] = new Customer(_rand.Next());
                    break;
            }
        }
    }

    public class ListViewCellFactoryStyle : GridCellFactory
    {
        public Style LeftCellStyle { get; set; }
        public Style CenterCellStyle { get; set; }
        public Style RightCellStyle { get; set; }

        public override void PrepareCell(GridCellType cellType, GridCellRange range, GridCellView cell, Thickness internalBorders)
        {
            base.PrepareCell(cellType, range, cell, internalBorders);
            cell.ClearValue(Control.BorderThicknessProperty);
            if (cellType == GridCellType.Cell)
            {
                if (range.Column == 0)
                {
                    cell.Style = LeftCellStyle ?? CenterCellStyle;
                }
                else if (range.Column == Grid.Columns.Count - 1)
                {
                    cell.Style = RightCellStyle ?? CenterCellStyle;
                }
                else
                {
                    cell.Style = CenterCellStyle;
                }
            }
        }
    }
}
