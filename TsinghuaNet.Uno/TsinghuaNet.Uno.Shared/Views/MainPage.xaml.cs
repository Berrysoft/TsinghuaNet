using System;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using wux = Windows.UI.Xaml;
using TsinghuaNet.Models;

#if WINDOWS_UWP
using Microsoft.Toolkit.Uwp.Connectivity;
#endif

namespace TsinghuaNet.Uno.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
#if WINDOWS_UWP
            // 监视网络情况变化
            NetworkHelper.Instance.NetworkChanged += NetworkChanged;
#endif
        }

        /// <summary>
        /// 保存设置
        /// </summary>
        internal void SaveSettings() => Model.SaveSettings();

#if WINDOWS_UWP
        public static readonly wux.DependencyProperty FreeOffsetProperty = wux.DependencyProperty.Register(nameof(FreeOffset), typeof(double), typeof(MainPage), new wux.PropertyMetadata(0.0));
        public double FreeOffset
        {
            get => (double)GetValue(FreeOffsetProperty);
            set => SetValue(FreeOffsetProperty, value);
        }

        public static readonly wux.DependencyProperty FluxOffsetProperty = wux.DependencyProperty.Register(nameof(FluxOffset), typeof(double), typeof(MainPage), new wux.PropertyMetadata(0.0));
        public double FluxOffset
        {
            get => (double)GetValue(FluxOffsetProperty);
            set => SetValue(FluxOffsetProperty, value);
        }
#else
        public static readonly wux.DependencyProperty FluxBrushProperty = wux.DependencyProperty.Register(nameof(FluxBrush), typeof(Brush), typeof(MainPage), new wux.PropertyMetadata(new SolidColorBrush(AccentColor)));
        public Brush FluxBrush
        {
            get => (Brush)GetValue(FluxBrushProperty);
            set => SetValue(FluxBrushProperty, value);
        }

        private static readonly Color AccentColor = Color.FromArgb(0xFF, 0x00, 0x78, 0xD7);
        private static readonly Color AccentColorDark1 = Color.FromArgb(0xFF, 0x00, 0x5A, 0x9E);
        private static readonly Color AccentColorDark2 = Color.FromArgb(0xFF, 0x00, 0x42, 0x75);
#endif

        internal void BeginFluxAnimation()
        {
            var maxf = FluxHelper.GetMaxFlux(Model.OnlineUser.Flux, Model.OnlineUser.Balance);
#if WINDOWS_UWP
            FluxAnimation.To = Model.OnlineUser.Flux / maxf;
            FreeAnimation.To = FluxHelper.BaseFlux / maxf;
            FluxStoryboard.Begin();
#else
            var fluxOffset = Model.OnlineUser.Flux / maxf;
            var freeOffset = FluxHelper.BaseFlux / maxf;
            FluxBrush = new LinearGradientBrush()
            {
                StartPoint = new Point(0, 0.5),
                EndPoint = new Point(1, 0.5),
                GradientStops = new GradientStopCollection()
                {
                    new GradientStop() { Offset = fluxOffset, Color = AccentColor },
                    new GradientStop() { Offset = fluxOffset, Color = AccentColorDark1 },
                    new GradientStop() { Offset = Math.Max(freeOffset, fluxOffset), Color = AccentColorDark1 },
                    new GradientStop() { Offset = Math.Max(freeOffset, fluxOffset), Color = AccentColorDark2 }
                }
            };
#endif
        }

        /// <summary>
        /// 页面装载时触发
        /// </summary>
        private async void PageLoaded(object sender, wux.RoutedEventArgs e)
        {
            // 刷新状态
            await Model.RefreshStatusAsync();
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
            // 自动登录的条件为：
            // 打开了自动登录
            // 不知道后台任务成功登录
            // 密码不为空
            if (Model.Settings.AutoLogin && !ToastLogined && !string.IsNullOrEmpty(Model.Credential.Password))
                await Model.LoginAsync();
            else
                await Model.RefreshAsync();
        }

        private void PageSizeChanged(object sender, wux.SizeChangedEventArgs e)
        {
            string state = (wux.Window.Current.Bounds.Width > wux.Window.Current.Bounds.Height) ? "HorizonalState" : "VerticalState";
            wux.VisualStateManager.GoToState(this, state, true);
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
            if (!string.IsNullOrEmpty(Model.Credential.Password))
                await Model.LoginAsync();
            else
                await Model.RefreshAsync();
        }

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
#if WINDOWS_UWP
                await HideResponseStoryboard.BeginAsync();
#else
                HideResponseStoryboard.Begin();
#endif
            }
        }

        private void ShowDetail(object sender, wux.RoutedEventArgs e) => NavigateToType<DetailPage>();

        private void ShowAbout(object sender, wux.RoutedEventArgs e) => NavigateToType<AboutPage>();

        private void ShowSettings(object sender, wux.RoutedEventArgs e) => NavigateToType<SettingsPage>();

        private void NavigateToType<T>() where T : Page
        {
            if (wux.Window.Current.Content is Frame rootFrame)
            {
                rootFrame.Navigate(typeof(T));
            }
        }
    }

    static class UserContentHelper
    {
        public static double Max(double d1, double d2) => Math.Max(d1, d2);
    }
}
