using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DragCells
{
    /// <summary>
    /// Some POCO object. This is the model.
    /// </summary>
    public class MyDataObject
    {
        public MyDataObject(int r, int c)
        {
            Name = string.Format("cell {0},{1}", r, c);
            Temperature = (r + c) * 20;
        }
        public string Name { get; set; }
        public int Temperature { get; set; }
    }
}
