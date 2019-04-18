Imports System.Net
Imports Berrysoft.Tsinghua.Net
Imports Microsoft.Toolkit.Uwp.Connectivity
Imports TsinghuaNetUWP.Helper
Imports Windows.ApplicationModel.Core
Imports Windows.UI

''' <summary>
''' 可用于自身或导航至 Frame 内部的空白页。
''' </summary>
Public NotInheritable Class MainPage
    Inherits Page

    Private settings As New SettingsHelper
    Private mainTimer As New DispatcherTimer
    Private networkListener As NetworkHelper = NetworkHelper.Instance

    Public Sub New()
        InitializeComponent()
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
        AddHandler networkListener.NetworkChanged, AddressOf NetworkChanged
    End Sub

    Public Sub SaveSettings()
        settings.Theme = Model.SettingsTheme
        settings.ContentType = Model.ContentType
        settings.SaveSettings()
    End Sub

    Private Async Sub PageLoaded()
        RefreshStatus()
        Dim un = settings.StoredUsername
        If Not String.IsNullOrEmpty(un) Then
            Model.Username = un
            Dim pw = CredentialHelper.GetCredential(un)
            Model.Password = pw
            If Not ToastLogined AndAlso Not String.IsNullOrEmpty(pw) Then
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

    End Sub

    Private Sub MainTimerTick()

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

    End Sub

    Private Async Sub RefreshNetUsers()
        Await RefreshNetUsersImpl()
    End Sub

    Public Property ToastLogined As Boolean

    Private Async Function LoginImpl() As Task
        Try
            Dim helper = GetHelper()
            If helper IsNot Nothing Then
                Await helper.LoginAsync()
            End If
            Await RefreshImpl(helper)
        Catch ex As Exception

        End Try
    End Function

    Private Async Function LogoutImpl() As Task
        Try
            Dim helper = GetHelper()
            If helper IsNot Nothing Then
                Await helper.LogoutAsync()
            End If
            Await RefreshImpl(helper)
        Catch ex As Exception

        End Try
    End Function

    Private Async Function RefreshImpl() As Task
        Try
            Dim helper = GetHelper()
            Await RefreshImpl(helper)
        Catch ex As Exception

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
            content.User = New FluxUserBox(flux)
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

        End Try
    End Function

    Private Function GetHelper() As IConnect
        Return ConnectHelper.GetHelper(Model.State, Model.Username, Model.Password)
    End Function

    Private Async Function RefreshNetUsersImpl() As Task
        Try
            If Model.State <> NetState.Unknown Then
                Dim helper As New UseregHelper(Model.Username, Model.Password)
                Await helper.LoginAsync()
                Await RefreshNetUsersImpl(helper)
            End If
        Catch ex As Exception

        End Try
    End Function

    Private Async Function RefreshNetUsersImpl(helper As UseregHelper) As Task
        Dim users = (Await helper.GetUsersAsync()).ToList()
        Dim usersmodel = Model.NetUsers
        Dim i As Integer = 0
        Do While i < usersmodel.Count
            Dim olduser As NetUserBox = usersmodel(i)
            For j = 0 To users.Count - 1
                Dim user As NetUser = users(j)
                If olduser.Equals(user) Then
                    users.RemoveAt(j)
                    i += 1
                    Continue Do
                End If
            Next
            usersmodel.RemoveAt(i)
        Loop
        For Each user In users
            Dim u As New NetUserBox(user)
            AddHandler u.DropUser, AddressOf DropUser
            usersmodel.Add(u)
        Next
    End Function
End Class
