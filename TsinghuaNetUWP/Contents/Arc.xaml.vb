Imports System.Numerics

Public NotInheritable Class Arc
    Inherits Canvas

    Public Shared ReadOnly ThicknessProperty As DependencyProperty = DependencyProperty.Register(NameOf(Thickness), GetType(Double), GetType(Arc), New PropertyMetadata(30.0, AddressOf OnSizePropertyChanged))
    Public Property Thickness As Double
        Get
            Return GetValue(ThicknessProperty)
        End Get
        Set(value As Double)
            SetValue(ThicknessProperty, value)
        End Set
    End Property

    Public Shared ReadOnly ValueProperty As DependencyProperty = DependencyProperty.Register(NameOf(Value), GetType(Double), GetType(Arc), New PropertyMetadata(0, AddressOf OnSizePropertyChanged))
    Public Property Value As Double
        Get
            Return GetValue(ValueProperty)
        End Get
        Set(value As Double)
            SetValue(ValueProperty, value)
        End Set
    End Property

    Private Shared Sub OnSizePropertyChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Dim arc As Arc = d
        arc.DrawArc()
    End Sub

    Public Shared ReadOnly FillProperty As DependencyProperty = DependencyProperty.Register(NameOf(Fill), GetType(Brush), GetType(Arc), New PropertyMetadata(Nothing))
    Public Property Fill As Brush
        Get
            Return GetValue(FillProperty)
        End Get
        Set(value As Brush)
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
        Dim dir As New Vector2(-Math.Sin(angle), Math.Cos(angle))
        Return (dir * radius + origin.ToVector2()).ToPoint()
    End Function

    Private Sub DrawArc()
        Dim size = RenderSize
        Dim length = Math.Min(size.Width, size.Height)
        Dim radius = (length - Thickness) / 2
        Dim centerPoint = size.ToVector2() / 2
        Dim angle = GetAngle(Value)
        Dim circleStart As New Point(centerPoint.X, centerPoint.Y + radius)
        ArcFigureSegment.IsLargeArc = angle > Math.PI
        ArcFigureSegment.Point = ScaleUnitCirclePoint(centerPoint.ToPoint(), radius, angle)
        ArcFigureSegment.Size = New Size(radius, radius)
        ArcFigure.StartPoint = circleStart
    End Sub
End Class
