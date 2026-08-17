using System;
using System.Collections.Generic;
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
using DashboardModel;
using C1.WPF.FlexGrid;
using System.ComponentModel;

namespace DashboardWPF
{
    /// <summary>
    /// Interaction logic for Tasks.xaml
    /// </summary>
    public partial class Tasks : Page
    {
        public Tasks()
        {
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            All.CellFactory = new TaskCellFactory();
            InProgress.CellFactory = new TaskCellFactory();
            Completed.CellFactory = new TaskCellFactory();
            Deferred.CellFactory = new TaskCellFactory();
            Urgent.CellFactory = new TaskCellFactory();
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateCampainTask();
        }

        private void OnDateRangeChanged(object sender, DateRangeChangedEventArgs e)
        {
            DataService.GetService().DateRange = e.NewValue;
            UpdateCampainTask();
        }

        void UpdateCampainTask()
        {
            int selectedIndex = TabRoot.SelectedIndex;
            DataService.GetService().CampainTaskType = (CampainTaskType)selectedIndex;
            switch (selectedIndex)
            {
                case 0:
                    All.ItemsSource = DataService.GetService().CampainTaskCollction;
                    break;
                case 1:
                    InProgress.ItemsSource = DataService.GetService().CampainTaskCollction;
                    break;
                case 2:
                    Completed.ItemsSource = DataService.GetService().CampainTaskCollction;
                    break;
                case 3:
                    Deferred.ItemsSource = DataService.GetService().CampainTaskCollction;
                    break;
                case 4:
                    Urgent.ItemsSource = DataService.GetService().CampainTaskCollction;
                    break;
            }

            ICollectionView cv = All.CollectionView;
            if (cv != null && cv.CanGroup == true)
            {
                cv.GroupDescriptions.Clear();
                cv.GroupDescriptions.Add(new PropertyGroupDescription("AssignedTo"));
            }
        }
    }
}
