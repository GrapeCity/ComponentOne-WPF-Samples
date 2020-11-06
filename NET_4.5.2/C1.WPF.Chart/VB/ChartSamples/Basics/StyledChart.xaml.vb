Imports System.Net
Imports System.Windows.Media.Animation

#If (SILVERLIGHT) Then
Imports C1.Silverlight.Chart
#Else
Imports C1.WPF.C1Chart
Imports Palette = C1.WPF.C1Chart.ColorGeneration
#End If

Namespace ChartSamples
  Partial Public Class StyledChart
	  Inherits UserControl

	Private loaded As Boolean = False
	Private st As ScaleTransform
	Private sampleData As SampleData

	Public Sub New()
	  InitializeComponent()

	  sampleData = New SampleData(AddressOf DataSeries_Loaded)

	  st = New ScaleTransform() With {.ScaleX = 0, .ScaleY = 0.3}
	  c1chart.Data = sampleData.DefaultData
	End Sub

	Public Property ChartType() As ChartType
	  Get
		  Return c1chart.ChartType
	  End Get
	  Set(ByVal value As ChartType)
                If value <> c1chart.ChartType Then
                    ' several chart requires special data
                    If value = ChartType.HighLowOpenClose OrElse value = ChartType.Candle Then
                        c1chart.Data = sampleData.FinancialData
                    ElseIf value = ChartType.Bubble Then
                        c1chart.Data = sampleData.BubbleData
                    ElseIf value = ChartType.Gantt Then
                        c1chart.Data = sampleData.GanttData
                    Else
                        c1chart.Data = sampleData.DefaultData
                    End If

                    StartAnimation()
                    c1chart.ChartType = value
                End If
	  End Set
	End Property

        Public Property C1Chart_ColorGeneration() As Palette
            Get
                Return c1chart.Palette
            End Get
            Set(ByVal value As C1.WPF.C1Chart.ColorGeneration)
                StartAnimation()
                c1chart.Palette = value
            End Set
        End Property

#If (SILVERLIGHT) Then
	Public Property Theme() As ChartTheme
		Get
			Return c1chart.Theme
		End Get
		Set(ByVal value As ChartTheme)
			StartAnimation()
			c1chart.Theme = value
		End Set
	End Property
#End If

	Private Sub StartAnimation()
	  If loaded Then
		st.ScaleX = 0
		st.ScaleY = 0

		Dim da1 As New DoubleAnimation()
		da1.From = 0
		da1.To = 1
		da1.Duration = New Duration(TimeSpan.FromSeconds(0.4))
		'da1.BeginTime = TimeSpan.FromSeconds(0.2);
		Storyboard.SetTargetProperty(da1, New PropertyPath("ScaleX"))
		Dim da2 As New DoubleAnimation()
		da2.From = 0
		da2.To = 1
		da2.Duration = New Duration(TimeSpan.FromSeconds(0.4))
		Storyboard.SetTargetProperty(da2, New PropertyPath("ScaleY"))

		st.BeginAnimation(ScaleTransform.ScaleXProperty, da1)
		st.BeginAnimation(ScaleTransform.ScaleYProperty, da2)
	  End If
	End Sub


	Private Sub c1chart_Loaded(ByVal sender As Object, ByVal e As RoutedEventArgs)
	  loaded = True
	  StartAnimation()
	End Sub

	Private Sub DataSeries_Loaded(ByVal sender As Object, ByVal e As EventArgs)
	  ' DataSeries.Loaded fires whenever plot element related
	  ' to the series is loaded
	  Dim pe As PlotElement = TryCast(sender, PlotElement)
	  If pe IsNot Nothing Then
		pe.RenderTransform = st
		pe.RenderTransformOrigin = New Point(0.5, 0.5)

		AddHandler pe.MouseEnter, AddressOf pe_MouseEnter
		AddHandler pe.MouseLeave, AddressOf pe_MouseLeave
	  End If
	End Sub

	Private Sub pe_MouseLeave(ByVal sender As Object, ByVal e As MouseEventArgs)
	  Dim pe As PlotElement = TryCast(sender, PlotElement)
	  If pe IsNot Nothing Then
#If (SILVELIGHT) Then
		Dim shape As Shape = pe.Shape
#Else
		Dim shape As Shape = pe
#End If
		Canvas.SetZIndex(pe, 0)
		Dim sb As Storyboard = CType(Resources("sbEnter"), Storyboard)
		sb.Stop()

		sb = CType(Resources("sbLeave"), Storyboard)
		sb.Stop()
		Storyboard.SetTarget(sb, shape)
		sb.Begin()

		shape.OpacityMask = Nothing
	  End If
	End Sub

	Private Sub pe_MouseEnter(ByVal sender As Object, ByVal e As MouseEventArgs)
	  Dim pe As PlotElement = TryCast(sender, PlotElement)
	  If pe IsNot Nothing Then
#If (SILVELIGHT) Then
		Dim shape As Shape = pe.Shape
#Else
		Dim shape As Shape = pe
#End If
		shape.OpacityMask = CType(Resources("mask"), Brush)

		If Not(TypeOf pe Is Area) Then
		  Canvas.SetZIndex(pe, 1)
		End If

		Dim sb As Storyboard = CType(Resources("sbEnter"), Storyboard)
		sb.Stop()
		Storyboard.SetTarget(sb, shape)
		sb.Begin()
	  End If
	End Sub
  End Class
End Namespace
