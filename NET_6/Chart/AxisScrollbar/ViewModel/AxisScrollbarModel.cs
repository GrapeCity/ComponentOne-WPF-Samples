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

        public string Description => Resources.AppResources.Desc;

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

    }
}
