Imports System.Net
Imports System.Net.Http
Imports System.Text
Imports Microsoft.Toolkit.Uwp.Connectivity
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
        ' 监视网络情况变化
        AddHandler NetworkHelper.Instance.NetworkChanged, AddressOf NetworkChanged
    End Sub

    ''' <summary>
    ''' 保存设置
    ''' </summary>
    Friend Sub SaveSettings()
        Model.SaveSettings()
    End Sub

    ''' <summary>
    ''' 页面装载时触发
    ''' </summary>
    Private Async Sub PageLoaded()
        ' 刷新状态
        Await Model.RefreshStatusAsync()
        Model.Credential.State = Model.SuggestState
        ' 调整后台任务
        If Await BackgroundHelper.RequestAccessAsync() Then
            BackgroundHelper.RegisterLogin(Model.BackgroundAutoLogin)
            BackgroundHelper.RegisterLiveTile(Model.BackgroundLiveTile)
        End If
        If Not String.IsNullOrEmpty(Model.Credential.Username) Then
            ' 自动登录的条件为：
            ' 打开了自动登录
            ' 不知道后台任务成功登录
            ' 密码不为空
            If Model.AutoLogin AndAlso Not ToastLogined AndAlso Not String.IsNullOrEmpty(Model.Credential.Password) Then
                Await Model.LoginAsync()
            Else
                Await Model.RefreshAsync()
            End If
            ' 刷新当前用户所有连接状态
            Await ConnectionModel.RefreshNetUsersAsync()
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
        Await ConnectionModel.RefreshNetUsersAsync()
    End Function

    Private Sub OpenSettings()
        Split.IsPaneOpen = True
    End Sub

    Private Async Sub DropUser(sender As Object, e As IPAddress)
        Await ConnectionModel.DropAsync(e)
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
            Model.Credential.Username = un
            Model.Credential.Password = pw
            ' 关闭设置栏并登录
            Split.IsPaneOpen = False
            Await Model.LoginAsync()
        End If
    End Sub

    Friend Property ToastLogined As Boolean

    Private Sub Model_ReceivedResponse(sender As Object, res As LogResponse)
        ShowResponse(res)
    End Sub

    Private Async Sub ShowResponse(response As LogResponse)
        Model.Response = response.Message
        Await ShowResponseStoryboard.BeginAsync()
        If response.Succeed Then
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
