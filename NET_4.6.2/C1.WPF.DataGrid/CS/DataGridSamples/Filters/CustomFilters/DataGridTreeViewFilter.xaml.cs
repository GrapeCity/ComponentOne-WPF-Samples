using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using C1.WPF;
using C1.WPF.DataGrid;

namespace DataGridSamples
{
    public partial class DataGridTreeViewFilter : UserControl, IDataGridFilterUnity
    {
        DataGridFilterState _filter = null;

        public DataGridTreeViewFilter()
        {
            InitializeComponent();

            treeItems.SetBinding(C1TreeView.ItemsSourceProperty, new Binding("Source") { Source = this });
            chkSelectAll.SetBinding(CheckBox.IsCheckedProperty, new Binding("Source")
                {
                    Source = this,
                    Mode = BindingMode.TwoWay,
                    Converter = CustomConverter.Create(
                    (value, type, parameter, culture) =>
                    {
                        if (Source != null && Source.All(g => g.IsChecked.HasValue && g.IsChecked.Value))
                            return true;
                        else if (Source != null && Source.All(g => g.IsChecked.HasValue && !g.IsChecked.Value))
                            return false;
                        else
                            return null;
                    },
                    (value, type, parameter, culture) =>
                    {
                        bool newValue = (bool?)value ?? false;
                        foreach (var p in Source)
                        {
                            p.IsChecked = newValue;
                        }
                        RaisePropertyChanged("Filter");
                        return value;
                    }),
                });
        }

        public List<GroupElement> Source
        {
            get { return (List<GroupElement>)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(List<GroupElement>), typeof(DataGridTreeViewFilter), new PropertyMetadata(null, (d, a) =>
                {
                    ((DataGridTreeViewFilter)d).SourceChanged(a.OldValue as List<GroupElement>);
                }));

        private void SourceChanged(List<GroupElement> oldSource)
        {
            if (oldSource != null)
            {
                foreach (var group in oldSource)
                {
                    group.PropertyChanged -= new PropertyChangedEventHandler(group_PropertyChanged);
                }
            }
            if (Source != null)
            {
                foreach (var group in Source)
                {
                    group.PropertyChanged += new PropertyChangedEventHandler(group_PropertyChanged);
                }
            }
        }

        private void group_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!_isSettingValues)
            {
                _filter = GetFilter();
                RaisePropertyChanged("Filter");
            }
        }

        public DataGridFilterState Filter
        {
            get
            {
                return _filter;
            }
            set
            {
                if (_filter != value)
                {
                    _filter = value;
                    SetFilter(_filter);
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="e:PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">Name of the property that was changed.</param>
        protected void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private bool _isSettingValues = false;

        private void SetFilter(DataGridFilterState filter)
        {
            _isSettingValues = true;
            if (filter != null &&
                filter.FilterInfo != null &&
                filter.FilterInfo.Count > 0 &&
                filter.FilterInfo[0] != null &&
                filter.FilterInfo[0].FilterOperation == DataGridFilterOperation.IsOneOf &&
                filter.FilterInfo[0].Value is List<ProductElement>)
            {
                foreach (var p in Source)
                {
                    p.IsChecked = false;
                }
                foreach (var p in ((List<ProductElement>)filter.FilterInfo[0].Value))
                {
                    p.IsChecked = true;
                }
            }
            else
            {
                foreach (var p in Source)
                {
                    p.IsChecked = true;
                }
            }
            _isSettingValues = false;
        }

        private DataGridFilterState GetFilter()
        {
            if (Source.All(gr => gr.IsChecked.HasValue && gr.IsChecked.Value))
            {
                return new DataGridFilterState
                {
                    FilterInfo = new List<DataGridFilterInfo>
                        {
                            new DataGridFilterInfo
                            {
                                FilterOperation = DataGridFilterOperation.All,
                                FilterType = DataGridFilterType.MultiValue,
                            }
                        }
                };
            }
            else if (Source.All(gr => gr.IsChecked.HasValue && !gr.IsChecked.Value))
            {
                return new DataGridFilterState
                {
                    FilterInfo = new List<DataGridFilterInfo>
                        {
                            new DataGridFilterInfo
                            {
                                FilterOperation = DataGridFilterOperation.None,
                                FilterType = DataGridFilterType.MultiValue,
                            }
                        }
                };
            }
            else
            {
                var checkedItems = Source.Aggregate((IEnumerable<ProductElement>)new List<ProductElement>(), (accumulated, gr) => accumulated.Concat(gr.Children.Where(pe => pe.IsChecked))).Select(pe => pe.Product.ProductNumber).ToList();
                return new DataGridFilterState
                {
                    FilterInfo = new List<DataGridFilterInfo>
                        { 
                            new DataGridFilterInfo
                            { 
                                FilterOperation = DataGridFilterOperation.IsOneOf, 
                                FilterType = DataGridFilterType.MultiValue, 
                                Value = checkedItems,
                            }
                        }
                };
            }
        }
    }

    public class CustomTemplateSelector : C1DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is GroupElement)
            {
                return Resources["GroupTemplate"] as DataTemplate;
            }
            else if (item is ProductElement)
            {
                return Resources["ProductTemplate"] as DataTemplate;
            }
            else
            {
                return null;
            }
        }
    }

    public class GroupElement : INotifyPropertyChanged
    {
        public GroupElement(IGrouping<string, Product> group)
        {
            Key = group.Key;
            Children = group.Select(p => new ProductElement(this, p)).ToList();
        }

        public string Key { get; private set; }
        public IEnumerable<ProductElement> Children { get; private set; }
        public bool? IsChecked
        {
            get
            {
                if (Children.All(p => p.IsChecked))
                {
                    return true;
                }
                else if (Children.All(p => !p.IsChecked))
                {
                    return false;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                bool newValue = value ?? false;
                foreach (var p in Children)
                {
                    p.IsChecked = newValue;
                }
            }
        }

        public override string ToString()
        {
            return Key ?? base.ToString();
        }

        internal void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class ProductElement : INotifyPropertyChanged
    {
        bool _isChecked = true;

        public ProductElement(GroupElement group, Product product)
        {
            Group = group;
            Product = product;
        }

        public GroupElement Group { get; private set; }
        public Product Product { get; private set; }

        public bool IsChecked
        {
            get
            {
                return _isChecked;
            }
            set
            {
                _isChecked = value;
                RaisePropertyChanged("IsChecked");
                Group.RaisePropertyChanged("IsChecked");
            }
        }

        protected void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
