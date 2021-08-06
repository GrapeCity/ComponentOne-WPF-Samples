using C1.DataCollection;
using C1.WPF.DataFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;

namespace FlexGridExplorer
{
    public class ColorFilter : CustomFilter
    {
        private ColorFilterPresenter _colorFilterPresenter;

        public ColorFilter()
        {
            Control = _colorFilterPresenter = new ColorFilterPresenter();
            _colorFilterPresenter.SelectedChanged += (s, e) => OnValueChanged(new ValueChangedEventArgs() { ApplyFilter = true });
        }

        public void SetColors(List<Color> colors) => _colorFilterPresenter.SetColors(colors);

        public override Expression Expression 
        { 
            get
            {
                var colors = _colorFilterPresenter.GetSelectedColors();
                var expr = new CombinationExpression() { FilterCombination = FilterCombination.Or };
                foreach (var color in colors)
                {
                    expr.Expressions.Add(new OperationExpression() { Value = color.Color, FilterOperation = FilterOperation.Equal, PropertyName = PropertyName });
                }
                return expr;
            }
            set
            {
                var selectedColors = GetColors(value).ToList();
                _colorFilterPresenter.SetSelectedColors(selectedColors);
            }
        }

        private IEnumerable<Color> GetColors(Expression expression)
        {
            if (expression is OperationExpression operation)
            {
                if (operation.PropertyName != PropertyName) yield break;

                var color = _colorFilterPresenter.Colors.FirstOrDefault(c => c == (Color)operation.Value);
                if (color != default)
                    yield return color;

            }
            else if (expression is CombinationExpression combination)
            {
                foreach (var e in combination.Expressions)
                {
                    foreach (var color in GetColors(e))
                    {
                        yield return color;
                    }
                }
            }
            yield break;
        }


        public override bool IsApplied => _colorFilterPresenter.GetSelectedColors().Any();
    }

    public class ColorFilterPresenter : ItemsControl
    {
        private bool _isInitializing = false;
        public List<Color> Colors { get; set; }

        public void SetColors(List<Color> colors)
        {
            Colors = colors;
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
                    Background = new SolidColorBrush(color)
                };
                cb.Checked += CB_CheckedChanged;
                cb.Unchecked += CB_CheckedChanged;
                Items.Add(cb);
            }
        }

        public event EventHandler SelectedChanged;

        private void CB_CheckedChanged(object sender, EventArgs e)
        {
            if (_isInitializing) return;
            SelectedChanged?.Invoke(this, e);
        }

        public IEnumerable<SolidColorBrush> GetSelectedColors()
        {
            foreach (CheckBox cb in Items)
                if (cb.IsChecked ?? false)
                    yield return (SolidColorBrush)cb.Background;
        }

        internal void SetSelectedColors(List<Color> selectedColors)
        {
            try
            {
                _isInitializing = true;
                foreach (CheckBox cb in Items)
                {
                    cb.IsChecked = cb.Background is SolidColorBrush solidColor && selectedColors.Contains(solidColor.Color);
                }
            }
            finally
            {
                _isInitializing = false;
            }
        }
    }
}
