Imports TsinghuaNetUWP.Helper

Public Class MainViewModel
    Inherits DependencyObject

    ' TODO: readonly
    Public Shared ReadOnly UserContentProperty As DependencyProperty = DependencyProperty.Register(NameOf(UserContent), GetType(UIElement), GetType(MainViewModel), New PropertyMetadata(Nothing))
    Public Property UserContent As UIElement
        Get
            Return GetValue(UserContentProperty)
        End Get
        Set(value As UIElement)
            SetValue(UserContentProperty, value)
        End Set
    End Property

    Public Shared ReadOnly UsernameProperty As DependencyProperty = DependencyProperty.Register(NameOf(Username), GetType(String), GetType(MainViewModel), New PropertyMetadata(String.Empty))
    Public Property Username As String
        Get
            Return GetValue(UsernameProperty)
        End Get
        Set(value As String)
            SetValue(UsernameProperty, value)
        End Set
    End Property

    Public Shared ReadOnly PasswordProperty As DependencyProperty = DependencyProperty.Register(NameOf(Password), GetType(String), GetType(MainViewModel), New PropertyMetadata(String.Empty))
    Public Property Password As String
        Get
            Return GetValue(PasswordProperty)
        End Get
        Set(value As String)
            SetValue(PasswordProperty, value)
        End Set
    End Property

    Public Shared ReadOnly StateProperty As DependencyProperty = DependencyProperty.Register(NameOf(State), GetType(NetState), GetType(MainViewModel), New PropertyMetadata(NetState.Unknown))
    Public Property State As NetState
        Get
            Return GetValue(StateProperty)
        End Get
        Set(value As NetState)
            SetValue(StateProperty, value)
        End Set
    End Property

    Public Shared ReadOnly NetStatusProperty As DependencyProperty = DependencyProperty.Register(NameOf(NetStatus), GetType(InternetStatus), GetType(MainViewModel), New PropertyMetadata(InternetStatus.Unknown))
    Public Property NetStatus As InternetStatus
        Get
            Return GetValue(NetStatusProperty)
        End Get
        Set(value As InternetStatus)
            SetValue(NetStatusProperty, value)
        End Set
    End Property

    Public Shared ReadOnly SsidProperty As DependencyProperty = DependencyProperty.Register(NameOf(Ssid), GetType(String), GetType(MainViewModel), New PropertyMetadata(String.Empty))
    Public Property Ssid As String
        Get
            Return GetValue(SsidProperty)
        End Get
        Set(value As String)
            SetValue(SsidProperty, value)
        End Set
    End Property

    Public Shared ReadOnly SuggestStateProperty As DependencyProperty = DependencyProperty.Register(NameOf(SuggestState), GetType(NetState), GetType(MainViewModel), New PropertyMetadata(NetState.Unknown))
    Public Property SuggestState As NetState
        Get
            Return GetValue(SuggestStateProperty)
        End Get
        Set(value As NetState)
            SetValue(SuggestStateProperty, value)
        End Set
    End Property

    Public ReadOnly Property NetUsers As IObservableVector(Of Object) = New ObservableCollection(Of Object)

    Public ReadOnly Property Version As PackageVersion
        Get
            Return Package.Current.Id.Version
        End Get
    End Property
End Class
