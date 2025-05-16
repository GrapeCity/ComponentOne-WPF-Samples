using C1.Zip;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Reflection;
using System.Windows.Controls;
using System.Xml;

namespace FlexPivotExplorer
{
    public class SampleItem
    {
        public SampleItem(string name, string title, Lazy<UserControl> sample)
        {
            Name = name;
            Title = title;
            Sample = sample;
        }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description => Sample.Value.Tag?.ToString();
        public Lazy<UserControl> Sample { get; set; }
    }

    public static class Utils
    {
        static Utils()
        {
            PivotDataSet = new DataSet();
            var asm = Assembly.GetExecutingAssembly();
            using (var s = asm.GetManifestResourceStream("FlexPivotExplorer.Resources.nwind.zip"))
            {
                var zip = new C1ZipFile(s);
                using (var zr = zip.Entries[0].OpenReader())
                {
                    // load data
                    PivotDataSet.ReadXml(zr);
                }
            }

            PivotViews = new Dictionary<string, string>();
            using (var s = asm.GetManifestResourceStream("FlexPivotExplorer.Resources.FlexPivotViews.xml"))
            using (var reader = XmlReader.Create(s))
            {
                // read predefined view definitions
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "FlexPivotPage")
                    {
                        var id = reader.GetAttribute("id");
                        var def = reader.ReadOuterXml();
                        PivotViews[id] = def;
                    }
                }
            }
        }
        public static DataSet PivotDataSet { get; set; }

        public static Dictionary<string, string> PivotViews { get; set; }

    }
}
