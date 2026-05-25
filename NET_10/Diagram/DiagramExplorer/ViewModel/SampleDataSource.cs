using C1.Chart.Standard;
using C1.WPF.Chart.Palettes;
using DiagramExplorer.Resources;
using DiagramExplorer.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DiagramExplorer.ViewModel
{
    class SampleDataSource
    {
        private ObservableCollection<ISampleItem> _allItems = new ObservableCollection<ISampleItem>();

        /// <summary>
        /// Creates an instance of sample data source.
        /// </summary>
        public SampleDataSource()
        {
            _allItems.Add(new SampleItem<Intro>() { Controls = "Direction,EdgeRouting,Palette" });

            _allItems.Add(new SampleGroup()
            {
                Name = "📁 Unbound",
                Children = new List<ISampleItem>()
                {
                    new SampleItem<FlexChartFamily>(),
                    new SampleItem<Dynamic>(){ Controls = "Direction,EdgeRouting,Palette" },
                    new SampleItem<Clock>(),
                    new SampleItem<CustomShape>(),
                }
            });


            _allItems.Add(new SampleGroup()
            {
                Name = "📁 Data Binding",
                Children = new List<ISampleItem>()
                {
                    new SampleItem<OrgChart>() { Controls = "Direction,EdgeRouting,Palette" },
                    new SampleItem<Animals>(),
                    new SampleItem<NodeTemplate>(),
                    new SampleItem<Nested>(),
                }
            });


            _allItems.Add(new SampleGroup()
            {
                Name = "🖱️ Interaction",
                Children = new List<ISampleItem>()
                {
                    new SampleItem<HitTesting>() { Controls = "Direction,EdgeRouting,Palette" },
                    new SampleItem<Selection>(),
                    new SampleItem<Tooltips>(),
                    new SampleItem<Collapsible>()  { Controls = "Direction,EdgeRouting,Palette" },
                }
            });

            _allItems.Add(new SampleGroup()
            {
                Name = "📁 Usage Scenarios",
                Children = new List<ISampleItem>()
                {
                    new SampleItem<Literature>(),
                    new SampleItem<DecisionTree>(),
                    new SampleItem<ProgrammingLanguages>(),
                    new SampleItem<FamilyTree>(),
                }
            });
        }

        /// <summary>
        /// Gets the all samples.
        /// </summary>
        public ObservableCollection<ISampleItem> AllItems => _allItems;
    }
}
