using C1.WPF;
using C1.WPF.FlexGrid;
using FlexGrid101.Resources;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Data;

namespace FlexGrid101
{
    /// <summary>
    /// Interaction logic for MouseHover.xaml
    /// </summary>
    public partial class MouseHover : Page
    {
        const int ROW_COUNT = 500;
        public MouseHover()
        {
            InitializeComponent();
            Tag = AppResources.MouseHoverDescription;

            var data = new ObservableCollection<Customer>();
            for (int i = 0; i < ROW_COUNT; i++)
            {
                data.Add(new Customer(i));
            }
            var view = new MyCollectionView(data);
            using (view.DeferRefresh())
            {
                view.GroupDescriptions.Clear();
                view.GroupDescriptions.Add(new PropertyGroupDescription("Country"));
                view.GroupDescriptions.Add(new PropertyGroupDescription("Active"));
                var gd = view.GroupDescriptions[0] as PropertyGroupDescription;
                gd.Converter = new CountryInitialConverter();
            }

            // bind grids to ListCollectionView
            grid.ItemsSource = view;
        }
    }
}
