Imports TsinghuaNet.Helper

Public NotInheritable Class ArcUserContent
    Inherits UserControl
    Implements IUserContent

    Public Shared ReadOnly UserProperty As DependencyProperty = DependencyProperty.Register(NameOf(User), GetType(FluxUser), GetType(ArcUserContent), New PropertyMetadata(Nothing, AddressOf UserPropertyChanged))
    Public Property User As FluxUser Implements IUserContent.User
        Get
            Return GetValue(UserProperty)
        End Get
        Set(value As FluxUser)
            SetValue(UserProperty, value)
        End Set
    End Property
    Private Shared Sub UserPropertyChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Dim content As ArcUserContent = d
        Dim flux As FluxUser = e.NewValue
        content.OnlineTime = flux.OnlineTime
        Dim maxf = FluxHelper.GetMaxFlux(flux.Flux, flux.Balance)
        content.FluxAnimation.To = flux.Flux / maxf
        content.FreeAnimation.To = FluxHelper.BaseFlux / maxf
    End Sub

    Public Shared ReadOnly OnlineTimeProperty As DependencyProperty = DependencyProperty.Register(NameOf(OnlineTime), GetType(TimeSpan), GetType(ArcUserContent), New PropertyMetadata(TimeSpan.Zero))
    Public Property OnlineTime As TimeSpan Implements IUserContent.OnlineTime
        Get
            Return GetValue(OnlineTimeProperty)
        End Get
        Set(value As TimeSpan)
            SetValue(OnlineTimeProperty, value)
        End Set
    End Property

    Public Shared ReadOnly FreeOffsetProperty As DependencyProperty = DependencyProperty.Register(NameOf(FreeOffset), GetType(Double), GetType(ArcUserContent), New PropertyMetadata(0.0))
    Public Property FreeOffset As Double
        Get
            Return GetValue(FreeOffsetProperty)
        End Get
        Set(value As Double)
            SetValue(FreeOffsetProperty, value)
        End Set
    End Property

    Public Shared ReadOnly FluxOffsetProperty As DependencyProperty = DependencyProperty.Register(NameOf(FluxOffset), GetType(Double), GetType(ArcUserContent), New PropertyMetadata(0.0))
    Public Property FluxOffset As Double
        Get
            Return GetValue(FluxOffsetProperty)
        End Get
        Set(value As Double)
            SetValue(FluxOffsetProperty, value)
        End Set
    End Property

    Public Property IsProgressActive As Boolean Implements IUserContent.IsProgressActive
        Get
            Return Progress.IsActive
        End Get
        Set(value As Boolean)
            Progress.IsActive = value
        End Set
    End Property

    Public Sub BeginAnimation() Implements IUserContent.BeginAnimation
        FluxStoryboard.Begin()
    End Sub

    Public Function AddOneSecond() As Boolean Implements IUserContent.AddOneSecond
        If User.Username Is Nothing OrElse String.IsNullOrEmpty(User.Username) Then
            Return False
        Else
            OnlineTime += TimeSpan.FromSeconds(1)
            Return True
        End If
    End Function
End Class
