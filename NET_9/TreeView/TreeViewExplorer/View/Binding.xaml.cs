using System.Collections.Generic;
using System.Windows.Controls;

namespace TreeViewExplorer
{
    /// <summary>
    /// Interaction logic for SimpleC1TreeViewPage.xaml
    /// </summary>
    public partial class DemoSimpleC1TreeView : UserControl
    {
        public DemoSimpleC1TreeView()
        {
            InitializeComponent();
            Tag = Properties.Resources.Binding;
            LoadData();

        }

        private void LoadData()
        {
            List<Sport> sports = new List<Sport>
            {
                new Sport
                {
                    Name=Properties.Resources.Ball,
                    ChildSport=new List<Sport>
                    {
                        new Sport
                        {
                            Name=Properties.Resources.Football
                        },
                        new Sport
                        {
                            Name=Properties.Resources.Basketball
                        },
                        new Sport
                        {
                            Name=Properties.Resources.PingPong
                        }
                    }
                },
                new Sport
                {
                    Name=Properties.Resources.TrackandField,
                    ChildSport=new List<Sport>
                    {
                        new Sport
                        {
                            Name=Properties.Resources.Running
                        },
                        new Sport
                        {
                            Name=Properties.Resources.Marathon
                        },
                        new Sport
                        {
                            Name=Properties.Resources.RelayRace
                        }
                    }
                },
                new Sport
                {
                    Name=Properties.Resources.Others,
                    ChildSport=new List<Sport>
                    {
                        new Sport
                        {
                            Name=Properties.Resources.Swimming
                        },
                        new Sport
                        {
                            Name=Properties.Resources.WeightLifting
                        },
                        new Sport
                        {
                            Name=Properties.Resources.Gymnastics
                        }
                    }
                },
            };
            this.tree.DataContext = sports;
        }
    }
}
