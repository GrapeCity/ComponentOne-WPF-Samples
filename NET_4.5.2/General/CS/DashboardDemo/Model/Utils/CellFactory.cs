#if WPF
using C1.WPF.FlexGrid;
using DashboardWPF;
using System.Windows.Controls;
using System.Windows.Media;
#elif WINDOWS_UWP
using C1.Xaml.FlexGrid;
using Windows.UI.Xaml.Controls;
using DashboardUWP;
using Windows.UI;
using Windows.UI.Xaml.Media;
#endif
using System;
using System.Collections.Generic;
using System.Text;

namespace DashboardModel
{
#if !WinForms
    public class TaskCellFactory : CellFactory
    {
        public override void CreateCellContent(C1FlexGrid grid, Border bdr, CellRange rng)
        {
            base.CreateCellContent(grid, bdr, rng);
            var row = grid.Rows[rng.Row];
            var column = grid.Columns[rng.Column];
            switch (rng.Column)
            {
                case 3:
                    TextBlock date = new TextBlock();
#if WPF
                    date.Text = (row.DataItem as CampainTaskItem).DueDate.ToShortDateString();
#elif WINDOWS_UWP
                    date.Text = (row.DataItem as CampainTaskItem).DueDate.ToString("dd/mm/yyyy");
#endif
                    bdr.Child = date;
                    break;
                case 4:
                    TaskBar bar = new TaskBar();
                    bar.Height = row.ActualHeight;
                    bar.Width = column.ActualWidth;
                    bar.BarColor = new SolidColorBrush(Colors.Turquoise);
                    bar.PrecentComplete = (row.DataItem as CampainTaskItem).Complete;
                    bdr.Child = bar;
                    break;
            }
        }
    }
#endif
}
