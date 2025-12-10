using C1.GanttView;
using C1.WPF.GanttView;
using C1.WPF.RichTextBox;
using Localization.DataSource;
using Localization.ViewModel;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Localization.Views.TabViews
{
    /// <summary>
    /// Interaction logic for GanttViewTab.xaml
    /// </summary>
    public partial class GanttViewTab : UserControl
    {
        public GanttViewTab()
        {
            InitializeComponent();
            InitializeGanttView();
        }

        #region SetUP GanttView
        private void InitializeGanttView()
        {
            SetupTasks();
            SetupColumns();
        }

        private void SetupTasks()
        {
            var tasks = TaskFactoryGV.GetDefaultTasks(); 
            GV.Tasks.Clear();


            foreach (var task in tasks)
            {
                GV.Tasks.Add(task);
            }
        }

        private void SetupColumns()
        {
            GV.Columns.Clear();

            var columnProperties = new[]
            {
                TaskProperty.Name,
                TaskProperty.Start,
                TaskProperty.Finish,
                TaskProperty.Duration,
                TaskProperty.PercentComplete
            };

            foreach (var prop in columnProperties)
            {
                GV.Columns.Add(new TaskPropertyColumn
                {
                    Property = prop,
                    Visible = true
                });
            }


        }
        #endregion
    }
}