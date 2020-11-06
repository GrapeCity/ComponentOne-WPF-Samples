Imports System.Collections
Imports C1.WPF

Namespace C1TreeViewDragDropSample2010
	''' <summary>
	''' Represents a Helper class used to implement the custom Drag&Drop functionality
	''' for a C1TreeViewItem.
	''' </summary>
	Public Class CustomDragDrop
		''' <summary>
		''' Initializes a new instance of a <see cref="CustomDragDrop"/> object.
		''' </summary>
		''' <param name="tree"></param>
		Public Sub New(ByVal tree As C1TreeView)
			Me.Tree = tree
			Mark = New DropMark()
			Effect = DragDropEffect.Move
		End Sub

		''' <summary>
		''' The C1TreeView that will use this custom Drag&Drop implementation.
		''' </summary>
		Private Property Tree() As C1TreeView

		''' <summary>
		''' A custom visual element used to indicate what the target of the drop will be.
		''' </summary>
		Private Property Mark() As DropMark

		''' <summary>
		''' The desired effect for the drop action (move or copy elements)
		''' </summary>
		Public Property Effect() As DragDropEffect

		''' <summary>
		''' Used to handle the DragOver event.
		''' Adds the mark to indicate the drop target
		''' Determines if a given node of the tree is a valid drop location
		''' </summary>
		''' <param name="source">The source of the event</param>
		''' <param name="e">The event arguments</param>
		Public Sub HandleDragOver(ByVal source As Object, ByVal e As DragDropEventArgs)
			Dim dragDropManager As C1DragDropManager = TryCast(source, C1DragDropManager)
			If Mark.Parent Is Nothing Then
				dragDropManager.Canvas.Children.Add(Mark)
			End If
			Mark.Visibility = Visibility.Collapsed
			e.Effect = DragDropEffect.None

			' Check if is a valid drop action
			Dim target As C1TreeViewItem = Tree.GetNode(e.GetPosition(Nothing))
			If IsValidDrop(TryCast(e.DragSource, C1TreeViewItem), target) Then
				' get target with respect to drag/drop canvas
                Dim p As System.Windows.Point = target.C1TransformToVisual(dragDropManager.Canvas).Transform(New System.Windows.Point())
                Dim pos As System.Windows.Point = e.GetPosition(target)
				If pos.Y > target.RenderSize.Height \ 2 Then
					p.Y += target.RenderSize.Height
				End If
				Mark.SetValue(Canvas.LeftProperty, p.X)
				Mark.SetValue(Canvas.TopProperty, p.Y)
				Mark.Visibility = Visibility.Visible
				e.Effect = DragDropEffect.Move
			End If
		End Sub

		''' <summary>
		''' Used to handle the DragDrop event.
		''' Determines the target and fires the corresponding action (move or copy).
		''' </summary>
		''' <param name="source">The source of the event</param>
		''' <param name="e">The event arguments</param>
		Public Sub HandleDragDrop(ByVal source As Object, ByVal e As DragDropEventArgs)
			If e.Effect <> DragDropEffect.None Then
				' make the move...
				Dim item As C1TreeViewItem = TryCast(e.DragSource, C1TreeViewItem)
				If item IsNot Nothing Then
					' find target node
					Dim target As C1TreeViewItem = Tree.GetNode(e.GetPosition(Nothing))
					If target IsNot Nothing Then
                        Dim p As System.Windows.Point = e.GetPosition(target)
						Dim insertAfter As Boolean = (p.Y > target.RenderSize.Height \ 2)
                        If Effect = DragDropEffect.Move Then
                            DoMove(item, target, insertAfter)
                        Else
                            DoCopy(item, target, insertAfter)
                        End If
					End If
				End If
			End If
		End Sub

		''' <summary>
		''' Returns true if the target node is a valid drop location.
		''' For this sample, a valid target node is the one that:
		'''     Represents an employee (departments are not valid)
		'''     Is not being copied to the same department it is assigned
		'''     Is not being moved to a department it is already assigned
		''' </summary>
		''' <param name="source"></param>
		''' <param name="target"></param>
		''' <returns></returns>
		Private Function IsValidDrop(ByVal source As C1TreeViewItem, ByVal target As C1TreeViewItem) As Boolean
			Dim isValidTarget As Boolean = False
			' For this sample we will allow dragging of employees only
			If (source IsNot Nothing) AndAlso (target IsNot Nothing) AndAlso (TypeOf source.Header Is Employee) AndAlso (TypeOf target.Header Is Employee) Then
				' Cannot copy employees in the same department
				Dim sourceDataObject As Employee = CType(source.Header, Employee)
				Dim isAlreadyInDepartment As Boolean = (CType(target.ParentItemsSource, IList)).Contains(sourceDataObject)
                If (isAlreadyInDepartment) AndAlso ((Effect = DragDropEffect.Copy) OrElse (source.Parent IsNot target.Parent)) Then
                    isValidTarget = False
                Else
                    isValidTarget = ((Not source.IsAncestorOf(target)) AndAlso Not (source Is target))
                End If
			End If
			Return isValidTarget
		End Function

		''' <summary>
		''' Copies a node to the same collection the target node belongs to.
		''' </summary>
		''' <param name="source">The source node to copy</param>
		''' <param name="target">The target node</param>
		''' <param name="copyAfter">Indicates if the source node will be copied after
		''' (larger index) the target </param>
		Private Sub DoCopy(ByVal source As C1TreeViewItem, ByVal target As C1TreeViewItem, ByVal copyAfter As Boolean)
			Dim targetIndex As Integer = target.Index
			Dim targetCollection As IList = CType(target.ParentItemsSource, IList)

			' adjust target index if necessary (insert after or before the target)
			If copyAfter Then
				targetIndex += 1
			End If

			' copy source node and insert
			Dim sourceCopy As Employee = (CType(source.Header, Employee)).Clone()
			targetCollection.Insert(targetIndex, sourceCopy)
		End Sub

		''' <summary>
		''' Moves a node to the same collection the target node belongs to.
		''' </summary>
		''' <param name="source">The source node to move</param>
		''' <param name="target">The target node</param>
		''' <param name="copyAfter">Indicates if the source node will be moved after
		''' (larger index) the target </param>
		Private Sub DoMove(ByVal source As C1TreeViewItem, ByVal target As C1TreeViewItem, ByVal moveAfter As Boolean)
			' get source collection/index
			Dim sourceIndex As Integer = source.Index
			Dim targetIndex As Integer = target.Index
			Dim sourceCollection As IList = CType(source.ParentItemsSource, IList)
			Dim targetCollection As IList = CType(target.ParentItemsSource, IList)

			' adjust target index if necessary (insert after or before the target)
			If moveAfter Then
				targetIndex += 1
			End If
			If sourceCollection Is targetCollection AndAlso sourceIndex < targetIndex Then
				targetIndex -= 1
			End If

			' remove node from old position, insert into new
			sourceCollection.RemoveAt(sourceIndex)
			targetCollection.Insert(targetIndex, source.Header)
		End Sub
	End Class
End Namespace
