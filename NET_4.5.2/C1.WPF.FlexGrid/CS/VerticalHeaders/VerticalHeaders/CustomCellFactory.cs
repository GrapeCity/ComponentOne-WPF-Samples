using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C1.WPF.FlexGrid;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace VerticalHeaders
{
    public class CustomCellFactory : CellFactory
    {
        public override void CreateColumnHeaderContent(C1FlexGrid grid, Border bdr, CellRange range)
        {
            // let base class start
            base.CreateColumnHeaderContent(grid, bdr, range);

            // rotate the content of headers for numeric and Boolean columns
            var type = grid.Columns[range.Column].DataType;
            if (type == typeof(double) || type == typeof(int) || type == typeof(bool))
            {
                
                var cellContent = bdr.Child as FrameworkElement;
                //Creating a matrix
                 Matrix matrix = new Matrix();
                 matrix.Rotate(-90);
                 cellContent.VerticalAlignment = VerticalAlignment.Center;
                //Assigning matrix to the LayoutTransform
                cellContent.LayoutTransform=new MatrixTransform(matrix);
               
            
            }
        }
    }
}
