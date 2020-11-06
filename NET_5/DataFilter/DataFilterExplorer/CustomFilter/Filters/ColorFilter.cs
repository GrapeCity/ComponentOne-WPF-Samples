using C1.DataCollection;
using C1.WPF.DataFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;

namespace DataFilterExplorer
{
    public class ColorFilter : CustomFilter
    {
        private ColorFilterPresenter _colorFilterPresenter;

        public ColorFilter()
        {
            Control = _colorFilterPresenter = new ColorFilterPresenter();
            _colorFilterPresenter.SelectedChanged += (s, e) => OnValueChanged(new ValueChangedEventArgs() { ApplyFilter = true });
        }

        public void SetColors(IEnumerable<SolidColorBrush> colors) => _colorFilterPresenter.SetColors(colors);

        protected override Expression GetExpression()
        {
            var colors = _colorFilterPresenter.GetSelectedColors();
            var expr = new CombinationExpression() { FilterCombination = FilterCombination.Or };
            foreach (var color in colors)
            {
                expr.Expressions.Add(new OperationExpression() { Value = color.Color, FilterOperation = FilterOperation.Equal, PropertyName = PropertyName });
            }
            return expr;
        }

        protected override void SetExpression(Expression expression)
        {
        }

        public override bool IsApplied => _colorFilterPresenter.GetSelectedColors().Any();
    }

    public class ColorFilterPresenter : ItemsControl
    {
        public void SetColors(IEnumerable<SolidColorBrush> colors)
        {
            foreach (CheckBox cb in Items)
            {
                cb.Checked -= CB_CheckedChanged;
                cb.Unchecked -= CB_CheckedChanged;
            }
            Items.Clear();
            foreach (var color in colors)
            {
                var cb = new CheckBox
                {
                    Background = color
                };
                cb.Checked += CB_CheckedChanged;
                cb.Unchecked += CB_CheckedChanged;
                Items.Add(cb);
            }
        }

        public event EventHandler SelectedChanged;

        private void CB_CheckedChanged(object sender, EventArgs e) => SelectedChanged?.Invoke(this, e);

        public IEnumerable<SolidColorBrush> GetSelectedColors()
        {
            foreach (CheckBox cb in Items)
                if (cb.IsChecked == true)
                    yield return (SolidColorBrush)cb.Background;
        }
    }
}
