﻿using C1.DataCollection;
using C1.DataFilter;
using C1.WPF.DataFilter;
using C1.WPF.Input;
using CustomFilters.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace CustomFilters
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

        protected override Expression GetExpression()
        {
            var tags = _modelFilterPresenter.GetSelectedTagList();
            var expr = new CombinationExpression() { FilterCombination = FilterCombination.Or };
            foreach (var tag in tags)
            {
                expr.Expressions.Add(new OperationExpression() { Value = tag, FilterOperation = FilterOperation.Equal, PropertyName = PropertyName });
            }
            return expr;
        }

        protected override void SetExpression(Expression expression)
        {
        }

        public override bool IsApplied => _modelFilterPresenter.GetSelectedTagList().Count() > 0;
    }

    public class ModelFilterPresenter : ItemsControl
    {
        public event EventHandler SelectedChanged;

        private void MultiSelect_SelectionChanged(object sender, EventArgs e) => SelectedChanged?.Invoke(this, e);

        public void SetTagList(IEnumerable<Car> itemSource)
        {
            foreach (C1MultiSelect item in Items)
            {
                item.SelectionChanged -= MultiSelect_SelectionChanged;
            }
            Items.Clear();
            var ms = new C1MultiSelect
            {
                Height = 50,
                PlaceHolder = "Model of car",
                ItemsSource = itemSource,
                DisplayMemberPath = "Model",
                SelectedValuePath = "Model",
                AutoCompleteMode = AutoCompleteMode.SuggestAppend
            };
            ms.SelectionChanged += MultiSelect_SelectionChanged;
            Items.Add(ms);
        }

        public IEnumerable<string> GetSelectedTagList()
        {
            foreach (C1MultiSelect item in Items)
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
    }
}
