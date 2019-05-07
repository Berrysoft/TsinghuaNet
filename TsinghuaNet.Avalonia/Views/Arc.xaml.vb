Imports Avalonia
Imports Avalonia.Controls
Imports Avalonia.Controls.Shapes
Imports Avalonia.Markup.Xaml
Imports Avalonia.Media

Public Class Arc
    Inherits UserControl

    Public Sub New()
        InitializeComponent()
        AddHandler PropertyChanged, AddressOf OnSizeChanged
        DataContext = Me
    End Sub

    Private Sub InitializeComponent()
        AvaloniaXamlLoader.Load(Me)
    End Sub

    Public Shared ReadOnly ThicknessProperty As AvaloniaProperty(Of Double) = AvaloniaProperty.Register(Of Arc, Double)(NameOf(Thickness))
    Public Property Thickness As Double
        Get
            Return GetValue(ThicknessProperty)
        End Get
        Set(value As Double)
            SetValue(ThicknessProperty, value)
        End Set
    End Property

    Public Shared ReadOnly ValueProperty As AvaloniaProperty(Of Double) = AvaloniaProperty.Register(Of Arc, Double)(NameOf(Value), validate:=AddressOf CoerceValue)
    Public Property Value As Double
        Get
            Return GetValue(ValueProperty)
        End Get
        Set(value As Double)
            SetValue(ValueProperty, value)
        End Set
    End Property
    Private Shared Function CoerceValue(o As AvaloniaObject, e As Double) As Double
        Return Math.Min(1, Math.Max(0, e))
    End Function

    Public Shared ReadOnly FillProperty As AvaloniaProperty(Of Brush) = AvaloniaProperty.Register(Of Arc, Brush)(NameOf(Fill))
    Public Property Fill As Brush
        Get
            Return GetValue(FillProperty)
        End Get
        Set(value As Brush)
            SetValue(FillProperty, value)
        End Set
    End Property

    Private Sub OnSizeChanged(sender As Object, e As AvaloniaPropertyChangedEventArgs)
        If e.Property.Name = NameOf(Thickness) OrElse e.Property.Name = NameOf(Value) Then
            Dim a As Arc = sender
            a.DrawArc()
        End If
    End Sub

    Private Shared Function GetAngle(value As Double) As Double
        Dim angle = value * 2 * Math.PI
        If angle >= 2 * Math.PI Then
            angle = 2 * Math.PI - 0.000001
        End If
        Return angle
    End Function

    Private Shared Function ScaleUnitCirclePoint(origin As Point, radius As Double, angle As Double) As Point
        Dim dir As New Vector(-Math.Sin(angle), Math.Cos(angle))
        Return dir * radius + origin
    End Function

    Private Sub DrawArc()
        Dim size As New Size(Width, Height)
        Dim length = Math.Min(size.Width, size.Height)
        Dim radius = Math.Abs((length - Thickness) / 2)
        Dim centerPoint As New Point(size.Width / 2, size.Height / 2)
        Dim angle = GetAngle(Value)
        Dim circleStart As New Point(centerPoint.X, centerPoint.Y + radius)
        Dim ArcPath As Path = FindControl(Of Path)("ArcPath")
        Dim ArcFigure As PathFigure = CType(ArcPath.Data, PathGeometry).Figures(0)
        Dim ArcFigureSegment As ArcSegment = ArcFigure.Segments(0)
        ArcFigureSegment.IsLargeArc = angle > Math.PI
        ArcFigureSegment.Point = ScaleUnitCirclePoint(centerPoint, radius, angle)
        ArcFigureSegment.Size = New Size(radius, radius)
        ArcFigure.StartPoint = circleStart
    End Sub
End Class
