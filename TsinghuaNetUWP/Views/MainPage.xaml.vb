Imports System.Net
Imports System.Text
Imports Berrysoft.Tsinghua.Net
Imports TsinghuaNetUWP.Background
Imports TsinghuaNetUWP.Helper
Imports Windows.ApplicationModel.Core
Imports Windows.Networking.Connectivity
Imports Windows.UI

Public NotInheritable Class MainPage
    Inherits Page

    Private settings As New SettingsHelper
    Private mainTimer As New DispatcherTimer
    Private networkListener As New NetworkListener

    Public Sub New()
        InitializeComponent()
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance)
        Dim titleBar = ApplicationView.GetForCurrentView().TitleBar
        titleBar.BackgroundColor = Colors.Transparent
        titleBar.ButtonBackgroundColor = Colors.Transparent
        titleBar.ButtonInactiveBackgroundColor = Colors.Transparent
        ThemeChanged()
        Dim viewTitleBar = CoreApplication.GetCurrentView().TitleBar
        viewTitleBar.ExtendViewIntoTitleBar = True
        Window.Current.SetTitleBar(MainFrame)
        Model.SettingsTheme = settings.Theme
        Model.ContentType = settings.ContentType
        mainTimer.Interval = TimeSpan.FromSeconds(1)
        AddHandler mainTimer.Tick, AddressOf MainTimerTick
        AddHandler networkListener.NetworkStatusChanged, AddressOf NetworkChanged
        Model.RegisterPropertyChangedCallback(MainViewModel.AutoLoginProperty, AddressOf AutoLoginChanged)
        Model.RegisterPropertyChangedCallback(MainViewModel.BackgroundAutoLoginProperty, AddressOf BackgroundAutoLoginChanged)
        Model.RegisterPropertyChangedCallback(MainViewModel.BackgroundLiveTileProperty, AddressOf BackgroundLiveTileChanged)
    End Sub

    Friend Sub SaveSettings()
        settings.Theme = Model.SettingsTheme
        settings.ContentType = Model.ContentType
        settings.SaveSettings()
    End Sub

    Private Async Sub PageLoaded()
        RefreshStatus()
        Dim al = settings.AutoLogin
        Model.AutoLogin = al
        Dim bal = settings.BackgroundAutoLogin
        Model.BackgroundAutoLogin = bal
        Dim blt = settings.BackgroundLiveTile
        Model.BackgroundLiveTile = blt
        If Await BackgroundHelper.RequestAccessAsync() Then
            BackgroundHelper.RegisterLogin(bal)
            BackgroundHelper.RegisterLiveTile(blt)
        End If
        Dim un = settings.StoredUsername
        If Not String.IsNullOrEmpty(un) Then
            Model.Username = un
            Dim pw = CredentialHelper.GetCredential(un)
            Model.Password = pw
            If al AndAlso Not ToastLogined AndAlso Not String.IsNullOrEmpty(pw) Then
                Await LoginImpl()
            Else
                Await RefreshImpl()
            End If
            Await RefreshNetUsersImpl()
        End If
    End Sub

    Private Sub ThemeChanged()
        Dim titleBar = ApplicationView.GetForCurrentView().TitleBar
        Select Case ActualTheme
            Case ElementTheme.Light
                titleBar.ButtonForegroundColor = Colors.Black
            Case ElementTheme.Dark
                titleBar.ButtonForegroundColor = Colors.White
        End Select
    End Sub

    Private Async Sub NetworkChanged()
        Await Dispatcher.RunAsync(Core.CoreDispatcherPriority.Normal, Async Sub() Await NetworkChangedImpl())
    End Sub

    Private Async Function NetworkChangedImpl() As Task
        RefreshStatus()
        If Not String.IsNullOrEmpty(Model.Password) Then
            Await LoginImpl()
        Else
            Await RefreshImpl()
        End If
        Await RefreshNetUsersImpl()
    End Function

    Private Sub OpenSettings()
        Split.IsPaneOpen = True
    End Sub

    Private Async Sub Login()
        Await LoginImpl()
    End Sub

    Private Async Sub Logout()
        Await LogoutImpl()
    End Sub

    Private Async Sub Refresh()
        Await RefreshImpl()
    End Sub

    Private Async Sub DropUser(sender As Object, e As IPAddress)
        Await DropImpl(e)
    End Sub

    Private Async Sub ShowChangeUser()
        Dim dialog As New ChangeUserDialog(Model.Username)
        dialog.RequestedTheme = Model.Theme
        Dim result = Await dialog.ShowAsync()
        If result = ContentDialogResult.Primary Then
            Dim un As String = dialog.UnBox.Text
            Dim pw As String = dialog.PwBox.Password
            CredentialHelper.RemoveCredential(un)
            If dialog.SaveBox.IsChecked.Value Then
                CredentialHelper.SaveCredential(un, pw)
            End If
            settings.StoredUsername = un
            Model.Username = un
            Model.Password = pw
            Split.IsPaneOpen = False
            Await LoginImpl()
        End If
    End Sub

    Private Sub MainTimerTick()
        Dim content As IUserContent = Model.UserContent
        If Not content.AddOneSecond() Then
            mainTimer.Stop()
        End If
    End Sub

    Private Sub RefreshStatus()
        Dim tuple = SettingsHelper.GetInternetStatus()
        Dim state = settings.SuggestNetState(tuple.Status, tuple.Ssid)
        Model.NetStatus = tuple.Status
        Model.Ssid = tuple.Ssid
        Model.SuggestState = state
        Model.State = state
    End Sub

    Private Async Sub ShowEditSuggestion()
        Dim dialog As New EditSuggestionDialog
        dialog.RequestedTheme = Model.Theme
        dialog.LanCombo.Value = settings.LanState
        dialog.WwanCombo.Value = settings.WwanState
        Dim s = settings.WlanStates
        dialog.RefreshWlanList(s)
        Dim result = Await dialog.ShowAsync()
        If result = ContentDialogResult.Primary Then
            settings.LanState = dialog.LanCombo.Value
            settings.WwanState = dialog.WwanCombo.Value
            s.Clear()
            For Each item In dialog.WlanList
                s.Add(item.Ssid, item.Value)
            Next
            RefreshStatus()
        End If
    End Sub

    Private Async Sub RefreshNetUsers()
        Await RefreshNetUsersImpl()
    End Sub

    Private Sub AutoLoginChanged()
        settings.AutoLogin = Model.AutoLogin
    End Sub

    Private Async Sub BackgroundAutoLoginChanged()
        settings.BackgroundAutoLogin = Model.BackgroundAutoLogin
        If Await BackgroundHelper.RequestAccessAsync() Then
            BackgroundHelper.RegisterLogin(Model.BackgroundAutoLogin)
        End If
    End Sub

    Private Async Sub BackgroundLiveTileChanged()
        settings.BackgroundLiveTile = Model.BackgroundLiveTile
        If Await BackgroundHelper.RequestAccessAsync() Then
            BackgroundHelper.RegisterLiveTile(Model.BackgroundLiveTile)
        End If
    End Sub

    Private Async Sub ContentTypeChanged()
        Await RefreshImpl()
    End Sub

    Friend Property ToastLogined As Boolean

    Private Async Function LoginImpl() As Task
        Dim content As IUserContent = Model.UserContent
        Try
            content.IsProgressActive = True
            Dim helper = GetHelper()
            If helper IsNot Nothing Then
                ShowResponse(Await helper.LoginAsync(), True)
            End If
            Await RefreshImpl(helper)
        Catch ex As Exception
            ShowException(ex)
        Finally
            content.IsProgressActive = False
        End Try
    End Function

    Private Async Function LogoutImpl() As Task
        Dim content As IUserContent = Model.UserContent
        Try
            content.IsProgressActive = True
            Dim helper = GetHelper()
            If helper IsNot Nothing Then
                ShowResponse(Await helper.LogoutAsync(), False)
            End If
            Await RefreshImpl(helper)
        Catch ex As Exception
            ShowException(ex)
        Finally
            content.IsProgressActive = False
        End Try
    End Function

    Private Async Function RefreshImpl() As Task
        Dim content As IUserContent = Model.UserContent
        Try
            content.IsProgressActive = True
            Dim helper = GetHelper()
            Await RefreshImpl(helper)
        Catch ex As Exception
            ShowException(ex)
        Finally
            content.IsProgressActive = False
        End Try
    End Function

    Private Async Function RefreshImpl(helper As IConnect) As Task
        Dim flux As FluxUser = Nothing
        If helper IsNot Nothing Then
            flux = Await helper.GetFluxAsync()
        End If
        NotificationHelper.UpdateTile(flux)
        Dim content As IUserContent = Model.UserContent
        If content IsNot Nothing Then
            content.User = If(flux, New FluxUser(Nothing, 0, TimeSpan.Zero, 0))
            If TypeOf content Is GraphUserContent Then
                If Not String.IsNullOrEmpty(Model.Username) Then
                    Dim userhelper As New UseregHelper(Model.Username, Model.Password)
                    Await userhelper.LoginAsync()
                    Await CType(content, GraphUserContent).RefreshDetails(userhelper)
                End If
            End If
            content.BeginAnimation()
            mainTimer.Start()
        End If
    End Function

    Private Async Function DropImpl(e As IPAddress) As Task
        Try
            Dim helper As New UseregHelper(Model.Username, Model.Password)
            Await helper.LoginAsync()
            Await helper.LogoutAsync(e)
            Await RefreshNetUsersImpl(helper)
        Catch ex As Exception
            ShowException(ex)
        End Try
    End Function

    Private Function GetHelper() As IConnect
        Return ConnectHelper.GetHelper(Model.State, Model.Username, Model.Password)
    End Function

    Private Async Sub ShowResponse(response As LogResponse, Optional login As Boolean? = Nothing)
        Model.Response = response.Message
        If login.HasValue AndAlso response.Succeed Then
            If login.Value Then
                Model.Response = "登录成功"
            Else
                Model.Response = "注销成功"
            End If
        End If
        ResponseViewer.Visibility = Visibility.Visible
        If login.HasValue AndAlso login.Value Then
            Await Task.Delay(3000)
            ResponseViewer.Visibility = Visibility.Collapsed
        End If
    End Sub

    Private Sub ShowException(e As Exception)
        Try
            ShowResponse(New LogResponse(False, $"异常 0x{e.HResult:X}：{e.Message}"))
        Catch ex As Exception
            Debug.Fail(ex.Message)
        End Try
    End Sub

    Private Sub HelpSelection(sender As Object, e As RoutedEventArgs)
        HelpFlyout.ShowAt(e.OriginalSource)
    End Sub

    Private Async Function RefreshNetUsersImpl() As Task
        Try
            If Model.State <> NetState.Unknown Then
                Dim helper As New UseregHelper(Model.Username, Model.Password)
                Await helper.LoginAsync()
                Await RefreshNetUsersImpl(helper)
            End If
        Catch ex As Exception
            ShowException(ex)
        End Try
    End Function

    Private Async Function RefreshNetUsersImpl(helper As UseregHelper) As Task
        Dim users = (Await helper.GetUsersAsync()).ToList()
        Dim usersmodel = Model.NetUsers
        Dim i As Integer = 0
        Do While i < usersmodel.Count
            Dim olduser As NetUser = usersmodel(i)
            For j = 0 To users.Count - 1
                Dim user As NetUser = users(j)
                If NetUserHelper.Equals(olduser, user) Then
                    users.RemoveAt(j)
                    i += 1
                    Continue Do
                End If
            Next
            usersmodel.RemoveAt(i)
        Loop
        If users.Count > 0 Then
            usersmodel.AddRange(users)
        End If
    End Function

    Private Async Sub ShowAbout()
        Dim dialog As New AboutDialog()
        Await dialog.ShowAsync()
    End Sub
End Class

NotInheritable Class NetworkListener
    Public Event NetworkStatusChanged As NetworkStatusChangedEventHandler

    Protected Sub OnNetworkStatusChanged(sender As Object)
        RaiseEvent NetworkStatusChanged(sender)
    End Sub

    Public Sub New()
        AddHandler NetworkInformation.NetworkStatusChanged, AddressOf OnNetworkStatusChanged
    End Sub

    Protected Overrides Sub Finalize()
        RemoveHandler NetworkInformation.NetworkStatusChanged, AddressOf OnNetworkStatusChanged
    End Sub
End Class
