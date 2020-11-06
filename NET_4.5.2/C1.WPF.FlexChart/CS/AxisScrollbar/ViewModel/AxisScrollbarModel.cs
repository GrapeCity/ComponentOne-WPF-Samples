using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxisScrollbar
{
    public class AxisScrollbarModel
    {
        Random rnd = new Random();

        public List<DataItem> Data
        {
            get
            {
                var pointsCount = rnd.Next(1, 30);
                var pointsList = new List<DataItem>();
                for (DateTime date = new DateTime(DateTime.Now.Year - 3, 1, 1); date.Year < DateTime.Now.Year; date = date.AddDays(1))
                {
                    pointsList.Add(new DataItem()
                    {
                        Date = date,
                        Series1 = rnd.Next(100)
                    });
                }

                return pointsList;
            }
        }

        public string Description
        {
            get
            {
                return "Viewing large charts can be difficult in compact user interfaces. Hence, the AxisScrollbar gives you an option of adding scrollbar to the axis of the chart.\n"+ 
                        "This sample allows you to view the chart with the scrollbars on X and Y axis of the chart.";
            }
        }
    }
}
