using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace StockChart
{
    class ComparisonQuotesLabel : System.Windows.Controls.Control
    {
        public ComparisonQuotesLabel()
        {
        }
        
        public IEnumerable<QuoteInfo> QuotesInfo
        {
            get
            {
                return (IEnumerable<QuoteInfo>) this.GetValue(QuotesInfoProperty);
            }
            set
            {
                this.SetValue(QuotesInfoProperty, value);
            }
        }


        public readonly static DependencyProperty QuotesInfoProperty = DependencyProperty.Register(
            "QuotesInfo", typeof(IEnumerable<QuoteInfo>), typeof(ComparisonQuotesLabel), new PropertyMetadata(null, 
                new PropertyChangedCallback((o, e) => 
                {
                    ComparisonQuotesLabel cql = o as ComparisonQuotesLabel;
                    if (cql!=null)
                    {
                        cql.InvalidateVisual();
                    }
                })
                )
            );
        
        public DisplayMode DisplayMode
        {
            get
            {
                return (DisplayMode)this.GetValue(DisplayModeProperty);
            }
            set
            {
                this.SetValue(DisplayModeProperty, value);
            }
        }

        public readonly static DependencyProperty DisplayModeProperty = DependencyProperty.Register(
            "DisplayMode", typeof(DisplayMode), typeof(ComparisonQuotesLabel), new PropertyMetadata(DisplayMode.Independent,
                new PropertyChangedCallback((o, e) =>
                {
                    ComparisonQuotesLabel cql = o as ComparisonQuotesLabel;
                    if (cql != null)
                    {
                        cql.InvalidateVisual();
                    }
                })
                )
            );


        private const int GAP = 5;


        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            
            if (this.QuotesInfo == null || this.QuotesInfo.Count() == 0)
            {
                return;
            }

            double right = 0;

            if (this.DisplayMode == DisplayMode.Comparison)
            {

                foreach (var quote in this.QuotesInfo)
                {
                    var format = string.Format((quote.Value >= 0 ? "+{0:P2}" : "{0:P2}"), quote.Value);
                    if (quote.Value == 0)
                    {
                        format = "{0:P2}";
                    }

                    string price = string.Format(format, quote.Value);
                    var fPrice = new FormattedText(price,
                        CultureInfo.CurrentCulture,
                        System.Windows.FlowDirection.LeftToRight,
                        new Typeface(this.FontFamily, this.FontStyle, this.FontWeight, this.FontStretch),
                        12,
                        quote.Value >= 0 ? new SolidColorBrush(Color.FromArgb(255, 18, 155, 20))
                        : new SolidColorBrush(Color.FromArgb(255, 255, 30, 0))
                        );
                    
                    var thisWidth = GAP + fPrice.Width;
                    dc.DrawText(fPrice, new System.Windows.Point(this.ActualWidth - right - thisWidth, (this.ActualHeight - fPrice.Height) / 2));
                    right += thisWidth;
                    
                    var fCode = new FormattedText(quote.Code,
                        CultureInfo.CurrentCulture,
                        System.Windows.FlowDirection.LeftToRight,
                        new Typeface(this.FontFamily, this.FontStyle, FontWeights.Bold, this.FontStretch),
                        12,
                        new SolidColorBrush(quote.Color)
                        );
                    
                    thisWidth = GAP + fCode.Width;
                    dc.DrawText(fCode, new System.Windows.Point(this.ActualWidth - right - thisWidth, (this.ActualHeight - fCode.Height) / 2));
                    right += thisWidth;

                }
            }
            else
            {
                Color color = Color.FromArgb(255, 136, 136, 136);

                var quote = this.QuotesInfo.FirstOrDefault();

                if (quote != null)
                {
                    string volume = string.Format("Volume: {0:D0}", quote.Volume);
                    var fVolume = new FormattedText(volume,
                      CultureInfo.CurrentCulture,
                      System.Windows.FlowDirection.LeftToRight,
                      new Typeface(this.FontFamily, this.FontStyle, this.FontWeight, this.FontStretch),
                      12,
                      new SolidColorBrush(color)
                      );
                    var volumeWidth = GAP + fVolume.Width;


                    string price = string.Format("Price: {0:.##}", quote.Value);
                    var fPrice = new FormattedText(price,
                      CultureInfo.CurrentCulture,
                      System.Windows.FlowDirection.LeftToRight,
                      new Typeface(this.FontFamily, this.FontStyle, this.FontWeight, this.FontStretch),
                      12,
                      new SolidColorBrush(color)
                      );
                    var priceWidth = GAP + fPrice.Width;


                    string date = string.Format("{0:MMM dd, yyyy}", quote.Date);
                    var fDate = new FormattedText(date,
                      CultureInfo.CurrentCulture,
                      System.Windows.FlowDirection.LeftToRight,
                      new Typeface(this.FontFamily, this.FontStyle, this.FontWeight, this.FontStretch),
                      12,
                      new SolidColorBrush(color)
                      );
                    var dateWidth = GAP + fDate.Width;

                    var totalWidth = dateWidth + priceWidth + volumeWidth;


                    dc.DrawText(fVolume, new System.Windows.Point(this.ActualWidth - volumeWidth, (this.ActualHeight - fVolume.Height) / 2));

                    dc.DrawText(fPrice, new System.Windows.Point(this.ActualWidth - volumeWidth - priceWidth, (this.ActualHeight - fPrice.Height) / 2));


                    dc.DrawText(fDate, new System.Windows.Point(this.ActualWidth - volumeWidth - priceWidth - dateWidth, (this.ActualHeight - fDate.Height) / 2));
                    
                }
            }


        }


    }


    public enum DisplayMode
    {
        Independent,
        Comparison,
    }
}
