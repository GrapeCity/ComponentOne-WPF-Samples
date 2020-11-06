using C1.DataCollection;
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
    public class TransmissionFilter : CustomFilter
    {
        private TransmissionFilterPresenter _transmissionFilterPresenter;

        public TransmissionFilter()
        {
            Control = _transmissionFilterPresenter = new TransmissionFilterPresenter();
            _transmissionFilterPresenter.SelectedChanged += (s, e) => OnValueChanged(new ValueChangedEventArgs() { ApplyFilter = true });
        }

        protected override Expression GetExpression()
        {
            var tags = _transmissionFilterPresenter.GetSelectedValues();
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

        public override bool IsApplied => _transmissionFilterPresenter.GetSelectedValues().Count() > 0;
    }

    public class TransmissionFilterPresenter : ItemsControl
    {
        public TransmissionFilterPresenter()
        {
            foreach (RadioButton rb in Items)
            {
                rb.Checked -= RadioButton_CheckedChanged;
            }
            Items.Clear();

            CreateItem("All");
            CreateItem("Yes");
            CreateItem("No");
        }

        private void CreateItem(string content)
        {
            var rb = new RadioButton
            {
                Content = content,
                GroupName = "G",
                Margin= new System.Windows.Thickness(0, 0, 0, 5)
            };
            if(content == "All")
            {
                rb.IsChecked = true;
            }
            rb.Checked += RadioButton_CheckedChanged;
            Items.Add(rb);
        }

        public event EventHandler SelectedChanged;
        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            SelectedChanged?.Invoke(this, e);
        }

        public IEnumerable<string> GetSelectedValues()
        {
            foreach (RadioButton rb in Items)
            {
                if (rb.IsChecked == true)
                {
                    if(rb.Content.ToString() == "All")
                    {
                        yield return "Yes";
                        yield return "No";
                    }
                    else
                    {
                        yield return rb.Content.ToString();
                    }
                }
            }
        }
    }
}
