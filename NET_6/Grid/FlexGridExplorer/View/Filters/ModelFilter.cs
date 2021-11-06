using C1.DataCollection;
using C1.WPF.DataFilter;
using C1.WPF.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace FlexGridExplorer
{
    public class ModelFilter : CustomFilter
    {
        private ModelFilterPresenter _modelFilterPresenter;

        public ModelFilter()
        {
            Control = _modelFilterPresenter = new ModelFilterPresenter();
            _modelFilterPresenter.SelectedChanged += (s, e) => OnValueChanged(new ValueChangedEventArgs() { ApplyFilter = true });
        }

        public void SetTagList(IEnumerable<Car> itemSource)
        {
            _modelFilterPresenter.SetTagList(itemSource);
        }

        public override Expression Expression 
        { 
            get
            {
                var tags = _modelFilterPresenter.GetSelectedTagList();
                var expr = new CombinationExpression() { FilterCombination = FilterCombination.Or };
                foreach (var tag in tags)
                {
                    expr.Expressions.Add(new OperationExpression() { Value = tag, FilterOperation = FilterOperation.Equal, PropertyName = PropertyName });
                }
                return expr;
            }
            set
            {
                var models = GetModels(value).ToList();
                _modelFilterPresenter.SetSelectedModels(models);

            }
        }

        private IEnumerable<string> GetModels(Expression expression)
        {
            if (expression is OperationExpression operation)
            {
                if (operation.PropertyName != PropertyName) yield break;

                var model = _modelFilterPresenter.Cars.FirstOrDefault(s => s.Model == (string)operation.Value)?.Model;
                if (model != null)
                    yield return model;

            }
            else if (expression is CombinationExpression combination)
            {
                foreach (var e in combination.Expressions)
                {
                    foreach (var model in GetModels(e))
                    {
                        yield return model;
                    }
                }
            }
        }

        public override bool IsApplied => _modelFilterPresenter.GetSelectedTagList().Any();
    }

    public class ModelFilterPresenter : ItemsControl
    {
        public List<Car> Cars { get; set; }
        public event EventHandler SelectedChanged;

        private void MultiSelect_SelectionChanged(object sender, EventArgs e) => SelectedChanged?.Invoke(this, e);

        public void SetTagList(IEnumerable<Car> itemSource)
        {
            Cars = itemSource.ToList();

            foreach (ListBox item in Items)
            {
                item.SelectionChanged -= MultiSelect_SelectionChanged;
            }
            Items.Clear();
            var ms = new ListBox
            {
                SelectionMode = System.Windows.Controls.SelectionMode.Multiple,
                Height = 100,
                //PlaceHolder = "Model of car",
                ItemsSource = Cars,
                DisplayMemberPath = "Model",
                SelectedValuePath = "Model",
                Style = null,
            };
            ms.SelectedIndex = -1;
            ms.SelectionChanged += MultiSelect_SelectionChanged;
            Items.Add(ms);
        }

        public IEnumerable<string> GetSelectedTagList()
        {
            foreach (ListBox item in Items)
            {
                if (item.SelectedItems != null)
                {
                    foreach (var selectedItem in item.SelectedItems)
                    {
                        yield return (selectedItem as Car).Model;
                    }
                }
            }
        }

        public void SetSelectedModels(IEnumerable<string> models)
        {
            ListBox listBoxItem = Items[0] as ListBox;
            if (listBoxItem != null)
            {
                var itemsource = listBoxItem.ItemsSource as List<Car>;
                foreach (var model in models)
                {
                    var car = itemsource.FirstOrDefault(s => s.Model == model);
                    listBoxItem.SelectedItems.Add(car);
                }
            }
        
        }
    }
}
