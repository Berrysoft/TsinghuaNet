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

    Private mainTimer As New DispatcherTimer

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
        ' 设置计时器
        mainTimer.Interval = TimeSpan.FromSeconds(1)
        AddHandler mainTimer.Tick, AddressOf MainTimerTick
        ' 监视网络情况变化
        AddHandler NetworkHelper.Instance.NetworkChanged, AddressOf NetworkChanged
        ' 响应选项变化
        Model.RegisterPropertyChangedCallback(MainViewModel.AutoLoginProperty, AddressOf AutoLoginChanged)
        Model.RegisterPropertyChangedCallback(MainViewModel.BackgroundAutoLoginProperty, AddressOf BackgroundAutoLoginChanged)
        Model.RegisterPropertyChangedCallback(MainViewModel.BackgroundLiveTileProperty, AddressOf BackgroundLiveTileChanged)
    End Sub

    ''' <summary>
    ''' 保存设置
    ''' </summary>
    Friend Sub SaveSettings()
        SettingsHelper.Theme = Model.SettingsTheme
        SettingsHelper.ContentType = Model.ContentType
    End Sub

    ''' <summary>
    ''' 页面装载时触发
    ''' </summary>
    Private Async Sub PageLoaded()
        ' 刷新状态
        RefreshStatus()
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
        ' 上一次登录的用户名
        Dim un = SettingsHelper.StoredUsername
        If Not String.IsNullOrEmpty(un) Then
            ' 设置为当前用户名并获取密码
            Model.Username = un
            Dim pw = CredentialHelper.GetCredential(un)
            Model.Password = pw
            ' 自动登录的条件为：
            ' 打开了自动登录
            ' 不知道后台任务成功登录
            ' 密码不为空
            If al AndAlso Not ToastLogined AndAlso Not String.IsNullOrEmpty(pw) Then
                Await LoginImpl()
            Else
                Await RefreshImpl()
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

    ''' <summary>
    ''' 打开“更改用户”对话框
    ''' </summary>
    Private Async Sub ShowChangeUser()
        Dim dialog As New ChangeUserDialog(Model.Username)
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
            Model.Username = un
            Model.Password = pw
            ' 关闭设置栏并登录
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

    ''' <summary>
    ''' 根据网络类型与SSID判断建议网络类型
    ''' </summary>
    Private Sub RefreshStatus()
        Dim tuple = InternetStatusHelper.GetInternetStatus()
        Dim state = SettingsHelper.SuggestNetState(tuple.Status, tuple.Ssid)
        Model.NetStatus = tuple.Status
        Model.Ssid = tuple.Ssid
        Model.SuggestState = state
        Model.State = state
    End Sub

    ''' <summary>
    ''' 打开“编辑建议”对话框
    ''' </summary>
    Private Async Sub ShowEditSuggestion()
        Dim dialog As New EditSuggestionDialog
        dialog.RequestedTheme = Model.Theme
        dialog.LanCombo.SelectedIndex = SettingsHelper.LanState
        dialog.WwanCombo.SelectedIndex = SettingsHelper.WwanState
        Dim s = SettingsHelper.WlanStates
        dialog.RefreshWlanList(s)
        Dim result = Await dialog.ShowAsync()
        If result = ContentDialogResult.Primary Then
            SettingsHelper.LanState = dialog.LanCombo.SelectedIndex
            SettingsHelper.WwanState = dialog.WwanCombo.SelectedIndex
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
        SettingsHelper.AutoLogin = Model.AutoLogin
    End Sub

    Private Async Sub BackgroundAutoLoginChanged()
        SettingsHelper.BackgroundAutoLogin = Model.BackgroundAutoLogin
        If Await BackgroundHelper.RequestAccessAsync() Then
            BackgroundHelper.RegisterLogin(Model.BackgroundAutoLogin)
        End If
    End Sub

    Private Async Sub BackgroundLiveTileChanged()
        SettingsHelper.BackgroundLiveTile = Model.BackgroundLiveTile
        If Await BackgroundHelper.RequestAccessAsync() Then
            BackgroundHelper.RegisterLiveTile(Model.BackgroundLiveTile)
        End If
    End Sub

    Private Async Sub ContentTypeChanged()
        Await RefreshImpl()
    End Sub

    Friend Property ToastLogined As Boolean

    ''' <summary>
    ''' 登录当前用户并刷新
    ''' </summary>
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

    ''' <summary>
    ''' 注销当前用户并刷新
    ''' </summary>
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

    ''' <summary>
    ''' 刷新
    ''' </summary>
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

    ''' <summary>
    ''' 具体的刷新操作
    ''' </summary>
    ''' <param name="helper">网络连接辅助类，用于执行刷新任务</param>
    Private Async Function RefreshImpl(helper As IConnect) As Task
        Dim flux As FluxUser = Nothing
        If helper IsNot Nothing Then
            flux = Await helper.GetFluxAsync()
        End If
        ' 更新磁贴
        NotificationHelper.UpdateTile(flux)
        ' 设置内容
        Dim content As IUserContent = TryCast(Model.UserContent, IUserContent)
        If content IsNot Nothing Then
            content.User = flux
            ' 刷新图表
            If flux.Username IsNot Nothing AndAlso TypeOf content Is GraphUserContent AndAlso Not String.IsNullOrEmpty(Model.Username) Then
                Dim userhelper = GetUseregHelper()
                Await userhelper.LoginAsync()
                Await CType(content, GraphUserContent).RefreshDetails(userhelper)
            End If
            content.BeginAnimation()
            mainTimer.Start()
        End If
    End Function

    ''' <summary>
    ''' 根据IP强制下线某个连接
    ''' </summary>
    ''' <param name="e">连接的IP地址</param>
    Private Async Function DropImpl(e As IPAddress) As Task
        Try
            Dim helper = GetUseregHelper()
            Await helper.LoginAsync()
            Await helper.LogoutAsync(e)
            Await RefreshNetUsersImpl(helper)
        Catch ex As Exception
            ShowException(ex)
        End Try
    End Function

    ''' <summary>
    ''' 根据当前类型、用户名与密码实例化辅助类
    ''' </summary>
    ''' <returns>辅助类</returns>
    Private Function GetHelper() As IConnect
        Return ConnectHelper.GetHelper(Model.State, Model.Username, Model.Password, Client)
    End Function

    Private Function GetUseregHelper() As UseregHelper
        Return New UseregHelper(Model.Username, Model.Password, Client)
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
            If Model.State <> NetState.Unknown Then
                Dim helper = GetUseregHelper()
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
        Dim helper = GetUseregHelper()
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
