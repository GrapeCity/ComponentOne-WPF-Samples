Imports System.Reflection
Imports System.ComponentModel
Imports System.Net
Imports System.Windows.Media.Animation

Namespace CustomTypeDescriptor
	Partial Public Class SearchBox
		Inherits UserControl
		Private _propertyInfo As New PropertyList()
		Private _timer As New C1.Util.Timer()

		Public Sub New()
			InitializeComponent()
			_timer.Interval = TimeSpan.FromMilliseconds(800)
			AddHandler _timer.Tick, AddressOf _timer_Tick
		End Sub
		Private privateView As ICollectionView
		Public Property View() As ICollectionView
			Get
				Return privateView
			End Get
			Set(ByVal value As ICollectionView)
				privateView = value
			End Set
		End Property
		Public ReadOnly Property FilterProperties() As PropertyList
			Get
				Return _propertyInfo
			End Get
		End Property
		Private Sub _txtSearch_TextChanged(ByVal sender As Object, ByVal e As TextChangedEventArgs)
			If _imgClear IsNot Nothing Then
				' update clear button visibility
				_imgClear.Visibility = If(String.IsNullOrEmpty(_txtSearch.Text), Visibility.Collapsed, Visibility.Visible)

				' start ticking
				_timer.Stop()
				_timer.Start()
			End If
		End Sub
		Private Sub _imgClear_MouseLeftButtonDown(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
			_txtSearch.Text = String.Empty
		End Sub
		Private Sub _timer_Tick(ByVal sender As Object, ByVal e As EventArgs)
			_timer.Stop()
			If View IsNot Nothing AndAlso _propertyInfo.Count > 0 Then
				View.Filter = Nothing
				View.Filter = Function(item) AnonymousMethod1(item)
			End If
		End Sub
		
		'INSTANT VB TODO TASK: The return type of this anonymous method could not be determined by Instant VB:
		Private Function AnonymousMethod1(ByVal item As Object) As Object
			Dim srch = _txtSearch.Text
			If String.IsNullOrEmpty(srch) Then
				Return True
			End If
			For Each pi As PropertyInfo In _propertyInfo
				Dim value = TryCast(pi.GetValue(item, Nothing), String)
				If value IsNot Nothing AndAlso value.IndexOf(srch, StringComparison.OrdinalIgnoreCase) > -1 Then
					Return True
				End If
			Next pi
			Return False
		End Function
	End Class
	''' <summary>
	''' This list allows adding PropertyDescriptor items in addition to regular
	''' PropertyInfo items. It does this by automatically creating CustomPropertyInfo
	''' objects to wrap the PropertyDescriptor items.
	''' </summary>
	Public Class PropertyList
		Inherits List(Of PropertyInfo)
		' automatically convert PropertyDescriptor into PropertyInfo
		Public Sub Add(ByVal pd As PropertyDescriptor)
			MyBase.Add(New CustomPropertyInfo(pd))
		End Sub
		Friend Class CustomPropertyInfo
			Inherits PropertyInfo
			Private _pd As PropertyDescriptor
			Public Sub New(ByVal pd As PropertyDescriptor)
				_pd = pd
			End Sub
			Public Overrides ReadOnly Property Name() As String
				Get
					Return _pd.Name
				End Get
			End Property
			Public Overrides ReadOnly Property PropertyType() As Type
				Get
					Return _pd.PropertyType
				End Get
			End Property
			Public Overrides Overloads Function GetValue(ByVal obj As Object, ByVal index() As Object) As Object
				Return _pd.GetValue(obj)
			End Function
			Public Overrides Overloads Function GetValue(ByVal obj As Object, ByVal invokeAttr As System.Reflection.BindingFlags, ByVal binder As System.Reflection.Binder, ByVal index() As Object, ByVal culture As System.Globalization.CultureInfo) As Object
				Return _pd.GetValue(obj)
			End Function
			Public Overrides Overloads Sub SetValue(ByVal obj As Object, ByVal value As Object, ByVal index() As Object)
				_pd.SetValue(obj, value)
			End Sub
			Public Overrides Overloads Sub SetValue(ByVal obj As Object, ByVal value As Object, ByVal invokeAttr As System.Reflection.BindingFlags, ByVal binder As System.Reflection.Binder, ByVal index() As Object, ByVal culture As System.Globalization.CultureInfo)
				_pd.SetValue(obj, value)
			End Sub
			Public Overrides ReadOnly Property CanRead() As Boolean
				Get
					Return True
				End Get
			End Property
			Public Overrides ReadOnly Property CanWrite() As Boolean
				Get
					Return Not _pd.IsReadOnly
				End Get
			End Property

			Public Overrides Function IsDefined(ByVal attributeType As Type, ByVal inherit As Boolean) As Boolean
				Throw New NotImplementedException()
			End Function
			Public Overrides Overloads Function GetCustomAttributes(ByVal inherit As Boolean) As Object()
				Throw New NotImplementedException()
			End Function
			Public Overrides Overloads Function GetCustomAttributes(ByVal attributeType As Type, ByVal inherit As Boolean) As Object()
				Throw New NotImplementedException()
			End Function
			Public Overrides ReadOnly Property DeclaringType() As Type
				Get
					Throw New NotImplementedException()
				End Get
			End Property
			Public Overrides ReadOnly Property ReflectedType() As Type
				Get
					Throw New NotImplementedException()
				End Get
			End Property
			Public Overrides ReadOnly Property Attributes() As System.Reflection.PropertyAttributes
				Get
					Throw New NotImplementedException()
				End Get
			End Property
			Public Overrides Function GetIndexParameters() As System.Reflection.ParameterInfo()
				Throw New NotImplementedException()
			End Function
			Public Overrides Overloads Function GetSetMethod(ByVal nonPublic As Boolean) As System.Reflection.MethodInfo
				Throw New NotImplementedException()
			End Function
			Public Overrides Overloads Function GetGetMethod(ByVal nonPublic As Boolean) As System.Reflection.MethodInfo
				Throw New NotImplementedException()
			End Function
			Public Overrides Overloads Function GetAccessors(ByVal nonPublic As Boolean) As System.Reflection.MethodInfo()
				Throw New NotImplementedException()
			End Function
		End Class
	End Class
End Namespace
