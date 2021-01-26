## Validation
#### [Download as zip](https://grapecity.github.io/DownGit/#/home?url=https://github.com/GrapeCity/ComponentOne-WPF-Samples/tree/master/NET_4.5.2/C1.WPF.FlexGrid/CS/Validation)
____
#### Shows how the C1FlexGrid supports validation when the ShowErrors property is set to true.
____
The sample shows several types of validation and compares how the FlexGrid
handles errors compared to the Microsoft DataGrid control. The types of 
validation available are described below.

The main differences in error handling between the FlexGrid and the Microsoft
DataGrid are the following:

1) The FlexGrid has a ShowErrors property that allows you to turn validation
on and off.

2) The FlexGrid has an ErrorStyle property that allows you to control the style
of cells that contain errors. By default, they are shown with a red border. But
you can use the ErrorStyle property to make them display red bold content for 
example, or use any other style you prefer.

3) The FlexGrid shows errors on all rows simultaneously, whether or not the
rows are being edited. The Microsoft DataGrid only shows errors on the row
being edited.

4) The Microsoft DataGrid shows some types of errors in a window docked to
the bottom of the grid. The FlexGrid does not do that.

There are three types of validation in WPF:

1) Classes may perform validation when a property is being set. If the new
value is invalid, the setter simply throws an exception and keeps the original
value. This is the simplest type of validation. For example:

```
	public double Cost
	{
		get { return _cost; }
		set
		{
			if (value <= 0)
			{
				throw new Exception("Cost must be greater than zero!");
			}
			_cost = value;
		}
	}
```
2) Classes may implement the IDataErrorInfo interface and return a property-level
error message when callers inspect the errors by member name, or an item-level
error message get the Error property. For example:

```
	// property-level validation
    string IDataErrorInfo.this[string columnName]
    {
        get
        {
			if (columnName == "Cost" && Cost <= 0)
			{
				return "Cost must be greater than zero!";
			}

			// no errors
            return null; 
        }
    }

	// item-level validation
    string IDataErrorInfo.Error
    {
        get
        {
            return Price < Cost
                ? "Price must be greater than Cost!"
                : null;
        }
    }
```
3) Classes may implement the INotifyDataErrorInfo interface and maintain a list of
errors associated with specific properties or with the whole item. The
INotifyDataErrorInfo interface includes an event that gets fired when the errors
change, so it supports asynchronous validation.

The implementation of INotifyDataErrorInfo is too complex to include here, but you
can find a sample implementation in this project and another here:

	http://msdn.microsoft.com/en-us/library/system.componentmodel.inotifydataerrorinfo(v=vs.95).aspx

4) Classes may use the validation attributes found in the ComponentModel.DataAnnotations
namespace to implement validation in a declarative way. This method doesn't require any code.

If a property-specific validation rule is violated while editing, the editor displays a red 
border around the cell and a tooltip with the error description. The user must then correct 
the error or cancel the changes before they can finish editing.

If an item-level validation rule is violated while editing, the grid displays an error icon 
on the first row header. The error icon has a tooltip with the error message. If multiple 
rows contain errors, they all get marked with the error icon.

Note that validation support in the C1FlexGrid is enabled only when the ShowErrors property
is set to true (ShowErrors is set to true by default).

