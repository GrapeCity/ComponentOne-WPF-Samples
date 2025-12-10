using System.Collections.Generic;
using System;
using System.Windows.Controls;

namespace RulesManagerExplorer
{
    public partial class ListViewIntegrationDemo : UserControl
    {
        public ListViewIntegrationDemo()
        {
            InitializeComponent();
            Tag = Properties.Resources.ListViewIntegrationDescription;

            listView.ItemsSource = GetStudents();            
        }

        public List<Student> GetStudents()
        {
            string[] firstNames = "Andy|Ben|Charlie|Dan|Ed|Fred|Gil|Herb|Jack|Karl|Larry|Mark|Noah|Oprah|Paul|Quince|Rich|Steve|Ted|Ulrich|Vic|Xavier|Zeb".Split('|');
            string[] lastNames = "Ambers|Bishop|Cole|Danson|Evers|Frommer|Griswold|Heath|Jammers|Krause|Lehman|Myers|Neiman|Orsted|Paulson|Quaid|Richards|Stevens|Trask|Ulam".Split('|');
            
            var result = new List<Student>();
            var rnd = new Random();
            for (var i = 0; i < 100; i++)
            {
                result.Add(new Student()
                {
                    Name = string.Format("{0} {1}", firstNames[rnd.Next(firstNames.Length)], lastNames[rnd.Next(lastNames.Length)]),
                    Score = rnd.Next(101),
                });
            }
            return result;
        }

        private void C1RulesEngine_RulesChanged(object sender, EventArgs e)
        {
            if (listView is not null)
                for (int i = 0; i < listView.Items.Count; i++)
                    listView.Refresh(i);
        }
    }
}
