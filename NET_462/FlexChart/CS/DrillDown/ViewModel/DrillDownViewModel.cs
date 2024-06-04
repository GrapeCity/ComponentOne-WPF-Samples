using System.Windows.Media;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DrillDown
{
    public class DrillDownViewModel
    {
        private SunburstDataLayer _sunburstLayer;
        private TreemapDataLayer _treemapLayer;
        private string _groupBy = "Country,City";
        private DrillDownManager _manager;
        private DrillDownManager _asyncDrillDownManager;

        public bool IsBusy { get; private set; }

        public DrillDownManager Manager
        {
            get
            {
                if (_manager == null)
                {
                    _manager = new DrillDownManager(DataService.Instance.GetData(500), "Amount", GroupBy)
                    {
                        Binding = "Value"
                    };

                }
                return _manager;
            }
        }

        public DrillDownManager AsyncDrillDownManager
        {
            get
            {
                if(_asyncDrillDownManager == null)
                {
                    _asyncDrillDownManager = new DrillDownManager(DataService.Instance.GetGroupedData("Country"), "YValue", "XValue")
                    {
                        Binding = "Value"
                    };
                    _asyncDrillDownManager.GroupNames.AddRange(new[] { "Country", "City", "Year", "Month", "Day" });
                }
                return _asyncDrillDownManager;
            }
        }

        public List<string> ChartTypes
        {
            get
            {
                return new List<string>() { "Column","LineSymbols", "Pie" };
            }
        }

        public Dictionary<string, string> Groups
        {
            get
            {
                var groups = new Dictionary<string, string>();
                groups.Add("Country and City", "Country,City");
                groups.Add("Country and Year", "Country,Year");
                groups.Add("Year and Country", "Year,Country");
                groups.Add("Country, City and Year", "Country,City,Year");
                groups.Add("Year, Country and City", "Year,Country,City");

                return groups;
            }
        }

        public Dictionary<string, AggregateType> Aggregates
        {
            get
            {
                var aggTypes = new Dictionary<string, AggregateType>();
                aggTypes.Add("Sum", AggregateType.Sum);
                aggTypes.Add("Count of Non-Null", AggregateType.Cnt);
                aggTypes.Add("Average", AggregateType.Avg);
                aggTypes.Add("Maximum", AggregateType.Max);
                aggTypes.Add("Minimum", AggregateType.Min);
                aggTypes.Add("Deviation", AggregateType.Rng);
                aggTypes.Add("Standard Deviation", AggregateType.Std);
                aggTypes.Add("Variance", AggregateType.Var);
                aggTypes.Add("Population Standard Deviation", AggregateType.StdPop);
                aggTypes.Add("Population variance", AggregateType.VarPop);

                return aggTypes;
            }
        }

        public string GroupBy
        {
            get { return _groupBy; }
            set
            {
                _groupBy = value;
                Manager.DrillDownPath = _groupBy;
            }
        }

        public SunburstDataLayer SunburstDataLayer
        {
            get
            {
                if (_sunburstLayer == null)
                {
                    _sunburstLayer = new SunburstDataLayer(DataService.Instance.GetSunburstData())
                    {
                        Binding = "Value",
                        BindingX = "Year,Quarter,Month",
                        CustomPalette = new List<Brush>()
                        {
                            GetBrush("#ff88bde6"),
                            GetBrush("#fffbb258"),
                            GetBrush("#ff90cd97"),
                            GetBrush("#fff6aac9"),
                            GetBrush("#ffbfa554"),
                            GetBrush("#ffbc99c7"),
                            GetBrush("#ffeddd46"),
                        }
                    };
                }
                return _sunburstLayer;
            }
        }

        public TreemapDataLayer TreemapDataLayer
        {
            get
            {
                if (_treemapLayer == null)
                {
                    _treemapLayer = new TreemapDataLayer(DataService.Instance.GetTreemapData());
                }
                return _treemapLayer;
            }
        }

        private Brush GetBrush(string clr)
        {
            var color = (Color)ColorConverter.ConvertFromString(clr);
            return new SolidColorBrush() { Color = color, Opacity = 0.7 };
        }


        public Task<IEnumerable<object>> FetchDataAsync(int drilldownLevel,object value)
        {
            IsBusy = true;
            IEnumerable<object> data = null;
            return Task.Factory.StartNew(() =>
            {
                switch (drilldownLevel)
                {
                    case 0:
                        data = DataService.Instance.GetGroupedData("Country", value, "City");
                        break;
                    case 1:
                        data = DataService.Instance.GetGroupedData("City", value, "Year");
                        break;
                    case 2:
                        data = DataService.Instance.GetGroupedData("Year", value, "Month");
                        break;
                    case 3:
                        data = DataService.Instance.GetGroupedData("Month", value, "Day");
                        break;
                    default:
                        break;
                }
                IsBusy = false;
                return data;
            });
        }

    }


}
