## ConditionalFormatting Olap
#### [Download as zip](https://downgit.github.io/#/home?url=https://github.com/GrapeCity/ComponentOne-WPF-Samples/tree/master/NET_4.5.2/C1.WPF.Olap/VB/ConditionalFormatting/ConditionalFormatting)
____
#### Shows how to apply conditional formatting to an OlapGrid control.
____
The C1OlapField class has three properties that determine how the field
is displayed in a C1OlapGrid:

Style: Specifies the default background, foreground, and font weight
properties for the field. This property is useful mainly when the view
contains multiple value fields.

StyleHigh: Specifies the background, foreground, and font weight properties 
for cells that contain "high" values. The style also specifies the criteria
for selecting high values (absolute or percentile).

StyleLow: Specifies the background, foreground, and font weight properties 
for cells that contain "low" values. The style also specifies the criteria
for selecting low values (absolute or percentile).

The sample loads some sample data and uses this code to build an OLAP view 
with conditional formatting:

```
    // prepare to build view 
    _c1OlapPage.DataSource = ds.Tables[0].DefaultView;
    var olap = _c1OlapPage.OlapPanel.OlapEngine;
    olap.BeginUpdate();

    // show sales by country and category/product
    olap.ColumnFields.Add("Country");
    olap.RowFields.Add("Category", "Product");
    olap.Fields["Product"].Width = 250;
    var value = olap.Fields["Sales"];
    olap.ValueFields.Add(value);
    value.Format = "n0";

    // show values in the top 90% percentile in green
    var sh = value.StyleHigh;
    sh.Value = .90;
    sh.ConditionType = C1.Olap.ConditionType.Percentage;
    sh.ForeColor = Color.FromArgb(0xff, 0, 50, 0);
    sh.BackColor = Color.FromArgb(0xff, 200, 250, 200);
    sh.FontBold = true;

    // show values in the bottom 2% percentile in red
    var sl = value.StyleLow;
    sl.Value = .02;
    sl.ConditionType = C1.Olap.ConditionType.Percentage;
    sl.ForeColor = Color.FromArgb(0xff, 50, 0, 0);
    sl.BackColor = Color.FromArgb(0xff, 250, 200, 200);
    sl.FontBold = true;

    // view is ready
    olap.EndUpdate();
```