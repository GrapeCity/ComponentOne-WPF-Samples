using C1.DataCollection;
using C1.WPF.DataFilter;
using System;
using System.IO;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace DataFilterExplorer
{
    /// <summary>
    /// Interaction logic for FilterSummarySample.xaml
    /// </summary>
    public partial class FilterEditorSample : UserControl
    {
        string pathToXmlFile = Directory.GetCurrentDirectory() + "\\FilterExpression.xml";

        public FilterEditorSample()
        {
            InitializeComponent();
            Tag = "This sample shows the basic features of C1FilterEditor. \r" +
                "This sample demonstrates basic functionality of the C1FilterEditor control. \r" +
                "The FilterEditor control represents a filter in the form of a tree.Tree nodes can be logical conditions \"And\" and \"Or\" or a filter for a data source property. \r" +
                "The C1FilterEditor.SetExpression method is used to load predefined filter. \r" +
                "You can use the GetExpression method to get the current filter expression, which you can use for xml serialization. \r" +
                "In this sample FlexGrid.DataSource property and FilterEditor.DataSource property are both set to the same data collection. \r" +
                "That allows to filter FlexGrid content based on multiple conditions selected in the C1FilterEditor.";
            C1DataCollection<Car> data = new C1DataCollection<Car>(DataProvider.GetCars());
            filterEditor.ItemsSource = data;
            flexGrid.ItemsSource = data;
            InitExpressionAndApply();
            filterEditor.FilterChanged += filterEditor_FilterChanged;
        }

        async void InitExpressionAndApply()
        {
            CombinationExpression filterExpression = null;

            if (File.Exists(pathToXmlFile))
            {
                filterExpression = LoadFilterFromFile(pathToXmlFile);
                filterEditor.Expression = filterExpression;
            }

            await filterEditor.ApplyFilterAsync();
        }

        CombinationExpression LoadFilterFromFile(string filePath)
        {
            CombinationExpression filterExpression;
            var xmlSerializer = new XmlSerializer(typeof(CombinationExpression));
            using (var fs = File.Open(filePath, FileMode.Open))
            {
                filterExpression = xmlSerializer.Deserialize(fs) as CombinationExpression;
            }

            return filterExpression;
        }

        private void filterEditor_FilterChanged(object sender, EventArgs e)
        {
            SaveFilterToFile(pathToXmlFile);
        }

        private void SaveFilterToFile(string filePath)
        {
            var expression = filterEditor.Expression;
            var xmlSerializer = new XmlSerializer(typeof(CombinationExpression));
            using (var fs = File.Create(filePath))
            {
                xmlSerializer.Serialize(fs, expression);
            }
        }
    }    
}
