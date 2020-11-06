Imports System.Text
Imports C1.WPF.FlexGrid

Namespace ExcelGrid
	''' <summary>
	''' Specifies the color scheme used to display the control.
	''' </summary>
	Public Enum ColorScheme
		''' <summary>
		''' Office 2007 blue color sheme.
		''' </summary>
		Blue
		''' <summary>
		''' Office 2007 silver color sheme.
		''' </summary>
		Silver
		''' <summary>
		''' Office 2007 black color sheme.
		''' </summary>
		Black
	End Enum

	''' <summary>
	''' Class that defines and applies color schemes to C1FlexGrid controls.
	''' </summary>
	Public NotInheritable Class ColorSchemeManager
		Private Sub New()
		End Sub
		Public Shared Sub ApplyColorScheme(ByVal grid As C1FlexGrid, ByVal cs As ColorScheme)
			Dim c = PaletteProvider.GetPalette(cs)
			If c IsNot Nothing Then
				grid.EditorBackground = New SolidColorBrush(Colors.Transparent)
				grid.CursorBackground = grid.EditorBackground
				grid.AlternatingRowBackground = grid.CursorBackground
				grid.RowBackground = grid.AlternatingRowBackground
				grid.TopLeftCellBackground = New SolidColorBrush(c(0))
				grid.RowHeaderBackground = New SolidColorBrush(c(1))
				grid.RowHeaderSelectedBackground = New SolidColorBrush(c(2))
				grid.ColumnHeaderBackground = CreateGradientBrush(c(3), c(4))
				grid.ColumnHeaderSelectedBackground = CreateGradientBrush(c(5), c(6))
				grid.GridLinesBrush = New SolidColorBrush(c(7))

				grid.HeaderGridLinesBrush = New SolidColorBrush(c(8))
				grid.SelectionBackground = New SolidColorBrush(c(9))
			End If
		End Sub
		Private Shared Function CreateGradientBrush(ByVal top As Color, ByVal bot As Color) As LinearGradientBrush
			Dim lgb = New LinearGradientBrush()
			Dim gsc = lgb.GradientStops
			gsc.Add(New GradientStop() With {.Color = top, .Offset = 0})
			gsc.Add(New GradientStop() With {.Color = bot, .Offset = 1})
			lgb.EndPoint = New Point(0, 1)
			Return lgb
		End Function
		Public NotInheritable Class PaletteProvider
			Private Sub New()
			End Sub
			Public Shared Function GetPalette(ByVal scheme As ColorScheme) As Color()
				Select Case scheme
					Case ColorScheme.Blue
						Return New Color() { Color.FromArgb(&Hff, &Ha9, &Hc4, &He9), Color.FromArgb(&Hff, &He4, &Hec, &Hf7), Color.FromArgb(&Hff, &Hff, &Hd5, &H8d), Color.FromArgb(&Hff, &Hf6, &Hfa, &Hfb), Color.FromArgb(&Hff, &Hd5, &Hdd, &Hea), Color.FromArgb(&Hff, &Hf8, &Hd7, &H9b), Color.FromArgb(&Hff, &Hf1, &Hc2, &H63), Color.FromArgb(&Hff, &Hd0, &Hd7, &He5), Color.FromArgb(&Hff, &H9e, &Hb6, &Hce), Color.FromArgb(&Hff, &Hea, &Hec, &Hf5) }
					Case ColorScheme.Silver
						Return New Color() { Color.FromArgb(&Hff, &Hc6, &Hc6, &Hc6), Color.FromArgb(&Hff, &He7, &He7, &He7), Color.FromArgb(&Hff, &Hf5, &Hc7, &H95), Color.FromArgb(&Hff, &Hf1, &Hf3, &Hf3), Color.FromArgb(&Hff, &Hc8, &Hc9, &Hca), Color.FromArgb(&Hff, &Hff, &Hc9, &H96), Color.FromArgb(&Hff, &Hff, &H9b, &H68), Color.FromArgb(&Hff, &Hd0, &Hd7, &He5), Color.FromArgb(&Hff, &H90, &H91, &H92), Color.FromArgb(&Hff, &Hea, &Hec, &Hf5) }
					Case ColorScheme.Black
						Return New Color() { Color.FromArgb(&Hff, &Hc9, &Hc9, &Hc9), Color.FromArgb(&Hff, &Hed, &Hed, &Hed), Color.FromArgb(&Hff, &Hff, &Hd5, &H8d), Color.FromArgb(&Hff, &Hf6, &Hf6, &Hf6), Color.FromArgb(&Hff, &Hde, &Hde, &Hde), Color.FromArgb(&Hff, &Hf8, &Hd7, &H9b), Color.FromArgb(&Hff, &Hf1, &Hc1, &H5f), Color.FromArgb(&Hff, &Hd0, &Hd7, &He5), Color.FromArgb(&Hff, &Hb6, &Hb6, &Hb6), Color.FromArgb(&Hff, &Hea, &Hec, &Hf5) }
				End Select

				' invalid color scheme
				Return Nothing
			End Function
		End Class
	End Class
End Namespace
