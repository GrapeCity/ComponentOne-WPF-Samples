using C1.DataCollection;
using C1.WPF.DataFilter;
using DataFilterExplorer.Resources;
using System;
using System.Globalization;
using System.IO;
using System.Threading;
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
            Tag = AppResources.FilterEditorTag;
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
