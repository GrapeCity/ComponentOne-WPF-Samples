using System.Windows;
using System.Windows.Markup;
using C1.WPF.DataGrid;

namespace DataGridSamples
{
    [ContentProperty("Content")]
    public class DataGridTemplateRowPresenter : DataGridSelectableRowPresenter
    {
        public DataGridTemplateRowPresenter()
            : base()
        {
        }
        public object Content
        {
            get { return (object)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Content.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(object), typeof(DataGridTemplateRowPresenter), new PropertyMetadata(null));

    }
}
