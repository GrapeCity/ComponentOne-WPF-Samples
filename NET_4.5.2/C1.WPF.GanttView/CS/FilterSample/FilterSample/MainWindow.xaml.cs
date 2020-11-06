using C1.GanttView;
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

namespace FilterSample
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

        private void btnMissNaming_Click(object sender, EventArgs e)
        {
            AdvancedFilter filter = new AdvancedFilter();

            ConditionTaskFilter condition = new ConditionTaskFilter();
            condition.FilterField = FilterField.Name;
            condition.TestOperator = TestOperators.Equal;
            filter.Conditions.Add(condition);

            gv.ApplyFilter(filter);
        }

        private void btnMissingResource_Click(object sender, EventArgs e)
        {
            // Filter the non-summary tasks and not milestone that're missing resource
            AdvancedFilter advancedFilter = new AdvancedFilter();

            ConditionTaskFilter summaryCondition = new ConditionTaskFilter();
            summaryCondition.FilterField = FilterField.Summary;
            summaryCondition.TestOperator = TestOperators.Equal;
            summaryCondition.FilterValue = false;
            advancedFilter.Conditions.Add(summaryCondition);

            ConditionTaskFilter milestoneCondition = new ConditionTaskFilter();
            milestoneCondition.FilterField = FilterField.Milestone;
            milestoneCondition.TestOperator = TestOperators.Equal;
            milestoneCondition.FilterValue = false;
            advancedFilter.Conditions.Add(milestoneCondition);

            ConditionTaskFilter resourceCondition = new ConditionTaskFilter();
            resourceCondition.FilterField = FilterField.ResourceNames;
            resourceCondition.TestOperator = TestOperators.Equal;
            resourceCondition.FilterValue = string.Empty;
            advancedFilter.Conditions.Add(resourceCondition);

            gv.ApplyFilter(advancedFilter);
        }

        private void btnPerformWithin_Click(object sender, EventArgs e)
        {
            AdvancedFilter filter = new AdvancedFilter();

            // Filter the tasks start from 2014/03/16...
            ConditionTaskFilter startCondition = new ConditionTaskFilter();
            startCondition.FilterField = FilterField.Start;
            startCondition.TestOperator = TestOperators.IsGreaterThanOrEqualTo;
            startCondition.FilterValue = new DateTime(2014, 3, 16);
            filter.Conditions.Add(startCondition);

            // ...finish before or at 2014/04/01...
            ConditionTaskFilter finishCondition = new ConditionTaskFilter();
            finishCondition.FilterField = FilterField.Finish;
            finishCondition.TestOperator = TestOperators.IsLessThanOrEqualTo;
            finishCondition.FilterValue = new DateTime(2014, 4, 1);
            filter.Conditions.Add(finishCondition);

            // ...and are not non-summary tasks.
            ConditionTaskFilter summaryCondition = new ConditionTaskFilter();
            summaryCondition.FilterField = FilterField.Summary;
            summaryCondition.TestOperator = TestOperators.Equal;
            summaryCondition.FilterValue = false;
            filter.Conditions.Add(summaryCondition);

            gv.ApplyFilter(filter);
        }

        private void btnHaveNotes_Click(object sender, EventArgs e)
        {
            AdvancedFilter filter = new AdvancedFilter();

            ConditionTaskFilter notesCondition = new ConditionTaskFilter();
            notesCondition.FilterField = FilterField.Notes;
            notesCondition.TestOperator = TestOperators.DoesNotEqual;
            filter.Conditions.Add(notesCondition);

            gv.ApplyFilter(filter);
        }

        private void btnLateOn_Click(object sender, EventArgs e)
        {
            LateTasksFilter filter = new LateTasksFilter(new DateTime(2014, 3, 16));
            gv.ApplyFilter(filter);
        }

        private void btnCompletedByCarole_Click(object sender, EventArgs e)
        {
            AdvancedFilter advancedFilter = new AdvancedFilter();

            // Filter the tasks that those ResourceNames contain "Carole Poland"...
            ConditionTaskFilter resourceCondition = new ConditionTaskFilter();
            resourceCondition.FilterField = FilterField.ResourceNames;
            resourceCondition.TestOperator = TestOperators.Contain;
            resourceCondition.FilterValue = "Carole Poland";
            advancedFilter.Conditions.Add(resourceCondition);

            ConditionTaskFilter percentCompleteCondition = new ConditionTaskFilter();
            percentCompleteCondition.FilterField = FilterField.PercentComplete;
            percentCompleteCondition.TestOperator = TestOperators.Equal;
            percentCompleteCondition.FilterValue = 1.0;
            advancedFilter.Conditions.Add(percentCompleteCondition);

            gv.ApplyFilter(advancedFilter);
        }

        private void btnTobyOrToni_Click(object sender, EventArgs e)
        {
            AdvancedFilter advancedFilter = new AdvancedFilter();

            // Filter the tasks that those ResourceNames contain "Toby Nixon"...
            ConditionTaskFilter tobyCondition = new ConditionTaskFilter();
            tobyCondition.FilterField = FilterField.ResourceNames;
            tobyCondition.TestOperator = TestOperators.Contain;
            tobyCondition.FilterValue = "Toby Nixon";
            advancedFilter.Conditions.Add(tobyCondition);

            // ...and "Toni Poe".
            ConditionTaskFilter sharonCondition = new ConditionTaskFilter();
            // Using Or combination operator.
            sharonCondition.CombinationOperator = CombinationOperator.Or;
            sharonCondition.FilterField = FilterField.ResourceNames;
            sharonCondition.TestOperator = TestOperators.Contain;
            sharonCondition.FilterValue = "Toni Poe";
            advancedFilter.Conditions.Add(sharonCondition);

            gv.ApplyFilter(advancedFilter);
        }
    }
}
