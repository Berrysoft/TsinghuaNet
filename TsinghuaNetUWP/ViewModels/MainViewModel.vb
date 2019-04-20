Imports TsinghuaNetUWP.Helper

Public Class MainViewModel
    Inherits DependencyObject

    Public Shared ReadOnly UserContentProperty As DependencyProperty = DependencyProperty.Register(NameOf(UserContent), GetType(UIElement), GetType(MainViewModel), New PropertyMetadata(Nothing))
    Public Property UserContent As UIElement
        Get
            Return GetValue(UserContentProperty)
        End Get
        Set(value As UIElement)
            SetValue(UserContentProperty, value)
        End Set
    End Property

    Public Shared ReadOnly ResponseProperty As DependencyProperty = DependencyProperty.Register(NameOf(Response), GetType(String), GetType(MainViewModel), New PropertyMetadata(Nothing))
    Public Property Response As String
        Get
            Return GetValue(ResponseProperty)
        End Get
        Set(value As String)
            SetValue(ResponseProperty, value)
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

    Public Shared ReadOnly AutoLoginProperty As DependencyProperty = DependencyProperty.Register(NameOf(AutoLogin), GetType(Boolean), GetType(MainViewModel), New PropertyMetadata(True))
    Public Property AutoLogin As Boolean
        Get
            Return GetValue(AutoLoginProperty)
        End Get
        Set(value As Boolean)
            SetValue(AutoLoginProperty, value)
        End Set
    End Property

    Public Shared ReadOnly BackgroundAutoLoginProperty As DependencyProperty = DependencyProperty.Register(NameOf(BackgroundAutoLogin), GetType(Boolean), GetType(MainViewModel), New PropertyMetadata(True))
    Public Property BackgroundAutoLogin As Boolean
        Get
            Return GetValue(BackgroundAutoLoginProperty)
        End Get
        Set(value As Boolean)
            SetValue(BackgroundAutoLoginProperty, value)
        End Set
    End Property

    Public Shared ReadOnly BackgroundLiveTileProperty As DependencyProperty = DependencyProperty.Register(NameOf(BackgroundLiveTile), GetType(Boolean), GetType(MainViewModel), New PropertyMetadata(True))
    Public Property BackgroundLiveTile As Boolean
        Get
            Return GetValue(BackgroundLiveTileProperty)
        End Get
        Set(value As Boolean)
            SetValue(BackgroundLiveTileProperty, value)
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

    Public Shared ReadOnly SettingsThemeProperty As DependencyProperty = DependencyProperty.Register(NameOf(SettingsTheme), GetType(UserTheme), GetType(MainViewModel), New PropertyMetadata(UserTheme.Default, AddressOf SettingsThemePropertyChanged))
    Public Property SettingsTheme As UserTheme
        Get
            Return GetValue(SettingsThemeProperty)
        End Get
        Set(value As UserTheme)
            SetValue(SettingsThemeProperty, value)
        End Set
    End Property
    Private Shared Sub SettingsThemePropertyChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Dim model As MainViewModel = d
        Dim theme As UserTheme = e.NewValue
        Dim actheme As ElementTheme = theme
        If theme = UserTheme.Auto Then
            Dim now As Date = Date.Now
            If now.Hour <= 6 OrElse now.Hour >= 18 Then
                actheme = ElementTheme.Dark
            Else
                actheme = ElementTheme.Light
            End If
        End If
        model.Theme = actheme
    End Sub

    Public Shared ReadOnly ThemeProperty As DependencyProperty = DependencyProperty.Register(NameOf(Theme), GetType(ElementTheme), GetType(MainViewModel), New PropertyMetadata(ElementTheme.Default))
    Public Property Theme As ElementTheme
        Get
            Return GetValue(ThemeProperty)
        End Get
        Set(value As ElementTheme)
            SetValue(ThemeProperty, value)
        End Set
    End Property

    Public Shared ReadOnly ContentTypeProperty As DependencyProperty = DependencyProperty.Register(NameOf(ContentType), GetType(UserContentType), GetType(MainViewModel), New PropertyMetadata(UserContentType.Ring, AddressOf OnContentTypePropertyChanged))
    Public Property ContentType As UserContentType
        Get
            Return GetValue(ContentTypeProperty)
        End Get
        Set(value As UserContentType)
            SetValue(ContentTypeProperty, value)
        End Set
    End Property
    Private Shared Sub OnContentTypePropertyChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Dim model As MainViewModel = d
        Dim type As UserContentType = e.NewValue
        Dim oldc As IUserContent = model.UserContent
        Dim newc As IUserContent = Nothing
        Select Case type
            Case UserContentType.Line
                newc = New LineUserContent()
            Case UserContentType.Ring
                newc = New ArcUserContent()
            Case UserContentType.Water
                newc = New WaterUserContent()
            Case UserContentType.Graph
                newc = New GraphUserContent()
        End Select
        If oldc IsNot Nothing AndAlso newc IsNot Nothing Then
            newc.User = oldc.User
        End If
        model.UserContent = newc
        model.OnContentTypeChanged(type)
    End Sub

    Public Event ContentTypeChanged As EventHandler(Of UserContentType)
    Protected Overridable Sub OnContentTypeChanged(e As UserContentType)
        RaiseEvent ContentTypeChanged(Me, e)
    End Sub

    Public ReadOnly Property NetUsers As New ObservableCollection(Of NetUserBox)

    Public ReadOnly Property Version As PackageVersion
        Get
            Return Package.Current.Id.Version
        End Get
    End Property

    Public Function GetVersionString(ver As PackageVersion) As String
        Return $"{ver.Major}.{ver.Minor}.{ver.Build}.{ver.Revision}"
    End Function
End Class
