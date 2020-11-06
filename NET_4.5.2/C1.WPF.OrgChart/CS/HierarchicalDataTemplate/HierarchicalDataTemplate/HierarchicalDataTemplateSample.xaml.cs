using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HierarchicalDataTemplateWPF
{
    /// <summary>
    /// Interaction logic for HierarchicalDataTemplateSample.xaml
    /// </summary>
    public partial class HierarchicalDataTemplateSample : UserControl
    {
        public HierarchicalDataTemplateSample()
        {
            InitializeComponent();

            // create data object
            var league = League.GetLeague();

            // show it in C1OrgChart
            _chart.Header = league;

            // show it in TreeView
            _tree.ItemsSource = new object[] { league };
        }
    }

    /// <summary>
    /// Top level in the data hierarchy.
    /// </summary>
    public class League
    {
        public string Name { get; set; }
        public List<Division> Divisions { get; set; }

        public static League GetLeague()
        {
            var league = new League();
            league.Name = "Main League";
            league.Divisions = new List<Division>();
            foreach (var div in "East,West,North,South".Split(','))
            {
                var d = new Division();
                league.Divisions.Add(d);
                d.Name = div;
                d.Teams = new List<Team>();
                switch (div)
                {
                    case "East":
                        AddNewTeams(d, "Redskins,Eagles,Giants,Cowboys");
                        break;
                    case "West":
                        AddNewTeams(d, "Cardinals,Seahawks,Rams,49ers");
                        break;
                    case "North":
                        AddNewTeams(d, "Vikings, Packers, Lions, Bears");
                        break;
                    case "South":
                        AddNewTeams(d, "Panthers,Falcons,Saints,Buccaneers");
                        break;
                }
            }
            return league;
        }

        static void AddNewTeams(Division d, string teamNames)
        {
            foreach (var team in teamNames.Split(','))
            {
                var t = new Team();
                d.Teams.Add(t);
                t.Name = team;
            }
        }
    }
    /// <summary>
    /// Middle level in the data hierarchy.
    /// </summary>
    public class Division
    {
        public string Name { get; set; }
        public List<Team> Teams { get; set; }
    }
    /// <summary>
    /// Bottom level in the data hierarchy.
    /// </summary>
    public class Team
    {
        public string Name { get; set; }
    }

    public class foo : HeaderedItemsControl
    {
        protected override void OnItemsSourceChanged(System.Collections.IEnumerable oldValue, System.Collections.IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);
        }
        protected override void OnItemsChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.ItemTemplate = null;
            this.ItemTemplateSelector = null;
            base.OnItemsChanged(e);
        }
        protected override void OnItemTemplateChanged(DataTemplate oldItemTemplate, DataTemplate newItemTemplate)
        {
            base.OnItemTemplateChanged(oldItemTemplate, newItemTemplate);
        }
        protected override void OnItemTemplateSelectorChanged(DataTemplateSelector oldItemTemplateSelector, DataTemplateSelector newItemTemplateSelector)
        {
            base.OnItemTemplateSelectorChanged(oldItemTemplateSelector, newItemTemplateSelector);
        }
    }

}
