Imports System.Collections
Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.Reflection
Imports System.Text

Imports C1.Schedule
Imports C1.WPF

Namespace CustomDialogs
	''' <summary>
	''' Interaction logic for SelectFromListScene.xaml
	''' </summary>
	Partial Public Class SelectFromListScene
		Inherits UserControl
		#Region "** fields"
		Private _sourceCollection As IList
		Private _sourceCollectionEdited As IList
		Private _targetCollection As IList
		Private _targetCollectionEdited As IList
		Private _itemType As Type
		Private _allowEdit As Boolean = True
		Private _allowMultipleSelection As Boolean = True
		Private _cleaningExtraItems As Boolean = False
		Private _sortedSourceView As CollectionView
		Private _isLoaded As Boolean = False
		Private _parentWindow As ContentControl = Nothing
		#End Region

		#Region "** initializing"
		''' <summary>
		''' Initializes the new instance of the <see cref="SelectFromListScene"/> control.
		''' </summary>
		Public Sub New()
			InitializeComponent()
		End Sub

		Private Sub UserControl_Loaded(ByVal sender As Object, ByVal e As RoutedEventArgs)
			_isLoaded = True
			If Not System.Windows.Interop.BrowserInteropHelper.IsBrowserHosted Then
				_parentWindow = CType(C1.WPF.VTreeHelper.GetParentOfType(Me, GetType(Window)), ContentControl)
			Else
			_parentWindow = CType(C1.WPF.VTreeHelper.GetParentOfType(Me, GetType(C1Window)), ContentControl)
			End If
			If _parentWindow IsNot Nothing Then
				sourceListBox.ItemsSource = SortedSourceView
				sourceListBox.SelectedIndex = -1
				sourceListBox.Focus()
				If ItemType IsNot GetType(Category) Then
					resetButton.Visibility = Visibility.Collapsed
				End If
				UpdateSelectedValues()
				Dim allowEdit = TryFindResource("AllowEdit")
				If allowEdit IsNot Nothing Then
					Me.AllowEdit = CBool(allowEdit)
				End If
				Dim allowMultipleSelection = TryFindResource("AllowMultipleSelection")
				If allowMultipleSelection IsNot Nothing Then
					Me.AllowMultipleSelection = CBool(allowMultipleSelection)
				End If
			End If
		End Sub

		Private Sub SourceItemSelectedCB_Loaded(ByVal sender As Object, ByVal e As RoutedEventArgs)
			Dim chk As CheckBox = TryCast(sender, CheckBox)
			If chk Is Nothing Then
				Return
			End If
			If sourceListBox.SelectedIndex < 0 Then
				sourceListBox.SelectedIndex = 0
				chk.Focus()
			End If
			UpdateCheckBoxes()
		End Sub
		#End Region

		#Region "** public interface"
		''' <summary>
		''' Gets or sets boolean value determining whether dialog should allow adding or deleting items.
		''' </summary>
		''' <value>The default value is true.</value>
		Public Property AllowEdit() As Boolean
			Get
				Return _allowEdit
			End Get
			Set(ByVal value As Boolean)
				_allowEdit = value
				If (Not _allowEdit) AndAlso _isLoaded Then
					deleteButton.Visibility = Visibility.Collapsed
					newItemText.Visibility = deleteButton.Visibility
					addButton.Visibility = newItemText.Visibility
					resetButton.Visibility = addButton.Visibility
				End If
			End Set
		End Property

		''' <summary>
		''' Gets or sets boolean value determining whether multiple selection is allowed.
		''' </summary>
		''' <value>The default value is true.</value>
		Public Property AllowMultipleSelection() As Boolean
			Get
				Return _allowMultipleSelection
			End Get
			Set(ByVal value As Boolean)
				_allowMultipleSelection = value
				If (Not _allowMultipleSelection) AndAlso _isLoaded Then
					sourceListBox.SelectionMode = SelectionMode.Single
				End If
			End Set
		End Property

		''' <summary>
		''' Gets the reference to the source collection.
		''' </summary>
		Public ReadOnly Property SourceCollection() As IList
			Get
				_sourceCollection = If(_sourceCollection, TryCast(TryFindResource("SourceCollection"), IList))
				Return _sourceCollection
			End Get
		End Property

		''' <summary>
		''' Gets the reference to the target collection.
		''' </summary>
		Public ReadOnly Property TargetCollection() As IList
			Get
				_targetCollection = If(_targetCollection, TryCast(TryFindResource("TargetCollection"), IList))
				Return _targetCollection
			End Get
		End Property

		''' <summary>
		''' Gets the reference to the edited copy of the target collection.
		''' </summary>
		Public ReadOnly Property TargetCollectionEdited() As IList
			Get
				If _targetCollectionEdited Is Nothing Then
					_targetCollectionEdited = New BaseObjectList()
					For Each o As Object In TargetCollection
						_targetCollectionEdited.Add(o)
					Next o
				End If
				Return _targetCollectionEdited
			End Get
		End Property

		''' <summary>
		''' Gets the reference to the edited copy of the source collection.
		''' </summary>
		Public ReadOnly Property SourceCollectionEdited() As IList
			Get
				If _sourceCollectionEdited Is Nothing Then
					If TypeOf SourceCollection Is ResourceCollection Then
						_sourceCollectionEdited = New ResourceCollection()
					ElseIf TypeOf SourceCollection Is CategoryCollection Then
						_sourceCollectionEdited = New CategoryCollection()
						_sourceCollectionEdited.Clear()
					ElseIf TypeOf SourceCollection Is ContactCollection Then
						_sourceCollectionEdited = New ContactCollection()
					Else
						_sourceCollectionEdited = New System.Collections.ObjectModel.ObservableCollection(Of Object)()
					End If
					For Each o As Object In SourceCollection
						_sourceCollectionEdited.Add(o)
					Next o
				End If
				Return _sourceCollectionEdited
			End Get
		End Property

		''' <summary>
		''' Gets the type of items in collections.
		''' </summary>
		Public ReadOnly Property ItemType() As Type
			Get
				_itemType = If(_itemType, TryCast(TryFindResource("ItemType"), Type))
				Return _itemType
			End Get
		End Property

		''' <summary>
		''' Gets the sorted view of SourceCollectionEdited to display in the list of available objects.
		''' </summary>
		Public ReadOnly Property SortedSourceView() As CollectionView
			Get
				If _sortedSourceView Is Nothing Then
					_sortedSourceView = New ListCollectionView(SourceCollectionEdited)
					_sortedSourceView.SortDescriptions.Add(New SortDescription("Text", ListSortDirection.Ascending))
				End If
				Return _sortedSourceView
			End Get
		End Property
		#End Region

		#Region "** private stuff"
		Private Sub UpdateSelectedValues()
			selectedValues.Text = CType(TargetCollectionEdited, Object).ToString()
		End Sub

		Private Overloads Function TryFindResource(ByVal key As String) As Object
			If _parentWindow IsNot Nothing Then
				If _parentWindow.Resources.Contains(key) Then
					Return _parentWindow.Resources(key)
				End If
			End If
			Return Nothing
		End Function

		Private Sub SourceItemSelectedCB_GotFocus(ByVal sender As Object, ByVal e As RoutedEventArgs)
			Dim uiItem As FrameworkElement = TryCast(e.OriginalSource, FrameworkElement)
			If uiItem Is Nothing Then
				Return
			End If
			Dim item As Object = uiItem.DataContext
            ' the second check is required for WPF 4
            If item Is Nothing OrElse (Not item.GetType().IsSubclassOf(GetType(C1.Schedule.BasePersistableObject))) Then
                Return
            End If
            sourceListBox.SelectedItem = item
		End Sub

		'[Obfuscation(Exclude = true)]
		Private Sub SourceItemSelectedCB_Checked(ByVal sender As Object, ByVal e As RoutedEventArgs)
			AddOrRemoveItem(True, e)
		End Sub

		'[Obfuscation(Exclude = true)]
		Private Sub SourceItemSelectedCB_Unchecked(ByVal sender As Object, ByVal e As RoutedEventArgs)
			AddOrRemoveItem(False, e)
		End Sub


		Private Sub AddOrRemoveItem(ByVal add As Boolean, ByVal e As RoutedEventArgs)
			If (Not _isLoaded) OrElse _cleaningExtraItems OrElse TargetCollection Is Nothing Then
				Return
			End If
			Dim uiItem As FrameworkElement = TryCast(e.OriginalSource, FrameworkElement)
			If uiItem Is Nothing Then
				Return
			End If
			AddOrRemoveItem(add, uiItem.DataContext)
		End Sub

		Private Sub AddOrRemoveItem(ByVal add As Boolean, ByVal item As Object)
			If (Not _isLoaded) OrElse _cleaningExtraItems OrElse TargetCollection Is Nothing Then
				Return
			End If
			If item Is Nothing Then
				Return
			End If
			If add Then
				AddItemToTargetCollection(item)
			Else
				TargetCollectionEdited.Remove(item)
			End If
			UpdateCheckBoxes()
			UpdateSelectedValues()
		End Sub

		Private Sub UpdateCheckBoxes()
			If (Not _isLoaded) OrElse _cleaningExtraItems OrElse TargetCollection Is Nothing Then
				Return
			End If
			Try
				_cleaningExtraItems = True

				For Each obj As Object In Me.sourceListBox.Items
					Dim container As ListBoxItem = TryCast(Me.sourceListBox.ItemContainerGenerator.ContainerFromItem(obj), ListBoxItem)
					If container IsNot Nothing Then
						Dim list As IList(Of DependencyObject) = New List(Of DependencyObject)()
						VTreeHelper.GetChildrenOfType(container, GetType(CheckBox), list)
						If list.Count > 0 Then
							CType(list(0), CheckBox).IsChecked = TargetCollectionEdited.Contains(obj)
						End If
					End If
				Next obj
			Finally
				_cleaningExtraItems = False
			End Try
		End Sub

		Private Shared Function FindItemWithText(ByVal list As IList, ByVal text As String) As Object
			If list Is Nothing OrElse text Is Nothing OrElse text.Trim().Length = 0 Then
				Return Nothing
			End If
			Dim textProp As PropertyInfo = Nothing
			For Each curItem As Object In list
				If curItem IsNot Nothing Then
					textProp = If(textProp, curItem.GetType().GetProperty("Text"))
					If textProp Is Nothing Then
						Return Nothing
					End If
					Dim curText As String = TryCast(textProp.GetValue(curItem, Nothing), String)
					If String.Compare(text.ToLower(), curText.ToLower()) = 0 Then
						Return curItem
					End If
				End If
			Next curItem

			Return Nothing
		End Function

		Private Sub addButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			Dim tb As TextBox = newItemText
			Dim str As String = tb.Text
			If Not String.IsNullOrEmpty(str) Then
				Dim obj As Object = FindItemWithText(SourceCollectionEdited, str)
				If obj Is Nothing Then
                    If ItemType Is GetType(Category) Then
                        Dim cat As New Category(str)
                        cat.MenuCaption = str
                        obj = cat
                    ElseIf ItemType Is GetType(Contact) Then
                        Dim cnt As New Contact()
                        cnt.Text = str
                        cnt.MenuCaption = str
                        obj = cnt
                    ElseIf ItemType Is GetType(Resource) Then
                        Dim res As New Resource()
                        res.Text = str
                        res.MenuCaption = str
                        obj = res
                    ElseIf ItemType Is GetType(C1.Schedule.Label) Then
                        obj = New C1.Schedule.Label(str, str)
                    ElseIf ItemType Is GetType(Status) Then
						obj = New Status(str, str)
					End If
				End If
				If obj IsNot Nothing Then
					SourceCollectionEdited.Add(obj)
					AddOrRemoveItem(True, obj)
				End If
				tb.ClearValue(TextBox.TextProperty)
			End If
		End Sub

		Private Sub AddItemToTargetCollection(ByVal item As Object)
			If item IsNot Nothing Then
				If Not AllowMultipleSelection Then
					TargetCollectionEdited.Clear()
				End If
				If Not TargetCollectionEdited.Contains(item) Then
					TargetCollectionEdited.Add(item)
				End If
			End If
		End Sub

		Private Sub deleteButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			Dim o As Object = sourceListBox.SelectedItem
			If o IsNot Nothing Then
				SourceCollectionEdited.Remove(o)
				TargetCollectionEdited.Remove(o)
			End If
			UpdateCheckBoxes()
			UpdateSelectedValues()
		End Sub

		Private Sub resetButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			Dim coll As CategoryCollection = TryCast(SourceCollectionEdited, CategoryCollection)
			If coll IsNot Nothing Then
				coll.LoadDefaults()
				For i As Integer = TargetCollectionEdited.Count - 1 To 0 Step -1
					Dim category As Category = TryCast(TargetCollectionEdited(i), Category)
					If Not coll.Contains(category) Then
						TargetCollectionEdited.RemoveAt(i)
					End If
				Next i
			End If
			UpdateCheckBoxes()
			UpdateSelectedValues()
		End Sub

		Private Sub okButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			For i As Integer = SourceCollection.Count - 1 To 0 Step -1
				Dim o As Object = SourceCollection(i)
				If Not SourceCollectionEdited.Contains(o) Then
					SourceCollection.RemoveAt(i)
				End If
			Next i
			For Each o As Object In SourceCollectionEdited
				If Not SourceCollection.Contains(o) Then
					SourceCollection.Add(o)
				End If
			Next o
			For i As Integer = TargetCollection.Count - 1 To 0 Step -1
				Dim o As Object = TargetCollection(i)
				If Not TargetCollectionEdited.Contains(o) Then
					TargetCollection.RemoveAt(i)
				End If
			Next i
			For Each o As Object In TargetCollectionEdited
				If Not TargetCollection.Contains(o) Then
					TargetCollection.Add(o)
				End If
			Next o
			cancelButton_Click(sender, e)
		End Sub

		Private Sub cancelButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			If _parentWindow Is Nothing Then
				Return
			End If
			If TypeOf _parentWindow Is Window Then
				CType(_parentWindow, Window).Close()
			Else
			If TypeOf _parentWindow Is C1Window Then
				CType(_parentWindow, C1Window).Close()
			End If
			End If
		End Sub
		Private Sub accessKeyPressed(ByVal sender As Object, ByVal e As AccessKeyPressedEventArgs)
			If Keyboard.Modifiers <> ModifierKeys.Alt Then
				e.Scope = sender
				e.Handled = True
			End If
		End Sub
		Protected Overrides Sub OnPreviewKeyDown(ByVal e As KeyEventArgs)
			If e.Key = Key.Escape Then
				Dim parentControl As DependencyObject = VTreeHelper.GetParentOfType(CType(e.OriginalSource, DependencyObject), Me.GetType())
				If parentControl IsNot Nothing Then
					cancelButton_Click(Me, Nothing)
				End If
			End If
			MyBase.OnPreviewKeyDown(e)
		End Sub
        Protected Overrides Sub OnMouseWheel(ByVal e As System.Windows.Input.MouseWheelEventArgs)
            e.Handled = True
            MyBase.OnMouseWheel(e)
        End Sub
		#End Region
	End Class


	Public Class BaseObjectList
		Inherits ObservableCollection(Of BaseObject)
		Public Sub New()
			MyBase.New()
		End Sub

		''' <summary>
		''' Overrides the default behavior.
		''' Returned text will be displayed in selectedValues TextBox.
		''' </summary>
		''' <returns></returns>
		Public Overrides Function ToString() As String
			Dim sb As New StringBuilder()
			For Each item As BaseObject In Items
				sb.Append(item.ToString() & "; ")
			Next item
			Return sb.ToString()
		End Function

	End Class

End Namespace