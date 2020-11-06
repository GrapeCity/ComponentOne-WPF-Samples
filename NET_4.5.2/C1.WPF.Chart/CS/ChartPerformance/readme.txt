ChartPerformance
--------------------------------------------------------------------
This sample shows the performance of the C1Chart with three 1,800 point series.

Here are some performance tips:

1) Set RenderMode property on data series to Fast or Bitmap.

2) For large data sets, use line charts instead of line and symbol charts.
   Drawing many symbols takes time (and if you have too many data points,
   the symbols would make the chart hard to read anyway).

3) If you are binding the X series to values, make sure you set up the labels
   using ItemsValueBinding (and not ItemNamesBinding, which should be used
   only when the X values are strings). For example:

<code>
	var ax = _chart.View.AxisX;
	ax.ItemsValueBinding = new Binding("Month");
    ax.AnnoFormat = "MMM-yy";
</code>

4) If you are binding the X series to DateTime values, make sure you set
   the IsDate property to true. For example:

<code>
	var ax = _chart.View.AxisX;
	ax.IsDate = true;
	ax.ItemsValueBinding = new Binding("Month");
    ax.AnnoFormat = "MMM-yy";
</code>

5) If you are doing any hit-testing, do not attach event handlers to the chart
   elements that represent the symbols. Instead, do the hit-testing at the chart
   level and use code to look for the data point near the mouse.

   The sample implements a TryGetClosestPoint method that is quite general and
   can be re-used in other applications.

   This approach has several advantages when the number of points is large:

	- is more efficient than attaching event handlers to each chart point
    - allows hit testing for line charts (symbol elements are not required)
	- allows near-hit testing (clicking near but not on an element)
	- reduces the chances of memory leaks when the chart is regenerated and
      event handlers attached to old chart elements are not removed.
