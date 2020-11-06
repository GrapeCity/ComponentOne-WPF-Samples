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
            Tag = "This sample shows how to bind data and create a custom template.";
            LoadData();

        }

        private void LoadData()
        {
            List<Sport> sports = new List<Sport>
            {
                new Sport
                {
                    Name="Ball",
                    ChildSport=new List<Sport>
                    {
                        new Sport
                        {
                            Name="Football"
                        },
                        new Sport
                        {
                            Name="Basketball"
                        },
                        new Sport
                        {
                            Name="PingPong"
                        }
                    }
                },
                new Sport
                {
                    Name="Track and Field",
                    ChildSport=new List<Sport>
                    {
                        new Sport
                        {
                            Name="Running"
                        },
                        new Sport
                        {
                            Name="Marathon"
                        },
                        new Sport
                        {
                            Name="Relay Race"
                        }
                    }
                },
                new Sport
                {
                    Name="Others",
                    ChildSport=new List<Sport>
                    {
                        new Sport
                        {
                            Name="Swimming"
                        },
                        new Sport
                        {
                            Name="Weight Lifting"
                        },
                        new Sport
                        {
                            Name="Gymnastics"
                        }
                    }
                },
            };
            this.tree.DataContext = sports;
        }
    }
}
