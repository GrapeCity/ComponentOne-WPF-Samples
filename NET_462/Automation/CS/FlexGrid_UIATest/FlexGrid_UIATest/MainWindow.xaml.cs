using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace FlexGrid_UIATest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var data = new ObservableCollection<WorkItem>();
            Enumerable.Range(1, 10).ToList().ForEach(p => data.Add(new WorkItem(p)));
            DataContext = data;
            btn_AddRow.Click += (o, e) => data.Insert(data.Count, new WorkItem(data.Count + 1));
            btn_RemoveRow.Click += (o, e) => data.RemoveAt(flexgrid.Rows.Count - 1);
            btn_SelectedIndex.Click += (o, e) => flexgrid.SelectedIndex = flexgrid.Rows.Count - 1;
            btn_Copy.Click += (o, e) => 
                { 
                    flexgrid.Copy();
                    try
                    {
                        btn_Copy.Content = Clipboard.GetText();
                    }
                    catch
                    {// Clipboard operation might fail if there are no permissions
                    }
                };
            flexgrid.ClipboardCopyMode = C1.WPF.FlexGrid.ClipboardCopyMode.ExcludeHeader;
        }
    }

    public class WorkItem
    {
        public WorkItem(int i)
        {
            ID = i;
            Title = "Title " + i;
            State = "Active";
            ActivatedDate = new DateTime(2014, 2, 14).AddDays(i);
        }
        public int ID { get; set; }
        public string Title { get; set; }
        public DateTime ActivatedDate { get; set; }
        public string State { get; set; }
        public WorkItem() { }
    }        
}
