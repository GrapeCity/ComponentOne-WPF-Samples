using C1.DataCollection;
using C1.WPF.DataFilter;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace DataFilterExplorer
{
    public class PriceFilter : ChecklistFilter
    {
        public PriceFilter()
        {
        }

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
            set
            {
                var priceIntervals = GetPriceIntervals(value);
                foreach (var interval in priceIntervals)
                {
                    ChecklistItem ckItem = Items.FirstOrDefault(i => i.Value.Equals(interval));
                    ckItem.Selected = true;
                }
            }
        }

        private IEnumerable<PriceInterval> GetPriceIntervals(Expression expression)
        {
            if (expression is OperationExpression operation)
            {
                if (operation.PropertyName != PropertyName) yield break;

                PriceInterval singleInterval = new PriceInterval();
                if (operation.FilterOperation == FilterOperation.LessThan || operation.FilterOperation == FilterOperation.LessThanOrEqual)
                {
                    singleInterval.MaxPrice = (double)operation.Value;
                }
                else if (operation.FilterOperation == FilterOperation.GreaterThan || operation.FilterOperation == FilterOperation.GreaterThanOrEqual)
                {
                    singleInterval.MinPrice = (double)operation.Value;
                }
                yield return singleInterval;

            }
            else if (expression is CombinationExpression combination)
            {
                if (combination.FilterCombination == FilterCombination.Or)
                {
                    foreach (var e in combination.Expressions)
                    {
                        foreach (var interval in GetPriceIntervals(e))
                        {
                            yield return interval;
                        }
                    }
                }
                else
                {
                    PriceInterval rangeInterval = new PriceInterval();
                    foreach (var e in combination.Expressions)
                    {
                        OperationExpression op = e as OperationExpression;
                        if (op.FilterOperation == FilterOperation.LessThan || op.FilterOperation == FilterOperation.LessThanOrEqual)
                        {
                            rangeInterval.MaxPrice = (double)op.Value;
                        }
                        else if (op.FilterOperation == FilterOperation.GreaterThan || op.FilterOperation == FilterOperation.GreaterThanOrEqual)
                        {
                            rangeInterval.MinPrice = (double)op.Value;
                        }
                    }
                    yield return rangeInterval;
                }
            }
            yield break;
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

        public override bool Equals(object obj)
        {
            if (obj is not PriceInterval)
                return false;

            PriceInterval other = obj as PriceInterval;
            return other.MaxPrice == this.MaxPrice && other.MinPrice == this.MinPrice;
        }
    }
}
