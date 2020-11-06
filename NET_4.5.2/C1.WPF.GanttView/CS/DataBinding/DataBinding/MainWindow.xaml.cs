using C1.GanttView;
using DataBinding.NwindDataSetTableAdapters;
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

namespace DataBinding
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private NwindDataSet c1NwindDataSet1 = new NwindDataSet();
        private TasksTableAdapter tasksTableAdapter = new TasksTableAdapter();
        private CalendarsTableAdapter calendarsTableAdapter = new CalendarsTableAdapter();
        private ResourcesTableAdapter resourcesTableAdapter = new ResourcesTableAdapter();
        private PropertiesTableAdapter propertiesTableAdapter = new PropertiesTableAdapter();

        public MainWindow()
        {
            InitializeComponent();

            this.tasksTableAdapter.Fill(c1NwindDataSet1.Tasks);
            this.resourcesTableAdapter.Fill(c1NwindDataSet1.Resources);
            this.propertiesTableAdapter.Fill(c1NwindDataSet1.Properties);
            this.calendarsTableAdapter.Fill(c1NwindDataSet1.Calendars);

            gv.Loaded += delegate
            {
                C1GanttViewStorage storage = gv.DataStorage;

                storage.CalendarStorage.Mappings.CalendarID.MappingName = "CalendarID";
                storage.CalendarStorage.Mappings.Data.MappingName = "Data";
                storage.CalendarStorage.Mappings.IdMapping.MappingName = "Id";
                storage.CalendarStorage.Mappings.Name.MappingName = "Name";
                storage.CalendarStorage.DataMember = "Calendars";
                storage.CalendarStorage.DataSource = this.c1NwindDataSet1;
                storage.PropertyStorage.Key.MappingName = "Key";
                storage.PropertyStorage.Value.MappingName = "Value";
                storage.PropertyStorage.DataMember = "Properties";
                storage.PropertyStorage.DataSource = this.c1NwindDataSet1;
                storage.ResourceStorage.Mappings.Cost.MappingName = "Cost";
                storage.ResourceStorage.Mappings.IdMapping.MappingName = "Id";
                storage.ResourceStorage.Mappings.Name.MappingName = "Name";
                storage.ResourceStorage.Mappings.Notes.MappingName = "Notes";
                storage.ResourceStorage.Mappings.ResourceID.MappingName = "ResourceID";
                storage.ResourceStorage.Mappings.ResourceType.MappingName = "ResourceType";
                storage.ResourceStorage.Mappings.UnitOfMeasure.MappingName = "UnitOfMeasure";
                storage.ResourceStorage.DataMember = "Resources";
                storage.ResourceStorage.DataSource = this.c1NwindDataSet1;

                storage.TasksStorage.Mappings.CalendarID.MappingName = "CalendarID";
                storage.TasksStorage.Mappings.ConstraintDate.MappingName = "ConstraintDate";
                storage.TasksStorage.Mappings.ConstraintType.MappingName = "ConstraintType";
                storage.TasksStorage.Mappings.CustomFields.MappingName = "CustomFields";
                storage.TasksStorage.Mappings.Deadline.MappingName = "Deadline";
                storage.TasksStorage.Mappings.Duration.MappingName = "Duration";
                storage.TasksStorage.Mappings.DurationUnits.MappingName = "DurationUnits";
                storage.TasksStorage.Mappings.Finish.MappingName = "Finish";
                storage.TasksStorage.Mappings.HideBar.MappingName = "HideBar";
                storage.TasksStorage.Mappings.IdMapping.MappingName = "Id";
                storage.TasksStorage.Mappings.Initialized.MappingName = "Initialized";
                storage.TasksStorage.Mappings.Mode.MappingName = "Mode";
                storage.TasksStorage.Mappings.Name.MappingName = "Name";
                storage.TasksStorage.Mappings.NextID.MappingName = "NextID";
                storage.TasksStorage.Mappings.Notes.MappingName = "Notes";
                storage.TasksStorage.Mappings.OutlineParentID.MappingName = "OutlineParentID";
                storage.TasksStorage.Mappings.Parts.MappingName = "Parts";
                storage.TasksStorage.Mappings.PercentComplete.MappingName = "PercentComplete";
                storage.TasksStorage.Mappings.Predecessors.MappingName = "Predecessors";
                storage.TasksStorage.Mappings.Resources.MappingName = "Resources";
                storage.TasksStorage.Mappings.Start.MappingName = "Start";
                storage.TasksStorage.Mappings.Summary.MappingName = "Summary";
                storage.TasksStorage.Mappings.TaskID.MappingName = "TaskID";
                storage.TasksStorage.DataMember = "Tasks";
                storage.TasksStorage.DataSource = this.c1NwindDataSet1;
            };

        }
    }
}
