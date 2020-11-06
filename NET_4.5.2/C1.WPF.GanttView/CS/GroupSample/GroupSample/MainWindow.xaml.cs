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
using C1.GanttView;
using C1.WPF.GanttView;

namespace GroupSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ganttView.ProgressLine.StatusDate = new DateTime(2016, 12, 11, 0, 0, 0, 0);
            ganttView.ProgressLine.Visible = true;
            GroupSelector.ItemsSource = CreateGroupTypeItems();
            GroupSelector.SelectedValue = GroupType.Duration;
        }

        List<GroupTypeItem> CreateGroupTypeItems()
        {
            var items = new List<GroupTypeItem>();
            items.Add(new GroupTypeItem() { Name = "None Group", Value = GroupType.None });
            items.Add(new GroupTypeItem() { Name ="Task Mode", Value = GroupType.TaskMode });
            items.Add(new GroupTypeItem() { Name = "Task Complete", Value = GroupType.TaskComplete });
            items.Add(new GroupTypeItem() { Name = "Constraint Type", Value = GroupType.ConstraintType });
            items.Add(new GroupTypeItem() { Name = "Duration", Value = GroupType.Duration });
            items.Add(new GroupTypeItem() { Name = "Milestones", Value = GroupType.Milestones });
            items.Add(new GroupTypeItem() { Name = "Resource", Value = GroupType.Resource });
            items.Add(new GroupTypeItem() { Name = "Status", Value = GroupType.Status });
            return items;
        }

        private void GroupSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GroupType selectedValue = (e.AddedItems[0] as GroupTypeItem).Value;
            SummaryGroup.IsEnabled = selectedValue.Equals(GroupType.None);
            switch (selectedValue)
            {
                case GroupType.None:
                    ganttView.ClearGroup();
                    break;
                case GroupType.ConstraintType:
                    ganttView.Group(new ConstraintTypeGroup(true));
                    break;
                case GroupType.Duration:
                    ganttView.Group(new DurationGroup(true));
                    break;
                case GroupType.Milestones:
                    ganttView.Group(new MilestonesGroup(true));
                    break;
                case GroupType.Resource:
                    ganttView.Group(new ResourceGroup(true));
                    break;
                case GroupType.Status:
                    DateTime statusDate = DateTime.Today;
                    if (ganttView.ProgressLine.StatusDate.HasValue)
                        statusDate = ganttView.ProgressLine.StatusDate.Value;
                    ganttView.Group(new StatusGroup(statusDate, true));
                    break;
                case GroupType.TaskComplete:
                    ganttView.Group(new TaskCompleteGroup(true));
                    break;
                case GroupType.TaskMode:
                    ganttView.Group(new TaskModeGroup(true));
                    break;
            }
        }

        private void ShowProjectSummary_Click(object sender, RoutedEventArgs e)
        {
            ganttView.ShowProjectSummary = ShowProjectSummary.IsChecked.Value;
        }

        private void MaintainHierarchy_Click(object sender, RoutedEventArgs e)
        {
            ganttView.GroupDefinition.MaintainHierarchy = MaintainHierarchy.IsChecked.Value;
        }

        private void Indent_Click(object sender, RoutedEventArgs e)
        {
            ganttView.ClickButton(CommandButton.Indent);
        }

        private void Outdent_Click(object sender, RoutedEventArgs e)
        {
            ganttView.ClickButton(CommandButton.Outdent);
        }
    }

    class GroupTypeItem
    {
        public string Name { get; set; }
        public GroupType Value { get; set; }
    }
}
