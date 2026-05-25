using C1.DataCollection;
using C1.WPF.DataFilter;
using C1.WPF.Input;
using FlexGridExplorer.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace FlexGridExplorer
{
    public class TransmissionFilter : CustomFilter
    {
        private TransmissionFilterPresenter _transmissionFilterPresenter;

        public TransmissionFilter()
        {
            Control = _transmissionFilterPresenter = new TransmissionFilterPresenter();
            _transmissionFilterPresenter.SelectedChanged += (s, e) => OnValueChanged(new ValueChangedEventArgs() { ApplyFilter = true });
        }

        public override Expression Expression
        {
            get
            {
                var tags = _transmissionFilterPresenter.GetSelectedValues();
                var expr = new CombinationExpression() { FilterCombination = FilterCombination.Or };
                foreach (var tag in tags)
                {
                    var value = GetValue(tag);
                    expr.Expressions.Add(new OperationExpression() { Value = value, FilterOperation = FilterOperation.Equal, PropertyName = PropertyName });
                }
                return expr;
            }
            set
            {
                var selectedValues = GetSelectedValues(value).ToList();
                _transmissionFilterPresenter.SetSelectedValues(selectedValues);
            }
        }

        private bool? GetValue(string tag)
        {
            if (tag == AppResources.NoLabel)
                return false;
            if (tag == AppResources.YesLabel)
                return true;
            return null;
        }

        private IEnumerable<string> GetSelectedValues(Expression expression)
        {
            switch (expression)
            {
                case CombinationExpression combination when combination.FilterCombination == FilterCombination.Or:
                    foreach (var e in combination.Expressions)
                    {
                        foreach (var v in GetSelectedValues(e))
                        {
                            yield return v;
                        }
                    }
                    break;
                case OperationExpression operation when operation.FilterOperation == FilterOperation.Equal:
                    yield return operation.Value?.ToString();
                    break;
            }
        }

        public override bool IsApplied => _transmissionFilterPresenter.GetSelectedValues().Any();
    }

    public class TransmissionFilterPresenter : ItemsControl
    {
        private bool _isInitializing = false;

        public TransmissionFilterPresenter()
        {
            foreach (RadioButton rb in Items)
            {
                rb.Checked -= RadioButton_CheckedChanged;
            }
            Items.Clear();

            CreateItem(AppResources.AllLabel);
            CreateItem(AppResources.YesLabel);
            CreateItem(AppResources.NoLabel);
        }

        private void CreateItem(string content)
        {
            var rb = new RadioButton
            {
                Content = content,
                GroupName = "G",
                Margin = new System.Windows.Thickness(2)
            };
            if (content == AppResources.AllLabel)
            {
                rb.IsChecked = true;
            }
            rb.Checked += RadioButton_CheckedChanged;
            Items.Add(rb);
        }

        public event EventHandler SelectedChanged;
        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (_isInitializing) return;
            SelectedChanged?.Invoke(this, e);
        }

        public IEnumerable<string> GetSelectedValues()
        {
            foreach (RadioButton rb in Items)
            {
                if (rb.IsChecked ?? false)
                {
                    if (rb.Content.ToString() == AppResources.AllLabel)
                    {
                        continue;
                    }
                    else
                    {
                        yield return rb.Content.ToString();
                    }
                }
            }
        }

        internal void SetSelectedValues(List<string> selectedValues)
        {
            try
            {
                _isInitializing = true;
                if (selectedValues.Count == 0)
                    selectedValues.Add(AppResources.AllLabel);
                foreach (var radioButton in Items.OfType<RadioButton>())
                {
                    radioButton.IsChecked = selectedValues?.Contains((string)radioButton.Content) ?? false;
                }
            }
            finally
            {
                _isInitializing = false;
            }
        }
    }
}
