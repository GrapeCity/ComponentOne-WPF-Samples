Imports System.Text
Imports System.Collections.Specialized
Imports System.ComponentModel

Namespace Financial
	''' <summary>
	''' Interaction logic for MainWindow.xaml
	''' </summary>
	Partial Public Class MainWindow
		Inherits Window
		Private _financialData As FinancialDataList

		Public Sub New()
			InitializeComponent()
			PopulateFinancialGrid()
		End Sub

		Private Sub PopulateFinancialGrid()
			_financialData = FinancialData.GetFinancialData()
			Dim view = New ListCollectionView(_financialData)
			_flexFinancial.ItemsSource = view
			_flexFinancial.Columns.Frozen = 1
			_flexFinancial.Columns(0).AllowDragging = False

			' configure search box
			_srchCompanies.View = view
			Dim props = _srchCompanies.FilterProperties
			props.Add(GetType(FinancialData).GetProperty("Name"))
			props.Add(GetType(FinancialData).GetProperty("Symbol"))

			' show company info
			UpdateCompanyStatus()

			UpdateCellFactory()
		End Sub

        ' update company status
		Private Sub UpdateCompanyStatus()
			Dim view = TryCast(_flexFinancial.ItemsSource, ICollectionView)
			Dim companies = view.OfType(Of FinancialData)()
            _txtCompanies.Text = String.Format("{0:n0} companies.", (
       From c In companies
       Select c.Symbol).Distinct().Count())
		End Sub

		' control update frequency
		Private Sub _chkAutoUpdate_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			_financialData.AutoUpdate = (CType(sender, CheckBox)).IsChecked.Value
		End Sub
		Private Sub _cmbUpdateInterval_SelectionChanged(ByVal sender As Object, ByVal e As SelectionChangedEventArgs)
			If _financialData IsNot Nothing Then
				Dim cmb = TryCast(sender, ComboBox)
				Dim val = TryCast((CType(cmb.SelectedItem, ComboBoxItem)).Content, String)
				val = val.Split(" "c)(0).Trim()
				_financialData.UpdateInterval = Integer.Parse(val)
			End If
		End Sub
		Private Sub _cmbBatchSize_SelectionChanged(ByVal sender As Object, ByVal e As SelectionChangedEventArgs)
			If _financialData IsNot Nothing Then
				Dim cmb = TryCast(sender, ComboBox)
				Dim val = TryCast((CType(cmb.SelectedItem, ComboBoxItem)).Content, String)
				val = val.Split(" "c)(0).Trim()
				_financialData.BatchSize = Integer.Parse(val)
			End If
		End Sub
		Private Sub _chkOwnerDrawFinancial_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			UpdateCellFactory()
		End Sub

		Private Sub UpdateCellFactory()
			_flexFinancial.CellFactory = If(_chkOwnerDrawFinancial.IsChecked.Value, New FinancialCellFactory(), Nothing)
		End Sub
	End Class
End Namespace
