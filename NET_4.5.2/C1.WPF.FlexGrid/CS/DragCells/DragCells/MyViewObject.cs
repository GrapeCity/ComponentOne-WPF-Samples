using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using C1.WPF.FlexGrid;
using System.Windows;
namespace DragCells
{
    /// <summary>
    /// Framework element that represents a cell. This is the view.
    /// Each cell is bound to a MyDataObject in the view.
    /// </summary>
    public class MyViewObject : StackPanel
    {
        C1FlexGrid _flex;

        public MyViewObject(C1FlexGrid flex, MyDataObject data)
        {
            _flex = flex;
            DataObject = data;

            Orientation = System.Windows.Controls.Orientation.Vertical;

            var tb1 = new TextBlock();
            tb1.Text = data.Name;
            tb1.HorizontalAlignment = HorizontalAlignment.Center;
            tb1.FontSize = 12;
            Children.Add(tb1);

            var tb2 = new TextBlock();
            tb2.Text = data.Temperature.ToString();
            tb2.HorizontalAlignment = HorizontalAlignment.Center;
            tb2.FontSize = 8;
            Children.Add(tb2);
        }
        public MyDataObject DataObject { get; set; }
    }
}
