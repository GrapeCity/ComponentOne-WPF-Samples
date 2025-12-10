using C1.WPF.Grid;
using FlexGridExplorer.Resources;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace FlexGridExplorer
{
    /// <summary>
    /// Interaction logic for FullTextFilter.xaml
    /// </summary>
    public partial class FullTextFilter : UserControl
    {
        /// <summary>
        /// Identifies the <see cref="FilteredFields"/> dependency property.
        /// This property holds the collection of field names that the <c>FullTextFilterBehavior</c> 
        /// will use when filtering the C1FlexGrid. 
        /// </summary>
        public static readonly DependencyProperty FilteredFieldsProperty =
            DependencyProperty.Register(
                nameof(FilteredFields),
                typeof(IEnumerable<string>),
                typeof(FullTextFilter),
                new PropertyMetadata(null));
        /// <summary>
        /// Specify the fields that should be taken into account for performing filtering
        /// </summary>
        public IEnumerable<string> FilteredFields
        {
            get => (IEnumerable<string>)GetValue(FilteredFieldsProperty);
            set => SetValue(FilteredFieldsProperty, value);
        }
        public FullTextFilter()
        {
            InitializeComponent();
            Tag = AppResources.FullTextFilterDescription;
            var data = Customer.GetCustomerList(100);
            grid.ItemsSource = data;
            grid.MinColumnWidth = 85;
            Loaded += FullTextFilterGrid_Loaded;
            
        }

        private void FullTextFilterGrid_Loaded(object sender, RoutedEventArgs e)
        {
            FilteredFields = null;
        }
    }
}
