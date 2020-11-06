using C1.DataCollection;
using C1.DataFilter;
using C1.WPF.DataFilter;
using System.Collections.Generic;
using System.Globalization;

namespace CustomFilters
{
    public class PriceFilter: ChecklistFilter
    {
        public PriceFilter(string propertyName = "") : base(propertyName)
        {
            
        }

        public void SetPriceIntervals(List<PriceInterval> intervals)
        {
            foreach (var interval in intervals)
                Items.Add(new ChecklistItem() { DisplayValue = interval.ToString(), Value = interval });
        }

        public override Expression Expression
        {
            get
            {
                var expr = new CombinationExpression() { FilterCombination = FilterCombination.Or };
                foreach (var item in SelectedItems)
                {
                    expr.Expressions.Add(((PriceInterval)item.Value).GetExpression(PropertyName));
                }
                return expr;
            }
        }        
    }

    public class PriceInterval
    {
        public double MinPrice { get; set; } = double.NegativeInfinity;
        public double MaxPrice { get; set; } = double.PositiveInfinity;

        public Expression GetExpression(string propertyName)
        {
            if (double.IsNegativeInfinity(MinPrice))
                return new OperationExpression() { FilterOperation = FilterOperation.LessThan, Value = MaxPrice, PropertyName = propertyName };
            if (double.IsPositiveInfinity(MaxPrice))
                return new OperationExpression() { FilterOperation = FilterOperation.GreaterThan, Value = MinPrice, PropertyName = propertyName };
            else
            {
                var exp = new CombinationExpression() { FilterCombination = FilterCombination.And };
                exp.Expressions.Add(new OperationExpression() { FilterOperation = FilterOperation.GreaterThanOrEqual, Value = MinPrice, PropertyName = propertyName });
                exp.Expressions.Add(new OperationExpression() { FilterOperation = FilterOperation.LessThanOrEqual, Value = MaxPrice, PropertyName = propertyName });
                return exp;
            }
        }

        public override string ToString()
        {
            if (double.IsNegativeInfinity(MinPrice))
                return $"Less than {string.Format(CultureInfo.CurrentCulture, "{0:C}", MaxPrice)}";
            if (double.IsPositiveInfinity(MaxPrice))
                return $"More than {string.Format(CultureInfo.CurrentCulture, "{0:C}", MinPrice)}";
            else            
                return $"From {string.Format(CultureInfo.CurrentCulture, "{0:C}", MinPrice)} to {string.Format(CultureInfo.CurrentCulture, "{0:C}", MaxPrice)}";            
        }
    }
}
