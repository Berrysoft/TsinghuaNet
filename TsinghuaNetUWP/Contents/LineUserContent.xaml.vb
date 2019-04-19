Imports Berrysoft.Tsinghua.Net
Imports TsinghuaNetUWP.Helper

Public NotInheritable Class LineUserContent
    Inherits UserControl
    Implements IUserContent

    Public Shared ReadOnly UserProperty As DependencyProperty = DependencyProperty.Register(NameOf(User), GetType(FluxUser), GetType(LineUserContent), New PropertyMetadata(Nothing, AddressOf UserPropertyChanged))
    Public Property User As FluxUser Implements IUserContent.User
        Get
            Return GetValue(UserProperty)
        End Get
        Set(value As FluxUser)
            SetValue(UserProperty, value)
        End Set
    End Property
    Private Shared Sub UserPropertyChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Dim content As LineUserContent = d
        Dim flux As FluxUser = e.NewValue
        If flux IsNot Nothing Then
            content.OnlineTime = flux.OnlineTime
            Dim maxf = UserHelper.GetMaxFlux(flux.Flux, flux.Balance)
            content.FluxAnimation.To = flux.Flux / maxf
            content.FreeAnimation.To = UserHelper.BaseFlux / maxf
        End If
    End Sub

    Public Shared ReadOnly OnlineTimeProperty As DependencyProperty = DependencyProperty.Register(NameOf(OnlineTime), GetType(TimeSpan), GetType(LineUserContent), New PropertyMetadata(TimeSpan.Zero))
    Public Property OnlineTime As TimeSpan Implements IUserContent.OnlineTime
        Get
            Return GetValue(OnlineTimeProperty)
        End Get
        Set(value As TimeSpan)
            SetValue(OnlineTimeProperty, value)
        End Set
    End Property

    Public Shared ReadOnly FreeOffsetProperty As DependencyProperty = DependencyProperty.Register(NameOf(FreeOffset), GetType(Double), GetType(LineUserContent), New PropertyMetadata(0.0))
    Public Property FreeOffset As Double
        Get
            Return GetValue(FreeOffsetProperty)
        End Get
        Set(value As Double)
            SetValue(FreeOffsetProperty, value)
        End Set
    End Property

    Public Shared ReadOnly FluxOffsetProperty As DependencyProperty = DependencyProperty.Register(NameOf(FluxOffset), GetType(Double), GetType(LineUserContent), New PropertyMetadata(0.0))
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
            Return Progress.IsIndeterminate
        End Get
        Set(value As Boolean)
            Progress.IsIndeterminate = value
            MainRect.Visibility = If(value, Visibility.Collapsed, Visibility.Visible)
        End Set
    End Property

    Public Sub BeginAnimation() Implements IUserContent.BeginAnimation
        FluxStoryboard.Begin()
    End Sub

    Public Function AddOneSecond() As Boolean Implements IUserContent.AddOneSecond
        If User Is Nothing OrElse String.IsNullOrEmpty(User.Username) Then
            Return False
        Else
            OnlineTime += TimeSpan.FromSeconds(1)
            Return True
        End If
    End Function
End Class
