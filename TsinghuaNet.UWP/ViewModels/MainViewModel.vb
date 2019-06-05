Imports TsinghuaNet.Helper
Imports TsinghuaNet.UWP.Background
Imports TsinghuaNet.UWP.Helper

Public Class MainViewModel
    Inherits NetViewModel

    Private mainTimer As New DispatcherTimer

    Public Sub New()
        MyBase.New()
        Status = New InternetStatus()
        ' 设置计时器
        mainTimer.Interval = TimeSpan.FromSeconds(1)
        AddHandler mainTimer.Tick, AddressOf MainTimerTick
    End Sub

    Public Overrides Sub LoadSettings()
        ' 上一次登录的用户名
        Dim un = SettingsHelper.StoredUsername
        ' 设置为当前用户名并获取密码
        Credential.Username = un
        Credential.Password = CredentialHelper.GetCredential(un)
        ' 获取用户设置的主题
        SettingsTheme = SettingsHelper.Theme
        ContentType = SettingsHelper.ContentType
        ' 自动登录
        AutoLogin = SettingsHelper.AutoLogin
        ' 后台任务
        BackgroundAutoLogin = SettingsHelper.BackgroundAutoLogin
        BackgroundLiveTile = SettingsHelper.BackgroundLiveTile
        ' 流量限制
        If SettingsHelper.FluxLimit IsNot Nothing Then
            FluxLimit = SettingsHelper.FluxLimit
            EnableFluxLimit = True
        End If
    End Sub

    Public Overrides Sub SaveSettings()
        SettingsHelper.StoredUsername = Credential.Username
        SettingsHelper.AutoLogin = AutoLogin
        SettingsHelper.Theme = SettingsTheme
        SettingsHelper.ContentType = ContentType
        SettingsHelper.FluxLimit = If(EnableFluxLimit, CType(FluxLimit, Long?), Nothing)
    End Sub


    Private _UserContent As UIElement
    Public Property UserContent As UIElement
        Get
            Return _UserContent
        End Get
        Set(value As UIElement)
            SetProperty(_UserContent, value)
        End Set
    End Property

    Protected Overrides Async Function RefreshAsync(helper As IConnect) As Task(Of LogResponse)
        Dim res = Await MyBase.RefreshAsync(helper)
        ' 先启动计时器
        mainTimer?.Start()
        ' 更新磁贴
        NotificationHelper.UpdateTile(OnlineUser)
        If EnableFluxLimit Then
            NotificationHelper.SendWarningToast(OnlineUser, FluxLimit)
        End If
        ' 设置内容
        Dim content As IUserContent = TryCast(UserContent, IUserContent)
        If content IsNot Nothing Then
            content.User = OnlineUser
            ' 刷新图表
            If OnlineUser.Username IsNot Nothing AndAlso TypeOf content Is GraphUserContent AndAlso Not String.IsNullOrEmpty(Credential.Username) Then
                Dim userhelper = Credential.GetUseregHelper()
                Await userhelper.LoginAsync()
                Await CType(content, GraphUserContent).RefreshDetails(userhelper)
            End If
            content.BeginAnimation()
        End If
        Return res
    End Function

    Private Sub MainTimerTick()
        Dim content As IUserContent = UserContent
        If Not content.AddOneSecond() Then
            mainTimer.Stop()
        End If
    End Sub

    Protected Overrides Sub OnIsBusyChanged()
        MyBase.OnIsBusyChanged()
        Dim content As IUserContent = UserContent
        If content IsNot Nothing Then
            content.IsProgressActive = IsBusy
        End If
    End Sub

    Private _Response As String
    Public Property Response As String
        Get
            Return _Response
        End Get
        Set(value As String)
            SetProperty(_Response, value)
        End Set
    End Property

    Private _BackgroundAutoLogin As Boolean
    Public Property BackgroundAutoLogin As Boolean
        Get
            Return _BackgroundAutoLogin
        End Get
        Set(value As Boolean)
            SetProperty(_BackgroundAutoLogin, value, onChanged:=AddressOf OnBackgroundAutoLoginChanged)
        End Set
    End Property
    Private Async Sub OnBackgroundAutoLoginChanged()
        SettingsHelper.BackgroundAutoLogin = BackgroundAutoLogin
        If Await BackgroundHelper.RequestAccessAsync() Then
            BackgroundHelper.RegisterLogin(BackgroundAutoLogin)
        End If
    End Sub

    Private _BackgroundLiveTile As Boolean
    Public Property BackgroundLiveTile As Boolean
        Get
            Return _BackgroundLiveTile
        End Get
        Set(value As Boolean)
            SetProperty(_BackgroundLiveTile, value, onChanged:=AddressOf OnBackgroundLiveTileChanged)
        End Set
    End Property

    Private Async Sub OnBackgroundLiveTileChanged()
        SettingsHelper.BackgroundLiveTile = BackgroundLiveTile
        If Await BackgroundHelper.RequestAccessAsync() Then
            BackgroundHelper.RegisterLiveTile(BackgroundLiveTile)
        End If
    End Sub

    Private _SettingsTheme As UserTheme
    Public Property SettingsTheme As UserTheme
        Get
            Return _SettingsTheme
        End Get
        Set(value As UserTheme)
            SetProperty(_SettingsTheme, value, onChanged:=AddressOf OnSettingsThemeChanged)
        End Set
    End Property
    Private Sub OnSettingsThemeChanged()
        Dim settheme As UserTheme = SettingsTheme
        Dim actheme As ElementTheme = settheme
        If settheme = UserTheme.Auto Then
            Dim now As Date = Date.Now
            If now.Hour <= 6 OrElse now.Hour >= 18 Then
                actheme = ElementTheme.Dark
            Else
                actheme = ElementTheme.Light
            End If
        End If
        Theme = actheme
    End Sub

    Private _Theme As ElementTheme
    Public Property Theme As ElementTheme
        Get
            Return _Theme
        End Get
        Set(value As ElementTheme)
            SetProperty(_Theme, value)
        End Set
    End Property

    Private _ContentType As UserContentType
    Public Property ContentType As UserContentType
        Get
            Return _ContentType
        End Get
        Set(value As UserContentType)
            SetProperty(_ContentType, value, onChanged:=AddressOf OnContentTypeChanged)
        End Set
    End Property
    Private Sub OnContentTypeChanged()
        Dim oldc As IUserContent = UserContent
        Dim newc As IUserContent = Nothing
        Select Case ContentType
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
        UserContent = newc
        Refresh()
    End Sub
End Class
