Imports System.Net
Imports Berrysoft.Tsinghua.Net
Imports TsinghuaNetUWP.Helper
Imports Windows.ApplicationModel.Core
Imports Windows.UI

''' <summary>
''' 可用于自身或导航至 Frame 内部的空白页。
''' </summary>
Public NotInheritable Class MainPage
    Inherits Page

    Private settings As SettingsHelper

    Public Sub New()
        InitializeComponent()
        Dim titleBar = ApplicationView.GetForCurrentView().TitleBar
        titleBar.BackgroundColor = Colors.Transparent
        titleBar.ButtonBackgroundColor = Colors.Transparent
        titleBar.ButtonInactiveBackgroundColor = Colors.Transparent

        Dim viewTitleBar = CoreApplication.GetCurrentView().TitleBar
        viewTitleBar.ExtendViewIntoTitleBar = True

        Window.Current.SetTitleBar(MainFrame)
    End Sub

    Public Sub SaveSettings()
        settings.SaveSettings()
    End Sub

    Private Async Sub PageLoaded()
        RefreshStatusImpl()
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

    Private Sub RefreshStatus()
        RefreshStatusImpl()
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

    Private Sub RefreshStatusImpl()

    End Sub

    Private Async Function RefreshNetUsersImpl() As Task

    End Function

    Private Async Function RefreshNetUsersImpl(helper As IConnect) As Task

    End Function
End Class
