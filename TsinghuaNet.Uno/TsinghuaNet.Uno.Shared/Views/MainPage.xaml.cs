using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.UI.Animations;
using TsinghuaNet.Uno.Helpers;
using Windows.ApplicationModel.Core;
using Windows.System;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using TsinghuaNet.Uno.ViewModels;
using TsinghuaNet.Helpers;

#if WINDOWS_UWP
using Microsoft.Toolkit.Uwp.Connectivity;
using TsinghuaNet.Uno.UWP.Background;
#endif

namespace TsinghuaNet.Uno.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            //if (Resources["DropUser"] is DropCommand c)
            //{
            //    c.DropUser += DropUser;
            //}
#if WINDOWS_UWP
            // 调整标题栏的颜色为透明
            // 按钮的背景色为透明
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.BackgroundColor = Colors.Transparent;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            // 按钮的前景色根据主题调节
            ThemeChangedImpl(titleBar);
            var viewTitleBar = CoreApplication.GetCurrentView().TitleBar;
            viewTitleBar.ExtendViewIntoTitleBar = true;
            // 监视网络情况变化
            NetworkHelper.Instance.NetworkChanged += NetworkChanged;
#endif
        }

        /// <summary>
        /// 保存设置
        /// </summary>
        internal void SaveSettings() => Model.SaveSettings();

        public static readonly DependencyProperty FreeOffsetProperty = DependencyProperty.Register(nameof(FreeOffset), typeof(double), typeof(MainPage), new PropertyMetadata(0.0));
        public double FreeOffset
        {
            get => (double)GetValue(FreeOffsetProperty);
            set => SetValue(FreeOffsetProperty, value);
        }

        public static readonly DependencyProperty FluxOffsetProperty = DependencyProperty.Register(nameof(FluxOffset), typeof(double), typeof(MainPage), new PropertyMetadata(0.0));
        public double FluxOffset
        {
            get => (double)GetValue(FluxOffsetProperty);
            set => SetValue(FluxOffsetProperty, value);
        }

        internal void BeginFluxAnimation()
        {
            var maxf = FluxHelper.GetMaxFlux(Model.OnlineUser.Flux, Model.OnlineUser.Balance);
            FluxAnimation.To = Model.OnlineUser.Flux / maxf;
            FreeAnimation.To = FluxHelper.BaseFlux / maxf;
            FluxStoryboard.Begin();
        }

        /// <summary>
        /// 页面装载时触发
        /// </summary>
        private async void PageLoaded(object sender, RoutedEventArgs e)
        {
            // 刷新状态
            await Model.RefreshStatusAsync();
            Model.Credential.State = Model.SuggestState;
            switch (Model.Credential.State)
            {
                case NetState.Unknown:
                    UnknownStateButton.IsChecked = true;
                    break;
                case NetState.Net:
                    NetStateButton.IsChecked = true;
                    break;
                case NetState.Auth4:
                    Auth4StateButton.IsChecked = true;
                    break;
                case NetState.Auth6:
                    Auth6StateButton.IsChecked = true;
                    break;
            }
#if WINDOWS_UWP
            // 调整后台任务
            if (await BackgroundHelper.RequestAccessAsync())
            {
                BackgroundHelper.RegisterLogin(Model.BackgroundAutoLogin);
                BackgroundHelper.RegisterLiveTile(Model.BackgroundLiveTile);
            }
#endif
            if (!string.IsNullOrEmpty(Model.Credential.Username))
            {
                // 自动登录的条件为：
                // 打开了自动登录
                // 不知道后台任务成功登录
                // 密码不为空
                if (Model.AutoLogin && !ToastLogined && !string.IsNullOrEmpty(Model.Credential.Password))
                    await Model.LoginAsync();
                else
                    await Model.RefreshAsync();
            }
        }

        private void PageSizeChanged(object sender, SizeChangedEventArgs e)
        {
            string state = (Window.Current.Bounds.Width > Window.Current.Bounds.Height) ? "HorizonalState" : "VerticalState";
            VisualStateManager.GoToState(this, state, true);
        }

        /// <summary>
        /// 根据主题调节标题栏按钮前景色
        /// </summary>
        private void ThemeChanged(FrameworkElement sender, object e) => ThemeChangedImpl(ApplicationView.GetForCurrentView().TitleBar);

        private void ThemeChangedImpl(ApplicationViewTitleBar titleBar)
        {
#if WINDOWS_UWP
            switch (ActualTheme)
            {
                case ElementTheme.Light:
                    titleBar.ButtonForegroundColor = Colors.Black;
                    break;
                case ElementTheme.Dark:
                    titleBar.ButtonForegroundColor = Colors.White;
                    break;
            }
#endif
        }

#if WINDOWS_UWP
        /// <summary>
        /// 调用Dispatcher刷新网络状态
        /// </summary>
        private async void NetworkChanged(object sender, object e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () => await NetworkChangedImpl());
        }
#endif

        /// <summary>
        /// 刷新网络状态
        /// </summary>
        private async Task NetworkChangedImpl()
        {
            await Model.RefreshStatusAsync();
            Model.Credential.State = Model.SuggestState;
            if (!string.IsNullOrEmpty(Model.Credential.Password))
                await Model.LoginAsync();
            else
                await Model.RefreshAsync();
        }

        //private void OpenSettings(object sender, RoutedEventArgs e) => Split.IsPaneOpen = true;

        //private async void DropUser(object sender, IPAddress e) => await ConnectionModel.DropAsync(e);

        //private async Task<ContentDialogResult> ShowDialogAsync<T>(T dialog) where T : ContentDialog
        //{
        //    dialog.RequestedTheme = Model.Theme;
        //    return await dialog.ShowAsync();
        //}

        //private Task<ContentDialogResult> ShowDialogAsync<T>() where T : ContentDialog, new() => ShowDialogAsync(new T());

        ///// <summary>
        ///// 打开“更改用户”对话框
        ///// </summary>
        //private async void ShowChangeUser(object sender, RoutedEventArgs e)
        //{
        //    ChangeUserDialog dialog = new ChangeUserDialog(Model.Credential.Username);
        //    // 显示对话框
        //    var result = await ShowDialogAsync(dialog);
        //    // 确定
        //    if (result == ContentDialogResult.Primary)
        //    {
        //        string un = dialog.UnBox.Text;
        //        string pw = dialog.PwBox.Password;
        //        // 不管是否保存，都需要先删除
        //        CredentialHelper.RemoveCredential(un);
        //        if (dialog.SaveBox.IsChecked.Value)
        //            CredentialHelper.SaveCredential(un, pw);
        //        // 同步
        //        Model.Credential.Username = un;
        //        Model.Credential.Password = pw;
        //        // 关闭设置栏并登录
        //        Split.IsPaneOpen = false;
        //        await Model.LoginAsync();
        //    }
        //}

        internal bool ToastLogined { get; set; }

        private void Model_ReceivedResponse(object sender, LogResponse res) => ShowResponse(res);

        private async void ShowResponse(LogResponse response)
        {
            Model.Response = response.Message;
#if WINDOWS_UWP
            await ShowResponseStoryboard.BeginAsync();
#else
                        ShowResponseStoryboard.Begin();
#endif
            if (response.Succeed)
            {
                await Task.Delay(3000);
                HideResponseStoryboard.Begin();
            }
        }

        //private void HelpSelection(object sender, RoutedEventArgs e) => HelpFlyout.ShowAt((FrameworkElement)e.OriginalSource);

        //private async void ShowDetail(object sender, RoutedEventArgs e) => await ShowDialogAsync<DetailDialog>();

        //private async void ShowAbout(object sender, RoutedEventArgs e) => await ShowDialogAsync<AboutDialog>();
    }

    static class UserContentHelper
    {
        public static double Max(double d1, double d2) => Math.Max(d1, d2);
    }
}
