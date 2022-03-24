Imports System.Text
Imports C1.WPF.SpellChecker
Imports C1.WPF.DataGrid
Imports System.Collections
Imports System.Reflection

Namespace SpellCheckerSamples
	Public Class DataGridSpellWrapper
        Implements ISpellCheckableEditor

        Private _grid As C1DataGrid ' DataGrid being checked
        Private _cols As List(Of C1.WPF.DataGrid.DataGridTextColumn) ' columns to be spell-checked
        Private _items As IList ' items being checked
        Private _row, _col As Integer ' cell being checked
        Private _selStart, _selLength As Integer ' selection within cell

        Public Sub New(ByVal grid As C1DataGrid)
            _grid = grid
            _cols = New List(Of C1.WPF.DataGrid.DataGridTextColumn)()
            Initialize()
        End Sub

        '-----------------------------------------------------------------------------
#Region "** ISpellCheckableEditor"

        Public ReadOnly Property Control() As Control Implements C1.WPF.SpellChecker.ISpellCheckableEditor.Control
            Get
                Return _grid
            End Get
        End Property
        Public Property Text() As String Implements C1.WPF.SpellChecker.ISpellCheckableEditor.Text
            Get
                ' get value from data object
                Dim item = _items(_row)
                Dim pi = GetPropertyInfo(item, _cols(_col))

                Return CStr(pi.GetValue(item, Nothing))
            End Get
            Set(ByVal value As String)
                ' set value in data object
                Dim item = _items(_row)
                Dim pi = GetPropertyInfo(item, _cols(_col))
                pi.SetValue(item, value, Nothing)

            End Set
        End Property
        Public Property SelectionStart() As Integer Implements C1.WPF.SpellChecker.ISpellCheckableEditor.SelectionStart
            Get
                Return _selStart
            End Get
            Set(ByVal value As Integer)
                _selStart = value
            End Set
        End Property
        Public Property SelectionLength() As Integer Implements C1.WPF.SpellChecker.ISpellCheckableEditor.SelectionLength
            Get
                Return _selLength
            End Get
            Set(ByVal value As Integer)
                _selLength = value
            End Set
        End Property
        Public Property SelectedText() As String Implements C1.WPF.SpellChecker.ISpellCheckableEditor.SelectedText
            Get
                Return Text.Substring(_selStart, _selLength)
            End Get
            Set(ByVal value As String)
                Dim text As String = Me.Text
                text = String.Format("{0}{1}{2}", text.Substring(0, _selStart), value, text.Substring(_selStart + _selLength))
                Me.Text = text
            End Set
        End Property
        Public Sub [Select](ByVal start As Integer, ByVal length As Integer) Implements C1.WPF.SpellChecker.ISpellCheckableEditor.Select
            ' keep track of selection within the cell
            _selStart = start
            _selLength = length

            ' check that the cell being checked is selected
            _grid.SelectedIndex = _row
            _grid.CurrentColumn = _cols(_col)
            _grid.ScrollIntoView(_grid.SelectedItem, _grid.CurrentColumn)
        End Sub
        Public Function HasMoreText() As Boolean Implements C1.WPF.SpellChecker.ISpellCheckableEditor.HasMoreText
            Return MoveNext()
        End Function
        Public Sub BeginSpell() Implements C1.WPF.SpellChecker.ISpellCheckableEditor.BeginSpell
            Initialize()
        End Sub
        Public Sub EndSpell() Implements C1.WPF.SpellChecker.ISpellCheckableEditor.EndSpell
        End Sub

#End Region

        '-----------------------------------------------------------------------------
#Region "** private stuff"

        ' initialize list of items to check
        Private Sub Initialize()
            'build list of items to check
            _items = TryCast(_grid.ItemsSource, IList)
            If _items Is Nothing Then
                _items = New List(Of Object)()
                For Each item As Object In _grid.ItemsSource
                    _items.Add(item)
                Next item
            End If

            ' build list of text columns
            _cols.Clear()
            For Each col As C1.WPF.DataGrid.DataGridColumn In _grid.Columns
                Dim textCol = TryCast(col, C1.WPF.DataGrid.DataGridTextColumn)
                If textCol IsNot Nothing AndAlso (Not textCol.IsReadOnly) Then
                    _cols.Add(textCol)
                End If
            Next col

            ' move to first cell
            _row = -1
            MoveNext()
        End Sub

        ' move on to the next cell
        Private Function MoveNext() As Boolean
            ' initialize or increment row/col position
            If _row < 0 Then
                ' initialize
                _row = 0
                _col = 0
            ElseIf _col < _cols.Count - 1 Then
                ' next column
                _col += 1
            Else
                ' next row
                _row += 1
                _col = 0
            End If

            ' return true if we still have valid cells
            Return _row < _items.Count AndAlso _col < _cols.Count
        End Function

        ' get PropertyInfo for getting/setting value of the current cell
        Private Function GetPropertyInfo(ByVal item As Object, ByVal column As C1.WPF.DataGrid.DataGridBoundColumn) As PropertyInfo
            Dim propName As String = column.Binding.Path.Path
            Dim pi As PropertyInfo = item.GetType().GetProperty(propName)
            Return pi
        End Function

#End Region

    End Class
End Namespace
