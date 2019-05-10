Imports Avalonia
Imports Avalonia.Controls
Imports Avalonia.Controls.Shapes
Imports Avalonia.Markup.Xaml
Imports Avalonia.Media

Public Class Arc
    Inherits UserControl

    Public Sub New()
        InitializeComponent()
        ThicknessProperty.Changed.Subscribe(AddressOf DrawArc)
        ValueProperty.Changed.Subscribe(AddressOf DrawArc)
        FillProperty.Changed.Subscribe(AddressOf DrawArc)
    End Sub

    Private Sub InitializeComponent()
        AvaloniaXamlLoader.Load(Me)
    End Sub

    Public Shared ReadOnly ThicknessProperty As StyledProperty(Of Double) = AvaloniaProperty.Register(Of Arc, Double)(NameOf(Thickness))
    Public Property Thickness As Double
        Get
            Return GetValue(ThicknessProperty)
        End Get
        Set(value As Double)
            SetValue(ThicknessProperty, value)
        End Set
    End Property

    Public Shared ReadOnly ValueProperty As StyledProperty(Of Double) = AvaloniaProperty.Register(Of Arc, Double)(NameOf(Value))
    Public Property Value As Double
        Get
            Return GetValue(ValueProperty)
        End Get
        Set(value As Double)
            SetValue(ValueProperty, value)
        End Set
    End Property

    Public Shared ReadOnly FillProperty As StyledProperty(Of IBrush) = AvaloniaProperty.Register(Of Arc, IBrush)(NameOf(Fill))
    Public Property Fill As IBrush
        Get
            Return GetValue(FillProperty)
        End Get
        Set(value As IBrush)
            SetValue(FillProperty, value)
        End Set
    End Property

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

    Friend Sub DrawArc() Handles Me.Initialized, Me.LayoutUpdated
        Dim size As New Vector(Width, Height)
        Dim length = Math.Min(size.X, size.Y)
        Dim radius = Math.Abs((length - Thickness) / 2)
        Dim centerPoint As Point = size / 2
        Dim angle = GetAngle(Value)
        Dim circleStart As New Point(centerPoint.X, centerPoint.Y + radius)
        Dim ArcPath As Path = FindControl(Of Path)("ArcPath")
        Dim ArcGeometry As PathGeometry = ArcPath.Data
        Dim ArcFigure As PathFigure = ArcGeometry.Figures(0)
        Dim ArcFigureSegment As ArcSegment = ArcFigure.Segments(0)
        ArcFigureSegment.IsLargeArc = angle > Math.PI
        ArcFigureSegment.Point = ScaleUnitCirclePoint(centerPoint, radius, angle)
        ArcFigureSegment.Size = New Size(radius, radius)
        ArcFigure.StartPoint = circleStart
        ArcPath.Data = Nothing
        ArcGeometry.Figures.Add(ArcFigure)
        ArcGeometry.Figures.RemoveAt(0)
        ArcPath.Data = ArcGeometry
    End Sub
End Class
