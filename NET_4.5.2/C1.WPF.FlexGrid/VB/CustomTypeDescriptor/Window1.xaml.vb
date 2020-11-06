Imports System.Text
Imports System.Reflection
Imports System.ComponentModel
Imports C1.WPF.FlexGrid

Namespace CustomTypeDescriptor
	''' <summary>
	''' Interaction logic for Window1.xaml
	''' </summary>
	Partial Public Class Window1
		Inherits Window
		Private _mainView As ICollectionView

		Public Sub New()
			InitializeComponent()

			' DataTable as source
			Dim dt = New DataTable()
			dt.Columns.Add("Name", GetType(String))
			dt.Columns.Add("Serial", GetType(Integer))
			dt.Columns.Add("Price", GetType(Decimal))
			dt.Columns.Add("Current", GetType(Boolean))
			Dim rnd = New Random()
			For r As Integer = 0 To 99
				dt.Rows.Add(String.Format("item {0}", r), r, New Decimal(rnd.NextDouble() * 10000 + 1000), rnd.NextDouble() < 0.8)
			Next r
			Dim dtView = New ListCollectionView(dt.DefaultView)

			' CustomTypeDescriptor as source
			Dim fd = FinancialData.GetFinancialData()
			Dim fdView = New ListCollectionView(fd)

			' bind c1 grids
			_flexDataTable.ItemsSource = dtView
			_flexFinancial.ItemsSource = fdView

			' save main view to customize in event handlers
			_mainView = fdView
			AddHandler _mainView.CollectionChanged, AddressOf _mainView_CollectionChanged

			' configure search box
			_search.View = _mainView
			Dim ctd = TryCast(_mainView.CurrentItem, ICustomTypeDescriptor)
			For Each pd As PropertyDescriptor In ctd.GetProperties()
				If pd.Name = "Symbol" OrElse pd.Name = "Name" Then
					_search.FilterProperties.Add(pd)
				End If
			Next pd

			' customize group row content
			_flexFinancial.GroupHeaderConverter = New SymbolGroupHeaderConverter()
		End Sub

		' event handlers
		Private Sub _chkGroup_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			' toggle grouping
			_mainView.GroupDescriptions.Clear()
			If _chkGroup.IsChecked.Value Then
				_mainView.GroupDescriptions.Add(New PropertyGroupDescription("Symbol"))
				Dim gd = TryCast(_mainView.GroupDescriptions(0), PropertyGroupDescription)
				gd.Converter = New StringInitialConverter()
			End If
		End Sub
		Private Sub _chkCustomCells_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			If _chkCustomCells.IsChecked.Value Then
				' create custom cell factory
				_flexFinancial.CellFactory = New FinancialCellFactory()

				' make custom columns wider
				For Each id As String In "LastSale,Bid,Ask".Split(","c)
					_flexFinancial.Columns(id).Width = New GridLength(150)
				Next id
			Else
				_flexFinancial.CellFactory = Nothing
			End If
		End Sub
		Private Sub _mainView_CollectionChanged(ByVal sender As Object, ByVal e As System.Collections.Specialized.NotifyCollectionChangedEventArgs)
			Dim count = ( _
			    From x As Object In _mainView _
			    Select x).Count()
			_txtStatus.Text = String.Format("Record Count: {0:n0}", count)

			' note: this works too, but the count includes the group rows:
			'var count = _flexFinancial.Rows.Count;
		End Sub

		' converter used to group strings by their first letter
		Private Class StringInitialConverter
			Implements IValueConverter
			Public Function Convert(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.Convert
				Return (CStr(value))(0).ToString().ToUpper()
			End Function
			Public Function ConvertBack(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
				Throw New NotImplementedException()
			End Function
		End Class

		''' <summary>
		''' Converter used to customize content of group rows.
		''' </summary>
		Private Class SymbolGroupHeaderConverter
			Implements IValueConverter
			Public Function Convert(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.Convert
				Dim gr = TryCast(parameter, GroupRow)
				Dim group = gr.Group
				If group IsNot Nothing AndAlso gr IsNot Nothing AndAlso targetType Is GetType(String) Then
					Return String.Format("{0:n0} symbols starting with '{1}'", group.ItemCount, group.Name)
				End If

				' default
				Return value
			End Function
			Public Function ConvertBack(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
				Throw New NotImplementedException()
			End Function
		End Class
	End Class
End Namespace
