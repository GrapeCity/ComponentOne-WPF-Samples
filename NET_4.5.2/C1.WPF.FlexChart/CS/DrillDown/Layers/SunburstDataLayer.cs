using System.Collections;
using System.Windows.Media;
using System.Collections.Generic;

namespace DrillDown
{
    public class SunburstDataLayer : Bindable
    {
        private string _bindingX;
        private string _binding;
        private List<DataItem> _itemsSource;
        private IList<Brush> _customPalette;
        private object _previousItem;
        private Stack _histories = new Stack();

        public SunburstDataLayer(List<DataItem> dataSource)
        {
            ItemsSource = dataSource;
        }

        public string Binding
        {
            get { return _binding; }
            set
            {
                SetProperty(ref _binding, value, "Binding");
            }
        }

        public string BindingX
        {
            get { return _bindingX; }
            set
            {
                SetProperty(ref _bindingX, value, "BindingX");
            }
        }

        public List<DataItem> ItemsSource
        {
            get { return _itemsSource; }
            set
            {
                SetProperty(ref _itemsSource, value, "ItemsSource");
            }
        }

        public IList<Brush> CustomPalette
        {
            get { return _customPalette; }
            set
            {
                SetProperty(ref _customPalette, value, "CustomPalette");
            }
        }

        public bool CanBack
        {
            get
            {
                return _histories.Count > 0;
            }
        }

        private int GetRootIndexFromItem(DataItem item)
        {
            var len = ItemsSource.Count;
            var index = -1;
            for (var i = 0; i < len; i++)
            {
                if (Contains(ItemsSource[i], item))
                {
                    index = i;
                }
            }
            return index;
        }

        private bool Contains(DataItem sourceItem, DataItem targetItem)
        {
            bool result = false;
            if (sourceItem == targetItem)
            {
                result = true;
            }
            else
            {
                foreach (var item in sourceItem.Items)
                {
                    if (Contains(item, targetItem))
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }

        public void DrillDown(object item)
        {
            var dataItem = item as DataItem;
            var index = GetRootIndexFromItem(dataItem);
            if (dataItem != null && dataItem.Items.Count != 0 && _previousItem != item)
            {
                _previousItem = item;
                _histories.Push(BindingX);
                _histories.Push(ItemsSource);
                _histories.Push(CustomPalette);
                if (index != -1)
                {
                    CustomPalette = new List<Brush>() { CustomPalette[index] };
                }
                ItemsSource = new List<DataItem> { dataItem };
                if (!string.IsNullOrEmpty(dataItem.Month))
                    BindingX = "Month";
                else if (!string.IsNullOrEmpty(dataItem.Quarter))
                    BindingX = "Quarter,Month";
                Notify("CanBack");
            }
        }

        public void Back()
        {
            if (CanBack)
            {
                CustomPalette = _histories.Pop() as IList<Brush>;
                ItemsSource =  _histories.Pop() as List<DataItem>;
                BindingX = _histories.Pop() as string;
                _previousItem = null;
                Notify("CanBack");
            }
        }
    }
}
