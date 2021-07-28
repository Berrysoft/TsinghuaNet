using System;
using System.Linq;
using System.Reflection;
using System.Text;
using Syncfusion.SfChart.XForms.UWP;
using Syncfusion.SfDataGrid.XForms.UWP;
using Syncfusion.SfNumericTextBox.XForms.UWP;
using Syncfusion.XForms.UWP.ComboBox;
using Syncfusion.XForms.UWP.PopupLayout;
using TsinghuaNet.XF.UWP.Helpers;
using TsinghuaNet.XF.UWP.Services;
using TsinghuaNet.XF.UWP.Views;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using xf = Xamarin.Forms;

namespace TsinghuaNet.XF.UWP
{
    /// <summary>
    /// 提供特定于应用程序的行为，以补充默认的应用程序类。
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// 初始化单一实例应用程序对象。这是执行的创作代码的第一行，
        /// 已执行，逻辑上等同于 main() 或 WinMain()。
        /// </summary>
        public App()
        {
            InitializeComponent();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        /// <summary>
        /// 在应用程序由最终用户正常启动时进行调用。
        /// 将在启动应用程序以打开特定文件等情况下使用。
        /// </summary>
        /// <param name="e">有关启动请求和过程的详细信息。</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            OnActivated(e);
        }

        protected override void OnActivated(IActivatedEventArgs args)
        {
            // 不要在窗口已包含内容时重复应用程序初始化，
            // 只需确保窗口处于活动状态
            if (!(Window.Current.Content is Frame rootFrame))
            {
                // 创建要充当导航上下文的框架，并导航到第一页
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                xf.Forms.SetFlags("Brush_Experimental");
                SfDataGridRenderer.Init();
                SfPopupLayoutRenderer.Init();
                xf.Forms.Init(args, new Assembly[] {
                    typeof(SfChartRenderer).GetTypeInfo().Assembly,
                    typeof(SfDataGridRenderer).GetTypeInfo().Assembly,
                    typeof(SfNumericTextBoxRenderer).GetTypeInfo().Assembly,
                    typeof(SfComboBoxRenderer).GetTypeInfo().Assembly,
                    typeof(SfPopupLayoutRenderer).GetTypeInfo().Assembly
                });
                xf.DependencyService.Register<InternetStatus>();
                xf.DependencyService.Register<BackgroundManager>();

                // 将框架放在当前窗口中
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // 当导航堆栈尚未还原时，导航到第一页，
                // 并通过将所需信息作为导航参数传入来配置
                // 参数
                rootFrame.Navigate(typeof(MainPage));
            }
            // 确保当前窗口处于活动状态
            Window.Current.Activate();
        }

        /// <summary>
        /// 导航到特定页失败时调用
        /// </summary>
        /// <param name="sender">导航失败的框架</param>
        /// <param name="e">有关导航失败的详细信息</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }
    }
}
