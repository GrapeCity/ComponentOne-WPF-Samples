using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnnotationExplorer
{
    public class AnnotationViewModel
    {
        List<DataItem> _data;
        List<FinancialItem> _financialData;
        List<DataItem> _simpleData;
        Random rnd = new Random();

        public List<DataItem> Data
        {
            get
            {
                if (_data == null)
                {
                    _data = new List<DataItem>();
                    for (int i = 1; i < 51; i++)
                    {
                        _data.Add(new DataItem()
                        {
                            X = i,
                            Y = rnd.Next(10, 80)
                        });
                    }
                }

                return _data;
            }
        }

        public List<DataItem> SimpleData
        {
            get
            {
                if (_simpleData == null)
                {
                    _simpleData = new List<DataItem>();
                    _simpleData.Add(new DataItem() { X = 1, Y = 30 });
                    _simpleData.Add(new DataItem() { X = 2, Y = 20 });
                    _simpleData.Add(new DataItem() { X = 3, Y = 30 });
                    _simpleData.Add(new DataItem() { X = 4, Y = 65 });
                    _simpleData.Add(new DataItem() { X = 5, Y = 70 });
                    _simpleData.Add(new DataItem() { X = 6, Y = 60 });
                }

                return _simpleData;
            }
        }

        public List<FinancialItem> FinancialData
        {
            get
            {
                if (_financialData == null)
                {
                    _financialData = new List<FinancialItem>();
                    var current = new DateTime(DateTime.Now.Year, 1, 1);
                    for (int j = 10, k = 30; k <= 110; j += 20, k += 20)
                        for (int i = 1; i < 20; i++)
                        {
                            current = current.AddDays(1);
                            var item = new FinancialItem()
                            {
                                Date = current,
                                Close = rnd.Next(j, k) + Math.Round(rnd.NextDouble(), 2),
                                Volume = rnd.Next(0, 10)
                            };
                            item.Hight = item.Close + Math.Round(rnd.NextDouble(), 2);
                            item.Low = item.Close - Math.Round(rnd.NextDouble(), 2);
                            item.Open = rnd.Next(100) % 2 == 0 ? item.Close - rnd.Next(2) - Math.Round(rnd.NextDouble(), 2) : item.Close + rnd.Next(2) + Math.Round(rnd.NextDouble(), 2);

                            _financialData.Add(item);
                        }
                }

                return _financialData;
            }
        }
    }
}
