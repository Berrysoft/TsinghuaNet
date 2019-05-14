Imports System.Net
Imports System.Net.Http
Imports System.Text
Imports Microsoft.Toolkit.Uwp.Connectivity
Imports TsinghuaNet.Helper
Imports TsinghuaNet.UWP.Background
Imports TsinghuaNet.UWP.Helper
Imports Windows.ApplicationModel.Core
Imports Windows.UI
Imports WinRTXamlToolkit.AwaitableUI

Public NotInheritable Class MainPage
    Inherits Page

    Private Shared ReadOnly Client As New HttpClient

    Public Sub New()
        InitializeComponent()
        ' 调整标题栏的颜色为透明
        ' 按钮的背景色为透明
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance)
        Dim titleBar = ApplicationView.GetForCurrentView().TitleBar
        titleBar.BackgroundColor = Colors.Transparent
        titleBar.ButtonBackgroundColor = Colors.Transparent
        titleBar.ButtonInactiveBackgroundColor = Colors.Transparent
        ' 按钮的前景色根据主题调节
        ThemeChangedImpl(titleBar)
        Dim viewTitleBar = CoreApplication.GetCurrentView().TitleBar
        viewTitleBar.ExtendViewIntoTitleBar = True
        ' 将用户区拓展到全窗口
        Window.Current.SetTitleBar(MainFrame)
        ' 获取用户设置的主题
        Model.SettingsTheme = SettingsHelper.Theme
        Model.ContentType = SettingsHelper.ContentType
        ' 监视网络情况变化
        AddHandler NetworkHelper.Instance.NetworkChanged, AddressOf NetworkChanged
    End Sub

    ''' <summary>
    ''' 保存设置
    ''' </summary>
    Friend Sub SaveSettings()
        SettingsHelper.AutoLogin = Model.AutoLogin
        SettingsHelper.Theme = Model.SettingsTheme
        SettingsHelper.ContentType = Model.ContentType
        SettingsHelper.FluxLimit = If(Model.EnableFluxLimit, CType(Model.FluxLimit, Long?), Nothing)
    End Sub

    ''' <summary>
    ''' 页面装载时触发
    ''' </summary>
    Private Async Sub PageLoaded()
        ' 刷新状态
        Await Model.RefreshStatusAsync()
        Model.Credential.State = Model.SuggestState
        ' 自动登录
        Dim al = SettingsHelper.AutoLogin
        Model.AutoLogin = al
        ' 后台任务
        Dim bal = SettingsHelper.BackgroundAutoLogin
        Model.BackgroundAutoLogin = bal
        Dim blt = SettingsHelper.BackgroundLiveTile
        Model.BackgroundLiveTile = blt
        ' 调整后台任务
        If Await BackgroundHelper.RequestAccessAsync() Then
            BackgroundHelper.RegisterLogin(bal)
            BackgroundHelper.RegisterLiveTile(blt)
        End If
        If SettingsHelper.FluxLimit IsNot Nothing Then
            Model.FluxLimit = SettingsHelper.FluxLimit
            Model.EnableFluxLimit = True
        End If
        ' 上一次登录的用户名
        Dim un = SettingsHelper.StoredUsername
        If Not String.IsNullOrEmpty(un) Then
            ' 设置为当前用户名并获取密码
            Model.Credential.Username = un
            Dim pw = CredentialHelper.GetCredential(un)
            Model.Credential.Password = pw
            ' 自动登录的条件为：
            ' 打开了自动登录
            ' 不知道后台任务成功登录
            ' 密码不为空
            If al AndAlso Not ToastLogined AndAlso Not String.IsNullOrEmpty(pw) Then
                Await Model.LoginAsync()
            Else
                Await Model.RefreshAsync()
            End If
            ' 刷新当前用户所有连接状态
            Await RefreshNetUsersImpl()
        End If
    End Sub

    ''' <summary>
    ''' 根据主题调节标题栏按钮前景色
    ''' </summary>
    Private Sub ThemeChanged()
        ThemeChangedImpl(ApplicationView.GetForCurrentView().TitleBar)
    End Sub

    Private Sub ThemeChangedImpl(titleBar As ApplicationViewTitleBar)
        Select Case ActualTheme
            Case ElementTheme.Light
                titleBar.ButtonForegroundColor = Colors.Black
            Case ElementTheme.Dark
                titleBar.ButtonForegroundColor = Colors.White
        End Select
    End Sub

    ''' <summary>
    ''' 调用Dispatcher刷新网络状态
    ''' </summary>
    Private Async Sub NetworkChanged()
        Await Dispatcher.RunAsync(Core.CoreDispatcherPriority.Normal, Async Sub() Await NetworkChangedImpl())
    End Sub

    ''' <summary>
    ''' 刷新网络状态
    ''' </summary>
    Private Async Function NetworkChangedImpl() As Task
        Await Model.RefreshStatusAsync()
        Model.Credential.State = Model.SuggestState
        If Not String.IsNullOrEmpty(Model.Credential.Password) Then
            Await Model.LoginAsync()
        Else
            Await Model.RefreshAsync()
        End If
        Await RefreshNetUsersImpl()
    End Function

    Private Sub OpenSettings()
        Split.IsPaneOpen = True
    End Sub

    Private Async Sub DropUser(sender As Object, e As IPAddress)
        Await DropImpl(e)
    End Sub

    ''' <summary>
    ''' 打开“更改用户”对话框
    ''' </summary>
    Private Async Sub ShowChangeUser()
        Dim dialog As New ChangeUserDialog(Model.Credential.Username)
        dialog.RequestedTheme = Model.Theme
        ' 显示对话框
        Dim result = Await dialog.ShowAsync()
        ' 确定
        If result = ContentDialogResult.Primary Then
            Dim un As String = dialog.UnBox.Text
            Dim pw As String = dialog.PwBox.Password
            ' 不管是否保存，都需要先删除
            CredentialHelper.RemoveCredential(un)
            If dialog.SaveBox.IsChecked.Value Then
                CredentialHelper.SaveCredential(un, pw)
            End If
            ' 同步
            SettingsHelper.StoredUsername = un
            Model.Credential.Username = un
            Model.Credential.Password = pw
            ' 关闭设置栏并登录
            Split.IsPaneOpen = False
            Await Model.LoginAsync()
        End If
    End Sub

    Private Async Sub RefreshNetUsers()
        Await RefreshNetUsersImpl()
    End Sub

    Friend Property ToastLogined As Boolean

    ''' <summary>
    ''' 根据IP强制下线某个连接
    ''' </summary>
    ''' <param name="e">连接的IP地址</param>
    Private Async Function DropImpl(e As IPAddress) As Task
        Try
            Dim helper = Model.Credential.GetUseregHelper()
            Await helper.LoginAsync()
            Await helper.LogoutAsync(e)
            Await RefreshNetUsersImpl(helper)
        Catch ex As Exception
            ShowException(ex)
        End Try
    End Function

    Private Async Sub ShowResponse(response As LogResponse, Optional login As Boolean? = Nothing)
        Model.Response = response.Message
        If login.HasValue AndAlso response.Succeed Then
            Model.Response = If(login.Value, "登录成功", "注销成功")
        End If
        Await ShowResponseStoryboard.BeginAsync()
        If login.HasValue AndAlso login.Value Then
            Await Task.Delay(3000)
            HideResponseStoryboard.Begin()
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

    ''' <summary>
    ''' 刷新所有连接情况
    ''' </summary>
    Private Async Function RefreshNetUsersImpl() As Task
        Try
            If Model.Credential.State <> NetState.Unknown Then
                Dim helper = Model.Credential.GetUseregHelper()
                Await helper.LoginAsync()
                Await RefreshNetUsersImpl(helper)
            End If
        Catch ex As Exception
            ShowException(ex)
        End Try
    End Function

    ''' <summary>
    ''' 使用给定的帮助类刷新所有连接情况。
    ''' 在调用这个方法前要调用<see cref="UseregHelper.LoginAsync"/>。
    ''' </summary>
    ''' <param name="helper">帮助类实例</param>
    Private Async Function RefreshNetUsersImpl(helper As UseregHelper) As Task
        Dim users = (Await helper.GetUsersAsync()).ToList()
        Dim usersmodel = Model.NetUsers
        Dim i As Integer = 0
        Do While i < usersmodel.Count
            Dim olduser As NetUser = usersmodel(i)
            ' 循环判断旧元素是否存在于新集合中
            For j = 0 To users.Count - 1
                Dim user As NetUser = users(j)
                ' 如果存在则移除新元素
                If olduser = user Then
                    users.RemoveAt(j)
                    i += 1
                    Continue Do
                End If
            Next
            ' 反之移除旧元素
            usersmodel.RemoveAt(i)
        Loop
        ' 最后添加新增元素
        ' 判断大小以防止索引错误
        If users.Count > 0 Then
            usersmodel.AddRange(users)
        End If
    End Function

    Private Async Sub ShowDetail()
        Dim helper = Model.Credential.GetUseregHelper()
        Await helper.LoginAsync()
        Dim dialog As New DetailDialog(helper)
        dialog.RequestedTheme = Model.Theme
        Await dialog.ShowAsync()
    End Sub

    Private Async Sub ShowAbout()
        Dim dialog As New AboutDialog()
        dialog.RequestedTheme = Model.Theme
        Await dialog.ShowAsync()
    End Sub
End Class
