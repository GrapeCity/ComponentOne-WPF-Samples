using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ExpressionEditorExplorer
{
    /// <summary>
    /// Interaction logic for ExpressionEditorCustomizationDemo.xaml
    /// </summary>
    public partial class ExpressionEditorCustomizationDemo : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionEditorCustomizationDemo"/> class.
        /// </summary>
        public ExpressionEditorCustomizationDemo()
        {
            InitializeComponent();
            Tag = Properties.Resources.CustomizationSampleDescription;

            expressionEditor.Engine.DataSource = Customer.GenerateSampleData(50).ToList();
            expressionEditor.Engine.AddAlias("Age", "ClientAge");
        }

        private void ExpressionEditorPanel_AutoGeneratingCategory(object? sender, C1.WPF.ExpressionEditor.AutoGeneratingCategoryEventArgs e)
        {
            if (e.Category.Type != C1.CalcEngine.ItemType.AggregateFuncs &&
                e.Category.Type != C1.CalcEngine.ItemType.ConvertFuncs &&
                e.Category.Type != C1.CalcEngine.ItemType.DateTimeFuncs &&
                e.Category.Type != C1.CalcEngine.ItemType.LogicalFuncs &&
                e.Category.Type != C1.CalcEngine.ItemType.MathFuncs &&
                e.Category.Type != C1.CalcEngine.ItemType.TextFuncs)
                e.Cancel = true;
        }
    }
}
