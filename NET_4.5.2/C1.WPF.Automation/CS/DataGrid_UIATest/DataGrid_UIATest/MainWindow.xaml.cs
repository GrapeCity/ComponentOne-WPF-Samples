using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using C1.WPF.DataGrid;

namespace DataGrid_UIATest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();           
            DataContext = WorkItem.Get_Data();
            btn_BeginEdit.Click += (o, e) => {
                c1datagrid.CurrentColumn = c1datagrid.Columns[2];
                c1datagrid.CurrentRow = c1datagrid.Rows[1];
                c1datagrid.BeginEdit(); 
            };
            btn_Reset.Click += (o, e) => DataContext = WorkItem.Get_Data();
            btn_GroupBy.Click += (o, e) => c1datagrid.GroupBy(new DataGridColumn[] { c1datagrid.Columns[2] });
        }
    }

    public enum StateEnum
    {
        Active,
        Resolved,
        Closed
    }

    public class WorkItem
    {
        public WorkItem(int i)
        {
            ID = i;
            Title = "Title " + i;
            State = i % 2 == 0 ? StateEnum.Active : StateEnum.Closed;
            ActivatedDate = new DateTime(2010, 3, 19).AddDays(i%3);
        }
        public int ID { get; set; }
        public string Title { get; set; }
        public DateTime ActivatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public StateEnum State { get; set; }
        public WorkItem() { }
        public static CollectionViewSource Get_Data()
        {
            var model = new CollectionViewSource();
            var data = new ObservableCollection<WorkItem>();
            Enumerable.Range(1, 10).ToList().ForEach(p1 => data.Add(new WorkItem(p1)));
            model.Source = data;
            model.SortDescriptions.Add(
                new System.ComponentModel.SortDescription(
                    "ActivatedDate", 
                    System.ComponentModel.ListSortDirection.Ascending));
            return model;
        }
    }        
}
